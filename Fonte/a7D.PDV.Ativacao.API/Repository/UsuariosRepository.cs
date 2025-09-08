using System.Security.Cryptography;
using System.Text;
using a7D.PDV.Ativacao.API.Data;
using a7D.PDV.Ativacao.API.Enums;
using a7D.PDV.Ativacao.API.Exceptions;
using a7D.PDV.Ativacao.API.Model;
using a7D.PDV.Ativacao.API.Services.EmailService;
using Microsoft.EntityFrameworkCore;
using static BCrypt.Net.BCrypt;

namespace a7D.PDV.Ativacao.API.Repository
{
    public class UsuariosRepository : BaseRepository<Usuario>
    {
        readonly IEmailService _emailService;
        const int WorkFactor = 10;

        public UsuariosRepository(ApplicationDbContext context, IEmailService emailService)
            : base(context)
        {
            _emailService = emailService;
        }

        static string NewSecureToken(int bytesLength = 32)
        {
            var bytes = RandomNumberGenerator.GetBytes(bytesLength);
            var b64 = Convert.ToBase64String(bytes);
            return b64.Replace("+", "-").Replace("/", "_").TrimEnd('=');
        }

        /// <summary>
        /// Devolve uma cópia "segura" (sem Senha/Hash/DtSolicitacao) para retorno público.
        /// Não altera a entidade rastreada pelo EF.
        /// </summary>
        static Usuario ToSafeUser(Usuario u)
            => new Usuario
            {
                IDUsuario = u.IDUsuario,
                Nome = u.Nome,
                Email = u.Email,
                Adm = u.Adm,
                Ativo = u.Ativo,
                Excluido = u.Excluido,
                CadastroPendente = u.CadastroPendente,
                DtUltimaAlteracao = u.DtUltimaAlteracao,
                // Campos sensíveis propositalmente não copiados:
                // Senha, HashAlterarSenha, DtSolicitacaoAlteracaoSenha
            };

        async Task<Usuario?> BuscarPorEmailAsync(string email, bool asNoTracking = true, CancellationToken ct = default)
        {
            var q = Set.Where(u => u.Email == email && !u.Excluido);
            if (asNoTracking) q = q.AsNoTracking();
            return await q.FirstOrDefaultAsync(ct);
        }

        public async Task<Usuario?> BuscarPorIdLimpoAsync(int idUsuario, CancellationToken ct = default)
        {
            var usuario = await GetAsync(idUsuario, ct);
            if (usuario is null) return null;
            return ToSafeUser(usuario);
        }

        public async Task<bool> EmailExisteAsync(string email, int? idIgnorar = null, CancellationToken ct = default)
        {
            var q = Set.Where(u => u.Email == email && !u.Excluido);
            if (idIgnorar.HasValue)
                q = q.Where(u => u.IDUsuario != idIgnorar.Value);

            return await q.AnyAsync(ct);
        }

        async Task<bool> ExisteEmailAsync(string email, CancellationToken ct = default)
            => await Set.AnyAsync(u => !u.Excluido && u.Email == email, ct);

        public async Task<Usuario?> BuscarPorHashAsync(string hash, CancellationToken ct = default)
        {
            var usuario = await Set.FirstOrDefaultAsync(u => u.HashAlterarSenha == hash, ct);
            if (usuario is null || usuario.Excluido)
                return null;

            var expira = usuario.DtSolicitacaoAlteracaoSenha?.ToLocalTime().AddDays(1);
            if (expira is not null && expira < DateTime.Now)
            {
                usuario.HashAlterarSenha = null;
                usuario.DtSolicitacaoAlteracaoSenha = null;
                await StoreAsync(ct);
                return null;
            }

            return ToSafeUser(usuario);
        }

        public async Task<Usuario> AutenticarAsync(string email, string senha, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));
            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentNullException(nameof(senha));

            var usuario = await BuscarPorEmailAsync(email, asNoTracking: false, ct);
            if (usuario is null)
                throw new InvalidOperationException("Usuário não encontrado.");

            if (!usuario.Ativo)
                throw new InvalidOperationException("Usuário inativo.");

            if (usuario.CadastroPendente)
                throw new CadastroPendenteException();

            if (!EnhancedVerify(senha, usuario.Senha))
                throw new InvalidOperationException("Credenciais inválidas.");

