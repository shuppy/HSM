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

namespace HSM.Controllers
{
    public class OccupationsController : Controller
    {
        private defaultcon db = new defaultcon();

        // GET: /Occupations/
        public async Task<ActionResult> Index()
        {
            return View(await db.Occupations.ToListAsync());
        }

        // GET: /Occupations/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Occupations occupations = await db.Occupations.FindAsync(id);
            if (occupations == null)
            {
                return HttpNotFound();
            }
            return View(occupations);
        }

        // GET: /Occupations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Occupations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="OccupationId,Description")] Occupations occupations)
        {
            if (ModelState.IsValid)
            {
                db.Occupations.Add(occupations);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(occupations);
        }

        // GET: /Occupations/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Occupations occupations = await db.Occupations.FindAsync(id);
            if (occupations == null)
            {
                return HttpNotFound();
            }
            return View(occupations);
        }

        // POST: /Occupations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="OccupationId,Description")] Occupations occupations)
        {
            if (ModelState.IsValid)
            {
                db.Entry(occupations).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(occupations);
        }

        // GET: /Occupations/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Occupations occupations = await db.Occupations.FindAsync(id);
            if (occupations == null)
            {
                return HttpNotFound();
            }
            return View(occupations);
        }

        // POST: /Occupations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Occupations occupations = await db.Occupations.FindAsync(id);
            db.Occupations.Remove(occupations);
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
