using DTO.CategoryDTO;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ManagerClient.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        
        public DeleteModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
           
        }

        public async Task<IActionResult> OnGet()
        {
            int idp = Convert.ToInt32(Request.Query["idp"].ToString());
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"Product/Detail/{idp}");
            string strData = await response.Content.ReadAsStringAsync();
            ProductListDTO product = System.Text.Json.JsonSerializer.Deserialize<ProductListDTO>(strData, option);

                await client.DeleteAsync(ApiUri + "Product/Delete/" + idp);
            
            return RedirectToPage("Index");
        }
    }
}
