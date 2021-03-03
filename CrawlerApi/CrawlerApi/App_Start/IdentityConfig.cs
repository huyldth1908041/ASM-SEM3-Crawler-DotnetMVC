using CrawlerApi.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IdentityConfig
{
    public class MyUserManager : UserManager<AppUser>
    {
        public MyUserManager(IUserStore<AppUser> store) : base(store)
        {
        }

        public static MyUserManager Create(IdentityFactoryOptions<MyUserManager> options, IOwinContext context)
        {
            OwinApiContext dbContext = context.Get<OwinApiContext>();
            UserStore<AppUser> userStore = new UserStore<AppUser>(dbContext);
            var manager = new MyUserManager(userStore);
            return manager;
        }


    }

    public class MyRoleManager : RoleManager<AppRole>
    {
        public MyRoleManager(RoleStore<AppRole> store) : base(store)
        {
        }

        public static MyRoleManager Create(IdentityFactoryOptions<MyRoleManager> options, IOwinContext context)
        {
            OwinApiContext dbContext = context.Get<OwinApiContext>();
            RoleStore<AppRole> roleStore = new RoleStore<AppRole>(dbContext);
            var manager = new MyRoleManager(roleStore);
            return manager;
        }


    }
}