using BusinessObject;
using DataAccess;
using DTO.AccountDTO;
using DTO.CategoryDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {

        [HttpGet("list/{name}")]
        public IActionResult GetAllCategoryByName(string name)
        {
            if (name.Equals("null"))
            {
                name = "";
            }
            List<Category> listcate = CategoryDAO.Instance.GetAllCategoryByName(name);
            if(listcate.Count == 0)
            {
                return NotFound();
            }
            List<CategoryDTO> list = new List<CategoryDTO>();
            foreach (Category category in listcate)
            {
                list.Add(new CategoryDTO
                {
                    Id = category.Id,
                    CategoryName = category.CategoryName
                });
            }
            return Ok(list);
        }

        [HttpGet("detail/{id}")]
        public IActionResult GetCategoryByID(int id)
        {
            var category = CategoryDAO.Instance.GetCategoryByID(id);
            if (category == null)
            {
                return NotFound();
            }
            CategoryDTO categorydto = new CategoryDTO
            {
                Id = category.Id,
                CategoryName = category.CategoryName
            };
            return Ok(categorydto);
        }

        [HttpPost]
        public IActionResult PostCategory(CategoryPostDTO catedto)
        {
            Category category = new Category
            {
                CategoryName = catedto.CategoryName
            };
            CategoryDAO.Instance.AddCategory(category);
            return Ok();
        }
        [HttpPut("put/{id}")]
        public IActionResult PutCategory(CategoryDTO categorydto)
        {
            var category = CategoryDAO.Instance.GetCategoryByID(categorydto.Id);
            if(category == null)
            {
                return NotFound();
            }
            Category cate = new Category
            {
                Id = categorydto.Id,
                CategoryName = categorydto.CategoryName
            };
            return Ok();
        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = CategoryDAO.Instance.GetCategoryByID(id);
            if (category == null)
            {
                return NotFound();
            }
            CategoryDAO.Instance.DeleteCategory(id);
            return Ok();
        }
    }
}
