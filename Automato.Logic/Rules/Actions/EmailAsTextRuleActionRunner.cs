using Automato.Model.Rules.Actions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Automato.Logic.Rules.Actions
{
    /// <summary>
    /// Execute a rule action that sends a text via email
    /// </summary>
    public class EmailAsTextRuleActionRunner : BaseRuleActionRunner<EmailAsTextRuleAction>
    {
        protected override async Task ExecuteActionAsync(EmailAsTextRuleAction action)
        {
            // TODO: GetUserById
            var users = new UserStore().GetAll();
            var user = users.FirstOrDefault(u => u.Id == action.UserId);

            if (user == null)
            {
                throw new ArgumentException(action.UserId);
            }

            var smtpHost = ConfigurationManager.AppSettings["SMTP.Host"];
            var port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTP.Port"]);
            var from = ConfigurationManager.AppSettings["SMTP.FromAddress"];
            var pwd = ConfigurationManager.AppSettings["SMTP.FromPassword"];

            // Send email via google smtp, requires a gmail account as the sender.
            var smtp = new SmtpClient
            {
                Host = smtpHost,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(from, pwd),
                Timeout = 3000
            };

            var msg = new MailMessage(from, user.TextAddress);
            msg.Body = action.Message;

            await smtp.SendMailAsync(msg);
        }
    }
}
