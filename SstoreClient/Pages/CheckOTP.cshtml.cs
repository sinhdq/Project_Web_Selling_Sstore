using DTO.AccountDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace SstoreClient.Pages
{
    public class CheckOTPModel : PageModel
    {
        private string jarKey = "jar";
        private string otpkey = "otp";
        public string message { get; set; }
        private readonly HttpClient client = null;
        private string ApiUri = "";

        public CheckOTPModel(){
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
        }
        public async Task<IActionResult> OnGet()
        {
            message = "Check your email register";
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            string jar = HttpContext.Session.GetString(jarKey) ?? "";
            string otp = HttpContext.Session.GetString(otpkey) ?? "";
            string getotp = Request.Form["otp"];

            if (string.IsNullOrEmpty(jar) || string.IsNullOrEmpty(otpkey))
            {
                return RedirectToPage("Login");
            }

            AccountAddDTO account = JsonConvert.DeserializeObject<AccountAddDTO>(jar);

            if (account.FullName.Equals(""))
            {
                message = "Has 1 error in processing, try again!";
            }

            if (string.IsNullOrEmpty(getotp))
            {
                message = "Enter your otp!";
                return Page();
            }

            if (!otp.Equals(getotp)) {

                message = "OTP failded";
                return Page();
            }

            account.Active = true;

            string json = System.Text.Json.JsonSerializer.Serialize(account);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PostAsync(ApiUri + "Account", content);

            HttpContext.Session.Clear();
            return RedirectToPage("Login");
        }
    }
}
