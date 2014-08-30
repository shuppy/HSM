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
    public class VendorsController : Controller
    {
        private defaultcon db = new defaultcon();

        // GET: /Vendors/
        public async Task<ActionResult> Index()
        {
            return View(await db.Vendors.ToListAsync());
        }

        // GET: /Vendors/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendors vendors = await db.Vendors.FindAsync(id);
            if (vendors == null)
            {
                return HttpNotFound();
            }
            return View(vendors);
        }

        // GET: /Vendors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Vendors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="id,Fullname,Address,Mobile,eMail,BBMPin,BankAC,PayableAC,ReceivableAC,ExpenseAC,photo")] Vendors vendors)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Vendors.Add(vendors);
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
            return View(vendors);
        }

        // GET: /Vendors/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendors vendors = await db.Vendors.FindAsync(id);
            if (vendors == null)
            {
                return HttpNotFound();
            }
            return View(vendors);
        }

        // POST: /Vendors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="id,Fullname,Address,Mobile,eMail,BBMPin,BankAC,PayableAC,ReceivableAC,ExpenseAC,photo")] Vendors vendors)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vendors).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Details", new { id = vendors.id });
            }
            return View(vendors);
        }

        // GET: /Vendors/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Vendors vendors = await db.Vendors.FindAsync(id);
            if (vendors == null)
            {
                return HttpNotFound();
            }
            return View(vendors);
        }

        // POST: /Vendors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Vendors vendors = await db.Vendors.FindAsync(id);
            db.Vendors.Remove(vendors);
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
