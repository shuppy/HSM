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
    public class ChoirSplitsController : Controller
    {
        private defaultcon db = new defaultcon();
        //GET: /ChoirSplits - Splits Management
       
        public ActionResult SearchSplits(string ChoirSplit)
        {
            var choirlist = getchoirlist(ChoirSplit );
            return PartialView(choirlist);
        }

        private List<vwMembersList_General> getchoirlist(string choirsplit)
        {
            return  db.vwMembersList_General.Where( c => c.Split.Contains(choirsplit)).ToList();
            
        }
        
        public ActionResult viewsplit(FormCollection form)
        {
            //Get Choir split
            int split = Convert.ToInt32(form["ChoirSplit_Id"]);
            //Redirect to url
            return RedirectToAction("Reshuffle?split=" + split); //this will give error
        }

        [HttpPost ]
        public ActionResult viewpart(FormCollection form)
        {
            //Get Choir split
            int part = Convert.ToInt32(form["PartId"]);
            //Redirect to url
            return RedirectToAction("Reshuffle?part=" + part);
        }

        [HttpPost ]
        public ActionResult viewsplitpart(FormCollection form)
        {
            //Get Choir split
            int split = Convert.ToInt32(form["ChoirSplit_Id"]);
            //Redirect to url
            return RedirectToAction("Reshuffle?split=" + split);
        }
        [HttpPost]
        public ActionResult movetosplit(FormCollection form)
        {
            // get collection of selected members
            var selected = form.GetValues("movechk");
            int choir = Convert.ToInt32(form["Choir"]);

            //Now loop through and assign to choir one
            foreach (var id in selected )
            {
                int i = Convert.ToInt32(id);
                Members member = db.Members.Find(i);
                member.ChoirSplit_Id = choir;

                //Save
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();

                //loop
            }
            return RedirectToAction("Reshuffle", "ChoirSplits", new { split = choir });
        }
        //public async Task<ActionResult> Reshuffle()
        //{
        //    return View(await db.vwMembersList_General.ToListAsync());
        //}

        
        public ViewResult Reshuffle(string split ="0",string part ="0")
        {
            ViewData["currentSplit"] = split ?? string.Empty;
            ViewData["currentPart"] = part ?? string.Empty;
            ViewBag.Splits = new SelectList(db.ChoirSplits.OrderBy(g => g.Description), "SplitId", "Description");
            ViewBag.Parts = new SelectList(db.ChoirParts.OrderBy(p => p.Part), "sn", "Part");
            //Check if to list all
            int npart = string.IsNullOrEmpty(part) ? 0 : Convert.ToInt32(part);
            int nsplit = String.IsNullOrEmpty(split ) ? 0:  Convert.ToInt32(split);
            var list = nsplit == 0 ? 
                npart == 0? db.vwMembersList_General :
                from v in db.vwMembersList_General 
                where v.PartId == npart
                select v:
                npart == 0?
                from v in db.vwMembersList_General
                where v.SplitId == nsplit 
                select v :
                from v in db.vwMembersList_General
                where v.SplitId == nsplit && v.PartId == npart 
                select v;

            return View(list.ToList());
        }
        // GET: /ChoirSplits/
        public async Task<ActionResult> Index()
        {
            return View(await db.ChoirSplits.ToListAsync());
        }

        // GET: /ChoirSplits/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChoirSplits choirsplits = await db.ChoirSplits.FindAsync(id);
            if (choirsplits == null)
            {
                return HttpNotFound();
            }
            return View(choirsplits);
        }

        // GET: /ChoirSplits/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /ChoirSplits/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="SplitId,Description")] ChoirSplits choirsplits)
        {
            if (ModelState.IsValid)
            {
                db.ChoirSplits.Add(choirsplits);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(choirsplits);
        }

        // GET: /ChoirSplits/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChoirSplits choirsplits = await db.ChoirSplits.FindAsync(id);
            if (choirsplits == null)
            {
                return HttpNotFound();
            }
            return View(choirsplits);
        }

        // POST: /ChoirSplits/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="SplitId,Description")] ChoirSplits choirsplits)
        {
            if (ModelState.IsValid)
            {
                db.Entry(choirsplits).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(choirsplits);
        }

        // GET: /ChoirSplits/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ChoirSplits choirsplits = await db.ChoirSplits.FindAsync(id);
            if (choirsplits == null)
            {
                return HttpNotFound();
            }
            return View(choirsplits);
        }

        // POST: /ChoirSplits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            ChoirSplits choirsplits = await db.ChoirSplits.FindAsync(id);
            db.ChoirSplits.Remove(choirsplits);
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

        /* Methods */

        
    }
}
