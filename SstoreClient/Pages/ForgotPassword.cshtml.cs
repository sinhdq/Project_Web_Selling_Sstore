using CommonLib;
using DTO.AccountDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SstoreClient.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        public string message { get; set; }
        private string keyotp = "otp";
        private string keyservice = "_service";
        private string keyemail = "_mail";

        public ForgotPasswordModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
        }
        public async Task<IActionResult> OnGet()
        {
            HttpContext.Session.SetString(keyservice, "forgotpassword");

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            string otp = HttpContext.Session.GetString(keyotp) ?? "";

            if (otp.Equals(""))
            {
                message = "Please, get otp";
                return Page();
            }
            if (Request.Form["newpassword"].Equals("") ||
                Request.Form["repassword"].Equals("") || Request.Form["otp"].Equals(""))
            {
                message = "Infomation must not null";
                return Page();
            }

            if (!otp.Equals(Request.Form["otp"]))
            {
                message = "Otp failded";
                return Page();
            }

            string email = HttpContext.Session.GetString(keyemail) ?? "";
           
            //respone get account by email
            HttpResponseMessage response = await client.GetAsync(ApiUri + $"Account/detailmail/{email}");
            string strData = await response.Content.ReadAsStringAsync();

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            AccountDTO account = System.Text.Json.JsonSerializer.Deserialize<AccountDTO>(strData, option);
            //respone get account by id
            HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"Account/detail/{account.Id}");
            string strData2 = await response2.Content.ReadAsStringAsync();

            AccountPofileDTO profile = System.Text.Json.JsonSerializer.Deserialize<AccountPofileDTO>(strData2, option);

            profile.Password = Encryption.Instance.hashMD5(Request.Form["newpassword"]);

            string json = System.Text.Json.JsonSerializer.Serialize(profile);
            StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
            await client.PutAsync(ApiUri + $"Account/put/{profile.Id}", content);
            HttpContext.Session.Clear();

            return RedirectToPage("Login");
        }

        
    }
}
