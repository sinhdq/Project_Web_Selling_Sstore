using DTO.AccountDTO;
using DTO.CategoryDTO;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SstoreClient.ModelTMP;
using System.Collections;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SstoreClient.Pages
{
    public class ShopModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        public string message { get; set; }
        public List<CategoryDTO> listcategory { get; set; }
        public List<ProductListDTO> productlist { get; set; }
        private static string category = "";
        private static string fproductname = "";
        private string loginKey = "_login";
        private string cartKey = "_cart";

        public ShopModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
        }

        public async Task<IActionResult> OnGet()
        {
             string service = "";
             category = Request.Query["categoryid"].ToString();

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (!Request.Query["sstore"].ToString().Equals(""))
            {
                service = Request.Query["sstore"].ToString();
            }

            if (category.Equals(""))
            {
                category = "null";
            }

            if (fproductname.Equals(""))
            {
                fproductname = "null";
            }

            if (service.Equals("AddToCart"))
            {

                string json = HttpContext.Session.GetString(loginKey) ?? "";
                int productid = Convert.ToInt32(Request.Query["productid"]);
                int quantity = Convert.ToInt32(Request.Query["quantity"]);

                //case user login page
                if (!json.Equals(""))
                {
                    AccountDTO account = JsonConvert.DeserializeObject<AccountDTO>(json);
                    //add to cart
                    await client.GetAsync(ApiUri + $"Shoping/addtocart/{account.Id}&{productid}&{quantity}");
                }
                else
                {
                    return RedirectToPage("Login");
                }
                
                
            }

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"Product/list/{category}&null");
            HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"Category/list/null");
            string strData = await response.Content.ReadAsStringAsync();
            string strData2 = await response2.Content.ReadAsStringAsync();

            listcategory = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strData2, option);
            productlist = System.Text.Json.JsonSerializer.Deserialize<List<ProductListDTO>>(strData, option);
            return Page();
        }

        public async Task<IActionResult> OnPostFillter()
        {
             fproductname = Request.Form["nameproduct"];

            if (fproductname.Equals(""))
            {
                fproductname = "null";
            }

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"Product/list/{category}&{fproductname}");
            HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"Category/list/null");
            string strData = await response.Content.ReadAsStringAsync();
            string strData2 = await response2.Content.ReadAsStringAsync();

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            listcategory = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strData2, option);
            productlist = System.Text.Json.JsonSerializer.Deserialize<List<ProductListDTO>>(strData, option);

            return Page();
        }


       
    }
}
