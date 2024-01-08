using DTO.AccountDTO;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SstoreClient.Pages
{
    public class ProductDetailModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        private string loginKey = "_login";
        private string productIdKey = "_productid";
        public string message { get;set; }
        public ProductListDTO product { get; set; }

        public ProductDetailModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
        }
        public async Task<IActionResult> OnGet()
        {
            try
            {
                int productid = Convert.ToInt32(Request.Query["idp"].ToString());
                HttpContext.Session.SetString(productIdKey, Request.Query["idp"].ToString());
                HttpResponseMessage response = await client.GetAsync(ApiUri + $"Product/Detail/{productid}");
                string strData = await response.Content.ReadAsStringAsync();
                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                product = System.Text.Json.JsonSerializer.Deserialize<ProductListDTO>(strData, option);

            }
            catch(Exception ex)
            {
                return RedirectToPage("Index");
            }
           
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            string json = HttpContext.Session.GetString(loginKey) ?? "";
            int productid = Convert.ToInt32(HttpContext.Session.GetString(productIdKey));
            int quantity = Convert.ToInt32(Request.Form["quantity"]);

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"Product/Detail/{productid}");
            string strData = await response.Content.ReadAsStringAsync();
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            product = System.Text.Json.JsonSerializer.Deserialize<ProductListDTO>(strData, option);

            if (json.Equals(""))
            {
                return RedirectToPage("Login");
            }
            if (quantity > product.Quantity)
            {
                message = "Quantity very big!";
                return Page();
            }

            AccountDTO account = JsonConvert.DeserializeObject<AccountDTO>(json);
            //add to cart
            await client.GetAsync(ApiUri + $"Shoping/addtocart/{account.Id}&{product.Id}&{quantity}");
            message = "Add to cart complete!";
            return Page();
        }
    }
}
