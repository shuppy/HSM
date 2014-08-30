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
    public class FriendshipGroupsController : Controller
    {
        private defaultcon db = new defaultcon();

        // GET: /FriendshipGroups/
        public async Task<ActionResult> Index()
        {
            
            return View(await db.FriendshipGroups.ToListAsync());
        }

        // GET: /FriendshipGroups/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FriendshipGroups friendshipgroups = await db.FriendshipGroups.FindAsync(id);
            if (friendshipgroups == null)
            {
                return HttpNotFound();
            }
            return View(friendshipgroups);
        }

        // GET: /FriendshipGroups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /FriendshipGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="ClubId,Description")] FriendshipGroups friendshipgroups)
        {
            if (ModelState.IsValid)
            {
                db.FriendshipGroups.Add(friendshipgroups);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(friendshipgroups);
        }

        // GET: /FriendshipGroups/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FriendshipGroups friendshipgroups = await db.FriendshipGroups.FindAsync(id);
            if (friendshipgroups == null)
            {
                return HttpNotFound();
            }
            return View(friendshipgroups);
        }

        // POST: /FriendshipGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="ClubId,Description")] FriendshipGroups friendshipgroups)
        {
            if (ModelState.IsValid)
            {
                db.Entry(friendshipgroups).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(friendshipgroups);
        }

        // GET: /FriendshipGroups/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FriendshipGroups friendshipgroups = await db.FriendshipGroups.FindAsync(id);
            if (friendshipgroups == null)
            {
                return HttpNotFound();
            }
            return View(friendshipgroups);
        }

        // POST: /FriendshipGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            FriendshipGroups friendshipgroups = await db.FriendshipGroups.FindAsync(id);
            db.FriendshipGroups.Remove(friendshipgroups);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public ViewResult Reshuffle(string club = "0", string member ="",string part="0")
        {
            
            ViewBag.Clubs = new SelectList(db.FriendshipGroups.OrderBy(g => g.Description), "ClubId", "Description");
          //  ViewBag.Members = new SelectList(db.vwMembersList_General.OrderBy(m =>m.Fullname), "MemberId", "Fullname");
            ViewBag.Parts = new SelectList(db.ChoirParts.OrderBy(p => p.Part), "sn", "Part");
            //Check if to list all
            int nclub = string.IsNullOrEmpty(club) ? 0 : Convert.ToInt32(club);
           // int xname = string.IsNullOrEmpty(member) ? 0 : Convert.ToInt32(member);
            int xpart = string.IsNullOrEmpty(part) ? 0: Convert.ToInt32(part);
            var list = nclub == 0 ?
                member == "" ?
                xpart == 0?
                from m in db.vwMembersList_General
                orderby m.Friendship_Group ,m.Fullname 
                select m : 
                from m in db.vwMembersList_General 
                where m.PartId == xpart 
                select m:
                from m in db.vwMembersList_General
                where m.Fullname.Contains(member )
                orderby m.Friendship_Group, m.Fullname
                select m :
                    from m in db.vwMembersList_General
                    where m.ClubId == nclub
                    orderby m.Friendship_Group, m.Fullname
                    select m;

            return View(list.ToList());
        }

        [HttpPost]

        public ActionResult filterclub( FormCollection form)
        {
            //Get the club id
            int club = Convert.ToInt32(form["clubs"]); //this will always return the value of the element
            return RedirectToAction("Reshuffle", "FriendshipGroups", new { club = club });
        }

        [HttpPost ]
        public ActionResult filtername(FormCollection form)
        {
            //Get the club id
            string name = form["name"].ToString(); //this will always return the value of the element
            return RedirectToAction("Reshuffle", "FriendshipGroups", new { member = name });
        }

        [HttpPost]
        public ActionResult filterpart(FormCollection form)
        {
            /*get the part id : int part = Convert.ToInt32(form["parts"]); 
            this will always return the value of the element */
            int part = Convert.ToInt32(form["parts"]);
            return RedirectToAction("Reshuffle", "FriendshipGroups", new { part = part });

        }

        [HttpPost]
        public ActionResult movetoclub(FormCollection form)
        {
            // get collection of selected members
            var selected = form.GetValues("movechk");
            int club = Convert.ToInt32(form["club"]);

            foreach (var id in selected)
            {
                int i = Convert.ToInt32(id);
                Members member = db.Members.Find(i);
                member.Friendship_Id = club;

                //Save
                db.Entry(member).State = EntityState.Modified;
                db.SaveChanges();

                //loop
            }
                

                return RedirectToAction("Reshuffle", "FriendshipGroups", new { club = club });
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
