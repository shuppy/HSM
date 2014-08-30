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
    public class nLedgersController : Controller
    {
        private defaultcon db = new defaultcon();

        // GET: /nLedgers/
        public async Task<ActionResult> Index()
        {
            var nominalledger = db.NominalLedger.Include(n => n.AccountTypes).Include(n => n.PLItems);
            return View(await nominalledger.ToListAsync());
        }

        // GET: /nLedgers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NominalLedger nominalledger = await db.NominalLedger.FindAsync(id);
            if (nominalledger == null)
            {
                return HttpNotFound();
            }
            return View(nominalledger);
        }

        // GET: /nLedgers/Create
        public ActionResult Create()
        {
            ViewBag.AccountType_id = new SelectList(db.AccountTypes, "id", "Description");
            ViewBag.PLItem_id = new SelectList(db.PLItems, "id", "Description");
            return View();
        }

        // POST: /nLedgers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="id,Description,Code,LastCode,Intervals,AccountType_id,PLItem_id")] NominalLedger nominalledger)
        {
            if (ModelState.IsValid)
            {
                db.NominalLedger.Add(nominalledger);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.AccountType_id = new SelectList(db.AccountTypes, "id", "Description", nominalledger.AccountType_id);
            ViewBag.PLItem_id = new SelectList(db.PLItems, "id", "Description", nominalledger.PLItem_id);
            return View(nominalledger);
        }

        // GET: /nLedgers/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NominalLedger nominalledger = await db.NominalLedger.FindAsync(id);
            if (nominalledger == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountType_id = new SelectList(db.AccountTypes, "id", "Description", nominalledger.AccountType_id);
            ViewBag.PLItem_id = new SelectList(db.PLItems, "id", "Description", nominalledger.PLItem_id);
            return View(nominalledger);
        }

        // POST: /nLedgers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="id,Description,Code,LastCode,Intervals,AccountType_id,PLItem_id")] NominalLedger nominalledger)
        {
            if (ModelState.IsValid)
            {
                db.Entry(nominalledger).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.AccountType_id = new SelectList(db.AccountTypes, "id", "Description", nominalledger.AccountType_id);
            ViewBag.PLItem_id = new SelectList(db.PLItems, "id", "Description", nominalledger.PLItem_id);
            return View(nominalledger);
        }

        // GET: /nLedgers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NominalLedger nominalledger = await db.NominalLedger.FindAsync(id);
            if (nominalledger == null)
            {
                return HttpNotFound();
            }
            return View(nominalledger);
        }

        // POST: /nLedgers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            NominalLedger nominalledger = await db.NominalLedger.FindAsync(id);
            db.NominalLedger.Remove(nominalledger);
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
