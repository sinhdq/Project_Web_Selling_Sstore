using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CategoryDAO
    {
        private static CategoryDAO instance = null;
        private static readonly object instanceLock = new object();
        private CategoryDAO() { }
        public static CategoryDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CategoryDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Category> GetAllCategoryByName(string name)
        {
            //get list account by full_name
            List<Category> list = new List<Category>();
            try
            {
                SstoreContext context = new SstoreContext();
                list = context.Categories.Where(p => EF.Functions.Like(p.CategoryName, $"%{name}%")).ToList();

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return list;
        }


        //----------------------------------------------------------------
        //get category by id
        public Category GetCategoryByID(int id)
        {
            Category category = null;
            try
            {
                SstoreContext context = new SstoreContext();
                category = context.Categories.FirstOrDefault(a => a.Id == id);

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            return category;
        }

        //---------------------------------------------------------------

        public void AddCategory(Category category)
        {
            try
            {
                SstoreContext context = new SstoreContext();
                context.Categories.Add(category);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
        //---------------------------------------------------------------
        public void UpdateCategory(Category category)
        {
            try
            {
                SstoreContext context = new SstoreContext();
                context.Entry<Category>(category).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        //--------------------------------------------------------------

        public void DeleteCategory(int id)
        {
            try
            {
                SstoreContext context = new SstoreContext();
                context.Categories.Remove(GetCategoryByID(id));

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
