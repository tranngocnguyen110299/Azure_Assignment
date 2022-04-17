using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Azure_Assignment
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Shop by category",
                url: "category-{id}",
                defaults: new { controller = "Shop", action = "Index", id = UrlParameter.Optional},
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
                name: "Shop by brand",
                url: "brand-{id}",
                defaults: new { controller = "Shop", action = "ShopByBrand", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
                name: "Contact",
                url: "contact",
                defaults: new { controller = "Home", action = "Contact", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
                name: "Blog Detail",
                url: "blog-{id}",
                defaults: new { controller = "Blog", action = "BlogDetail", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
                name: "Blog Category",
                url: "blogcategory-{id}",
                defaults: new { controller = "Blog", action = "BlogList", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
                name: "Blogs",
                url: "blogs",
                defaults: new { controller = "Blog", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
                name: "User Profile",
                url: "userprofile",
                defaults: new { controller = "UserProfile", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
                name: "Product Detail",
                url: "product-{id}",
                defaults: new { controller = "ProductDetail", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
                name: "Login",
                url: "login",
                defaults: new { controller = "Login", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );
            routes.MapRoute(
                name: "Logout",
                url: "logout",
                defaults: new { controller = "Login", action = "Logout", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
               name: "Register",
               url: "register",
               defaults: new { controller = "Register", action = "Index", id = UrlParameter.Optional },
               namespaces: new[] { "Azure_Assignment.Controllers" }
           );

            routes.MapRoute(
                name: "Cart",
                url: "cart",
                defaults: new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
                name: "Add To Cart",
                url: "add-to-cart",
                defaults: new { controller = "Cart", action = "AddItem", id = UrlParameter.Optional },
                namespaces: new[] { "Azure_Assignment.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new [] { "Azure_Assignment.Controllers" }
            );
        }
    }
}
