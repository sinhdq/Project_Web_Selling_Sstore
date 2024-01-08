using BusinessObject;
using DataAccess;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        [HttpGet("list/{categoryid}&{productname}")]
        public IActionResult GetAllProductByName(string categoryid, string productname)
        {
            if (productname.Equals("null"))
            {
                productname = "";
            }

            if(categoryid.Equals("null"))
            {
                categoryid = "";
            }

            List<ProductListDTO> listdto = new List<ProductListDTO>();
            List<Product> listproduct = ProductDAO.Instance.GetAllProductByName(categoryid, productname);
            foreach(var item in listproduct)
            {
                listdto.Add(new ProductListDTO
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    CategoryName = CategoryDAO.Instance.GetCategoryByID(item.CategoryId).CategoryName,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Image = item.Image,
                    Description = item.Description
                });
            }
            return Ok(listdto);
        }

        [HttpGet("Detail/{id}")]
        public IActionResult GetProductByID(int id)
        {
            Product product = ProductDAO.Instance.GetProductByID(id);
            if(product == null)
            {
                return NotFound("Product does not exist");
            }
            ProductListDTO productdto = new ProductListDTO
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CategoryName = CategoryDAO.Instance.GetCategoryByID(product.CategoryId).CategoryName,
                Price = product.Price,
                Quantity = product.Quantity,
                Image = product.Image,
                Description = product.Description
            };

            return Ok(productdto);
        }
        [HttpGet("Detail/ProductPut/{id}")]
        public IActionResult GetProductPutByID(int id)
        {
            Product product = ProductDAO.Instance.GetProductByID(id);
            if (product == null)
            {
                return NotFound("Product does not exist");
            }
            ProductPutDTO productdto = new ProductPutDTO
            {
                Id = product.Id,
                ProductName = product.ProductName,
                CategoryId = product.CategoryId,
                Price = product.Price,
                Quantity = product.Quantity,
                Image = product.Image,
                Description = product.Description
            };

            return Ok(productdto);
        }


        [HttpPost("Add")]
        public IActionResult PostProduct(ProductAddDTO productdto)
        {
            Product product = new Product
            {
                ProductName = productdto.ProductName,
                CategoryId = productdto.CategoryId,
                Price = productdto.Price,
                Quantity = productdto.Quantity,
                Image = productdto.Image,
                Description = productdto.Description
            };
            ProductDAO.Instance.AddProduct(product);
            return Ok();
        }

        [HttpPut("Put/{id}")]
        public IActionResult UpdateProduct(ProductPutDTO productdto)
        {

            var pTmp = ProductDAO.Instance.GetProductByID(productdto.Id);
            if (pTmp == null)
            {
                return NotFound();

            }
            Product product = new Product
            {
              Id = productdto.Id,
              ProductName = productdto.ProductName,
              CategoryId = productdto.CategoryId,
              Price = productdto.Price,
              Quantity = productdto.Quantity,
              Image = productdto.Image,
              Description = productdto.Description
            };
            ProductDAO.Instance.UpdateProduct(product);
            return Ok();

        }

        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteProduct(int id)
        {

            var p = ProductDAO.Instance.GetProductByID(id);
            if (p == null)
            {
                return NotFound();
            }

            ProductDAO.Instance.DeleteProduct(p.Id);
            return Ok();
        }

    }
}
