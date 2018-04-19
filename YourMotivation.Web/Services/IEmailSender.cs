using System.Threading.Tasks;

namespace YourMotivation.Web.Services
{
  public interface IEmailSender
  {
    Task<bool> SendEmailAsync(string email, string subject, string message);
  }
}
