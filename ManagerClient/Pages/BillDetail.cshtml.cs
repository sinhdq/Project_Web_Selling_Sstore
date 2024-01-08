using DTO.CartDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ManagerClient.Pages
{
    public class BillDetailModel : PageModel
    {
        public List<CartDTO> Carts { get; set; }
        public string TotalPrice { get; set; }
        public string message { get; set; }
        private readonly HttpClient client = null;
        private string ApiUri = "";
        private string loginKey = "_login";
        private string keyBill = "_bill";

        public BillDetailModel()
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

            int billid = Convert.ToInt32(HttpContext.Session.GetString(keyBill));

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"BillItem/BillDetail/{billid}");
            string strData = await response.Content.ReadAsStringAsync();
            Carts = System.Text.Json.JsonSerializer.Deserialize<List<CartDTO>>(strData, option);

            return Page();
        }
    }
}
