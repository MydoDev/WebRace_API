using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity.UI.Services;



namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Emails : ControllerBase
    {

     
        private readonly ILogger<Emails> _logger;

        public Emails(ILogger<Emails> logger)
        {
           _logger = logger;
        }

        [HttpPut]
        [Route("CreateNewCode")]
        public void CreateNewCode([FromBody]Sender s)
        {
            
            
                s.code = MyMethods.generateCode();
                s.date = DateTime.Now.AddMinutes(5);
                MyMethods.createRecord(s.email, s.code, s.date);
                MyMethods.sendCode(s.email, s.code);
            
            

        }
        [HttpPost]
        [Route("CheckCode")]
        public bool CheckCode([FromBody] Sender s)
        {
            if (MyMethods.checkExistAndValid(s.email, s.code))
            {
                //MyMethods.updateVertificationEmail(s.email);
                var smtpClient = new SmtpClient("smtp.gmail.com")
                {
                    Port = 587,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("wapit.cz@gmail.com", "pyisndvleqssvfjc"),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    EnableSsl = true,
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("wapit.cz@gmail.com"),
                    Subject = "Ovìøení bylo úspìšné",
                    Body = "Váš email byl úspìšnì ovìøen :)",
                    IsBodyHtml = true,
                };
                mailMessage.To.Add(s.email);
                smtpClient.Send(mailMessage);

            }
            return MyMethods.checkExistAndValid(s.email, s.code);
        }


    }
}
