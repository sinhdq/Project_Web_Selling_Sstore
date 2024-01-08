using DTO.AccountDTO;
using DTO.CategoryDTO;
using DTO.ContactDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace SstoreClient.Pages
{
    public class ContactModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        public AccountDTO account { get; set; }
        public string message { get; set; }
        private string loginKey = "_login";

        public ContactModel()
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

            account = JsonConvert.DeserializeObject<AccountDTO>(json);
            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            string json = HttpContext.Session.GetString(loginKey) ?? "";
            account = JsonConvert.DeserializeObject<AccountDTO>(json);

            if (Request.Form["message"].Equals(""))
            {
                message = "Please, enter message";
                return Page();

            }
            string feeback = Request.Form["message"];
            ContactPostDTO postdto = new ContactPostDTO
            {
                CustomerId = account.Id,
                Feeback = feeback,
                Status = false
            };
            string json2 = System.Text.Json.JsonSerializer.Serialize(postdto);
            StringContent content = new StringContent(json2, Encoding.UTF8, "application/json");
            await client.PostAsync(ApiUri + "Contact", content);
            message = "Feeback complete";

            return Page();
        }
    }
}
