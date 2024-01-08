using BusinessObject;
using CommonLib;
using DataAccess;
using DTO.BillDTO;
using DTO.BillItemDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShopingController : ControllerBase
    {
        [HttpGet("addtocart/{customerId}&{productId}&{quantity}")]
        public IActionResult AddToCart(int customerId, int productId, int quantity)
        {
            Product product = ProductDAO.Instance.GetProductByID(productId);
            BillDTO bill = BillDAO.Instance.GetBillByCustomerIDNew(customerId);
            Account account = AccountDAO.Instance.GetAccountByID(customerId);
            if (product == null)
            {
                return NotFound();
            }
            // case user dont have bill (dont order)
            if (bill==null)
            {
                BillPostDTO billPost = new BillPostDTO
                {
                    CustomerId = customerId,
                    Address = account.Address,
                    CreateDate = Convert.ToDateTime(Validate.Instance.DateNow()),
                    Status = false
                };

                BillDAO.Instance.AddBill(billPost);
                bill = BillDAO.Instance.GetBillByCustomerIDNew(customerId);
                
                BillItemDTO item = BillItemDAO.Instance.GetItemByBillIDAndProductID(bill.Id, productId);

                if(item == null)
                {
                    //create new billitem
                    item = new BillItemDTO
                    {
                        BillId = bill.Id,
                        ProductId = productId,
                        Quantity = quantity,
                        TotalPrice = product.Price * quantity
                    };
                    //add new item
                    BillItemDAO.Instance.AddItem(item);
                }
                else
                {
                    item.Quantity = item.Quantity + quantity;
                    item.TotalPrice = item.TotalPrice + product.Price * quantity;

                    BillItemDAO.Instance.UpdateItem(item);

                }

            }
            else
            {
                BillItemDTO item = BillItemDAO.Instance.GetItemByBillIDAndProductID(bill.Id, productId);

                if (item == null)
                {
                    //create new billitem
                    item = new BillItemDTO
                    {
                        BillId = bill.Id,
                        ProductId = productId,
                        Quantity = quantity,
                        TotalPrice = product.Price * quantity
                    };
                    //add new item
                    BillItemDAO.Instance.AddItem(item);
                }
                else
                {
                    item.Quantity = item.Quantity + quantity;
                    item.TotalPrice = item.TotalPrice + product.Price * quantity;

                    BillItemDAO.Instance.UpdateItem(item);

                }
            }
            return Ok();
        }
    }
}
