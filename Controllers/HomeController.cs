using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using UserSignupLogin.Models;

namespace UserSignupLogin.Controllers
{
    public class HomeController : Controller
    {
        DBuserSignupLoginEntities db = new DBuserSignupLoginEntities();

        // GET: Home
        public ActionResult Index()
        {
            return View(db.TBLUserInfoes.ToList());
        }



        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Signup(TBLUserInfo tBLUserInfo)
        {
            // Kiểm tra tính hợp lệ của dữ liệu nhập
            if (!ModelState.IsValid)
            {
                ViewBag.Notification = "Vui lòng điền đầy đủ thông tin hợp lệ.";
                return View(tBLUserInfo);
            }

            // 1️⃣ Kiểm tra tài khoản đã tồn tại chưa
            var checkUser = db.TBLUserInfoes.FirstOrDefault(x => x.UsernameUs == tBLUserInfo.UsernameUs);
            if (checkUser != null)
            {
                ViewBag.Notification = "Tài khoản này đã tồn tại!";
                return View(tBLUserInfo);
            }

            // 2️⃣ Kiểm tra mật khẩu nhập lại có khớp không
            if (tBLUserInfo.PasswordUs != tBLUserInfo.RePasswordUs)
            {
                ViewBag.Notification = "Mật khẩu nhập lại không khớp!";
                return View(tBLUserInfo);
            }
            var newUser = new TBLUserInfo
            {
                UsernameUs = tBLUserInfo.UsernameUs,
                PasswordUs = tBLUserInfo.PasswordUs
            };

            db.TBLUserInfoes.Add(newUser);

            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        System.Diagnostics.Debug.WriteLine(
                            "Property: {0} Error: {1}",
                            validationError.PropertyName,
                            validationError.ErrorMessage);
                    }
                }
                throw;
            }

            // 4️⃣ Gán Session sau khi đăng ký thành công
            var userCreated = db.TBLUserInfoes.FirstOrDefault(x => x.UsernameUs == tBLUserInfo.UsernameUs);
            if (userCreated != null)
            {
                Session["IdUsSS"] = userCreated.IdUs.ToString();
                Session["UsernameSS"] = userCreated.UsernameUs.ToString();
            }

            // 5️⃣ Điều hướng về trang chủ hoặc đăng nhập
            ViewBag.Notification = "Đăng ký thành công!";
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(TBLUserInfo tBLUserInfo)
        {
            var checkLogin = db.TBLUserInfoes.Where(x => x.UsernameUs.Equals(tBLUserInfo.UsernameUs) && x.PasswordUs.Equals(tBLUserInfo.PasswordUs)).FirstOrDefault();
            if (checkLogin != null)
            {
                Session["IdUsSS"] = tBLUserInfo.IdUs.ToString();
                Session["UsernameSS"] = tBLUserInfo.UsernameUs.ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Notification = "Wrong Username or password";
            }
            return View();
        }
        // ===== DETAILS =====
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.TBLUserInfoes.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        // ===== EDIT =====
        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.TBLUserInfoes.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TBLUserInfo tBLUserInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tBLUserInfo).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tBLUserInfo);
        }

        // ===== DELETE =====
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = db.TBLUserInfoes.Find(id);
            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var user = db.TBLUserInfoes.Find(id);
            if (user != null)
            {
                db.TBLUserInfoes.Remove(user);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

    }
}