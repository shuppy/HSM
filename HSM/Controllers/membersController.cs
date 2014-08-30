using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using HsmBI;
namespace HSM.Controllers
{
    public class membersController : Controller
    {
        private defaultcon  db = new defaultcon ();

        public ImageDimension PhotoDimesion
        {
            get{
            return new ImageDimension(202, 202);}
        }

        // GET: /men/
        public ActionResult Index()
        {
            var members = db.vwMembersList_General;// (m => m.Academy).Include(m => m.ChoirParts).Include(m => m.ChoirSplits).Include(m => m.FriendshipGroups).Include(m => m.Occupations).Include(m => m.Posts).Include(m => m.States).Include(m => m.NextOfKins);
            return View(members.Take(25).ToList());
        }

        // GET: /men/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members member = db.Members.First(x => x.MemberId == id); // db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // GET: /men/Create
        public ActionResult Create()
        {
            ViewBag.Academy_Id = new SelectList(db.Academy, "sn", "LevelCode");
            ViewBag.PartId = new SelectList(db.ChoirParts, "sn", "Part");
            ViewBag.ChoirSplit_Id = new SelectList(db.ChoirSplits, "SplitId", "Description");
            ViewBag.Friendship_Id = new SelectList(db.FriendshipGroups, "ClubId", "Code");
            ViewBag.OccupationId = new SelectList(db.Occupations, "OccupationId", "Description");
            ViewBag.PostId = new SelectList(db.Posts, "PostId", "Description");
            ViewBag.StateOfOriginId = new SelectList(db.States, "StateId", "Name");
            ViewBag.MemberId = new SelectList(db.NextOfKins, "MemberId", "LastName");
            ViewBag.Gender = new SelectList(new[] { new { Id = (int)collections.Gender.Male, Description = "Male" }, new { Id = (int)collections.Gender.Female, Description = "Female" } }, "Id", "Description");
            ViewBag.MaritalStatus = new SelectList(new[] { new { Id = (int)collections.MaritalStatus.Single, Description = "Single" }, new { Id = (int)collections.MaritalStatus.Married, Description = "Married" } }, "Id", "Description");
            return View();
        }

        // POST: /men/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="Application_id,MemberId,Code,LastName,OtherNames,Gender,Birthday,MaritalStatus,HomeAddress,OfficeAddress,MobileNo,EmailAddress,Nationality,Wedding,OccupationId,Nickname,ChoirSplit_Id,AvailabilityId,Remarks,StateOfOriginId,LGA,Photo,RobeNo,PartId,JoinedOn,PostId,HomeTown,Friendship_Id,Picture,Attendance_id,Academy_Id")] Members member, HttpPostedFileBase mfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (mfile == null)
                        throw new Exception("Please upload a photo and continue");

                    if (mfile.ContentType.Substring(0, 5).ToLower() != "image")
                        throw new Exception("The file uploaded is not a valid image type, please choose another file.");

                    var rec = this.PhotoDimesion;

                    member.RobeNo = string.Empty;
                    member.Photo = Shared.GenerateFileName(string.Empty, mfile.FileName.Split('.').Last());

                    var nuImg = Shared.ScaleImage(new System.Drawing.Bitmap(mfile.InputStream), rec.Width, rec.Height);
                        nuImg = Shared.CropImage((System.Drawing.Bitmap)nuImg, rec.Width, rec.Height);
                        nuImg.Save(string.Format("{1}\\content\\images\\photos\\{0}", member.Photo, Server.MapPath("~")));

