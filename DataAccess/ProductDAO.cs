using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ProductDAO
    {
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();
        private ProductDAO() { }
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Product> GetAllProductByName(string cid,string name)
        {
            //get list account by full_name
            List<Product> list = new List<Product>();
            try
            {
                SstoreContext context = new SstoreContext();
                list = context.Products.Where(p => EF.Functions.Like(p.ProductName, $"%{name}%"))
                                       .Where(p => EF.Functions.Like(p.CategoryId.ToString(), $"%{cid}%")).ToList();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return list;
        }

        //----------------------------------------------------------------
        //get product by id
        public Product GetProductByID(int id)
        {
            Product product = null;
            try
            {
                SstoreContext context = new SstoreContext();
                product = context.Products.FirstOrDefault(a => a.Id == id);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return product;
        }

        //---------------------------------------------------------------
        public void AddProduct(Product product)
        {
            try
            {
                SstoreContext context = new SstoreContext();
                context.Products.Add(product);
                context.SaveChanges();
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //---------------------------------------------------------------
        public void UpdateProduct(Product product)
        {
            try
            {
                SstoreContext context = new SstoreContext();
                context.Entry<Product>(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                context.SaveChanges();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //--------------------------------------------------------------

        public void DeleteProduct(int id)
        {
            try
            {
                SstoreContext context = new SstoreContext();
                context.Products.Remove(GetProductByID(id));
                context.SaveChanges();

            }
            catch (Exception e)
            {
                SstoreContext context = new SstoreContext();
                Product product = GetProductByID(id);

                product.Quantity = 0;

                UpdateProduct(product);
            }
        }

    }
}
