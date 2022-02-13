using a7D.PDV.BLL;
using a7D.PDV.Integracao.Servico.Core;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace a7D.PDV.Integracao.Server
{
    public class UDPServer : IDisposable
    {
        internal const int Port = 15000;

        private readonly UdpClient udp = new UdpClient(Port);

        public string SQLServer { get; set; } = "(local)";
        public string SQLInstance { get; set; } = "PDV7";
        public string SQLCatalog { get; set; } = "PDV7";
        public string WS2Server { get; set; } = "localhost:7777";
        public string Update => $"http://{WS2Server}/release/update.zip";

        public event OnMensagemListener AddLog;

        public void StartListening()
        {
            try
            {
                AddLog($"UDP Start");
                udp.BeginReceive(Receive, new object());
            }
            catch (Exception ex)
            {
                Logs.Erro(CodigoErro.AA00, ex);
            }
        }

        private void Receive(IAsyncResult ar)
        {
            try
            {
                var ip = new IPEndPoint(IPAddress.Any, 15000);
                var bytes = udp.EndReceive(ar, ref ip);
                string message = Encoding.ASCII.GetString(bytes);
                AddLog($"{ip.ToString()} RECV {message}");

                if (message.StartsWith("PDV7:"))
                {
                    // OK!
                    var parametro = message.Substring(5);
                    if (parametro == "SQL")
                    {
                        AddLog("SEND SQL " + SQLServer);
                        bytes = Encoding.ASCII.GetBytes(SQLServer);
                        udp.Send(bytes, bytes.Length, ip);
                    }
                    else if (parametro == "SQLDB")
                    {
                        AddLog("SEND SQLDB " + SQLServer);
                        bytes = Encoding.ASCII.GetBytes($@"{SQLServer}|{SQLInstance}|{SQLCatalog}");
                        udp.Send(bytes, bytes.Length, ip);
                    }
                    else if (parametro == "WS2")
                    {
                        AddLog("SEND WS2 " + WS2Server);
                        bytes = Encoding.ASCII.GetBytes(WS2Server);
                        udp.Send(bytes, bytes.Length, ip);
                    }
                    else if (parametro.StartsWith("UPDATE"))
                    {
                        var verCliente = new Version(parametro.Substring(6)); // Versão do cliente
                        var verBLL = new Version(AC.Versao); // Versão do servidor
                        if (verBLL > verCliente)
                        {
                            if (Atualizacao.TemUpdate)
                            {
                                AddLog("SEND UPDATE " + Update);
                                bytes = Encoding.ASCII.GetBytes(Update);
                                udp.Send(bytes, bytes.Length, ip);
                            }
                            else
                            {
                                AddLog("Sem atualização disponivel!!!");
                                bytes = Encoding.ASCII.GetBytes("Indiponivel");
                                udp.Send(bytes, bytes.Length, ip);
                            }
                        }
                        else
                        {
                            AddLog("Versão Válida: " + verBLL + "<=" + verCliente);
                            bytes = Encoding.ASCII.GetBytes("OK");
                            udp.Send(bytes, bytes.Length, ip);
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            StartListening();
        }

        public void Dispose()
        {
            udp?.Close();
        }
    }
}
