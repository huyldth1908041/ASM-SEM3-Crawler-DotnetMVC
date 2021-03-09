using CrawlerApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CrawlerApi.Controllers
{
    public class CrawlerConfigController : ApiController
    {
        private static OwinApiContext _db;
        public CrawlerConfigController()
        {
            _db = new OwinApiContext();
        }
        [HttpGet]
        public IHttpActionResult GetListConfig()
        {
            var listConfigsIndb = _db.CrawlerConfigs.ToList();
            var listConfigsBinding = new List<CrawlerConfigViewModel>();
            foreach (var item in listConfigsIndb)
            {
                var bindingModel = new CrawlerConfigViewModel()
                {
                    Id = item.Id,
                    ContentSelector = item.ContentSelector,
                    DescriptionSelector = item.DescriptionSelector,
                    LinkSelector = item.LinkSelector,
                    CategoryId = item.CategoryId,
                    Path = item.Path,
                    RemovalSelector = item.RemovalSelector,
                    Route = item.Route,
                    TitleSelector = item.TitleSelector
                };
                listConfigsBinding.Add(bindingModel);
            }
            return Json(listConfigsBinding);
        }
        [HttpPost]
        public IHttpActionResult CreateCrawlerConfig(CrawlerConfigDataBindingModel crawlerConfigDataBindingModel)
        {
            //to do check trung route+path
            //List<CrawlerConfig> existedCrawlerConfigs = _db.CrawlerConfigs.Where(c => c.Route == crawlerConfigDataBindingModel.Route).
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var newConfig = new CrawlerConfig()
            {

                Route = crawlerConfigDataBindingModel.Route,
                CategoryId = crawlerConfigDataBindingModel.CategoryId,
                ContentSelector = crawlerConfigDataBindingModel.ContentSelector,
                DescriptionSelector = crawlerConfigDataBindingModel.DescriptionSelector,
                LinkSelector = crawlerConfigDataBindingModel.LinkSelector,
                RemovalSelector = crawlerConfigDataBindingModel.RemovalSelector,
                Path = crawlerConfigDataBindingModel.Path,
                TitleSelector = crawlerConfigDataBindingModel.TitleSelector

            };
            _db.CrawlerConfigs.Add(newConfig);
            _db.SaveChanges();
            return Json(newConfig);
        }
        [HttpPut]
        public IHttpActionResult UpdateCrawlerConfig(int id, [FromBody] CrawlerConfigDataBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existedConfig = _db.CrawlerConfigs.Find(id);
            if (existedConfig == null)
            {
                return NotFound();
            }
            //update
            existedConfig.CategoryId = model.CategoryId;
            existedConfig.ContentSelector = model.ContentSelector;
            existedConfig.DescriptionSelector = model.DescriptionSelector;
            existedConfig.TitleSelector = model.TitleSelector;
            existedConfig.Route = model.Route;
            existedConfig.Path = model.Path;
            existedConfig.LinkSelector = model.LinkSelector;
            existedConfig.RemovalSelector = model.RemovalSelector;
            _db.SaveChanges();
            //return view model
            var viewmodel = new CrawlerConfigViewModel
            {
                Id = existedConfig.Id,
                CategoryId = existedConfig.CategoryId,
                ContentSelector = existedConfig.ContentSelector,
                DescriptionSelector = existedConfig.DescriptionSelector,
                LinkSelector = existedConfig.LinkSelector,
                Path = existedConfig.Path,
                RemovalSelector = existedConfig.RemovalSelector,
                Route = existedConfig.Route,
                TitleSelector = existedConfig.TitleSelector
            };
        
            return Ok(viewmodel);
        }

        [HttpDelete]
        public IHttpActionResult DeleteCrawlerConfig(int id)
        {
            var existed = _db.CrawlerConfigs.Find(id);
            if (existed == null)
            {
                return NotFound();
            }
            _db.CrawlerConfigs.Remove(existed);
            _db.SaveChanges();
            return Ok();
        }
    }
}