                    db.Members.Add(member);
                    db.SaveChanges();

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
            ViewBag.Academy_Id = new SelectList(db.Academy, "sn", "LevelCode", member.Academy_Id);
            ViewBag.PartId = new SelectList(db.ChoirParts, "sn", "Part", member.PartId);
            ViewBag.ChoirSplit_Id = new SelectList(db.ChoirSplits, "SplitId", "Description", member.ChoirSplit_Id);
            ViewBag.Friendship_Id = new SelectList(db.FriendshipGroups, "ClubId", "Code", member.Friendship_Id);
            ViewBag.OccupationId = new SelectList(db.Occupations, "OccupationId", "Description", member.OccupationId);
            ViewBag.PostId = new SelectList(db.Posts, "PostId", "Description", member.PostId);
            ViewBag.StateOfOriginId = new SelectList(db.States, "StateId", "Name", member.StateOfOriginId);
            ViewBag.MemberId = new SelectList(db.NextOfKins, "MemberId", "LastName", member.MemberId);
            ViewBag.Gender = new SelectList(new[] { new { Id = (int)collections.Gender.Male, Description = "Male" }, new { Id = (int)collections.Gender.Female, Description = "Female" } }, "Id", "Description");
            ViewBag.MaritalStatus = new SelectList(new[] { new { Id = (int)collections.MaritalStatus.Single, Description = "Single" }, new { Id = (int)collections.MaritalStatus.Married, Description = "Married" } }, "Id", "Description");
            //ViewBag.Gender = new SelectList(new[] { new { Id = (int)Lib.Gender.Male, Description = "Male" }, new { Id = (int)Lib.Gender.Female, Description = "Female" } }, "Id", "Description");
            return View(member);
        }

