using CommonLib;
using DTO.AccountDTO;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ManagerClient.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<ProductListDTO> listproduct { get; set; }
        private readonly HttpClient client = null;
        private string ApiUri = "";
        public string message { get; set; }
        private string loginKey = "_login";
        public LoginModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
        }
        public async Task<IActionResult> OnGet()
        {
            //message = "kasfjasbfsjnfksgkrgnkdrgnkABC";
            HttpContext.Session.Clear();
            return Page();
        }

        public async Task<IActionResult> OnPostSignin()
        {

            if (Request.Form["mail"].Equals("") || Request.Form["password"].Equals(""))
            {
                message = "Please, fill all infomations";
                return Page();
            }
            else
            {
                string mail = Encryption.Instance.hashMD5(Request.Form["mail"]);
                string password = Encryption.Instance.hashMD5(Request.Form["password"]);
                HttpResponseMessage response = await client.GetAsync(ApiUri + $"Account/authentication/{mail}&{password}");
                string strData = await response.Content.ReadAsStringAsync();

                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                AccountDTO acc = System.Text.Json.JsonSerializer.Deserialize<AccountDTO>(strData, option);
                if (acc != null && acc.Role == 1 && acc.Active == true)
                {
                    string json = JsonConvert.SerializeObject(acc);
                    HttpContext.Session.SetString(loginKey, json);
                    return RedirectToPage("Index");
                }

                else
                {
                    message = "Login failed";
                    return Page();
                }
            }

        }
    }
}
