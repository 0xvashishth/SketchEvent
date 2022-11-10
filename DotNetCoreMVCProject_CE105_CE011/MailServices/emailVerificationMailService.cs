using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System;
/*using System.Configuration;*/
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EventoWeb.MailServices
{
    public class emailVerificationMailService
    {
        private string _emailto;

        public emailVerificationMailService(string twilio_code, string twilio_username, string email)
        {
            _emailto=email;
            Execute(twilio_code, twilio_username).Wait();
        }

        public async Task Execute(string twilio_code, string twilio_username)
        {
            var _config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
           /* var IntExample = MyConfig.GetValue<int>("AppSettings:SampleIntValue");
            var AppName = MyConfig.GetValue<string>("AppSettings:APP_Name");*/

            var apiKey = _config.GetValue<string>("ConnectionStrings:apiString");
            var message = new SendGridMessage();
            message.Personalizations = new List<Personalization>(){
            new Personalization(){
                Tos = new List<EmailAddress>(){
                    /*new EmailAddress(){
                        Email = "hi@vashishth.co",
                        Name = "Vashishth Patel"
                    },*/
                    new EmailAddress(){
                        Email = _emailto,
                        /*Name = "Vashishth Patel"*/
                    }
                },
                /*Ccs = new List<EmailAddress>(){
                    new EmailAddress(){
                        Email = "jane_doe@example.com",
                        Name = "Jane Doe"
                    }
                },
                Bccs = new List<EmailAddress>(){
                    new EmailAddress(){
                        Email = "james_doe@example.com",
                        Name = "Jim Doe"
                    }
                }*/
            }
        };
            message.TemplateId = _config.GetValue<string>("ConnectionStrings:emailVerificationTemplateId");

            dynamicTemplateData dynamictemplatedata = new dynamicTemplateData(twilio_username, twilio_code);
            message.SetTemplateData(dynamictemplatedata);
            message.From = new EmailAddress()
            {
                Email = _config.GetValue<string>("ConnectionStrings:fromEmail"),
                Name = _config.GetValue<string>("ConnectionStrings:nameEmail")
            };

            var client = new SendGridClient(apiKey);
            var response = await client.SendEmailAsync(message).ConfigureAwait(false);

            Console.WriteLine(response.StatusCode);
            Console.WriteLine(response.Body.ReadAsStringAsync().Result);
            Console.WriteLine(response.Headers.ToString());
        }
    }

    public class dynamicTemplateData
    {
        public string? twilio_username;
        public string? twilio_code;
        public dynamicTemplateData(string _twilio_username, string _twilio_code)
        {
            twilio_username = _twilio_username;
            twilio_code = _twilio_code;
        }
    }
}
