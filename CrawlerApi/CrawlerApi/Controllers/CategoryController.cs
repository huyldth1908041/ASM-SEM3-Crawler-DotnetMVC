using CrawlerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CrawlerApi.Controllers
{
    public class CategoryController : ApiController
    {
        private static OwinApiContext _db;
        public CategoryController()
        {
            _db = new OwinApiContext();
        }
        //Create Category
        public IHttpActionResult CreateCategory([FromBody]CategoryBindindModel model)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newCategory = new Category()
            {
                Name = model.Name
            };
            _db.Categories.Add(newCategory);
            _db.SaveChanges();
            //convert to view model to return
            var viewModel = new CategoryViewModel()
            {
                Id = newCategory.Id,
                Name = newCategory.Name
            };
            return Content(HttpStatusCode.Created, viewModel);
        }


        public IHttpActionResult GetListCategory()
        {
            List<Category> listCate = _db.Categories.ToList();
            if (listCate == null || listCate.Count() == 0)
            {
                return NotFound();
            }
            List<CategoryViewModel> toReturnList = new List<CategoryViewModel>();
            foreach (var item in listCate)
            {
                toReturnList.Add(new CategoryViewModel()
                {
                    Id = item.Id,
                    Name = item.Name
                });
            }
            return Json(toReturnList);
        }
        [HttpPatch]
        public IHttpActionResult UpdateCategory(int id, CategoryBindindModel model)
        {
            var existedCategory = _db.Categories.Find(id);
            if(existedCategory == null)
            {
                return NotFound();
            } 
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //update
            existedCategory.Name = model.Name;
            _db.SaveChanges();
            //comvert to view model to return
            var categoryViewModel = new CategoryViewModel()
            {
                Id = existedCategory.Id,
                Name = existedCategory.Name
            };
            return Json(categoryViewModel);
        }

        [HttpDelete]
        public IHttpActionResult DeleteCategory(int id)
        {
            var existedCategory = _db.Categories.Find(id);
            if(existedCategory == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(existedCategory);
            _db.SaveChanges();
            return Ok();
        }

    }
}
