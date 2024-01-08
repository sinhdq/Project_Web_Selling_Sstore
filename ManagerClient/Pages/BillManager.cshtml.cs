using DTO.AccountDTO;
using DTO.BillDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ManagerClient.Pages
{
    public class BillManagerModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        private string loginKey = "_login";
        public List<BillManagerDTO> list { get; set; }
        private static string customername = "";
        public string message { get;set; }
        private string keyBill = "_bill";

        public BillManagerModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
        }

        public async Task<IActionResult> OnGet()
        {
            string json = HttpContext.Session.GetString(loginKey) ?? "";
            string service = "";

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (json.Equals(""))
            {
                return RedirectToPage("Login");
            }

            
            if (!Request.Query["sstore"].ToString().Equals(""))
            {
                service = Request.Query["sstore"].ToString();
            }

            if (service.Equals("startship"))
            {
                int id = Convert.ToInt32(Request.Query["bid"].ToString());

                HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"Bill/detail/{id}");
                string strData2 = await response2.Content.ReadAsStringAsync();
                BillDTO billdto = System.Text.Json.JsonSerializer.Deserialize<BillDTO>(strData2, option);

                billdto.ShippingDate = DateTime.Now;

                string billjson = System.Text.Json.JsonSerializer.Serialize(billdto);
                StringContent content = new StringContent(billjson, Encoding.UTF8, "application/json");

                await client.PutAsync(ApiUri + $"Bill/put", content);
                message = "Start shiping!";
            }

            if (service.Equals("detailorder"))
            {
                HttpContext.Session.SetString(keyBill, Request.Query["bid"].ToString());
                return RedirectToPage("BillDetail");
            }

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"Bill/manager/list/null");
            string strData = await response.Content.ReadAsStringAsync();
            list = System.Text.Json.JsonSerializer.Deserialize<List<BillManagerDTO>>(strData, option);

            return Page();
        }

        public async Task<IActionResult> OnPostFillter()
        {
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            string name = Request.Form["fname"];

            if(name.Equals(""))
            {
                name = "null";
            }

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"Bill/manager/list/{name}");
            string strData = await response.Content.ReadAsStringAsync();
            list = System.Text.Json.JsonSerializer.Deserialize<List<BillManagerDTO>>(strData, option);

            return Page();
        }


    }
}
