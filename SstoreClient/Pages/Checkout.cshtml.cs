using CommonLib;
using DTO.AccountDTO;
using DTO.BillDTO;
using DTO.CartDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SstoreClient.ModelTMP;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SstoreClient.Pages
{
    public class CheckoutModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        private string loginKey = "_login";
        public string message { get; set; }
        public List<CartDTO> Carts { get; set; }
        public string TotalPrice { get; set; }
        public AccountDTO account { get;set; }

        public CheckoutModel()
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

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            account = JsonConvert.DeserializeObject<AccountDTO>(json);

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"BillItem/cart/{account.Id}");
            string strData = await response.Content.ReadAsStringAsync();
            Carts = System.Text.Json.JsonSerializer.Deserialize<List<CartDTO>>(strData, option);

            HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"BillItem/Total/{account.Id}");
            TotalPrice = await response2.Content.ReadAsStringAsync();

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            string json = HttpContext.Session.GetString(loginKey) ?? "";
            if (json.Equals(""))
            {
                return RedirectToPage("Login");
            }

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            account = JsonConvert.DeserializeObject<AccountDTO>(json);

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"BillItem/cart/{account.Id}");
            string strData = await response.Content.ReadAsStringAsync();
            Carts = System.Text.Json.JsonSerializer.Deserialize<List<CartDTO>>(strData, option);

            HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"BillItem/Total/{account.Id}");
            TotalPrice = await response2.Content.ReadAsStringAsync(); 

            if (Request.Form["address"].Equals(""))
            {
                message = "Enter address";
                return Page();
            }

            HttpResponseMessage response3 = await client.GetAsync(ApiUri + $"Bill/detail/nocheckout/{account.Id}");
            string strData3 = await response3.Content.ReadAsStringAsync();
            BillDTO bill = System.Text.Json.JsonSerializer.Deserialize<BillDTO>(strData3, option);

            bill.RequiredDate = Convert.ToDateTime(Validate.Instance.DateNow());

            string billjson = System.Text.Json.JsonSerializer.Serialize(bill);
            StringContent content = new StringContent(billjson, Encoding.UTF8, "application/json");
            await client.PutAsync(ApiUri + $"Bill/put", content);


            return RedirectToPage("Shop");
        }
    }
}
