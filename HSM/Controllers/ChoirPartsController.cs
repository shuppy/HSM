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
    public class ChoirPartsController : Controller
    {
        private defaultcon db = new defaultcon();

        // GET: /ChoirParts/
        public async Task<ActionResult> Index()
        {
            return View(await db.ChoirParts.ToListAsync());
        }

        // GET: /ChoirParts/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChoirParts choirparts = await db.ChoirParts.FindAsync(id);
            if (choirparts == null)
            {
                return HttpNotFound();
            }
            return View(choirparts);
        }

        // GET: /ChoirParts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ChoirParts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="sn,Part,Code")] ChoirParts choirparts)
        {
            if (ModelState.IsValid)
            {
                db.ChoirParts.Add(choirparts);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(choirparts);
        }

        // GET: /ChoirParts/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChoirParts choirparts = await db.ChoirParts.FindAsync(id);
            if (choirparts == null)
            {
                return HttpNotFound();
            }
            return View(choirparts);
        }

        // POST: /ChoirParts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="sn,Part,Code")] ChoirParts choirparts)
        {
            if (ModelState.IsValid)
            {
                db.Entry(choirparts).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(choirparts);
        }

        // GET: /ChoirParts/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChoirParts choirparts = await db.ChoirParts.FindAsync(id);
            if (choirparts == null)
            {
                return HttpNotFound();
            }
            return View(choirparts);
        }

        // POST: /ChoirParts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ChoirParts choirparts = await db.ChoirParts.FindAsync(id);
            db.ChoirParts.Remove(choirparts);
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
