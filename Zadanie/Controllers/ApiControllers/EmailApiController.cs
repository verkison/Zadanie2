using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Http;
using Models.ApiModels;

namespace Zadanie.Controllers.ApiControllers
{
    public class EmailApiController : ApiController
    {
        public async Task<IHttpActionResult> SendEmail(EmailToSend model)
        {
            using (MailMessage mail = new MailMessage())
            {
                mail.From = new MailAddress("testmail.eg2137@gmail.com");
                mail.To.Add(model.EmailReceiver);
                mail.Subject = model.EmailSubject;
                mail.Body = model.EmailMessage;
                mail.IsBodyHtml = false;

                using(SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.Credentials = new NetworkCredential("testmail.eg2137@gmail.com", "Qweasd123!");
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(mail);
                }
            }

            return Ok();
        }
    }
}
