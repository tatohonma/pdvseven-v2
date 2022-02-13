using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace a7D.PDV.Iaago.WebApp.Services
{
    public class Email
    {
        public static string ErroTitle = "ERRO IAAGO";

        public static async Task EnviarTexto(string assunto, string corpo, string emailPara, bool html = false)
        {
            SmtpClient client = new SmtpClient
            {
                Host = "smtp.office365.com",
                Port = 587,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("suporte@pdvseven.com.br", "l7g3x8$@#"),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = true,
            };
            MailMessage mail = new MailMessage("suporte@pdvseven.com.br", emailPara)
            {
                Subject = assunto,
                Body = corpo,
                IsBodyHtml = html,
            };
            await client.SendMailAsync(mail);
        }

        public static async Task EnviaErro(Exception ex)
        {
            try
            {
                string erro = "ERRO: ";
                int max = 5; // para evitar loop em alguns casos muito específico
                while (ex != null && --max > 0)
                {
                    erro += ex.Message + "\r\n";
                    foreach (var key in ex.Data.Keys)
                    {
                        erro += $"\t{key}: {ex.Data[key]}\r\n";
                    }

                    erro += "\r\n" + ex.StackTrace + "\r\n\r\n";
                    ex = ex.InnerException;
                }

                await EnviarTexto(ErroTitle, erro, "ferreira@pdvseven.com.br");
            }
            catch
            {
            }
        }
    }
}
