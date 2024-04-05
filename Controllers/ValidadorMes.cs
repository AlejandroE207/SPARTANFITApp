using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SPARTANFITApp.Controllers
{
    public class ValidadorMes : Controller
    {
        // GET: ValidadorMes
        public ActionResult Index()
        {
            return View();
        }

        // GET: ValidadorMes/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ValidadorMes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ValidadorMes/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ValidadorMes/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ValidadorMes/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ValidadorMes/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ValidadorMes/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
