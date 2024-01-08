using CommonLib;
using DTO.AccountDTO;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace SstoreClient.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<ProductListDTO> listproduct { get; set; }
        private readonly HttpClient client = null;
        private string ApiUri = "";
        public string message { get; set; }
        private string loginKey = "_login";
        public LoginModel()
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
        }
        public async Task<IActionResult> OnGet()
        {
            //message = "kasfjasbfsjnfksgkrgnkdrgnkABC";
            HttpContext.Session.Clear();
            return Page();
        }
        
        public async Task<IActionResult> OnPostSignin()
        {

            if (Request.Form["mail"].Equals("") || Request.Form["password"].Equals(""))
            {
                message = "Please, fill all infomations";
                return Page();
            }
            else
            {
                string mail = Encryption.Instance.hashMD5(Request.Form["mail"]); 
                string password = Encryption.Instance.hashMD5(Request.Form["password"]);
                HttpResponseMessage response = await client.GetAsync(ApiUri + $"Account/authentication/{mail}&{password}");
                string strData = await response.Content.ReadAsStringAsync();

                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                AccountDTO acc = System.Text.Json.JsonSerializer.Deserialize<AccountDTO>(strData,option);
                if (acc != null && acc.Role == 0 && acc.Active == true)
                {
                    string json = JsonConvert.SerializeObject(acc);
                    HttpContext.Session.SetString(loginKey, json);
                    return RedirectToPage("Index");
                }

                else
                {
                    message = "Login failed";
                    return Page();
                }
            }

        }
        
        public async Task<IActionResult> OnPostSignup()
        {
            if (Request.Form["fullname"].Equals("") || Request.Form["Gender"].Equals("") ||
                Request.Form["rmail"].Equals("") || Request.Form["phone"].Equals("") ||
                Request.Form["dob"].Equals("") || Request.Form["rpassword"].Equals("") ||
                Request.Form["repassword"].Equals("") || Request.Form["address"].Equals(""))
            {
                message = "Please, fill all information!";
                return Page();
            }
            else
            {
                String fullname = Request.Form["fullname"];
                bool gender = false;
                if (Convert.ToInt32(Request.Form["Gender"]) == 1)
                {
                    gender = true;
                }
                //int gender = Convert.ToInt32(Request.Form["Gender"]);
                string mail = Request.Form["rmail"];
                string phone = Request.Form["phone"];
                DateTime dob = Convert.ToDateTime(Request.Form["dob"]);
                string address = Request.Form["address"];
                string password = Request.Form["rpassword"];
                string repass = Request.Form["repassword"];
                if (!Validate.Instance.Phone(phone))
                {
                    message = "Phone numer must 10 characters";
                    return Page();
                }

                if (!password.Equals(repass))
                {
                    message = "Password must the same repassword";
                    return Page();
                }

                if (!Validate.Instance.Mail(mail))
                {
                    message = "Email not exist";
                    return Page();
                }
                if (!Validate.Instance.Name(fullname))
                {
                    message = "Full name is Failed";
                    return Page();
                }

                string pass = Encryption.Instance.hashMD5(password);
                HttpResponseMessage response = await client.GetAsync(ApiUri + $"Account/detailmail/{mail}");
                string strData = await response.Content.ReadAsStringAsync();

                var option = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };

                AccountDTO acc = System.Text.Json.JsonSerializer.Deserialize<AccountDTO>(strData, option);

                if (acc.Id == 0)
                {
                    
                   AccountAddDTO a = new AccountAddDTO
                    {
                        FullName = fullname,
                        Email = mail,
                        Address = address,
                        Dob = dob,
                        Gender = gender,
                        Phone = phone,
                        Password = pass,
                        Active = false,
                        Role = 0

                    };

                    string jar = JsonConvert.SerializeObject(a);
                    string otp = SendMail.Instance.GetOTP();

                    HttpContext.Session.SetString("jar",jar);
                    HttpContext.Session.SetString("otp",otp);

                    try
                    {
                       SendMail.Instance.Send(mail, otp, "S Store active account");
                    }catch(Exception ex)
                    {
                        message = "Email does not exist!";
                      
                    }

                    return RedirectToPage("CheckOTP");
                }
                else
                {
                     message = "Email register exist, try again";
                 
                    return Page();
                }
            }

        }
        
    }
}
