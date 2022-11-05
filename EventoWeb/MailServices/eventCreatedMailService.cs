using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using System;
/*using System.Configuration;*/
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EventoWeb.MailServices
{
    public class eventCreatedMailService
    {
        private string _emailto;
        public eventCreatedMailService(string _user, string _event_name, string _event_url, string _event_end, string _event_start, string _event_venue, string email)
        {
            _emailto=email;
            Execute(_user, _event_name, _event_url, _event_end, _event_start, _event_venue).Wait();
        }

        public async Task Execute(string _user, string _event_name, string _event_url, string _event_end, string _event_start, string _event_venue)
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
            message.TemplateId = _config.GetValue<string>("ConnectionStrings:eventCreatedTemplateId");

            dynamicTemplateDataevent dynamictemplatedata = new dynamicTemplateDataevent(_user, _event_name, _event_url, _event_end, _event_start, _event_venue);
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
    public class dynamicTemplateDataevent
    {
        public string? user;
        public string? event_name;
        public string? event_url;
        public string? event_end;
        public string? event_start;
        public string? event_venue;
        public dynamicTemplateDataevent(string _user, string _event_name, string _event_url, string _event_end, string _event_start, string _event_venue)
        {
            user = _user;
            event_name = _event_name;
            event_url = _event_url;
            event_end = _event_end;
            event_start = _event_start;
            event_venue = _event_venue;
        }
    }
}
