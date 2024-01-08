using DTO.AccountDTO;
using DTO.CategoryDTO;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ManagerClient.Pages
{
    public class AccountManagerModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        private string loginKey = "_login";
        public List<AccountDTO> accountlist { get; set; }
        private static string category = "";
        private static string accountname = "";

        public AccountManagerModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
        }

        public async Task<IActionResult> OnGet()
        {
            string json = HttpContext.Session.GetString(loginKey) ?? "";
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (json.Equals(""))
            {
                return RedirectToPage("Login");
            }

            string service = "";

            if (!Request.Query["sstore"].ToString().Equals(""))
            {
                service = Request.Query["sstore"].ToString();
            }

            if (service.Equals("disable"))
            {
                int id = Convert.ToInt32(Request.Query["aid"].ToString());

                HttpResponseMessage response = await client.GetAsync(ApiUri + $"Account/GetAccountPut/{id}");
                string strData = await response.Content.ReadAsStringAsync();

                AccountPutDTO accountput = System.Text.Json.JsonSerializer.Deserialize<AccountPutDTO> (strData, option);

                accountput.Active = false;

                string jsonaccount = System.Text.Json.JsonSerializer.Serialize(accountput);
                StringContent content = new StringContent(jsonaccount, Encoding.UTF8, "application/json");

                await client.PutAsync(ApiUri + $"Account/put/{accountput.Id}", content);
            }

            if (service.Equals("enable"))
            {
                int id = Convert.ToInt32(Request.Query["aid"].ToString());

                HttpResponseMessage response = await client.GetAsync(ApiUri + $"Account/GetAccountPut/{id}");
                string strData = await response.Content.ReadAsStringAsync();

                AccountPutDTO accountput = System.Text.Json.JsonSerializer.Deserialize<AccountPutDTO>(strData, option);

                accountput.Active = true;

                string jsonaccount = System.Text.Json.JsonSerializer.Serialize(accountput);
                StringContent content = new StringContent(jsonaccount, Encoding.UTF8, "application/json");

                await client.PutAsync(ApiUri + $"Account/put/{accountput.Id}", content);
            }

            HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"Account/list/null");
            string strData2 = await response2.Content.ReadAsStringAsync();
            accountlist = System.Text.Json.JsonSerializer.Deserialize<List<AccountDTO>>(strData2, option);

            return Page();
        }
        public async Task OnPostFillter()
        {
            accountname = Request.Form["fname"];

            if (accountname.Equals(""))
            {
                accountname = "null";
            }

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            HttpResponseMessage response3 = await client.GetAsync(ApiUri + $"list/{accountname}");
            string strData3 = await response3.Content.ReadAsStringAsync();
            accountlist = System.Text.Json.JsonSerializer.Deserialize<List<AccountDTO>>(strData3, option);


        }
    }
}
