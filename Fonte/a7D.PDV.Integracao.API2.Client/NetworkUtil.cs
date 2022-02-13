using System.Collections.Generic;
using System.Net.NetworkInformation;

namespace a7D.PDV.Integracao.API2.Client
{
    public static class NetworUtil
    {
        public static string[] GetAllIP(out string log)
        {
            log = "";
            var ips = new List<string>();
            foreach (NetworkInterface netInterface in NetworkInterface.GetAllNetworkInterfaces())
            {
                var ipProps = netInterface.GetIPProperties();
                foreach (UnicastIPAddressInformation addr in ipProps.UnicastAddresses)
                {
                    bool lAdd = false;
                    if (addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        string ip = addr.Address.ToString();
                        if (!ip.StartsWith("169.") && !ip.StartsWith("127."))
                        {
                            if (!lAdd)
                            {
                                if (!string.IsNullOrEmpty(log))
                                    log += "\r\n";

                                lAdd = true;
                                log += $"\t{netInterface.Description} ({netInterface.OperationalStatus}) {netInterface.Name}: ";
                            }
                            log += ip.ToString() + " ";
                            ips.Add(ip);
                        }
                    }
                }
            }

            return ips.ToArray();
        }
    }
}
