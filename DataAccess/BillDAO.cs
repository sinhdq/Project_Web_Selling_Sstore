using BusinessObject;
using DTO.BillDTO;
using DTO.CartDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BillDAO
    {
        private static BillDAO instance = null;
        private static readonly object instanceLock = new object();
        private BillDAO() { }
        public static BillDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BillDAO();
                    }
                    return instance;
                }
            }
        }
       
        //---------------------------------------------------------------
        public List<Bill> GetAllBill()
        {
            List<Bill> list = new List<Bill>();
            try
            {
                SstoreContext context = new SstoreContext();
                list = context.Bills.ToList();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return list;
        }
        private string GetCustomerName(int cid)
        {
            string name = "";
            try
            {
               SstoreContext context = new SstoreContext();
               name = context.Accounts.FirstOrDefault(a=>a.Id == cid).FullName;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return name;
        }

        //---------------------------------------------------------------
        public List<BillManagerDTO> GetAllBillManager(string customername)
        {
            List<BillManagerDTO> listdto = new List<BillManagerDTO>();
            try
            {
                SstoreContext context = new SstoreContext();
                string sql = "select b.id, b.customer_id," +
                             "\r\n       b.address, b.create_date, b.shipping_date," +
                             "\r\n\t   b.required_date, b.status" +
                             "\r\n         from bill b, account a" +
                             "\r\n\t\t where b.customer_id = a.id" +
                             $"\r\n\t\t and a.full_name like '%{customername}%'";
                var bill = context.Bills.FromSqlRaw(sql);
                foreach (var item in bill)
                {
                    BillManagerDTO bdto = new BillManagerDTO
                    {
                        Id = item.Id,
                        CustomerId = item.CustomerId,
                        CustomerName = GetCustomerName(item.CustomerId),
                        Address = item.Address,
                        CreateDate = item.CreateDate,
                        ShippingDate = item.ShippingDate,
                        RequiredDate = item.RequiredDate,
                        Status = item.Status
                    };
                    listdto.Add(bdto);
                }


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return listdto;
        }

        //----------------------------------------------------------------
        public Bill GetBillByID(int id)
        {
            Bill bill = null;
            try
            {
                SstoreContext context = new SstoreContext();
                bill = context.Bills.FirstOrDefault(a => a.Id == id);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return bill;
        }
        //----------------------------------------------------------------
        public List<Bill> GetListBillByCustomerID(int cid)
        {
            List<Bill> list = new List<Bill>();
            try
            {
                SstoreContext context = new SstoreContext();
                list = context.Bills.Where(b => b.CustomerId == cid && b.Status == false && b.RequiredDate != null).ToList();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return list;
        }
        //---------------------------------------------------------------
        public BillDTO GetBillByCustomerIDNew(int cid)
        {
            Bill bill = null;
            BillDTO billdto = null;
            try
            {
                SstoreContext context = new SstoreContext();
                bill = context.Bills.FirstOrDefault(x=>x.CustomerId == cid && x.Status == false && x.ShippingDate == null && x.RequiredDate == null);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            if (bill != null)
            {
                billdto = new BillDTO
                {
                    Id = bill.Id,
                    CustomerId = bill.CustomerId,
                    Address = bill.Address,
                    CreateDate = bill.CreateDate,
                    ShippingDate = bill.ShippingDate,
                    RequiredDate = bill.RequiredDate,
                    Status = bill.Status
                };
            }
            return billdto;
        }

        //---------------------------------------------------------------

        public void AddBill(BillPostDTO billdto)
        {
            Bill b = new Bill
            {
                CustomerId = billdto.CustomerId,
                Address = billdto.Address,
                CreateDate = billdto.CreateDate,
                ShippingDate = billdto.ShippingDate,
                RequiredDate = billdto.RequiredDate,
                Status = billdto.Status,
            };
            try
            {
                SstoreContext context = new SstoreContext();
                context.Bills.Add(b);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //---------------------------------------------------------------
        public void UpdateBill(BillDTO billdto)
        {
            Bill bill = new Bill
            {
                Id = billdto.Id,
                CustomerId = billdto.CustomerId,
                Address = billdto.Address,
                CreateDate = billdto.CreateDate,
                ShippingDate = billdto.ShippingDate,
                RequiredDate = billdto.RequiredDate,
                Status = billdto.Status,
            };
            SstoreContext context = new SstoreContext();
                context.Entry<Bill>(bill).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();

        }
        //--------------------------------------------------------------

        public void DeleteBill(int id)
        {
            try
            {
                SstoreContext context = new SstoreContext();
                context.Bills.Remove(GetBillByID(id));
                context.SaveChanges();


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
