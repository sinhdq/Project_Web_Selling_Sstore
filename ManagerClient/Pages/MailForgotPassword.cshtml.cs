using CommonLib;
using DTO.AccountDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ManagerClient.Pages
{
    public class MailForgotPasswordModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        private string keyemail = "_mail";
        public string message { get; set; }

        public MailForgotPasswordModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
        }
        public async Task<IActionResult> OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            if (Request.Form["email"].Equals(""))
            {
                message = "Enter mail forgot password";
                return Page();
            }
            if (!Validate.Instance.Mail(Request.Form["email"]))
            {
                message = "Email is validate";
                return Page();
            }
            string mail = Request.Form["email"];

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"Account/detailmail/{mail}");
            string strData = await response.Content.ReadAsStringAsync();

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            AccountDTO account = System.Text.Json.JsonSerializer.Deserialize<AccountDTO>(strData, option);

            if (account.Id == 0)
            {
                message = "Email does not exist";
                return Page();
            }
            HttpContext.Session.SetString(keyemail, mail);

            return RedirectToPage("ForgotPassword");
        }
    }
}
