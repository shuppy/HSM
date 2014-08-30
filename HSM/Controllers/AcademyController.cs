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
    public class AcademyController : Controller
    {
        private defaultcon db = new defaultcon();

        // GET: /Academy/
        public async Task<ActionResult> Index()
        {
            return View(await db.Academy.ToListAsync());
        }

        // GET: /Academy/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Academy academy = await db.Academy.FindAsync(id);
            if (academy == null)
            {
                return HttpNotFound();
            }
            return View(academy);
        }

        // GET: /Academy/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Academy/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="sn,LevelCode,LevelName")] Academy academy)
        {
            if (ModelState.IsValid)
            {
                db.Academy.Add(academy);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(academy);
        }

        // GET: /Academy/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Academy academy = await db.Academy.FindAsync(id);
            if (academy == null)
            {
                return HttpNotFound();
            }
            return View(academy);
        }

        // POST: /Academy/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="sn,LevelCode,LevelName")] Academy academy)
        {
            if (ModelState.IsValid)
            {
                db.Entry(academy).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(academy);
        }

        // GET: /Academy/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Academy academy = await db.Academy.FindAsync(id);
            if (academy == null)
            {
                return HttpNotFound();
            }
            return View(academy);
        }

        // POST: /Academy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Academy academy = await db.Academy.FindAsync(id);
            db.Academy.Remove(academy);
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
