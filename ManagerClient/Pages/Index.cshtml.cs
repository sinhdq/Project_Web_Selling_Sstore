using DTO.AccountDTO;
using DTO.CategoryDTO;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc; 
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ManagerClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        private string loginKey = "_login";
        public List<CategoryDTO> listcategory { get; set; }
        public List<ProductListDTO> productlist { get; set; }
        private static string category = "";
        private static string fproductname = "";

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
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
            
            category = Request.Query["categoryid"].ToString();

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

           
            if (category.Equals(""))
            {
                category = "null";
            }

            if (fproductname.Equals(""))
            {
                fproductname = "null";
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