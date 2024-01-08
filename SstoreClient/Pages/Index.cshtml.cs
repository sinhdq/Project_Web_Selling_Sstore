using DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SstoreClient.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<ProductListDTO> listproduct { get;set; }
        private readonly HttpClient client = null;
        private string ApiUri = "";
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
            HttpResponseMessage response = await client.GetAsync(ApiUri + "Product/List/null&null");
            string strData = await response.Content.ReadAsStringAsync();

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            //  ViewData["listproduct"] = JsonSerializer.Deserialize<List<ProductListDTO>>(strData, option);
            listproduct = JsonSerializer.Deserialize<List<ProductListDTO>>(strData, option);


            return Page();
        }
    }
}