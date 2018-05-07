using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Localization;

namespace YourMotivation.Web.Services
{
  public static class EmailSenderExtensions
  {
    public static Task<bool> SendEmailConfirmationAsync(
      this IEmailSender emailSender,
      IHtmlLocalizer localizer, 
      string email,
      string link)
    {
      return emailSender.SendEmailAsync(
        email,
        localizer["Confirm your email"].Value,
        localizer.GetString("Please confirm your account by clicking this link: <a href='{0}'>link</a>", HtmlEncoder.Default.Encode(link)));
    }

    public static Task<bool> SendEmailResetPasswordAsync(
      this IEmailSender emailSender,
      IHtmlLocalizer localizer,
      string email, 
      string link)
    {
      return emailSender.SendEmailAsync(
        email,
        localizer["Reset Password"].Value,
        localizer.GetString("Please reset your password by clicking here: <a href='{0}'>link</a>", HtmlEncoder.Default.Encode(link)));
    }
  }
}
