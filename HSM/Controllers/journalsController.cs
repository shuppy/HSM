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
    public class journalsController : Controller
    {
        private defaultcon db = new defaultcon();

        // GET: /journals/
        public async Task<ActionResult> Index()
        {
            var journals = db.journals.Include(j => j.AccountItems).Include(j => j.AccountItems1).Include(j => j.Dues).Include(j => j.tickets).Include(j => j.tickets1);
            return View(await journals.ToListAsync());
        }

        // GET: /journals/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            journals journals = await db.journals.FindAsync(id);
            if (journals == null)
            {
                return HttpNotFound();
            }
            return View(journals);
        }

        // GET: /journals/Create
        public ActionResult Create()
        {
            ViewBag.DrAC = new SelectList(db.AccountItems, "ID", "Code");
            ViewBag.CrAC = new SelectList(db.AccountItems, "ID", "Code");
            ViewBag.journaltype_id = new SelectList(db.Dues, "id", "Description");
            ViewBag.ticketno = new SelectList(db.tickets, "id", "TicketNo");
            ViewBag.refticketno = new SelectList(db.tickets, "id", "TicketNo");
            return View();
        }

        // POST: /journals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="id,Narration,transdate,ticketno,refticketno,DrAC,CrAC,Amount,journaltype_id,User_Id")] journals journals)
        {
            if (ModelState.IsValid)
            {
                db.journals.Add(journals);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            ViewBag.DrAC = new SelectList(db.AccountItems, "ID", "Code", journals.DrAC);
            ViewBag.CrAC = new SelectList(db.AccountItems, "ID", "Code", journals.CrAC);
            ViewBag.journaltype_id = new SelectList(db.Dues, "id", "Description", journals.journaltype_id);
            ViewBag.ticketno = new SelectList(db.tickets, "id", "TicketNo", journals.ticketno);
            ViewBag.refticketno = new SelectList(db.tickets, "id", "TicketNo", journals.refticketno);
            return View(journals);
        }

        // GET: /journals/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            journals journals = await db.journals.FindAsync(id);
            if (journals == null)
            {
                return HttpNotFound();
            }
            ViewBag.DrAC = new SelectList(db.AccountItems, "ID", "Code", journals.DrAC);
            ViewBag.CrAC = new SelectList(db.AccountItems, "ID", "Code", journals.CrAC);
            ViewBag.journaltype_id = new SelectList(db.Dues, "id", "Description", journals.journaltype_id);
            ViewBag.ticketno = new SelectList(db.tickets, "id", "TicketNo", journals.ticketno);
            ViewBag.refticketno = new SelectList(db.tickets, "id", "TicketNo", journals.refticketno);
            return View(journals);
        }

        // POST: /journals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="id,Narration,transdate,ticketno,refticketno,DrAC,CrAC,Amount,journaltype_id,User_Id")] journals journals)
        {
            if (ModelState.IsValid)
            {
                db.Entry(journals).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewBag.DrAC = new SelectList(db.AccountItems, "ID", "Code", journals.DrAC);
            ViewBag.CrAC = new SelectList(db.AccountItems, "ID", "Code", journals.CrAC);
            ViewBag.journaltype_id = new SelectList(db.Dues, "id", "Description", journals.journaltype_id);
            ViewBag.ticketno = new SelectList(db.tickets, "id", "TicketNo", journals.ticketno);
            ViewBag.refticketno = new SelectList(db.tickets, "id", "TicketNo", journals.refticketno);
            return View(journals);
        }

        // GET: /journals/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            journals journals = await db.journals.FindAsync(id);
            if (journals == null)
            {
                return HttpNotFound();
            }
            return View(journals);
        }

        // POST: /journals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            journals journals = await db.journals.FindAsync(id);
            db.journals.Remove(journals);
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
