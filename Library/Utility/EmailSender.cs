using Mailjet.Client;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace Library.Utility
{
    public class EmailSender : IEmailSender
    {
        Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Execute(email, subject, htmlMessage);
        }
        public async Task Execute(string email, string subject, string body)
        {
            MailjetClient client = new MailjetClient(Environment.GetEnvironmentVariable("9dfdb7cd7aa4ae3a5eb8039b92a4e6c9"), Environment.GetEnvironmentVariable("8ed0dc5cb06467aa8516507539a7bb54"))
            {
              //version
            };
            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
             .Property(Send.Messages, new JArray {
     new JObject {
      {
       "From",
       new JObject {
        {"Email", "mmr16@protonmail.com"},
        {"Name", "Mostafa"}
       }
      }, {
       "To",
       new JArray {
        new JObject {
         {
          "Email",
         email
         }, {
          "Name",
          "MMR_Library"
         }
        }
       }
      }, {
       "Subject",
       subject
      }, {
       "HTMLPart",body
      }
     }
             });
            await client.PostAsync(request);
        }
    }
}
