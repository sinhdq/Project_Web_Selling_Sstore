using CommonLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ManagerClient.Pages
{
    public class GetOTPModel : PageModel
    {
        private string keyotp = "otp";
        private string keyservice = "_service";
        private string keyemail = "_mail";

        public async Task<IActionResult> OnGet()
        {
            string service = HttpContext.Session.GetString(keyservice) ?? "";
            string email = HttpContext.Session.GetString(keyemail) ?? "";

            if (service.Equals(""))
            {
                return RedirectToPage("Index");
            }
            //get otp change password
            if (service.Equals("changepassword"))
            {
                if (email.Equals(""))
                {
                    return RedirectToPage("ChangePassword");
                }
                string otp = SendMail.Instance.GetOTP();
                SendMail.Instance.Send(email, otp, "SStore Change Password");
                HttpContext.Session.SetString(keyotp, otp);
                return RedirectToPage("ChangePassword");
            }
            //get otp forgot password
            if (service.Equals("forgotpassword"))
            {
                if (email.Equals(""))
                {
                    return RedirectToPage("ForgotPassword");
                }

                string otp = SendMail.Instance.GetOTP();
                SendMail.Instance.Send(email, otp, "SStore Forgot Password");
                HttpContext.Session.SetString(keyotp, otp);

                return RedirectToPage("ForgotPassword");
            }

            return Page();
        }
    }
}
