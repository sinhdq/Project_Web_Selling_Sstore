using DTO.CategoryDTO;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace ManagerClient.Pages
{
    public class AddProductModel : PageModel
    {
        private readonly HttpClient client = null;
        private string ApiUri = "";
        private string loginKey = "_login";
        public string message { get; set; } 
        public List<CategoryDTO> listcategory { get; set; }
        public List<ProductListDTO> productlist { get; set; }
        private IWebHostEnvironment _environment;
        [Required(ErrorMessage = "Please choose at least one file")]
        [DataType(DataType.Upload)]
        [Display(Name = "Choose file(s) to upload")]
        [BindProperty]
        public IFormFile[] FileUploads { get; set; }

        public AddProductModel(IWebHostEnvironment environment)
        {
            client = new HttpClient();
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);
            ApiUri = "http://localhost:5007/api/";
            _environment = environment;
        }

        public async Task<IActionResult> OnGet()
        {
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"Category/list/null");
            string strData2 = await response2.Content.ReadAsStringAsync();
            listcategory = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strData2, option);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var option = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
            HttpResponseMessage response2 = await client.GetAsync(ApiUri + $"Category/list/null");
            string strData2 = await response2.Content.ReadAsStringAsync();
            listcategory = System.Text.Json.JsonSerializer.Deserialize<List<CategoryDTO>>(strData2, option);

            string filename = "";
            if (FileUploads != null)
            {
                var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".gif" };
                foreach (var FileUpload in FileUploads)
                {
                    var extension = Path.GetExtension(FileUpload.FileName);
                    if (allowedExtensions.Contains(extension.ToLower()))
                    {
                        string basepath = _environment.ContentRootPath.Substring(0, _environment.ContentRootPath.IndexOf("ManagerClient"));
                        var fileadmin = basepath + "ManagerClient\\wwwroot\\image\\" + FileUpload.FileName;
                        var fileuser = basepath + "SstoreClient\\wwwroot\\image\\" + FileUpload.FileName;
                        filename = FileUpload.FileName;
                        using (var fileSream = new FileStream(fileadmin, FileMode.Create))
                        {
                            await FileUpload.CopyToAsync(fileSream);
                        }
                        using (var fileSream = new FileStream(fileuser, FileMode.Create))
                        {
                            await FileUpload.CopyToAsync(fileSream);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Only the following file types are allowed: {string.Join(", ", allowedExtensions)}");
                    }
                }
            }

            if (Request.Form["pname"].Equals("") || Request.Form["department"].Equals("") || Request.Form["quantity"].Equals("") || Request.Form["price"].Equals("") || Request.Form["description"].Equals(""))
            {
                message = "Add failded, check all infomation";
                return Page();
            }
            else
            {

                int idp = Convert.ToInt32(Request.Form["pid"]);
                string namep = Request.Form["pname"];
                int idcategory = Convert.ToInt32(Request.Form["department"]);
                int quantity = Convert.ToInt32(Request.Form["quantity"]);
                double price = Convert.ToDouble(Request.Form["price"]);
                string description = Request.Form["description"];

                ProductAddDTO product = new ProductAddDTO
                {
                    ProductName = namep,
                    CategoryId = idcategory,
                    Quantity = quantity,
                    Image = filename,
                    Price = price,
                    Description = description
                };

                string json = System.Text.Json.JsonSerializer.Serialize(product);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                await client.PostAsync(ApiUri + "Product/Add", content);
                message = "Add product successfully";

            }
            
            return Page();

        }
    }
}
