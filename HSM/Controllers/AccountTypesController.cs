using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using HsmBI;
using System.Data.Entity.Validation;

namespace HSM.Controllers
{
    public class AccountTypesController : Controller
    {
        private defaultcon db = new defaultcon();

        // GET: /AccountTypes/
        public async Task<ActionResult> Index()
        {
            return View(await db.AccountTypes.ToListAsync());
        }

        // GET: /AccountTypes/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountTypes accounttypes = await db.AccountTypes.FindAsync(id);
            if (accounttypes == null)
            {
                return HttpNotFound();
            }
            return View(accounttypes);
        }

        // GET: /AccountTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /AccountTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="id,Description,Code")] AccountTypes accounttypes)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.AccountTypes.Add(accounttypes);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else throw new Exception("please ensure that all required fields are filled");
            }
            catch (DbEntityValidationException e)
            {
                string er = string.Empty;
                foreach (var eve in e.EntityValidationErrors)
                {
                    er += string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        er += string.Format("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                TempData["error"] = er;
            }
            catch (Exception ex)
            {
                TempData["error"] = ex.Message;
            }
            return View(accounttypes);
        }

        // GET: /AccountTypes/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountTypes accounttypes = await db.AccountTypes.FindAsync(id);
            if (accounttypes == null)
            {
                return HttpNotFound();
            }
            return View(accounttypes);
        }

        // POST: /AccountTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="id,Description,Code")] AccountTypes accounttypes)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accounttypes).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(accounttypes);
        }

        // GET: /AccountTypes/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountTypes accounttypes = await db.AccountTypes.FindAsync(id);
            if (accounttypes == null)
            {
                return HttpNotFound();
            }
            return View(accounttypes);
        }

        // POST: /AccountTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AccountTypes accounttypes = await db.AccountTypes.FindAsync(id);
            db.AccountTypes.Remove(accounttypes);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
