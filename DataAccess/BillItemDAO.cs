using BusinessObject;
using DTO.BillItemDTO;
using DTO.CartDTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BillItemDAO
    {
        private static BillItemDAO instance = null;
        private static readonly object instanceLock = new object();
        private BillItemDAO() { }
        public static BillItemDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BillItemDAO();
                    }
                    return instance;
                }
            }
        }
        //--------------------------------------------------------------
        public List<CartDTO> GetListCart(int customerid)
        {
            List<CartDTO> list = new List<CartDTO>();
            try
            {
                SstoreContext context = new SstoreContext();
                var cart = from b in context.Bills
                           join i in context.BillItems on b.Id equals i.BillId
                           join p in context.Products on i.ProductId equals p.Id
                           where b.CustomerId == customerid 
                           where b.Status == false
                           where b.ShippingDate == null
                           where b.RequiredDate == null
                           select new
                           {
                               i.BillId,
                               i.ProductId,
                               p.ProductName,
                               p.Price,
                               p.Image,
                               i.Quantity,
                               i.TotalPrice
                           };
               foreach( var item in cart) {
                    CartDTO cartdto = new CartDTO
                    {
                        BillId = item.BillId,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Image = item.Image,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice
                    };
                    list.Add(cartdto);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;

        }

        //--------------------------------------------------------------
        public List<CartDTO> GetBill(int bid)
        {
            List<CartDTO> list = new List<CartDTO>();
            try
            {
                SstoreContext context = new SstoreContext();
                var cart = from b in context.Bills
                           join i in context.BillItems on b.Id equals i.BillId
                           join p in context.Products on i.ProductId equals p.Id
                           where b.Id == bid
                           select new
                           {
                               i.BillId,
                               i.ProductId,
                               p.ProductName,
                               p.Price,
                               p.Image,
                               i.Quantity,
                               i.TotalPrice
                           };
                foreach (var item in cart)
                {
                    CartDTO cartdto = new CartDTO
                    {
                        BillId = item.BillId,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Image = item.Image,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice
                    };
                    list.Add(cartdto);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;

        }

        //--------------------------------------------------------------
        public List<CartDTO> GetListCartPurchase(int customerid, int billid)
        {
            List<CartDTO> list = new List<CartDTO>();
            try
            { 
                SstoreContext context = new SstoreContext();
                var cart = from b in context.Bills
                           join i in context.BillItems on b.Id equals i.BillId
                           join p in context.Products on i.ProductId equals p.Id
                           where b.CustomerId == customerid
                           where b.Status == false
                           where b.Id == billid
                           where b.RequiredDate != null
                           select new
                           {
                               i.BillId,
                               i.ProductId,
                               p.ProductName,
                               p.Price,
                               p.Image,
                               i.Quantity,
                               i.TotalPrice
                           };

                foreach (var item in cart)
                {
                    CartDTO cartdto = new CartDTO
                    {
                        BillId = item.BillId,
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Image = item.Image,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice
                    };
                    list.Add(cartdto);
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;

        }
        //--------------------------------------------------------------
        public double GetTotalCart(int customerid)
        {
            double total = 0;
            try
            {
                SstoreContext context = new SstoreContext();
                var cart = from b in context.Bills
                           join i in context.BillItems on b.Id equals i.BillId
                           join p in context.Products on i.ProductId equals p.Id
                           where b.CustomerId == customerid
                           where b.Status == false
                           where b.ShippingDate == null
                           where b.RequiredDate == null
                           select new
                           {
                               i.BillId,
                               i.ProductId,
                               p.ProductName,
                               p.Price,
                               p.Image,
                               i.Quantity,
                               i.TotalPrice
                           };
                foreach (var item in cart)
                {

                    total += item.TotalPrice;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return total;

        }
        //--------------------------------------------------------------
        public double GetTotalBillDetail(int bid)
        {
            double total = 0;
            try
            {
                SstoreContext context = new SstoreContext();
                var cart = from b in context.Bills
                           join i in context.BillItems on b.Id equals i.BillId
                           join p in context.Products on i.ProductId equals p.Id
                           where b.Id == bid
                           select new
                           {
                               i.BillId,
                               i.ProductId,
                               p.ProductName,
                               p.Price,
                               p.Image,
                               i.Quantity,
                               i.TotalPrice
                           };
                foreach (var item in cart)
                {

                    total += item.TotalPrice;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return total;

        }

        //--------------------------------------------------------------
        public double GetTotalCartPurchase(int customerid, int billid)
        {
            double total = 0;
            try
            {
                SstoreContext context = new SstoreContext();
                var cart = from b in context.Bills
                           join i in context.BillItems on b.Id equals i.BillId
                           join p in context.Products on i.ProductId equals p.Id
                           where b.CustomerId == customerid
                           where b.Status == false
                           where b.Id == billid
                           where b.RequiredDate != null
                           select new
                           {
                               i.BillId,
                               i.ProductId,
                               p.ProductName,
                               p.Price,
                               p.Image,
                               i.Quantity,
                               i.TotalPrice
                           };
                foreach (var item in cart)
                {

                    total += item.TotalPrice;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return total;

        }
        //--------------------------------------------------------------
        public int GetQuantityCart(int customerid)
        {
            int quantity = 0;
            try
            {
                SstoreContext context = new SstoreContext();
                var cart = from b in context.Bills
                           join i in context.BillItems on b.Id equals i.BillId
                           join p in context.Products on i.ProductId equals p.Id
                           where b.CustomerId == customerid
                           where b.Status == false
                           where b.ShippingDate == null
                           where b.RequiredDate == null
                           select new
                           {
                               i.BillId,
                               i.ProductId,
                               p.ProductName,
                               p.Price,
                               p.Image,
                               i.Quantity,
                               i.TotalPrice
                           };
                foreach (var item in cart)
                {

                    quantity += item.Quantity;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return quantity;

        }
        //---------------------------------------------------------------
        public List<BillItem> GetItemByBillID(int bid)
        {
            List<BillItem> list = new List<BillItem>();
            try
            {
                SstoreContext context = new SstoreContext();
                list = context.BillItems.Where(i => i.BillId == bid).ToList();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return list;
        }

        //----------------------------------------------------------------
        public BillItemDTO GetItemByBillIDAndProductID(int bid, int pid)
        {
            BillItem billItem = null;
            BillItemDTO itemdto = null;
            try
            {
                SstoreContext context = new SstoreContext();
                billItem = context.BillItems.Where(i => i.BillId == bid).Where(i => i.ProductId == pid).FirstOrDefault();
                if (billItem != null)
                {
                    itemdto = new BillItemDTO
                    {
                        BillId = billItem.BillId,
                        ProductId = billItem.ProductId,
                        Quantity = billItem.Quantity,
                        TotalPrice = billItem.TotalPrice
                    };
                }

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return itemdto;
        }



        //---------------------------------------------------------------
        public void AddItem(BillItemDTO item)
        {
            BillItem billItem = new BillItem
            {
                BillId = item.BillId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice
            };

            try
            {
                SstoreContext context = new SstoreContext();
                context.BillItems.Add(billItem);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //---------------------------------------------------------------
        public void UpdateItem(BillItemDTO item)
        {
            BillItem billItem = new BillItem
            {
                BillId = item.BillId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice
            };

            try
            {
                SstoreContext context = new SstoreContext();
                context.Entry<BillItem>(billItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //--------------------------------------------------------------
        public void DeleteItem(int bid, int pid)
        {
            try
            {
                SstoreContext context = new SstoreContext();
                BillItemDTO item = GetItemByBillIDAndProductID(bid, pid);


                BillItem billItem = new BillItem
                {
                    BillId = item.BillId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    TotalPrice = item.TotalPrice
                };

                context.BillItems.Remove(billItem);
                    context.SaveChanges();
               // context.BillItems.Remove();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

    }
}
