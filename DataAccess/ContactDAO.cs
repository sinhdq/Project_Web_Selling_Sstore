using BusinessObject;
using DTO.ContactDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ContactDAO
    {
        private static ContactDAO instance = null;
        private static readonly object instanceLock = new object();
        private ContactDAO() { }
        public static ContactDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ContactDAO();
                    }
                    return instance;
                }
            }
        }

        public List<ContactDTO> GetAllContact()
        {
            //get list account by full_name
            List<Contact> list = new List<Contact>();
            List<ContactDTO> list2 = new List<ContactDTO>();
            try
            {
                SstoreContext context = new SstoreContext();
                list = context.Contacts.ToList();

                foreach (var item in list)
                {
                    ContactDTO contact = new ContactDTO
                    {
                        Id = item.Id,
                        CustomerId = item.CustomerId,
                        Feeback = item.Feeback,
                        Status = item.Status
                    };
                    list2.Add(contact);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return list2;
        }
     
        //-----------------------------------------------------------------
        public List<ContactManagerDTO> GetAllContact2(string customername)
        {
            //get list account by full_name
            List<ContactManagerDTO> list = new List<ContactManagerDTO>();
            try
            {
                SstoreContext context = new SstoreContext();
                string sql = "select c.id, c.customer_id, c.feeback, c.status" +
                             "\r\n         from contact c, account a" +
                             "\r\n         where c.customer_id = a.id" +
                             $"\r\n\t\t and a.full_name like '%{customername}%'";

                var contact = context.Contacts.FromSqlRaw(sql);

                foreach (var item in contact)
                {
                    ContactManagerDTO cdto = new ContactManagerDTO
                    {
                        Id = item.Id,
                        CustomerName = AccountDAO.Instance.GetAccountByID(item.CustomerId).FullName,
                        CustomerEmail = AccountDAO.Instance.GetAccountByID(item.CustomerId).Email,
                        Feeback = item.Feeback,
                        Status = item.Status
                    };
                    list.Add(cdto);
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return list;
        }


        //----------------------------------------------------------------
        //get contact by id
        public ContactDTO GetContacttByID(int id)
        {
            Contact contact = null;
            ContactDTO contactDTO = null;
            try
            {
                SstoreContext context = new SstoreContext();
                contact = context.Contacts.FirstOrDefault(x => x.Id == id);
                contactDTO = new ContactDTO
                {
                    Id = contact.Id,
                    CustomerId = contact.CustomerId,
                    Feeback = contact.Feeback,
                    Status = contact.Status
                };

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return contactDTO;
        }

        //---------------------------------------------------------------

        public void AddContact(ContactPostDTO contactDTO)
        {   
            try
            {
                SstoreContext context = new SstoreContext();
                Contact contact = new Contact
                {
                    CustomerId = contactDTO.CustomerId,
                    Feeback = contactDTO.Feeback,
                    Status = contactDTO.Status
                };
                context.Contacts.Add(contact);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //---------------------------------------------------------------
        public void UpdateContact(ContactDTO contactdto)
        {
            try
            {
                SstoreContext context = new SstoreContext();
                Contact contact = new Contact
                {
                    Id = contactdto.Id,
                    CustomerId = contactdto.CustomerId,
                    Feeback = contactdto.Feeback,
                    Status = contactdto.Status
                };
                context.Entry<Contact>(contact).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //--------------------------------------------------------------
        public void DeleteContact(int id)
        {
            try
            {
                SstoreContext context = new SstoreContext();
                ContactDTO contactdto = GetContacttByID(id);
                Contact contact = new Contact
                {
                    CustomerId = contactdto.CustomerId,
                    Feeback = contactdto.Feeback,
                    Status = contactdto.Status
                };
                context.Contacts.Remove(contact);
                context.SaveChanges();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
