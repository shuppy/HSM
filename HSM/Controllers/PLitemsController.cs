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
    public class PLitemsController : Controller
    {
        private defaultcon db = new defaultcon();

        // GET: /PLitems/
        public async Task<ActionResult> Index()
        {
            return View(await db.PLItems.ToListAsync());
        }

        // GET: /PLitems/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PLItems plitems = await db.PLItems.FindAsync(id);
            if (plitems == null)
            {
                return HttpNotFound();
            }
            return View(plitems);
        }

        // GET: /PLitems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /PLitems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="id,Description")] PLItems plitems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.PLItems.Add(plitems);
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
            return View(plitems);
        }

        // GET: /PLitems/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PLItems plitems = await db.PLItems.FindAsync(id);
            if (plitems == null)
            {
                return HttpNotFound();
            }
            return View(plitems);
        }

        // POST: /PLitems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="id,Description")] PLItems plitems)
        {
            if (ModelState.IsValid)
            {
                db.Entry(plitems).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(plitems);
        }

        // GET: /PLitems/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PLItems plitems = await db.PLItems.FindAsync(id);
            if (plitems == null)
            {
                return HttpNotFound();
            }
            return View(plitems);
        }

        // POST: /PLitems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            PLItems plitems = await db.PLItems.FindAsync(id);
            db.PLItems.Remove(plitems);
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
