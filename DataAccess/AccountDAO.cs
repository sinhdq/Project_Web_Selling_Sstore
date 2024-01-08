using BusinessObject;
using CommonLib;
using DTO.AccountDTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class AccountDAO
    {

        private static AccountDAO instance = null;
        private static readonly object instanceLock = new object();
        private AccountDAO() { }
        public static AccountDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new AccountDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Account> GetAllAccountByName(string name)
        {
            //get list account by full_name
            List<Account> list = new List<Account>();
            try
            {
                SstoreContext context = new SstoreContext();
                list = context.Accounts.Where(p => EF.Functions.Like(p.FullName, $"%{name}%"))
                                       .ToList();
            }
            catch(Exception e)
            {
                throw new Exception(e.Message);
            }
            return list;
        }

        //----------------------------------------------------------------
        //get account by id
        public Account GetAccountByID(int id)
        {
            Account account = null;
            try
            {
                SstoreContext context = new SstoreContext();
                account = context.Accounts.FirstOrDefault(a=>a.Id == id);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return account;
        }

        //----------------------------------------------------------------
        public Account GetAccountByEmailAndPassword(string email, string password)
        {
            Account account = null;
            try
            {
                SstoreContext context = new SstoreContext();

                var list = context.Accounts.ToList();
                account = list.Where(a => (Encryption.Instance.hashMD5(a.Email).ToUpper().Equals(email.ToUpper())
                                           && a.Password.ToUpper().Equals(password.ToUpper()))).FirstOrDefault();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return account;
        }
        //----------------------------------------------------------------
        public Account GetAccountByEmail(string email)
        {
            Account account = null;
            try
            {
                SstoreContext context = new SstoreContext();
                account = context.Accounts.Where(a => a.Email.Equals(email)).FirstOrDefault();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return account;
        }

        //---------------------------------------------------------------

        public void AddAccount(Account account)
        {
            try
            {
      
                SstoreContext context = new SstoreContext();
                context.Accounts.Add(account);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        
        //---------------------------------------------------------------
        public void UpdateAccount(Account account)
        {
            try
            {
                SstoreContext context = new SstoreContext();
                context.Entry<Account>(account).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //--------------------------------------------------------------

        public void DeleteAccount(int id)
        {
            try
            {
                SstoreContext context = new SstoreContext();
                context.Accounts.Remove(GetAccountByID(id));
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
