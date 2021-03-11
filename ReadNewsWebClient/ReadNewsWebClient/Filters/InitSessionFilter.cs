using Newtonsoft.Json;
using ReadNewsWebClient.API;
using ReadNewsWebClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace ReadNewsWebClient.Filters
{
    public class InitSessionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {

        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //init session if token existed but session is not set for user only
            var existedSession = filterContext.HttpContext.Session["username"];

            var exsitedCookie = filterContext.HttpContext.Request.Cookies["token"];
            Debug.WriteLine("filter here");
            if (exsitedCookie != null && existedSession == null)
            {
                Debug.WriteLine("INIT SESSION");
                InitAuthenSesssion(filterContext);
            }
            InitCategorySession(filterContext); ;
        }

        private void InitAuthenSesssion(ActionExecutingContext context)
        {
            var existedToken = context.HttpContext.Request.Cookies["token"];

            if (existedToken != null)
            {
                var token = existedToken.Value;

                //3. Get user information
                var getUserInforEnpoint = ApiEndPoint.ApiDomain + ApiEndPoint.GetUserInfoPath;
                string role = "";
                string username = "";
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Add("Authorization", $"bearer {token}");
                        var responseMessage = client.GetAsync(getUserInforEnpoint).Result;
                        if (!responseMessage.IsSuccessStatusCode)
                        {
                            Debug.WriteLine("Get user information failed");
                            return;
                        }
                        string jsonData = responseMessage.Content.ReadAsStringAsync().Result;
                        var userInfo = JsonConvert.DeserializeObject<UserInforViewModel>(jsonData);
                        role = userInfo.Roles.FirstOrDefault();
                        username = userInfo.UserName;
                    }
                }
                catch (Exception err)
                {
                    Debug.WriteLine(err.Message);
                    return;
                }
                //Save username, role to session
                //delete if existed
                var existedUsernameSession = context.HttpContext.Session["username"];
                if (existedUsernameSession != null)
                {
                    context.HttpContext.Session.Remove("username");
                }
                else
                {
                    //create or update
                    context.HttpContext.Session["username"] = username;
                }

                var existedRoleSession = context.HttpContext.Session["role"];
                if (existedRoleSession != null)
                {
                    context.HttpContext.Session.Remove("role");
                }
                else
                {
                    //create or update
                    context.HttpContext.Session["role"] = role;
                }
            }
        }
        private void InitCategorySession(ActionExecutingContext context)
        {
            var listCate = GetCategory();
            //create or update
            context.HttpContext.Session["categories"] = listCate;

        }

        private List<Category> GetCategory()
        {
            var list = new List<Category>();
            var getListCategory = ApiEndPoint.ApiDomain + ApiEndPoint.GetListCategoryPath;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {


                    HttpResponseMessage getListResult = httpClient.GetAsync(getListCategory).Result;
                    if (!getListResult.IsSuccessStatusCode)
                    {

                        //request failed
                        Debug.WriteLine("Null list cate");


                    }

                    var jsonResult = getListResult.Content.ReadAsStringAsync().Result;
                    var listCate = JsonConvert.DeserializeObject<List<Category>>(jsonResult);
                    list = listCate;

                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                Debug.WriteLine("Can not connect to API");

            }
            return list;

        }

    }
}