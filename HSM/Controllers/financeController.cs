using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Common;
using HsmBI;
using System.Data.Entity.Validation;

namespace HSM.Controllers
{
    public class financeController : Controller
    {
        private defaultcon db = new defaultcon();

        public  ActionResult openingbalance()
        {
            return View();
        }

        #region Transactions
        public ActionResult dues()
        {
            ViewBag.members = new SelectList(db.vwMembersList_General ,"MemberId", "Fullname");
            ViewBag.due_id = new SelectList(from d in db.Dues where d.Source == "Member" select d, "id", "Description");
            
            return View();
        }

        [HttpPost ]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> dues([Bind(Include="id,Member_id,Amount,Date,Due_id,Narration")] Transactions due)
        {
            //Start db Transaction here.
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        //Raise Journal first
                        //Raise Ticket
                        string ticket = Shared.generateticket();

                        //Get new Ticket id: first save ticket then get the id
                        //Note: This will not be commited to db until savechanges is called

                        tickets tk = new tickets(); tk.TicketNo = ticket; db.tickets.Add(tk);
                        await db.SaveChangesAsync();
                        //Raise Journal
                        var fullname = (from m in db.vwMembersList_General
                                        where m.MemberId == due.Member_id
                                        select new { m.Fullname }).Take(1);

                        var accounts = (from a in db.Dues
                                        where a.id == due.Due_id
                                        select new { a.drAC, a.crAC }).Take(1);

                        string narration = string.Concat(due.Narration, " - ", fullname.FirstOrDefault().Fullname);

                        int drAC = accounts.FirstOrDefault().drAC;
                        int crAC = accounts.FirstOrDefault().crAC;

                        int i = db.raiseJournal(narration, due.Date, tk.id, tk.id, drAC, crAC, due.Amount, due.Due_id, 1);

                        //save changes too. I need the ledger id for next operation
                        await db.SaveChangesAsync();
                        //Check if successfull
                        
                        //journals journal = new journals();
                        //journal.Amount = due.Amount;
                        //journal.CrAC = due.Dues.crAC;
                        //journal.DrAC = due.Dues.drAC;
                        //journal.transdate = due.Date;
                        //journal.ticketno = tk.id;
                        //journal.refticketno = tk.id;

                        //journal.journaltype_id = due.Dues.id;
                        //journal.User_Id = 1;

                        //db.journals.Add(journal);

                        ////Raise Ledger
                        //ledger ledger = new ledger();


                        //Then raise transaction
                        //Get the ledger_id from Journal via Ticket.id
                        var ledgerid = (from l in db.journals
                                        where
                                            l.ticketno == tk.id
                                        select l.id).Take(1);

                        due.journal_id = Convert.ToInt32(ledgerid.FirstOrDefault());
                        due.Narration = narration;

                        db.Transactions.Add(due);
                        //db.AccountItems.Add(accountitems);
                        await db.SaveChangesAsync();

                        //Commit Transaction
                        trans.Commit();
                        return RedirectToAction("dues");
                    }
                    else throw new Exception("please ensure that all required fields are filled");
                }
                catch (DbEntityValidationException e)
                {
                    //Rollback db Transaction here.
                    trans.Rollback();
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
                    //Rollback db Transaction here.
                    trans.Rollback();
                    TempData["error"] = ex.Message;
                }
            }
      
            ViewBag.members = new SelectList(db.vwMembersList_General, "MemberId", "Fullname");
            ViewBag.due_id = new SelectList(from d in db.Dues where d.Source == "Member" select d, "id", "Description");
            return View();
        }

        public ActionResult income()
        {
            ViewBag.journaltype_id = new SelectList( from d in db.Dues where d.Source == "Ledger" select d, "id", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> income([Bind(Include = "id,Narration,transdate,Amount,journaltype_id")] journals journal)
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        //Raise Journal first
                        //Raise Ticket
                        string ticket = Shared.generateticket();

                        //Get new Ticket id: first save ticket then get the id
                        //Note: This will not be commited to db until savechanges is called

                        tickets tk = new tickets(); tk.TicketNo = ticket; db.tickets.Add(tk);
                        await db.SaveChangesAsync();
                        //Raise Journal

                        var accounts = (from a in db.Dues
                                        where a.id == journal.journaltype_id 
                                        select new { a.drAC, a.crAC }).Take(1);


                        int drAC = accounts.FirstOrDefault().drAC;
                        int crAC = accounts.FirstOrDefault().crAC;

                        int i = db.raiseJournal(journal.Narration, journal.transdate, tk.id, tk.id, drAC, crAC, journal.Amount, journal.journaltype_id , 1);

                        //save changes too. I need the ledger id for next operation
                        await db.SaveChangesAsync();
                        trans.Commit ();
                    }
                    else throw new Exception("please ensure that all required fields are filled");
                }
                catch (DbEntityValidationException valerr)
                {
                    //Rollback db Transaction here.
                    trans.Rollback();
                    string er = string.Empty;
                    foreach (var eve in valerr.EntityValidationErrors)
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
                    //Rollback db Transaction here.
                    trans.Rollback();
                    TempData["error"] = ex.Message;
                }
            }

            ViewBag.journaltype_id = new SelectList(from d in db.Dues where d.Source == "Ledger" select d, "id", "Description");
            return View();
        }

        public ActionResult expenses()
        {
            ViewBag.Vendor = new SelectList(from v in db.Vendors select v, "id", "Fullname");
            ViewBag.journaltype_id = new SelectList(from d in db.Dues where d.Source == "Ledger" select d, "id", "Description");
            return View();
        }

        [HttpPost ]
        [ValidateAntiForgeryToken]

        public async Task<ActionResult > expenses([Bind(Include = "id,Narration,transdate,Amount,journaltype_id")] journals journal, int vendor_id = 0 )
        {
            using (DbContextTransaction trans = db.Database.BeginTransaction())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        //Raise Journal first
                        //Raise Ticket
                        string ticket = Shared.generateticket();

                        //Get new Ticket id: first save ticket then get the id
                        //Note: This will not be commited to db until savechanges is called

                        tickets tk = new tickets(); tk.TicketNo = ticket; db.tickets.Add(tk);
                        await db.SaveChangesAsync();
                        //Raise Journal
                        var fullname = (from m in db.Vendors
                                        where m.id == vendor_id 
                                        select new { m.Fullname }).Take(1);

                        var accounts = (from a in db.Dues
                                        where a.id == journal.journaltype_id
                                        select new { a.drAC, a.crAC }).Take(1);


                        int drAC = accounts.FirstOrDefault().drAC;
                        int crAC = accounts.FirstOrDefault().crAC;

                        string narration = string.Concat(journal.Narration, " - ", fullname.FirstOrDefault().Fullname);

                        int i = db.raiseJournal(narration , journal.transdate, tk.id, tk.id, drAC, crAC, journal.Amount, journal.journaltype_id, 1);

                        //save changes too. I need the ledger id for next operation
                        await db.SaveChangesAsync();
                        trans.Commit();
                    }
                    else throw new Exception("please ensure that all required fields are filled");
                }
                catch (DbEntityValidationException valerr)
                {
                    //Rollback db Transaction here.
                    trans.Rollback();
                    string er = string.Empty;
                    foreach (var eve in valerr.EntityValidationErrors)
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
                    //Rollback db Transaction here.
                    trans.Rollback();
                    TempData["error"] = ex.Message;
                }
            }

            ViewBag.Vendor = new SelectList(from v in db.Vendors select v, "id", "Fullname");
            ViewBag.journaltype_id = new SelectList(from d in db.Dues where d.Source == "Ledger" select d, "id", "Description");
            return View();
        }

        #endregion

        #region AccountItems
        public async Task<ActionResult> AccountItems()
        {
            var accountitems = db.AccountItems.Include(a => a.NominalLedger);
            return View(await accountitems.ToListAsync());
        }

        // GET: /AccountItems/Create
        public ActionResult CreateAccount()
        {
            ViewBag.Ledger_Id = new SelectList(db.NominalLedger, "id", "Description");
            return View();
        }

        // POST: /AccountItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAccount([Bind(Include = "ID,Ledger_Id,Description")] AccountItems accountitems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    int result = db.CreateLedgerAccount(accountitems.Ledger_Id, accountitems.Description, false);
                    //db.AccountItems.Add(accountitems);
                    await db.SaveChangesAsync();
                    return RedirectToAction("AccountItems");
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

            ViewBag.Ledger_Id = new SelectList(db.NominalLedger, "id", "Description", accountitems.Ledger_Id);
            return View(accountitems);
        }

        // GET: /AccountItems/Edit/5
        public async Task<ActionResult> EditAccount(int? id)
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
        public async Task<ActionResult> EditAccount([Bind(Include = "ID,Code,Ledger_Id,Description")] AccountItems accountitems)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(accountitems).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("AccountItems");
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

            ViewBag.Ledger_Id = new SelectList(db.NominalLedger, "id", "Description", accountitems.Ledger_Id);
            return View(accountitems);
        }

        // GET: /AccountItems/Delete/5
        public async Task<ActionResult> DeleteAccount(int? id)
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
        public async Task<ActionResult> DeleteAccountConfirmed(int id)
        {
            AccountItems accountitems = await db.AccountItems.FindAsync(id);
            db.AccountItems.Remove(accountitems);
            await db.SaveChangesAsync();
            return RedirectToAction("AccountItems");
        }
        #endregion

        #region NominalLedger
        // GET: /Ledgers/
        public async Task<ActionResult> nLedgers()
        {
            var nominalledger = db.NominalLedger.Include(n => n.AccountTypes).Include(n => n.PLItems);
            return View(await nominalledger.ToListAsync());
        }


        // GET: /Ledgers/Create
        public ActionResult CreateLedger()
        {
            ViewBag.AccountType_id = new SelectList(db.AccountTypes, "id", "Description");
            ViewBag.PLItem_id = new SelectList(db.PLItems, "id", "Description");
            return View();
        }

        // POST: /Ledgers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateLedger([Bind(Include = "id,Description,Code,LastCode,Intervals,AccountType_id,PLItem_id")] NominalLedger nominalledger)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.NominalLedger.Add(nominalledger);
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

            ViewBag.AccountType_id = new SelectList(db.AccountTypes, "id", "Description", nominalledger.AccountType_id);
            ViewBag.PLItem_id = new SelectList(db.PLItems, "id", "Description", nominalledger.PLItem_id);
            return View(nominalledger);
        }

        // GET: /Ledgers/Edit/5
        public async Task<ActionResult> EditLedger(int? id)
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

        // POST: /Ledgers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditLedger([Bind(Include = "id,Description,Code,LastCode,Intervals,AccountType_id,PLItem_id")] NominalLedger nominalledger)
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

        // GET: /Ledgers/Delete/5
        public async Task<ActionResult> DeleteLedgerAC(int? id)
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

        // POST: /Ledgers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteLedgerACConfirmed(int id)
        {
            NominalLedger nominalledger = await db.NominalLedger.FindAsync(id);
            db.NominalLedger.Remove(nominalledger);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        #endregion

        #region reports

        public ActionResult reports()
        {
            return View();
        }
        #endregion

        // GET: /finance/
        public async Task<ActionResult> Index()
        {
            var accountitems = db.AccountItems.Include(a => a.NominalLedger);
            return View(await accountitems.ToListAsync());
        }

        // GET: /finance/Details/5
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

        // GET: /finance/Create
        public ActionResult Create()
        {
            ViewBag.Ledger_Id = new SelectList(db.NominalLedger, "id", "Description");
            return View();
        }

        // POST: /finance/Create
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

        // GET: /finance/Edit/5
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

        // POST: /finance/Edit/5
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

        // GET: /finance/Delete/5
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

        // POST: /finance/Delete/5
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