            return ToSafeUser(usuario);
        }

        public async IAsyncEnumerable<Usuario> BuscarUsuariosAsync(
            int page, int count,
            string? nome, string? email,
            string? admin, string? cadastroPendente,
            string ativo = "1", string excluido = "0",
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken ct = default)
        {
            var q = Set.AsNoTracking().OrderBy(u => u.Nome).AsQueryable();

            if (!string.IsNullOrWhiteSpace(nome))
                q = q.Where(u => u.Nome.Contains(nome));

            if (!string.IsNullOrWhiteSpace(email))
                q = q.Where(u => u.Email.Contains(email));

            if (!string.IsNullOrWhiteSpace(admin))
                q = q.Where(u => u.Adm == (admin == "1"));

            if (!string.IsNullOrWhiteSpace(cadastroPendente))
                q = q.Where(u => u.CadastroPendente == (cadastroPendente == "1"));

            if (!string.IsNullOrWhiteSpace(ativo))
                q = q.Where(u => u.Ativo == (ativo == "1"));

            if (!string.IsNullOrWhiteSpace(excluido))
                q = q.Where(u => u.Excluido == (excluido == "1"));
            else
                q = q.Where(u => !u.Excluido);

            if (page > 0 && count > 0)
                q = q.Skip((page - 1) * count).Take(count);
            else if (count > 0)
                q = q.Take(count);

            await foreach (var u in q.AsAsyncEnumerable().WithCancellation(ct))
                yield return ToSafeUser(u);
        }

        public async Task<Usuario> AdicionarUsuarioAsync(string email, string? nome = null, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            if (await ExisteEmailAsync(email, ct))
                throw new EmailExistenteException();

            var usuario = new Usuario
            {
                Ativo = true,
                Excluido = false,
                CadastroPendente = true,
                Email = email,
                DtUltimaAlteracao = DateTime.UtcNow,
                HashAlterarSenha = NewSecureToken(),
                Nome = nome
            };

            Set.Add(usuario);
            await StoreAsync(ct);

            return ToSafeUser(usuario);
        }

        public async Task<string?> SolicitarNovaSenhaAsync(string email, CancellationToken ct = default)
        {
            var usuario = await BuscarPorEmailAsync(email, asNoTracking: false, ct);
            if (usuario is { Excluido: false, Ativo: true })
            {
                var hash = NewSecureToken();
                usuario.DtSolicitacaoAlteracaoSenha = DateTime.UtcNow;
                usuario.HashAlterarSenha = hash;
                await StoreAsync(ct);
                return hash;
            }
            return null;
        }

        public async Task AlterarSenhaAsync(string hash, string nome, string novaSenha, CancellationToken ct = default)
        {
            var safe = await BuscarPorHashAsync(hash, ct);
            if (safe is null)
                throw new InvalidOperationException("Link inválido ou expirado.");

            var usuario = await GetAsync(safe.IDUsuario, ct) ?? throw new InvalidOperationException("Usuário não encontrado.");

            usuario.Nome = nome;
            usuario.Senha = EnhancedHashPassword(novaSenha, WorkFactor);
            usuario.HashAlterarSenha = null;
            usuario.DtSolicitacaoAlteracaoSenha = null;
            usuario.CadastroPendente = false;
            usuario.DtUltimaAlteracao = DateTime.UtcNow;

            await StoreAsync(ct);
        }

        public async Task AlterarCadastroAsync(int idUsuario, string? nome = null, bool? ativo = null, bool? adm = null, CancellationToken ct = default)
        {
            var usuario = await GetAsync(idUsuario, ct);
            if (usuario is null) return;

            if (!string.IsNullOrWhiteSpace(nome))
                usuario.Nome = nome;

            if (adm.HasValue)
                usuario.Adm = adm.Value;

            if (ativo.HasValue)
                usuario.Ativo = ativo.Value;

            usuario.DtUltimaAlteracao = DateTime.UtcNow;

            await StoreAsync(ct);
        }

        public async Task ExcluirUsuarioAsync(int idUsuario, CancellationToken ct = default)
        {
            var usuario = await GetAsync(idUsuario, ct);
            if (usuario is null)
                throw new ArgumentOutOfRangeException(nameof(idUsuario));

            usuario.Excluido = true;
            usuario.DtUltimaAlteracao = DateTime.UtcNow;

            await StoreAsync(ct);
        }

        public async Task EnviarEmailCadastroAsync(string email, CancellationToken ct = default)
        {
            var usuario = await BuscarPorEmailAsync(email, asNoTracking: false, ct);
            if (usuario?.CadastroPendente == true)
            {
                usuario.HashAlterarSenha = NewSecureToken();
                usuario.DtUltimaAlteracao = DateTime.UtcNow;
                await StoreAsync(ct);

                await _emailService.EnviarUsuarioAsync(ETipoEmailUsuario.NovoCadastro, ToSafeUser(usuario));
            }
        }

        public async Task EnviarEmailNovaSenhaAsync(string email, CancellationToken ct = default)
        {
            var usuario = await BuscarPorEmailAsync(email, asNoTracking: true, ct);
            if (usuario is not null)
                await _emailService.EnviarUsuarioAsync(ETipoEmailUsuario.EsqueciASenha, usuario);
        }

        // [Obsolete("Use NewSecureToken() para tokens de segurança.")]
        // static string LegacySha1(string input)
        // {
        //     using var sha1 = SHA1.Create();
        //     var hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(input));
        //     var sb = new StringBuilder(hash.Length * 2);
        //     foreach (var b in hash) sb.Append(b.ToString("x2"));
        //     return sb.ToString();
        // }
    }
}
