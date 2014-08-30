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
    public class AccountItemsController : Controller
    {
        private defaultcon db = new defaultcon();

        // GET: /AccountItems/
        public async Task<ActionResult> Index()
        {
            var accountitems = db.AccountItems.Include(a => a.NominalLedger);
            return View(await accountitems.ToListAsync());
        }

        // GET: /AccountItems/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountItems accountitems = await db.AccountItems.FindAsync(id);
            if (accountitems == null)
            {
                return HttpNotFound();
            }
            return View(accountitems);
        }

        // GET: /AccountItems/Create
        public ActionResult Create()
        {
            ViewBag.Ledger_Id = new SelectList(db.NominalLedger, "id", "Description");
            return View();
        }

        // POST: /AccountItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ID,Code,Ledger_Id,Description,Debit,Credit,Balance,ControlAC,Ref,Status")] AccountItems accountitems)
        {
            if (ModelState.IsValid)
            {
                db.AccountItems.Add(accountitems);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.Ledger_Id = new SelectList(db.NominalLedger, "id", "Description", accountitems.Ledger_Id);
            return View(accountitems);
        }

        // GET: /AccountItems/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountItems accountitems = await db.AccountItems.FindAsync(id);
            if (accountitems == null)
            {
                return HttpNotFound();
            }
            ViewBag.Ledger_Id = new SelectList(db.NominalLedger, "id", "Description", accountitems.Ledger_Id);
            return View(accountitems);
        }

        // POST: /AccountItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ID,Code,Ledger_Id,Description,Debit,Credit,Balance,ControlAC,Ref,Status")] AccountItems accountitems)
        {
            if (ModelState.IsValid)
            {
                db.Entry(accountitems).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.Ledger_Id = new SelectList(db.NominalLedger, "id", "Description", accountitems.Ledger_Id);
            return View(accountitems);
        }

        // GET: /AccountItems/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AccountItems accountitems = await db.AccountItems.FindAsync(id);
            if (accountitems == null)
            {
                return HttpNotFound();
            }
            return View(accountitems);
        }

        // POST: /AccountItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            AccountItems accountitems = await db.AccountItems.FindAsync(id);
            db.AccountItems.Remove(accountitems);
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
