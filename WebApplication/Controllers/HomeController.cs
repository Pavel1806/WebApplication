using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Db;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class HomeController : Controller
    {      
        
        /// <summary>
        /// Открытие главной страницы и вывод данных в таблице
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            using (AppDb db = new AppDb())
            {
                var t = db.Profiles.Join(db.Accounts, p => p.AccountId, c => c.Id,
                    (p, c) => new ViewModel
                    {
                       Name = c.Name,
                       Email = c.Email,
                       FirstName = p.FirstName,
                       LastName = p.LastName,
                       DepId = p.DepartmentId
                    }).Join(db.Departments, b=>b.DepId, a=>a.Id, (b , a)=> new ViewModel { 
                        Title = a.Title, Name=b.Name, Email=b.Email, FirstName=b.FirstName, LastName=b.LastName
                    }).ToList();
                return View(t);
            } 
        }

        /// <summary>
        /// Открытие страницы для регистрации 
        /// </summary>
        /// <returns></returns>
        public IActionResult Registration()
        {
            return View();
        }

        /// <summary>
        /// Удаление сотрудника если выделена строка с ним
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public IActionResult Delete(string email)
        {
            using (AppDb db = new AppDb())
            {
                var t = db.Profiles.Where(p => p.Account.Email == email).FirstOrDefault();
                if(t==null)
                {
                    return RedirectToAction("Error", "Home");
                }
                else
                {
                    db.Profiles.Remove(t);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Home");
                }   
            }
        }
        /// <summary>
        /// Корректировка аккаунта сотрудника
        /// </summary>
        /// <param name="name">Ник</param>
        /// <param name="email">Изменившийся Email</param>
        /// <param name="emaill">Изменяемый Email</param>
        /// <param name="firstname">Имя</param>
        /// <param name="lastname">Фамилия</param>
        /// <param name="dep">Департамент</param>
        /// <returns></returns>
        public IActionResult EditAccount(string name, string email, string emaill, string firstname, string lastname, string dep)
        {
            using (AppDb db = new AppDb())
            {
                var r = db.Accounts.Where(p => p.Email == email).FirstOrDefault();
                if(r == null)
                {
                    var t = db.Accounts.Where(p => p.Email == emaill).FirstOrDefault();

                    t.Name = name;
                    t.Email = email;
                    db.SaveChanges();

                    return RedirectToAction("EditProfile", "Home", new { name = name, email = email, firstname = firstname, lastname = lastname, dep = dep });
                }
                else if (email == emaill)
                {
                    var t = db.Accounts.Where(p => p.Email == emaill).FirstOrDefault();

                    t.Name = name;
                    t.Email = email;
                    db.SaveChanges();

                    return RedirectToAction("EditProfile", "Home", new { name = name, email = email, firstname = firstname, lastname = lastname, dep = dep });
                }
                else
                {
                    return RedirectToAction("ErrorEmailEdit", "Home", new { email = emaill});
                }
            }  
        }
        /// <summary>
        /// Страница для изменения профиля сотрудника
        /// </summary>
        /// <param name="name">Ник</param>
        /// <param name="email">Email</param>
        /// <param name="firstname">Имя</param>
        /// <param name="lastname">Фамилия</param>
        /// <param name="dep">Департамент</param>
        /// <returns></returns>
        public IActionResult EditProfile(string name, string email,  string firstname, string lastname, string dep)
        {
            ViewBag.Name = name;
            ViewBag.Email = email;
            ViewBag.FirstName = firstname;
            ViewBag.LastName = lastname;
            ViewBag.Dep = dep;

            return View();
        }
        /// <summary>
        /// Корректировка профиля сотрудника
        /// </summary>
        /// <param name="firstname">Имя</param>
        /// <param name="lastname">Изменившаяся фамилия</param>
        /// <param name="lastnamee">Изменяемая фамилия</param>
        /// <param name="dep">Департамент</param>
        /// <returns></returns>
        public IActionResult EditNewProfile(string firstname, string lastname, string lastnamee, string dep)
        {
            using (AppDb db = new AppDb())
            {
                var t = db.Profiles.Where(p => p.LastName == lastnamee).FirstOrDefault();
                t.FirstName = firstname;
                t.LastName = lastname;
                int depid = t.DepartmentId;

                var v = db.Departments.Where(p => p.Id == depid).FirstOrDefault();
                v.Title = dep;

                db.SaveChanges();

            }
            return RedirectToAction("Index", "Home");
        }

        /// <summary>
        /// Добавление нового аккаунта
        /// </summary>
        /// <param name="name">Ник</param>
        /// <param name="email">Email</param>
        /// <returns></returns>
        public IActionResult AddingNewAccount(string name, string email)
        {
            using(AppDb db = new AppDb())
            {
                var r = db.Profiles.Where(p => p.Account.Email == email).FirstOrDefault();

                if(r==null)
                {
                    Account acc = new Account(name, email);
                    db.Add(acc);
                    Department depart = new Department();
                    db.Add(depart);
                    Profile prof = new Profile(depart, acc);
                    db.Add(prof);
                    db.SaveChanges();

                    var t = db.Profiles.Where(p => p.Account.Email == email).FirstOrDefault();
                    int depid = t.DepartmentId;

                    return RedirectToAction("NewProfile", "Home", new { name = name, email = email, depid = depid });
                }
                else
                {
                    return RedirectToAction("ErrorEmail", "Home");
                } 
            }   
        }
        /// <summary>
        /// Вызов страницы с созданным аккаунтом
        /// </summary>
        /// <param name="name">Ник</param>
        /// <param name="email">Email</param>
        /// <param name="depid">Id департамента</param>
        /// <returns></returns>
        public IActionResult NewProfile(string name, string email, int depid)
        {
                ViewBag.Name = name;
                ViewBag.Email = email;
                ViewBag.DepId = depid;

            return View();
        }
        /// <summary>
        /// Создание профиля сотрудника
        /// </summary>
        /// <param name="firstname">Имя</param>
        /// <param name="lastname">Фамилия</param>
        /// <param name="dep">Департамент</param>
        /// <param name="email">Email</param>
        /// <param name="depid">Id департамента</param>
        /// <returns></returns>
        public IActionResult AddingNewProfile(string firstname, string lastname, string dep, string email, int depid)
        {
            using (AppDb db = new AppDb())
            {

                var t = db.Profiles.Where(p => p.Account.Email == email).FirstOrDefault();
                t.FirstName = firstname;
                t.LastName = lastname;

                var v = db.Departments.Where(p => p.Id == depid).FirstOrDefault();
                v.Title = dep;

                db.SaveChanges();
            }
           return RedirectToAction("Index", "Home");
        }
        /// <summary>
        /// Корректировка аккаунта и профиля сотрудника
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns></returns>
        public IActionResult Edit(string email)
        {
            using(AppDb db = new AppDb())
            {
                var t = db.Profiles.Join(db.Accounts, p => p.AccountId, c => c.Id,
                 (p, c) => new
                {
                     Name = c.Name,
                     Email = c.Email,
                     FirstName = p.FirstName,
                     LastName = p.LastName,
                     DepId = p.DepartmentId
                }).Join(db.Departments, b => b.DepId, a => a.Id, (b, a) => new
                {
                     Title = a.Title,
                     Name = b.Name,
                     Email = b.Email,
                     FirstName = b.FirstName,
                     LastName = b.LastName
                }).Where(g => g.Email == email).FirstOrDefault();

                if(t == null)
                {
                    return RedirectToAction("Error", "Home");
                }
                else
                {
                    ViewBag.Name = t.Name;
                    ViewBag.FirstName = t.FirstName;
                    ViewBag.LastName = t.LastName;
                    ViewBag.Email = t.Email;
                    ViewBag.Department = t.Title;
                    ViewBag.Email = email;
                    return View();
                }
            }  
        }
        /// <summary>
        /// Вызов страницы ошибки "Если не выделено ни одного поля"
        /// </summary>
        /// <returns></returns>
        public IActionResult Error()
        {
            return View();
        }

        /// <summary>
        /// Вызов страницы ошибки при создании сотрудника "Email уже существует"
        /// </summary>
        /// <returns></returns>
        public IActionResult ErrorEmail()
        {
            return View();
        }

        /// <summary>
        /// Вызов страницы ошибки при корректировании сотрудника "Email уже существует"
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns></returns>
        public IActionResult ErrorEmailEdit(string email)
        {
            ViewBag.Email = email;
            return View();
        }
    }
}
