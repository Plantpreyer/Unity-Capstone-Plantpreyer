using UnityEngine;
using UnityEngine.UI;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public class EmailFactory : MonoBehaviour
{
    public string bodyMessage;
    public string recipientEmail = "unitygamesijingdata@gmail.com";
    public string senderEmail = "unitygamesijingdata@outlook.com";
    private string password = "Unityemail@";

    public void SendEmail()
    {
        Debug.Log("Sending Email");
        MailMessage mail = new MailMessage();
        SmtpClient SmtpServer = new SmtpClient("smtp-mail.outlook.com");
        SmtpServer.Timeout = 10000;
        SmtpServer.DeliveryMethod = SmtpDeliveryMethod.Network;
        SmtpServer.UseDefaultCredentials = false;
        SmtpServer.Port = 587;

        mail.From = new MailAddress(senderEmail);
        mail.To.Add(new MailAddress(recipientEmail));
        
        mail.Subject = "Test Email through C Sharp App";
        mail.Body = bodyMessage;
        

        SmtpServer.Credentials = new System.Net.NetworkCredential(senderEmail, password) as ICredentialsByHost; SmtpServer.EnableSsl = true;
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };

        mail.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
        SmtpServer.Send(mail);
    }

    public void Awake(){
        SendEmail();
    }
}