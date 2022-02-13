using a7D.PDV.Ativacao.API.Context;
using a7D.PDV.Ativacao.API.Entities;
using a7D.PDV.Ativacao.API.Exceptions;
using a7D.PDV.Ativacao.API.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static BCrypt.Net.BCrypt;

namespace a7D.PDV.Ativacao.API.Repository
{
    public class UsuariosRepository : BaseRepository<Usuario>
    {

        private readonly int workFactor = 10;

        public UsuariosRepository(AtivacaoContext context) : base(context)
        {
        }

        private Usuario BuscarPorEmail(string email)
        {
            return _set.FirstOrDefault(u => u.Email == email && u.Excluido == false);
        }

        public async Task<Usuario> BuscarPorIdLimpo(int idUsuario)
        {
            var usuario = await BuscarPorId(idUsuario);
            if (usuario == null)
                return null;
            return LimparUsuario(usuario);
        }

        public bool EmailExiste(string email, int? idIgnorar = null)
        {
            IQueryable<Usuario> query = _set.Where(u => u.Email == email && u.Excluido == false);
            if (idIgnorar.HasValue)
                query = query.Where(u => u.IDUsuario != idIgnorar.Value);
            return query.Any();
        }

        private bool ExisteEmail(string email)
        {
            return _set.Any(u => u.Excluido == false && u.Email == email);
        }

        public async Task<Usuario> BuscarPorHash(string hash)
        {
            var usuario = _set.FirstOrDefault(u => u.HashAlterarSenha == hash);
            if (usuario == null)
                return null;

            if (usuario.Excluido)
                return null;

            if (usuario.DtSolicitacaoAlteracaoSenha?.ToLocalTime().AddDays(1) < DateTime.Now)
            {
                usuario.HashAlterarSenha = null;
                usuario.DtSolicitacaoAlteracaoSenha = null;
                await SalvarMudancas();
                return null;
            }
            return LimparUsuario(usuario);
        }

        public Usuario Autenticar(string email, string senha)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            if (string.IsNullOrWhiteSpace(senha))
                throw new ArgumentNullException(nameof(senha));

            var usuario = BuscarPorEmail(email);

            if (usuario == null)
                throw new Exception();

            if (usuario.Ativo == false)
                throw new Exception();

            if (usuario.CadastroPendente == true)
                throw new CadastroPendenteException();

            if (EnhancedVerify(senha, usuario.Senha))
            {
                return LimparUsuario(usuario);
            }
            throw new Exception();
        }

        internal IEnumerable<Usuario> BuscarUsuarios(int page, int count, string nome, string email, string admin, string cadastroPendente, string ativo = "1", string excluido = "0")
        {
            IQueryable<Usuario> query = _set.OrderBy(u => u.Nome);

            if (!string.IsNullOrWhiteSpace(nome))
                query = query.Where(u => u.Nome.Contains(nome));

            if (!string.IsNullOrWhiteSpace(email))
                query = query.Where(u => u.Email.Contains(email));

            if (!string.IsNullOrWhiteSpace(admin))
                query = query.Where(u => u.Adm == (admin == "1"));

            if (!string.IsNullOrWhiteSpace(cadastroPendente))
                query = query.Where(u => u.CadastroPendente == (cadastroPendente == "1"));

            if (!string.IsNullOrWhiteSpace(ativo))
                query = query.Where(u => u.Ativo == (ativo == "1"));

            if (!string.IsNullOrWhiteSpace(excluido))
                query = query.Where(u => u.Excluido == (excluido == "1"));
            else
                query = query.Where(u => u.Excluido == false);

            if (page > 0)
                query = query.Skip((page - 1) * count);
            if (count > 0)
                query.Take(count);

            foreach (var usuario in query)
                yield return LimparUsuario(usuario);
        }

        public async Task<Usuario> AdicionarUsuario(string email, string nome = null)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentNullException(nameof(email));

            if (ExisteEmail(email))
                throw new EmailExistenteException();

            var usuario = new Usuario
            {
                Ativo = true,
                Excluido = false,
                CadastroPendente = true,
                Email = email,
                DtUltimaAlteracao = DateTime.UtcNow,
                HashAlterarSenha = Hash(DateTime.Now.ToString()),
                Nome = nome
            };
            _set.Add(usuario);
            await SalvarMudancas();
            return LimparUsuario(BuscarPorEmail(email));
        }

        public async Task<string> SolicitarNovaSenha(string email)
        {
            var usuario = BuscarPorEmail(email);
            var hash = Hash(DateTime.Now.ToString());
            if (usuario?.Excluido == false && usuario?.Ativo == true)
            {
                usuario.DtSolicitacaoAlteracaoSenha = DateTime.UtcNow;
                usuario.HashAlterarSenha = hash;
                await SalvarMudancas();
                return hash;
            }
            return null;
        }

        public async Task AlterarSenha(string hash, string nome, string novaSenha)
        {
            var usuario = await BuscarPorHash(hash);
            if (usuario == null)
                throw new Exception();
            usuario = await BuscarPorId(usuario.IDUsuario);
            usuario.Nome = nome;
            usuario.Senha = EnhancedHashPassword(novaSenha, workFactor);
            usuario.HashAlterarSenha = null;
            usuario.DtSolicitacaoAlteracaoSenha = null;
            usuario.CadastroPendente = false;
            await SalvarMudancas();
        }

        public async Task AlterarCadastro(int idUsuario, string nome = null, bool? ativo = null, bool? adm = null)
        {
            var usuario = await BuscarPorId(idUsuario);

            if (usuario == null)
                return;

            if (!string.IsNullOrWhiteSpace(nome))
            {
                usuario.Nome = nome;
            }

            if (adm != null)
            {
                usuario.Adm = adm.Value;
            }

            if (ativo != null)
            {
                usuario.Ativo = ativo.Value;
            }

            await SalvarMudancas();
        }

        public async Task ExcluirUsuario(int idUsuario)
        {
            var usuario = await BuscarPorId(idUsuario);
            if (usuario == null)
                throw new ArgumentOutOfRangeException(nameof(idUsuario));
            usuario.Excluido = true;
            await SalvarMudancas();
        }

        private Usuario LimparUsuario(Usuario usuario)
        {
            usuario.DtSolicitacaoAlteracaoSenha = null;
            usuario.Senha = null;

            return UnProxy(usuario);
        }

        public async Task EnviarEmailCadastro(string email)
        {
            await Task.Run(async () =>
            {
                var usuario = BuscarPorEmail(email);
                if (usuario?.CadastroPendente == true)
                {
                    var hash = Hash(DateTime.Now.ToString());
                    usuario.HashAlterarSenha = hash;
                    usuario.DtUltimaAlteracao = DateTime.UtcNow;
                    await SalvarMudancas();
                    EmailServices.EnviarUsuario(ETipoEmailUsuario.NovoCadastro, usuario);
                }
            });
        }

        public void EnviarEmailNovaSenha(string email)
        {
            var usuario = BuscarPorEmail(email);
            if (usuario != null)
                EmailServices.EnviarUsuario(ETipoEmailUsuario.EsqueciASenha, usuario);
        }

        private static string Hash(string input)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }
    }
}