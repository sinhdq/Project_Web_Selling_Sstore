using DTO.AccountDTO;
using DTO.BillItemDTO;
using DTO.CartDTO;
using DTO.CategoryDTO;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.Json;

namespace SstoreClient.Pages
{
    public class CartModel : PageModel
    {
        public List<CartDTO> Carts { get; set; }
        public string TotalPrice { get;set; }
        public string message { get;set; }
        private readonly HttpClient client = null;
        private string ApiUri = "";
        private string loginKey = "_login";


        public CartModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
            //TotalPrice = 0;


        }

        public async Task<IActionResult> OnGet()
        {
            string service = "";
            string json = HttpContext.Session.GetString(loginKey) ?? "";

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (!Request.Query["sstore"].ToString().Equals(""))
            {
                service = Request.Query["sstore"].ToString();
            }

            if (json.Equals(""))
            {
                return RedirectToPage("Login");
            }

            AccountDTO account = JsonConvert.DeserializeObject<AccountDTO>(json);


            if (service.Equals("dequantityitem"))
            {
                int bid = Convert.ToInt32(Request.Query["idb"].ToString());
                int pid = Convert.ToInt32(Request.Query["idp"].ToString());
                //find bill item
                HttpResponseMessage response3 = await client.GetAsync(ApiUri + $"BillItem/detail/{bid}&{pid}");
                string strData3 = await response3.Content.ReadAsStringAsync();
                BillItemDTO item = System.Text.Json.JsonSerializer.Deserialize<BillItemDTO>(strData3, option);
                //find product
                HttpResponseMessage response4 = await client.GetAsync(ApiUri + $"Product/Detail/{pid}");
                string strData4 = await response4.Content.ReadAsStringAsync();
                ProductListDTO product = System.Text.Json.JsonSerializer.Deserialize<ProductListDTO>(strData4, option);
                //set item
                item.Quantity -= 1;
                item.TotalPrice -= product.Price;
                //put bill item
                string jsonitem = System.Text.Json.JsonSerializer.Serialize(item);
                StringContent content = new StringContent(jsonitem, Encoding.UTF8, "application/json");
                await client.PutAsync(ApiUri + $"BillItem/put", content);
            }

            if (service.Equals("removeitem"))
            {
                int bid = Convert.ToInt32(Request.Query["idb"].ToString());
                int pid = Convert.ToInt32(Request.Query["idp"].ToString());

                //find bill item
                HttpResponseMessage response3 = await client.GetAsync(ApiUri + $"BillItem/detail/{bid}&{pid}");
                string strData3 = await response3.Content.ReadAsStringAsync();
                BillItemDTO item = System.Text.Json.JsonSerializer.Deserialize<BillItemDTO>(strData3, option);
                //delete item
                await client.DeleteAsync(ApiUri + $"BillItem/delete/{item.BillId}&{item.ProductId}");
            }

            if (service.Equals("incquantityitem"))
            {
                int bid = Convert.ToInt32(Request.Query["idb"].ToString());
                int pid = Convert.ToInt32(Request.Query["idp"].ToString());

                //find bill item
                HttpResponseMessage response3 = await client.GetAsync(ApiUri + $"BillItem/detail/{bid}&{pid}");
                string strData3 = await response3.Content.ReadAsStringAsync();
                BillItemDTO item = System.Text.Json.JsonSerializer.Deserialize<BillItemDTO>(strData3, option);
                //find product
                HttpResponseMessage response4 = await client.GetAsync(ApiUri + $"Product/Detail/{pid}");
                string strData4 = await response4.Content.ReadAsStringAsync();
                ProductListDTO product = System.Text.Json.JsonSerializer.Deserialize<ProductListDTO>(strData4, option);
                //set item
                item.Quantity += 1;
                item.TotalPrice += product.Price;
                //put bill item
                string jsonitem = System.Text.Json.JsonSerializer.Serialize(item);
                StringContent content = new StringContent(jsonitem, Encoding.UTF8, "application/json");
                await client.PutAsync(ApiUri + $"BillItem/put", content);
            }

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"BillItem/cart/{account.Id}");
            string strData = await response.Content.ReadAsStringAsync();
            Carts = System.Text.Json.JsonSerializer.Deserialize<List<CartDTO>>(strData, option);

            HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"BillItem/Total/{account.Id}");
            TotalPrice = await response2.Content.ReadAsStringAsync();

            return Page();

        }

         
    }
}
