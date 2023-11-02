using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class CategoryManager : ICategoryService
    {
        ICategoryDal _categoryDal;

        public CategoryManager(ICategoryDal categoryDal)
        {
            _categoryDal = categoryDal;
        }

        public List<Category> GetAll()
        {
            return _categoryDal.GetAll();
        }

        public List<Category> GetById(int categoryId)
        {
            var category = _categoryDal.Get(c => c.CategoryId == categoryId);
            List<Entities.Concrete.Category> categoryList = new List<Entities.Concrete.Category> { category };
            return categoryList;
        }
    }
}
