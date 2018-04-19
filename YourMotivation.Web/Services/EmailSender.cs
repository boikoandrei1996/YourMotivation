using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace YourMotivation.Web.Services
{
  // This class is used by the application to send email for account confirmation and password reset.
  // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
  public class EmailSender : IEmailSender
  {
    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
    {
      Options = optionsAccessor.Value;
    }

    public AuthMessageSenderOptions Options { get; }

    public async Task<bool> SendEmailAsync(string email, string subject, string message)
    {
      var response = await this.ExecuteAsync(Options.SendGridKey, email, subject, message);

      if (response.StatusCode == System.Net.HttpStatusCode.OK || 
        response.StatusCode == System.Net.HttpStatusCode.Accepted)
      {
        return true;
      }
      else
      {
        return false;
      }
    }

    public Task<Response> ExecuteAsync(string apiKey, string email, string subject, string message)
    {
      var client = new SendGridClient(apiKey);

      var msg = new SendGridMessage()
      {
        From = new EmailAddress("BoikoAndrei1996@gmail.com", "Andrei Boika"),
        Subject = subject,
        PlainTextContent = message,
        HtmlContent = message
      };

      msg.AddTo(new EmailAddress(email));

      return client.SendEmailAsync(msg);
    }
  }
}
