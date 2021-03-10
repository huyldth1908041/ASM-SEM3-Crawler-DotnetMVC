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

namespace ReadNewsWebClient.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //validate model -> get token ->  get username, role -> save username, role to session -> save token to cookie -> return to dasboard 
            //1. VALIDARE MODEL

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //2. GET TOKEN 
            var getTokenUrl = ApiEndPoint.ApiDomain + ApiEndPoint.LoginPath;
            string accessToken = "";

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    HttpContent content = new FormUrlEncodedContent(new[]
                    {
                    new KeyValuePair<string, string>("grant_type", "password"),
                    new KeyValuePair<string, string>("username", model.Username),
                    new KeyValuePair<string, string>("password", model.Password)
                });

                    HttpResponseMessage getTokenResult = httpClient.PostAsync(getTokenUrl, content).Result;
                    if (!getTokenResult.IsSuccessStatusCode)
                    {
                        //request failed
                        TempData["LoginStatus"] = "Login failed: Please check your username and password then try again!";
                        return View();
                    }

                    string resultContent = getTokenResult.Content.ReadAsStringAsync().Result;
                    Debug.WriteLine(resultContent);
                    var token = JsonConvert.DeserializeObject<TokenDataBindingModel>(resultContent);
                    accessToken = token.access_token;

                }
            }
            catch (Exception err)
            {
                Debug.WriteLine(err.Message);
                TempData["LoginStatus"] = "Can not connect to API";
                return View();
            }
            //3. Get user information
            var getUserInforEnpoint = ApiEndPoint.ApiDomain + ApiEndPoint.GetUserInfoPath;
            string role = "";
            string username = "";
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Authorization", $"bearer {accessToken}");
                    var responseMessage = client.GetAsync(getUserInforEnpoint).Result;
                    if (!responseMessage.IsSuccessStatusCode)
                    {
                        Debug.WriteLine("Get user information failed");
                        TempData["UserInfoStatus"] = "Get user information failed";
                        return View();
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
                TempData["UserInfoStatus"] = "Get user information failed: Can not connect to api";
                return View();
            }
            //4. SAVE TOKEN TO COOKIE
            //deleted existed token in cookie
            var existedTokenCookie = Request.Cookies["token"];
            if (existedTokenCookie != null)
            {
                existedTokenCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(existedTokenCookie);
            }
            else
            {
                //create new cookie to save token
                Response.Cookies["token"].Value = accessToken;
                Response.Cookies["token"].Expires = DateTime.Now.AddDays(14);
            }


            //5. Save username, role to session
            //delete if existed
            var existedUsernameSession = Session["username"];
            if (existedUsernameSession != null)
            {
                Session.Remove("username");
            }
            else
            {
                //create or update
                Session["username"] = username;
            }

            var existedRoleSession = Session["role"];
            if (existedRoleSession != null)
            {
                Session.Remove("role");
            }
            else
            {
                //create or update
                Session["role"] = role;
            }


            return RedirectToAction("Index", "Article");
        }

        public ActionResult Logout()
        {
            //delete token
            //deleted existed token in cookie
            var existedTokenCookie = Request.Cookies["token"];
            if (existedTokenCookie != null)
            {
                Debug.WriteLine("Here");
                existedTokenCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(existedTokenCookie);
            }
            //delete Session
            var existedUsernameSession = Session["username"];
            if (existedUsernameSession != null)
            {
                Session.Remove("username");
            }

            var existedRoleSession = Session["role"];
            if (existedRoleSession != null)
            {
                Session.Remove("role");
            }
            return RedirectToAction("About", "Home");
        }
    }
}