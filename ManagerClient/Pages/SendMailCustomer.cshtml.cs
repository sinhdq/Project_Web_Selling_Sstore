using CommonLib;
using DTO.AccountDTO;
using DTO.ContactDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Json;

namespace ManagerClient.Pages
{
    public class SendMailCustomerModel : PageModel
    {
        public string message { get; set; }
        private readonly HttpClient client = null;
        private string ApiUri = "";
        private string loginKey = "_login";
        private string contactKey = "_contact";
        public ContactDTO contact { get; set; }
        public AccountPofileDTO customer { get;set; } 

        public SendMailCustomerModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
        }

        public async Task<IActionResult> OnGet()
        {
            string json = HttpContext.Session.GetString(loginKey) ?? "";
            string contactjson = HttpContext.Session.GetString(contactKey) ?? "";
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };

            if (json.Equals(""))
            {
                return RedirectToPage("Login");
            }

            contact = JsonSerializer.Deserialize<ContactDTO>(contactjson);

            HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"Account/detail/{contact.CustomerId}");
            string strData2 = await response2.Content.ReadAsStringAsync();
            customer = System.Text.Json.JsonSerializer.Deserialize<AccountPofileDTO>(strData2, option);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {
                string json = HttpContext.Session.GetString(loginKey) ?? "";
                string contactjson = HttpContext.Session.GetString(contactKey) ?? "";
                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                if (json.Equals(""))
                {
                    return RedirectToPage("Login");
                }

                contact = JsonSerializer.Deserialize<ContactDTO>(contactjson);

                HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"Account/detail/{contact.CustomerId}");
                string strData2 = await response2.Content.ReadAsStringAsync();
                customer = System.Text.Json.JsonSerializer.Deserialize<AccountPofileDTO>(strData2, option);

                if (Request.Form["mail"].Equals("") || Request.Form["message"].Equals(""))
                {
                    message = "Please, enter all infomation";
                    return Page();
                }
                string mail = Request.Form["mail"];
                string fb = Request.Form["message"];
                if (!Validate.Instance.Mail(mail))
                {
                    message = "Email address isvalid";
                    return Page();
                }
                SendMail.Instance.Send(mail, fb, "Feeback customer");
                message = "Send mail complete";
                return RedirectToPage("ContactManager");
            }
            catch (Exception ex)
            {
                message = "Send mail failded";
            }
            return Page();
        }
    }
}
