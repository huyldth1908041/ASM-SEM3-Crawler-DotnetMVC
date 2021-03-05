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
            var listConfigsBinding = new List<CrawlerConfigDataBindingModel>();
            foreach(var item in listConfigsIndb)
            {
                var bindingModel = new CrawlerConfigDataBindingModel()
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
    }
}
