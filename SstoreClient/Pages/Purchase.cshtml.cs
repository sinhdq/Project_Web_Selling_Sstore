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
    public class PurchaseModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        private string loginKey = "_login";
        public AccountDTO account { get; set; }
        public List<BillDTO> Listbill { get; set; }

        public PurchaseModel()
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


            string service = "";
            if (!Request.Query["sstore"].ToString().Equals(""))
            {
                service = Request.Query["sstore"].ToString();
            }

            if (service.Equals("Confirm"))
            {
                int bid = Convert.ToInt32(Request.Query["bid"].ToString());

                HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"Bill/detail/{bid}");
                string strData2 = await response2.Content.ReadAsStringAsync();
                BillDTO billdto = System.Text.Json.JsonSerializer.Deserialize<BillDTO>(strData2, option);

                billdto.Status = true;

                string billjson = System.Text.Json.JsonSerializer.Serialize(billdto);
                StringContent content = new StringContent(billjson, Encoding.UTF8, "application/json");

                await client.PutAsync(ApiUri + $"Bill/put", content);

            }

            account = JsonConvert.DeserializeObject<AccountDTO>(json);

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"Bill/customer/{account.Id}");
            string strData = await response.Content.ReadAsStringAsync();
            Listbill = System.Text.Json.JsonSerializer.Deserialize<List<BillDTO>>(strData, option);

            return Page();
        }
    }
}
