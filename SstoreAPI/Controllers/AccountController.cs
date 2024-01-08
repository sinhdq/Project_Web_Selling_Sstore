using BusinessObject;
using DataAccess;
using DTO.AccountDTO;
using DTO.ProductDTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

namespace SstoreAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [HttpGet("list/{name}")]
        public IActionResult GetAllAccountByName(string name)
        {
            if (name.Equals("null"))
            {
                name = "";
            }
            List<Account> listaccount = AccountDAO.Instance.GetAllAccountByName(name); 
            List<AccountDTO> listdto = new List<AccountDTO>();
            foreach(var item in listaccount)
            {
                listdto.Add(new AccountDTO
                {
                    Id = item.Id,
                    FullName = item.FullName,
                    Email = item.Email,
                    Address = item.Address,
                    Dob = item.Dob,
                    Gender = item.Gender,
                    Phone = item.Phone,
                    Active = item.Active,
                    Role = item.Role
                });
            }
            return Ok(listdto);
        }

        [HttpGet("detail/{id}")]
        public IActionResult GetAccountByID(int id)
        {
            Account account = AccountDAO.Instance.GetAccountByID(id);
            if(account == null)
            {
                return NotFound();
            }
            AccountPofileDTO accountdto = new AccountPofileDTO
            {
                Id = account.Id,
                FullName = account.FullName,
                Email = account.Email,
                Address = account.Address,
                Dob = account.Dob,
                Gender = account.Gender,
                Phone = account.Phone,
                Password = account.Password,
                Active = account.Active,
                Role = account.Role

            };
            return Ok(accountdto);
        }

        [HttpGet("GetAccountPut/{id}")]
        public IActionResult GetAccountPutByID(int id)
        {
            Account account = AccountDAO.Instance.GetAccountByID(id);
            if (account == null)
            {
                return NotFound();
            }
            AccountPutDTO accountdto = new AccountPutDTO
            {
                Id = account.Id,
                FullName = account.FullName,
                Email = account.Email,
                Address = account.Address,
                Dob = account.Dob,
                Gender = account.Gender,
                Phone = account.Phone,
                Password = account.Password,
                Active = account.Active,
                Role = account.Role

            };
            return Ok(accountdto);
        }

        [HttpGet("authentication/{email}&{password}")]
        public IActionResult Authentication(string email, string password)
        {
            Account account = AccountDAO.Instance.GetAccountByEmailAndPassword(email,password);
            if (account == null)
            {
                return NotFound();
            }
            AccountDTO accountdto = new AccountDTO
            {
                Id = account.Id,
                FullName = account.FullName,
                Email = account.Email,
                Address = account.Address,
                Dob = account.Dob,
                Gender = account.Gender,
                Phone = account.Phone,
                Active = account.Active,
                Role = account.Role

            };
            return Ok(accountdto);
        }

        [HttpGet("detailmail/{email}")]
        public IActionResult GetAccountByEmail(string email)
        {
            Account account = AccountDAO.Instance.GetAccountByEmail(email);
            if (account == null)
            {
                return NotFound();
            }
            AccountDTO accountdto = new AccountDTO
            {
                Id = account.Id,
                FullName = account.FullName,
                Email = account.Email,
                Address = account.Address,
                Dob = account.Dob,
                Gender = account.Gender,
                Phone = account.Phone,
                Active = account.Active,
                Role = account.Role

            };
            return Ok(accountdto);
        }

        [HttpPost]
        public IActionResult PostAccount(AccountAddDTO accountdto)
        {
            Account account = new Account
            {
                FullName = accountdto.FullName,
                Email = accountdto.Email,
                Address = accountdto.Address,
                Dob = accountdto.Dob,
                Gender = accountdto.Gender,
                Phone = accountdto.Phone,
                Password = accountdto.Password,
                Active = accountdto.Active,
                Role = accountdto.Role

            };
            AccountDAO.Instance.AddAccount(account);
            return Ok();
        }

        [HttpPut("put/{id}")]
        public IActionResult UpdateAccount(AccountPutDTO accountdto)
        {

            var aTmp = AccountDAO.Instance.GetAccountByID(accountdto.Id);
            if (aTmp == null)
            {
                return NotFound();

            }
            Account account = new Account
            {
                Id = accountdto.Id,
                FullName = accountdto.FullName,
                Email = accountdto.Email,
                Address = accountdto.Address,
                Dob = accountdto.Dob,
                Gender = accountdto.Gender,
                Phone = accountdto.Phone,
                Password = accountdto.Password,
                Active = accountdto.Active,
                Role = accountdto.Role

            };
            AccountDAO.Instance.UpdateAccount(account);
            return Ok();

        }

        [HttpDelete("delete/{id}")]
        public IActionResult DeleteAccount(int id)
        {

            var a = AccountDAO.Instance.GetAccountByID(id);
            if (a == null)
            {
                return NotFound();
            }

            AccountDAO.Instance.DeleteAccount(a.Id);
            return Ok();
        }


    }
}
