using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Objects.SqlClient;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Microsoft.Practices.Unity;
using NProject.Models;
using NProject.Models.Infrastructure;

namespace NProject.Controllers
{
    [HandleError]
    public class AccountController : Controller
    {
        public IFormsAuthenticationService FormsService { get; set; }

        [Dependency]
        public IMembershipService MembershipService { get; set; }

        [Dependency]
        public IAccessPoint AccessPoint { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (FormsService == null) { FormsService = new FormsAuthenticationService(); }

            base.Initialize(requestContext);
        }

        // **************************************
        // URL: /Account/LogOn
        // **************************************

        public ActionResult LogOn()
        {
            if(Request.IsAuthenticated)
                return RedirectToAction("index", "home");

            return View();
        }

        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {
                    FormsService.SignIn(model.UserName, model.RememberMe);
                    if (!String.IsNullOrEmpty(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // **************************************
        // URL: /Account/LogOff
        // **************************************

        public ActionResult LogOff()
        {
            FormsService.SignOut();

            return RedirectToAction("Index", "Home");
        }

        // **************************************
        // URL: /Account/Register
        // **************************************

        [Authorize(Roles = "admin")]
        public ActionResult Register()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            var roles =
                AccessPoint.Roles.Select(
                    r => new SelectListItem {Text = r.Name, Value = SqlFunctions.StringConvert((double) r.Id)}).
                    ToList();
            ViewData["Roles"] = roles;

            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                MembershipCreateStatus createStatus = MembershipService.CreateUser(model.UserName, model.Password, model.Email, model.RoleId);

                if (createStatus == MembershipCreateStatus.Success)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", AccountValidation.ErrorCodeToString(createStatus));
                }
            }

            // If we got this far, something failed, redisplay form
            var roles =
                AccessPoint.Roles.Select(
                    r => new SelectListItem { Text = r.Name, Value = SqlFunctions.StringConvert((double)r.Id) }).
                    ToList();
            ViewData["Roles"] = roles;
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;

            return View(model);
        }

        /// <summary>
        /// Outputs list of all users in system
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles="admin")]
        public ViewResult List([DefaultValue(1)]int page)
        {
            int total;
            int itemsPerPage = 10;
            var users = MembershipService.GetUsersList(page, itemsPerPage, out total);
            return View(users);
        }

        // **************************************
        // URL: /Account/ChangePassword
        // **************************************

        [Authorize]
        public ActionResult ChangePassword()
        {
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                {
                    return RedirectToAction("ChangePasswordSuccess");
                }
                else
                {
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                }
            }

            // If we got this far, something failed, redisplay form
            ViewData["PasswordLength"] = MembershipService.MinPasswordLength;
            return View(model);
        }

        // **************************************
        // URL: /Account/ChangePasswordSuccess
        // **************************************

        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }


        [Authorize(Roles = "admin")]
        public ActionResult Edit(int id)
        {
            if (id <= 0)
            {
                ModelState.AddModelError("", "Incorrect user id");
                return View("List");
            }
            else
            {
                var roles =
                    AccessPoint.Roles.Select(
                        r => new SelectListItem {Text = r.Name, Value = SqlFunctions.StringConvert((double) r.Id)}).
                        ToList();
                var user = MembershipService.GetUserById(id);
                roles.First(r => r.Text == user.Role.Name).Selected = true;
                ViewData["Roles"] = roles;
                ViewData["Username"] = user.Username;

                //save user state enum to list
                var values = Enum.GetValues(typeof (UserState));
                var names = Enum.GetNames(typeof (UserState));
                int i = 0;
                var result = new List<SelectListItem>();
                foreach (byte v in values)
                {
                    result.Add(new SelectListItem
                                   {Text = names[i++], Value = v.ToString(), Selected = v == (byte) user.UserState});

                }
                ViewData["UserStates"] = result;
                return View(user);
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, int roleId, int userStateId)
        {
            if (ModelState.IsValid)
            {
                if(MembershipService.UpdateUser(id, roleId, userStateId))
                {
                    TempData["InformMessage"] = "User has been updated";
                    return RedirectToAction("List", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect update");
                }
            }
            var roles =
                AccessPoint.Roles.Select(
                    r => new SelectListItem {Text = r.Name, Value = SqlFunctions.StringConvert((double) r.Id)}).ToList();
            var user = MembershipService.GetUserById(id);
            roles.First(r => r.Text == user.Role.Name).Selected = true;
            ViewData["Roles"] = roles;
            //save user state enum to list
            var values = Enum.GetValues(typeof(UserState));
            var names = Enum.GetNames(typeof(UserState));
            int i = 0;
            var result = new List<SelectListItem>();
            foreach (byte v in values)
            {
                result.Add(new SelectListItem { Text = names[i++], Value = v.ToString(), Selected = v == (byte)user.UserState });

            }
            ViewData["UserStates"] = result;
            ViewData["Username"] = user.Username;
            return View();
        }
    }
}
