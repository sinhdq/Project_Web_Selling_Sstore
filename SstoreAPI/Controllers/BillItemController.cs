using BusinessObject;
using DataAccess;
using DTO.BillItemDTO;
using DTO.CartDTO;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillItemController : ControllerBase
    {
        //------------------------------------------------------------------------
        [HttpGet("cart/{customerid}")]
        public IActionResult GetListCart(int customerid)
        {
            List<CartDTO> list = BillItemDAO.Instance.GetListCart(customerid);
            return Ok(list);
        }
        //------------------------------------------------------------------------
        [HttpGet("BillDetail/{bid}")]
        public IActionResult GetBill(int bid)
        {
            List<CartDTO> list = BillItemDAO.Instance.GetBill(bid);
            return Ok(list);
        }
        //------------------------------------------------------------------------
        [HttpGet("cart/purchase/{customerid}&{billid}")]
        public IActionResult GetListCartPurchase(int customerid, int billid)
        {
            List<CartDTO> list = BillItemDAO.Instance.GetListCartPurchase(customerid, billid);
            return Ok(list);
        }
        //------------------------------------------------------------------------
        [HttpGet("Total/{customerid}")]
        public IActionResult GetTotalCart(int customerid)
        {
            double total = BillItemDAO.Instance.GetTotalCart(customerid);
            return Ok(total);
        }
        //------------------------------------------------------------------------
        [HttpGet("Total/BillDetail/{bid}")]
        public IActionResult GetTotalBillDetail(int bid)
        {
            double total = BillItemDAO.Instance.GetTotalBillDetail(bid);
            return Ok(total);
        }
        //------------------------------------------------------------------------
        [HttpGet("Total/Purchase/{customerid}&{billid}")]
        public IActionResult GetTotalCartPurchase(int customerid, int billid)
        {
            double total = BillItemDAO.Instance.GetTotalCartPurchase(customerid, billid);
            return Ok(total);
        }
        //------------------------------------------------------------------------
        [HttpGet("Quantity/{customerid}")]
        public IActionResult GetQuantityCart(int customerid)
        {
            int quantity = BillItemDAO.Instance.GetQuantityCart(customerid);
            return Ok(quantity);
        }
        //------------------------------------------------------------------------
        [HttpGet("detail/{bid}&{pid}")]
        public IActionResult GetItem(int bid, int pid)
        {
            BillItemDTO item = BillItemDAO.Instance.GetItemByBillIDAndProductID(bid, pid);
            if (item==null)
            {
                return NotFound();
            }
            return Ok(item);
        }
        //------------------------------------------------------------------------
        [HttpPut("put")]
        public IActionResult UpdateItem(BillItemDTO item)
        {
            BillItemDTO itmp = BillItemDAO.Instance.GetItemByBillIDAndProductID(item.BillId, item.ProductId);
            if (itmp == null)
            {
                return NotFound();

            }
            BillItemDAO.Instance.UpdateItem(item);
            return Ok();

        }
        //------------------------------------------------------------------------
        [HttpDelete("delete/{bid}&{pid}")]
        public IActionResult DeleteItem(int bid, int pid)
        {
            BillItemDTO itmp = BillItemDAO.Instance.GetItemByBillIDAndProductID(bid, pid);
            if (itmp == null)
            {
                return NotFound();

            }
            BillItemDAO.Instance.DeleteItem(bid, pid);
            return Ok();

        }


    }

}
