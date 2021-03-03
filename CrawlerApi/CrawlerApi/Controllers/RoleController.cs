using CrawlerApi.Models;
using IdentityConfig;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
    public class RoleController : ApiController
    {
        private static MyUserManager _userManager;
        private static MyRoleManager _roleManager;

        public RoleController()
        {
            _userManager = HttpContext.Current.GetOwinContext().Get<MyUserManager>();
            _roleManager = HttpContext.Current.GetOwinContext().Get<MyRoleManager>();

        }


        [HttpPost]
        [Authorize(Roles ="Admin")] 
        public async Task<IHttpActionResult> CreateRole([FromBody] CreateRoleBingdingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var role = new AppRole
            {
                Name = model.Name,
                Description = model.Description
            };
            var result = await _roleManager.CreateAsync(role);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Content(HttpStatusCode.Created, role);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IHttpActionResult GetListRole()
        {
            var listRole = _roleManager.Roles.ToList();
            return Json(listRole);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> AddUserToRole([FromBody] AddUserToRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.AddToRoleAsync(user.Id, role.Name);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);

            }
            return Ok();
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> RemoveRole([FromUri] string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return NotFound();
            }
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }
            return Ok();

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> RemoveUserFromRole([FromBody] AddUserToRoleBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var role = await _roleManager.FindByNameAsync(model.RoleName);
            if (role == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                return NotFound();
            }
            var result = await _userManager.RemoveFromRoleAsync(user.Id, role.Name);
            if (!result.Succeeded)
            {
                return GetErrorResult(result);
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
