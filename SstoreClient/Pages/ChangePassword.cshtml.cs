using CommonLib;
using DTO.AccountDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SstoreClient.Pages
{
    public class ChangePasswordModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        public string message { get; set; }
        private string loginKey = "_login";
        private string keyotp = "otp";
        private string keyservice = "_service";
        private string keyemail = "_mail";

        public ChangePasswordModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
        }
        public async Task<IActionResult> OnGet()
        {
            string json = HttpContext.Session.GetString(loginKey) ?? "";

            if (json.Equals(""))
            {
                return RedirectToPage("Login");
            }

            HttpContext.Session.SetString(keyservice,"changepassword");
            AccountDTO account = JsonConvert.DeserializeObject<AccountDTO>(json);
            HttpContext.Session.SetString(keyemail, account.Email);
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            string json = HttpContext.Session.GetString(loginKey) ?? "";
            string otp = HttpContext.Session.GetString(keyotp) ?? "";

            if (otp.Equals(""))
            {
                message = "Please click button GET OTP";
                return Page();
            }
            if (json.Equals(""))
            {
                return RedirectToPage("Login");
            }

            if (Request.Form["oldpassword"].Equals("") || Request.Form["newpassword"].Equals("") ||
                Request.Form["repassword"].Equals("") || Request.Form["otp"].Equals(""))
            {
                message = "Infomation must not null";
                return Page();
            }

            if (!otp.Equals(Request.Form["otp"]))
            {
                message = "Otp faided";
                return Page();
            }
            AccountDTO account = JsonConvert.DeserializeObject<AccountDTO>(json);
            
            HttpResponseMessage response = await client.GetAsync(ApiUri + $"Account/detail/{account.Id}");
            string strData = await response.Content.ReadAsStringAsync();

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            AccountPofileDTO profile = System.Text.Json.JsonSerializer.Deserialize<AccountPofileDTO>(strData, option);

            profile.Password = Encryption.Instance.hashMD5(Request.Form["newpassword"]);

            json = System.Text.Json.JsonSerializer.Serialize(profile);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PutAsync(ApiUri + $"Account/put/{profile.Id}", content);
            HttpContext.Session.Clear();
            

            return RedirectToPage("Login");
        }

    }
}