        // GET: /men/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            ViewBag.Academy_Id = new SelectList(db.Academy, "sn", "LevelCode");
            ViewBag.PartId = new SelectList(db.ChoirParts, "sn", "Part");
            ViewBag.ChoirSplit_Id = new SelectList(db.ChoirSplits, "SplitId", "Description");
            ViewBag.Friendship_Id = new SelectList(db.FriendshipGroups, "ClubId", "Code");
            ViewBag.OccupationId = new SelectList(db.Occupations, "OccupationId", "Description");
            ViewBag.PostId = new SelectList(db.Posts, "PostId", "Description");
            ViewBag.StateOfOriginId = new SelectList(db.States, "StateId", "Name");
            ViewBag.MemberId = new SelectList(db.NextOfKins, "MemberId", "LastName");
            ViewBag.Gender = new SelectList(new[] { new { Id = (int)collections.Gender.Male, Description = "Male" }, new { Id = (int)collections.Gender.Female, Description = "Female" } }, "Id", "Description");
            ViewBag.MaritalStatus = new SelectList(new[] { new { Id = (int)collections.MaritalStatus.Single, Description = "Single" }, new { Id = (int)collections.MaritalStatus.Married, Description = "Married" } }, "Id", "Description");
            return View(member);
        }

        // POST: /men/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Application_id,MemberId,Code,LastName,OtherNames,Gender,Birthday,MaritalStatus,HomeAddress,OfficeAddress,MobileNo,EmailAddress,Nationality,Wedding,OccupationId,Nickname,ChoirSplit_Id,AvailabilityId,Remarks,StateOfOriginId,LGA,Photo,RobeNo,PartId,JoinedOn,PostId,HomeTown,Friendship_Id,Picture,Attendance_id,Academy_Id")] Members member, HttpPostedFileBase mfile)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (mfile != null)
                    {
                        if (mfile.ContentType.Substring(0, 5).ToLower() != "image")
                            throw new Exception("The file uploaded is not a valid image type, please choose another file.");

                        var rec = this.PhotoDimesion;

                        member.Photo = Shared.GenerateFileName(string.Empty, mfile.FileName.Split('.').Last());

                        var nuImg = Shared.ScaleImage(new System.Drawing.Bitmap(mfile.InputStream), rec.Width, rec.Height);
                            nuImg = Shared.CropImage((System.Drawing.Bitmap)nuImg, rec.Width, rec.Height);
                            nuImg.Save(string.Format("{1}\\content\\images\\photos\\{0}", member.Photo, Server.MapPath("~")));
                    }

                    db.Entry(member).State = EntityState.Modified;
                    db.SaveChanges();

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
            ViewBag.Academy_Id = new SelectList(db.Academy, "sn", "LevelCode");
            ViewBag.PartId = new SelectList(db.ChoirParts, "sn", "Part");
            ViewBag.ChoirSplit_Id = new SelectList(db.ChoirSplits, "SplitId", "Description");
            ViewBag.Friendship_Id = new SelectList(db.FriendshipGroups, "ClubId", "Code");
            ViewBag.OccupationId = new SelectList(db.Occupations, "OccupationId", "Description");
            ViewBag.PostId = new SelectList(db.Posts, "PostId", "Description");
            ViewBag.StateOfOriginId = new SelectList(db.States, "StateId", "Name");
            ViewBag.MemberId = new SelectList(db.NextOfKins, "MemberId", "LastName");
            ViewBag.Gender = new SelectList(new[] { new { Id = (int)HsmBI.collections.Gender.Male, Description = "Male" }, new { Id = (int)HsmBI.collections.Gender.Female, Description = "Female" } }, "Id", "Description");
            ViewBag.MaritalStatus = new SelectList(new[] { new { Id = (int)HsmBI.collections.MaritalStatus.Single, Description = "Single" }, new { Id = (int)HsmBI.collections.MaritalStatus.Married, Description = "Married" } }, "Id", "Description");
            return View(member);
        }

        // GET: /men/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Members member = db.Members.Find(id);
            if (member == null)
            {
                return HttpNotFound();
            }
            return View(member);
        }

        // POST: /men/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Members member = db.Members.Find(id);
            db.Members.Remove(member);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public JsonResult search(string query, int page, int size)
        {
            var co = string.Empty;
            //var shol = rep.GetStocks().Skip(page * size).Take(size + 1);
            var shol = db.vwMembersList_General.ToList().Where(x => x.Fullname.ToLower().Contains(query.ToLower())).OrderBy(x => x.Fullname); //.Skip(page * size).Take(size + 1);
            if (shol.Count() > 0)
            {
                foreach (var itm in shol)
                {
                    co +=
                    "<tr>\n" +
                    "    <td>\n" +
                    "        <a href=\"" + @Url.Action("details", new { id = itm.MemberId }) + "\"><img src=\"/Content/images/photos/@item.Photo\" style=\"height: 132px;\" /></a>\n" +
                    "    </td>\n" +
                    "    <td>\n" +
                    "        <h4>" + itm.Fullname + "</h4>\n" +
                    "        <p>" + string.Format("{0}, {1}", itm.MobileNo, itm.eMail) + "</p>\n" +
                    "        <p>\n" +
                    "            <a href =\"" + @Url.Action("edit", new { id = itm.MemberId }) + "\" class=\"btn btn-primary btn-xs\" role=\"button\">Edit</a>\n" +
                    "            <a href =\"" + @Url.Action("details", new {id = itm.MemberId }) + "\" class=\"btn btn-default btn-xs\" role=\"button\">Details</a>\n" +
                    "            <a href =\"" + @Url.Action("delete", new {id = itm.MemberId })  + "\" class=\"btn btn-danger btn-xs\" role=\"button\">Delete</a>\n" +
                    "        </p>\n" +
                    "    </td>\n" +
                    "</tr>";
                }
            }
            return Json(new
            {
                attrib = shol.Count() > size ? "visible" : "collapse",
                page = page + 1,
                details = co
            });
        }

        public JsonResult autocomplete(string filter)
        {
            var members = (from m in db.vwMembersList_General
                           where m.Fullname.Contains(filter)
                           select new { id = m.MemberId, m.Fullname }).ToList();
            return Json(members, JsonRequestBehavior.AllowGet);
        }

        [HttpPost ]
        public JsonResult memsnapshot(int id)
        {
            var content = string.Empty;
            //var member = (from m in db.vwMembersList_General
            //              where m.MemberId == id
            //              select new { m.Fullname, m.Photo }).Single();
            var member = db.vwMembersList_General.Where(m => m.MemberId == id).Select(m => new { m.Fullname, m.Photo }).ToList();
            // u can use return single or loop once. 

            if (member.Count > 0)
            {
                foreach (var item in member) //I know u will loop once
                {
                    string photo = item.Photo.Length < 1 ? "members.png" : item.Photo;
                    content += "<div class=\"col-md-8\"> \n" +
                    "<span> \n" +
                    "      <img alt=\"Member's picture\" src=\"/Content/images/" + photo + "\" style=\"max-width:240px; border-radius:50%\" /> \n " +
                    "</span> \n" +
                    "<p class=\"text-center text-capitalize\">" + item.Fullname + "</p> \n" +
                    "</div>";
                }
            }
            else
            {
                content = "Select a member to display the details";
            }
            return Json(new
            {
                details = content
            });
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
