using DTO.BillDTO;
using DTO.CartDTO;
using DTO.ContactDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ManagerClient.Pages
{
    public class ContactManagerModel : PageModel
    {
        public List<ContactManagerDTO> contacts { get; set; }
        public string message { get; set; }
        private readonly HttpClient client = null;
        private string ApiUri = "";
        private string loginKey = "_login";
        private string contactKey = "_contact";

        public ContactManagerModel()
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
            if (service.Equals("processing"))
            {
                int cid = Convert.ToInt32(Request.Query["cid"].ToString());

                HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"Contact/detail/{cid}");
                string strData2 = await response2.Content.ReadAsStringAsync();
                ContactDTO contactdto = System.Text.Json.JsonSerializer.Deserialize<ContactDTO>(strData2, option);

                contactdto.Status = true;
                
                string contactjson = System.Text.Json.JsonSerializer.Serialize(contactdto);
                StringContent content = new StringContent(contactjson, Encoding.UTF8, "application/json");

                await client.PutAsync(ApiUri + $"Contact/put", content);
                
                message = "Process complete!";

            }
            if (service.Equals("sendmail"))
            {
                int cid = Convert.ToInt32(Request.Query["cid"].ToString());
                HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"Contact/detail/{cid}");
                string strData2 = await response2.Content.ReadAsStringAsync();
                ContactDTO contactdto = System.Text.Json.JsonSerializer.Deserialize<ContactDTO>(strData2, option);
                string contactjson = System.Text.Json.JsonSerializer.Serialize(contactdto);

                HttpContext.Session.SetString(contactKey, contactjson);
                return RedirectToPage("SendMailCustomer");
            }

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"Contact/list/manager/null");
            string strData = await response.Content.ReadAsStringAsync();
            contacts = System.Text.Json.JsonSerializer.Deserialize<List<ContactManagerDTO>>(strData, option);

            return Page();
        }

        public async Task<IActionResult> OnPostFillter()
        {
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            string name = Request.Form["fname"];

            if (name.Equals(""))
            {
                name = "null";
            }

            HttpResponseMessage response = await client.GetAsync(ApiUri + $"Contact/list/manager/{name}");
            string strData = await response.Content.ReadAsStringAsync();
            contacts = System.Text.Json.JsonSerializer.Deserialize<List<ContactManagerDTO>>(strData, option);
            return Page();
        }
    }
}
