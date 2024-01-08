using CommonLib;
using DTO.AccountDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace ManagerClient.Pages
{
    public class ProfileModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        public string message { get; set; }
        public AccountPofileDTO profile { get; set; }
        private string loginKey = "_login";

        public ProfileModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
        }

        public async Task<IActionResult> OnGet()
        {
            string json = HttpContext.Session.GetString(loginKey) ?? "";

            if (string.IsNullOrEmpty(json))
            {
                return RedirectToPage("Index");
            }
            AccountDTO a1 = JsonConvert.DeserializeObject<AccountDTO>(json);
            HttpResponseMessage response = await client.GetAsync(ApiUri + $"Account/detail/{a1.Id}");
            string strData = await response.Content.ReadAsStringAsync();

            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            profile = System.Text.Json.JsonSerializer.Deserialize<AccountPofileDTO>(strData, option);

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            try
            {

                string json = HttpContext.Session.GetString(loginKey) ?? "";

                if (json.Equals(""))
                {
                    return RedirectToPage("Login");
                }

                AccountDTO account = JsonConvert.DeserializeObject<AccountDTO>(json);

                if (Request.Form["fullname"].Equals("") || Request.Form["address"].Equals("") ||
                    Request.Form["gender"].Equals("") || Request.Form["phone"].Equals("") ||
                    Request.Form["email"].Equals("") || Request.Form["dob"].Equals(""))
                {
                    message = "Infomation must not null";
                    return RedirectToPage("Profile");
                }
                else
                {
                    string fullname = Request.Form["fullname"];
                    string address = Request.Form["address"];
                    bool gender = (Convert.ToInt32(Request.Form["gender"]) == 1) ? true : false;
                    string phone = Request.Form["phone"];
                    DateTime dob = Convert.ToDateTime(Request.Form["dob"]);
                    string email = Request.Form["email"];
                    //get account profile
                    HttpResponseMessage response = await client.GetAsync(ApiUri + $"Account/detail/{account.Id}");
                    string strData = await response.Content.ReadAsStringAsync();
                    var option = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                    };

                    AccountPofileDTO accountprofile = System.Text.Json.JsonSerializer.Deserialize<AccountPofileDTO>(strData, option);


                    if (!Validate.Instance.Phone(phone))
                    {
                        message = "Phone numer must 10 characters";
                        return RedirectToPage("Profile");
                    }

                    if (!Validate.Instance.Mail(email))
                    {
                        message = "Email not exist";
                        return RedirectToPage("Profile");
                    }
                    if (!Validate.Instance.Name(fullname))
                    {
                        message = "Full name dont have number";
                        return RedirectToPage("Profile");
                    }

                    accountprofile.FullName = fullname;
                    accountprofile.Email = email;
                    accountprofile.Address = address;
                    accountprofile.Dob = dob;
                    accountprofile.Gender = gender;
                    accountprofile.Phone = phone;

                    json = System.Text.Json.JsonSerializer.Serialize(accountprofile);
                    StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                    await client.PutAsync(ApiUri + $"Account/put/{accountprofile.Id}", content);

                    HttpContext.Session.SetString(loginKey, json);
                    message = "Update profile complete";
                    return RedirectToPage("Profile");

                }
            }
            catch (Exception ex)
            {
                message = "Update profile failded";
                return RedirectToPage("Profile");

            }


        }
    }
}
