using DataAccess;
using DTO.ContactDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactController : ControllerBase
    {
        [HttpGet("list")]
        public IActionResult GetAllContact()
        {
            List<ContactDTO> list = ContactDAO.Instance.GetAllContact();
            return Ok(list);
        }
        //-------------------------------------------------------------------
        [HttpGet("list/manager/{cname}")]
        public IActionResult GetAllContactManager(string cname)
        {
            if (cname.Equals("null"))
            {
                cname = "";
            }
            List<ContactManagerDTO> list = ContactDAO.Instance.GetAllContact2(cname);
            return Ok(list);
        }
        //-------------------------------------------------------------------
        [HttpGet("detail/{id}")]
        public IActionResult GetContact(int id)
        {
            ContactDTO contactdto = ContactDAO.Instance.GetContacttByID(id);
            if (contactdto == null)
            {
                return NotFound();
            }
            return Ok(contactdto);
        }
        //-------------------------------------------------------------------
        [HttpPost]
        public IActionResult AddContact(ContactPostDTO contact)
        {
            ContactDAO.Instance.AddContact(contact);
            return Ok();
        }
        //-------------------------------------------------------------------
        [HttpPut("put")]
        public IActionResult PutContact(ContactDTO contact)
        {
            ContactDAO.Instance.UpdateContact(contact);
            return Ok();
        }
        //-------------------------------------------------------------------
        [HttpDelete]
        public IActionResult DeleteContact(int id)
        {
            ContactDTO contactdto =  ContactDAO.Instance.GetContacttByID(id);

            if (contactdto == null)
            {
                return NotFound();
            }
            ContactDAO.Instance.DeleteContact(id);
            return Ok();
        }
    }
}
