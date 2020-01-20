using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Estrutura.Exceptions;
using System.Configuration;
using System.Net;

namespace Estrutura.Util
{


    public static class Email
    {




        public static void Enviar(string assunto, string corpo, string path, params string[] para )
        {
            Enviar(assunto, null, null, corpo, null, null, null, para);
        }

        public static void Enviar(string assunto, string localTemplate, Dictionary<string, string> valoresSubst, string corpo, Attachment[] atalhos, string path, params string[] para)
        {
            Enviar(assunto, localTemplate, valoresSubst, corpo, atalhos, path, null, para);
        }

        public static async void EnviarAsync(string assunto, string localTemplate, Dictionary<string, string> valoresSubst, string corpo, Attachment[] atalhos, string path, params string[] para)
        {
            Enviar(assunto, localTemplate, valoresSubst, corpo, atalhos, path, null, para);
        }

        public static void Enviar(string assunto, string localTemplate, Dictionary<string, string> valoresSubst, string corpo, Attachment[] atalhos, string path, string nomeRemetente, params string[] para )
        {
            //if (para == null || !para.Any() || !para.Where(p => !string.IsNullOrEmpty(p)).Any())
            //    return;
            
            //string readFile = null;

            //if (!string.IsNullOrEmpty(localTemplate))
            //{
            //    StreamReader reader = new StreamReader(localTemplate);
            //    readFile = reader.ReadToEnd();
            //}

            //if (valoresSubst != null)
            //{
            //    foreach (var item in valoresSubst)
            //        readFile = readFile.Replace(string.Format("{0}", item.Key), item.Value);
            //}

            //readFile += string.Format("<br /> {0}", corpo);

            //MailMessage mail = new MailMessage();

            //AlternateView htmlView = AlternateView.CreateAlternateViewFromString(readFile, null, "text/html");
            //if (path != null)
            //{
            //    LinkedResource imagelink = new LinkedResource(path, "image/png");
            //    imagelink.ContentId = "imageId";
            //    imagelink.TransferEncoding = System.Net.Mime.TransferEncoding.Base64;
            //    htmlView.LinkedResources.Add(imagelink);

            //    mail.AlternateViews.Add(htmlView);
            //}

            //for (int i = 0; i < para.Length; i++)
            //{
            //    if(para[i] != null)
            //        mail.To.Add(para[i]);
            //}

            //if (atalhos != null)
            //    foreach (Attachment atalho in atalhos)
            //        mail.Attachments.Add(atalho);

            //mail.From = new MailAddress(ConfigurationManager.AppSettings["FromEmail"], ConfigurationManager.AppSettings["FromText"]);
            //mail.Subject = assunto;
            //mail.Body = readFile;

            //mail.Priority = MailPriority.High;
            //mail.IsBodyHtml = true;

            //SmtpClient smtp = new SmtpClient("smtp.Estrutura.com.br", 587)
            //{
            //    Credentials = new System.Net.NetworkCredential("naoresponder@Estrutura.com.br", "Trocar@123")
            //};

            ///* if (ConfigurationManager.AppSettings["notifica"] == "S")
            // {
            //     smtp.Send(mail);
            // }*/
            //smtp.Send(mail);
            //smtp.Dispose();



        }
    }
}
