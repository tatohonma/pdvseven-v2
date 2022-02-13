using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace a7D.PDV.Integracao.API2.Client
{
    public static class UDPClient
    {
        public static IPEndPoint ServerIP { get; private set; }

        public static string Send(string dado)
        {
            try
            {
                UdpClient client = new UdpClient();
                IPEndPoint ip = new IPEndPoint(IPAddress.Broadcast, 15000);
                byte[] bytes = Encoding.ASCII.GetBytes("PDV7:" + dado);
                client.Send(bytes, bytes.Length, ip);

                // Aguarda até 3 segundos!
                client.Client.ReceiveTimeout = 3000;
                bytes = client.Receive(ref ip);
                ServerIP = ip;

                client.Close();

                string message = Encoding.ASCII.GetString(bytes);
                return message;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}