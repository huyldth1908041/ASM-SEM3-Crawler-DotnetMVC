using CrawlerApi.Models;
using IdentityConfig;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CrawlerApi.Controllers
{
    public class AccountController : ApiController
    {
        private static MyUserManager _userManager;
        private static MyRoleManager _roleManager;
        private static OwinApiContext _db;
        public AccountController()
        {
            _userManager = HttpContext.Current.GetOwinContext().Get<MyUserManager>();
            _roleManager = HttpContext.Current.GetOwinContext().Get<MyRoleManager>();
            _db = new OwinApiContext();
        }
        private IAuthenticationManager Authentication
        {
            get { return HttpContext.Current.GetOwinContext().Authentication; }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IHttpActionResult> Register([FromBody] RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new AppUser
            {
                UserName = model.UserName,
                Email = model.Email
            };
            //create user
            var createUserResult = await _userManager.CreateAsync(user, model.Password);
            if (!createUserResult.Succeeded)
            {
                return GetErrorResult(createUserResult);
            }
            return Content(HttpStatusCode.Created, user);

        }

        [Authorize]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserInfo()
        {

            //var userClaim = Authentication.User;
            //var loginId = userClaim.Identity.GetUserId();
            var loginId = User.Identity.GetUserId();
            var user = await _userManager.FindByIdAsync(loginId);

            if (user == null)
            {
                return NotFound();
            }
            var listRole = await _userManager.GetRolesAsync(user.Id);

            UserInfoViewModel viewModel = new UserInfoViewModel
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                PhoneNumber = user.PhoneNumber,
                Roles = listRole,
                Address = user.Address
            };

            return Ok(viewModel);
        }

        [Authorize(Roles ="Admin")] 
        [HttpGet]
        public IHttpActionResult GetListUser()
        {
            var list = _userManager.Users.ToList();
            var returnList = new List<UserInfoViewModel>();
            foreach (var item in list)
            {
                var listRole = _userManager.GetRoles(item.Id);
                var viewModel = new UserInfoViewModel
                {
                    Id = item.Id,
                    Email = item.Email,
                    Roles = listRole,
                    PhoneNumber = item.PhoneNumber,
                    Address = item.Address,
                    UserName = item.UserName
                };
                returnList.Add(viewModel);

            }
            return Json(returnList);
        }

        [Authorize]
        [HttpPost]
        public async Task<IHttpActionResult> ChangePassword([FromBody] ChangePasswordBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            IdentityResult result = await _userManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword,
                model.NewPassword);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            return Ok();
        }

        [Authorize]
        [HttpPatch]
        public async Task<IHttpActionResult> UpdateUserInfor([FromBody] UpdateUserInforBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var currentUserId = User.Identity.GetUserId();
            var currentUser = await _userManager.FindByIdAsync(currentUserId);
            //update
            currentUser.Address = model.Address;
            currentUser.Email = model.Email;
            currentUser.PhoneNumber = model.PhoneNumber;
            var updateResult = await _userManager.UpdateAsync(currentUser);
            if (!updateResult.Succeeded)
            {
                return GetErrorResult(updateResult);
            }
            return Ok();

        }

        //helper methods
        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
