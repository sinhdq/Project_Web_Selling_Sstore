using BusinessObject;
using DataAccess;
using DTO.BillDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        [HttpGet("list")]
        public IActionResult GetAllBill()
        {
           List<BillDTO> listdto = new List<BillDTO>();
           List<Bill> list = BillDAO.Instance.GetAllBill();
            foreach (Bill b in list)
            {
                BillDTO billdto = new BillDTO
                {
                    Id = b.Id,
                    CustomerId = b.CustomerId,
                    Address = b.Address,
                    CreateDate = b.CreateDate,
                    ShippingDate = b.ShippingDate,
                    RequiredDate = b.RequiredDate,
                    Status = b.Status,
                };
                listdto.Add(billdto);
            }
            return Ok(listdto);
        }

        [HttpGet("manager/list/{customerName}")]
        public IActionResult GetAllBillManager(string customerName)
        {
            if (customerName.Equals("null"))
            {
                customerName = "";
            }
            var list = BillDAO.Instance.GetAllBillManager(customerName);
            return Ok(list);
        }

        [HttpGet("detail/{id}")]
        public IActionResult GetBillByID(int id) {
            var b = BillDAO.Instance.GetBillByID(id);
            if(b == null)
            {
                return NotFound();
            }
            BillDTO billdto = new BillDTO
            {
                Id = b.Id,
                CustomerId = b.CustomerId,
                Address = b.Address,
                CreateDate = b.CreateDate,
                ShippingDate = b.ShippingDate,
                RequiredDate = b.RequiredDate,
                Status = b.Status,
            };

            return Ok(billdto);
        }

        [HttpGet("detail/nocheckout/{id}")]
        public IActionResult GetBillByCustomerIDNew(int id)
        {
            var b = BillDAO.Instance.GetBillByCustomerIDNew(id);
            if (b == null)
            {
                return NotFound();
            }
            BillDTO billdto = new BillDTO
            {
                Id = b.Id,
                CustomerId = b.CustomerId,
                Address = b.Address,
                CreateDate = b.CreateDate,
                ShippingDate = b.ShippingDate,
                RequiredDate = b.RequiredDate,
                Status = b.Status,
            };

            return Ok(billdto);
        }

        [HttpGet("customer/{id}")]
        public IActionResult GetListBillByCustomerID(int id)
        {
            List<BillDTO> listdto = new List<BillDTO>();
            List<Bill> list = BillDAO.Instance.GetListBillByCustomerID(id);
            foreach (Bill b in list)
            {
                BillDTO billdto = new BillDTO
                {
                    Id = b.Id,
                    CustomerId = b.CustomerId,
                    Address = b.Address,
                    CreateDate = b.CreateDate,
                    ShippingDate = b.ShippingDate,
                    RequiredDate = b.RequiredDate,
                    Status = b.Status,
                };
                listdto.Add(billdto);
            }
            return Ok(listdto);
        }

        [HttpPost("post")]
        public IActionResult PostBill(BillPostDTO bill)
        {

            BillDAO.Instance.AddBill(bill);
            return Ok();
        }

        [HttpPut("put")]
        public IActionResult PutBill(BillDTO bill)
        {
            var bl = BillDAO.Instance.GetBillByID(bill.Id);
            if (bl == null)
            {
                return NotFound();
            }
            
            BillDAO.Instance.UpdateBill(bill);
            return Ok();
        }

        
    }
}