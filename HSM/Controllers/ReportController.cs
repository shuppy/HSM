using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using HsmBI;


namespace HSM.Controllers
{
    public class ReportController : Controller
    {
        defaultcon db = new defaultcon();
        // POST Methods
        [HttpPost ]
       public JsonResult getall()
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            int total = 0; string title = "Members List - General";
            var members = (from m in db.vwMembersList_General
                           orderby m.Part,m.Split, m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>M. Status</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.MaritalStatus + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found for the choir split and part selected.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }
        
        [HttpPost]
        public JsonResult getchoirbyname(string name)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            int total = 0; string title = "Members List - Name";
            var members = (from m in db.vwMembersList_General
                           where m.Fullname.Contains(name)
                           orderby m.Part, m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>M. Status</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.MaritalStatus + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found for the choir split and part selected.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }
        [HttpPost]
        public JsonResult getchoirbysplit(int Split_Id)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            int total = 0; string title = "Members List - Choir Split ";

            var members = (from m in db.vwMembersList_General
                           where m.SplitId == Split_Id
                           orderby m.Part, m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>M. Status</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.MaritalStatus + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found for the choir split selected.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }

        [HttpPost]
        public JsonResult getchoirbypart(int Part_id)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            int total = 0; string title = "Members List - Part";
            var members = (from m in db.vwMembersList_General
                           where m.PartId == Part_id
                           orderby m.Fullname, m.Split
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>M. Status</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.MaritalStatus + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found for the choir part selected.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }

        [HttpPost]
        public JsonResult getchoirbygender(string gender)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            int total = 0; string title = "Members List - Gender ";
            var members = (from m in db.vwMembersList_General
                           where m.Gender == gender
                           orderby m.Part, m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>M. Status</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + 
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.MaritalStatus + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found for the choir part selected.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }
        [HttpPost]
        public JsonResult getchoirbystatus(string status)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            int total = 0; string title = "Members List - M. Status";
            var members = (from m in db.vwMembersList_General
                           where m.MaritalStatus == status
                           orderby m.Part, m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>M. Status</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.MaritalStatus + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found for the choir part selected.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }
        [HttpPost]
        public JsonResult getchoirbysplitnpart(int part_id, int split_id)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            int total = 0;
            string title = "Members list - Part + Split";
            var members = (from m in db.vwMembersList_General
                           where m.SplitId == split_id && m.PartId == part_id
                           orderby m.Part, m.Split, m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>M. Status</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.MaritalStatus + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found for the choir split and part selected.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }

        [HttpPost]
        public JsonResult getchoirbysplitngender(int split_id, string gender)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            int total = 0; string title = "Members List - Split + Gender";
            var members = (from m in db.vwMembersList_General
                           where m.SplitId == split_id && m.Gender == gender
                           orderby m.Part, m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>M. Status</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.MaritalStatus + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found for the choir split and part selected.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }

        [HttpPost]
        public JsonResult getchoirbysplitnstatus(int split_id, string status)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            int total = 0; string title = "Members List - Split + M. Status";
            var members = (from m in db.vwMembersList_General
                           where m.SplitId == split_id && m.MaritalStatus == status
                           orderby m.Part, m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>M. Status</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.MaritalStatus + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found for the choir split and part selected.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }
        [HttpPost]
        public JsonResult getchoirbypartnstatus(int part_id, string status)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            int total = 0;
            string title = "Members list - Part + M. Status";
            var members = (from m in db.vwMembersList_General
                           where m.PartId == part_id && m.MaritalStatus == status
                           orderby m.Split, m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>M. Status</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.MaritalStatus + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found for the choir part selected.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }

        [HttpPost]
        public JsonResult getchoirbystatusngender(string status, string gender)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            int total = 0; string title = "Members List - Gender + M. Status";
            var members = (from m in db.vwMembersList_General
                           where m.Gender == gender && m.MaritalStatus == status
                           orderby m.Part, m.Split, m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>M. Status</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.MaritalStatus + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found for the choir split and part selected.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }

        [HttpPost]
        public JsonResult getchoirbypartnsplitnstatus(int part_id, int split_id, string status)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            int total = 0; string title = "Members List - Part, Split, + M. Status";
            var members = (from m in db.vwMembersList_General
                           where m.SplitId == split_id && m.PartId == part_id && m.MaritalStatus == status
                           orderby m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>M. Status</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.MaritalStatus + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found for the choir split and part selected.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }

        [HttpPost]
        public JsonResult getchoirbysplitngendernstatus(int split_id, string gender, string status)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            int total = 0; string title = "Members List - Split, Gender, + M. Status";
            var members = (from m in db.vwMembersList_General
                           where m.SplitId == split_id && m.Gender == gender && m.MaritalStatus == status
                           orderby m.Part, m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>M. Status</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.MaritalStatus + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found for the choir split and part selected.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }
        
        [HttpGet]

        public string getmonth(int month)
        {
            string result = "None";
            if (month == 1) { result = "January"; }
            if (month == 2) { result = "February"; }
            if (month == 3) { result = "March"; }
            if (month == 4) { result = "April"; }
            if (month == 5) { result = "May"; }
            if (month == 6) { result = "June"; }
            if (month == 7) { result = "July"; }
            if (month == 8) { result = "August"; }
            if (month == 9) { result = "September"; }
            if (month == 10) { result = "October"; }
            if (month == 11) { result = "November"; }
            if (month == 12) { result = "December"; }
            return result;
        }

        [HttpGet ]

        public DateTime  GetFirstDayOfWeek (DateTime sourceDateTime)
        {
            var daysAhead = (DayOfWeek.Sunday - (int)sourceDateTime.DayOfWeek);

            sourceDateTime = sourceDateTime.AddDays((int)daysAhead);

            return sourceDateTime;
        }

        [HttpGet]
        public  DateTime GetLastDayOfWeek( DateTime sourceDateTime)
        {
            var daysAhead = DayOfWeek.Saturday - (int)sourceDateTime.DayOfWeek;

            sourceDateTime = sourceDateTime.AddDays((int)daysAhead);

            return sourceDateTime;
        }

        [HttpPost]
        public JsonResult birthdaybymonth(int month)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            
            int page = 1;
            int total = 0; string title = "Birthday List - " + getmonth(month).ToUpper();
            var members = (from m in db.vwMembersList_Birthday
                           where m.Birthday.Month == month
                           orderby m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>Birthday</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.Birthday.ToString("dd-MMM") + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }
        [HttpPost ]
        public JsonResult birthdaythisweek()
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            DateTime firstday = GetFirstDayOfWeek(DateTime.Now);
            DateTime lastday = GetLastDayOfWeek(DateTime.Now);

            int total = 0; string title = "Birthday this week";
            var members = (from m in db.vwMembersList_Birthday
                           where ((m.Birthday.Day >= firstday.Day && m.Birthday.Month == firstday.Month) &&
                           (m.Birthday.Day <= lastday.Day && m.Birthday.Month == lastday.Month))
                           orderby m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>Birthday</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.Birthday.ToString("dd-MMM") + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }

        [HttpPost ]
        public JsonResult birthdaybydate(string date)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            int page = 1;
            date = string.Format("{0:dd/MMM/yyyy}", date);

            DateTime birthday = Convert.ToDateTime( date);

            int total = 0; string title = "Birthday this day - " +  birthday.ToLongDateString();
            var members = (from m in db.vwMembersList_Birthday
                           where (m.Birthday.Day == birthday.Day && m.Birthday.Month == birthday.Month)
                           orderby m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>Birthday</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + itm.Birthday.ToString("dd-MMM") + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }

        //Weddings
        [HttpPost ]
        public JsonResult weddingbymonth(int month)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate

            int page = 1;
            int total = 0; string title = "Wedding List - " + getmonth(month).ToUpper();
            var members = (from m in db.vwMembersList_Birthday
                           where m.WeddingMonth == month
                           orderby m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>Wedding</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + getmonth(itm.WeddingMonth) + " " + itm.WeddingDay.ToString()  + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }
        [HttpPost ]
        public JsonResult weddingthisweek()
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            DateTime firstday = GetFirstDayOfWeek(DateTime.Now);
            DateTime lastday = GetLastDayOfWeek(DateTime.Now);
            int page = 1;
            int total = 0; string title = "Wedding List - This week";
            var members = (from m in db.vwMembersList_Birthday
                           where ((m.WeddingDay  >= firstday.Day && m.WeddingMonth == firstday.Month) &&
                           (m.WeddingDay  <= lastday.Day && m.WeddingMonth  == lastday.Month))
                           orderby m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>Wedding</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + getmonth(itm.WeddingMonth) + " " + itm.WeddingDay.ToString() + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }
        [HttpPost ]
        public JsonResult weddingbydate(string date)
        {
            //define variable that will hold the html content.
            var content = string.Empty;
            //get data to iterate
            date = string.Format("{0:dd/MMM/yyyy}", date);

            DateTime birthday = Convert.ToDateTime(date);
            int page = 1;
            int total = 0; string title = "Wedding this day - " + birthday.ToLongDateString();
            var members = (from m in db.vwMembersList_Birthday
                           where (m.WeddingMonth == birthday.Month && m.WeddingDay == birthday.Day)
                           orderby m.Fullname
                           select m).ToList();
            if (members.Count() > 0) //check if result contains data
            {
                content += "<table class=\"table table-striped table-bordered table-hover\">\n" +  // \"is escape delimeter for " in buidling string \n is newline
                "       <thead>\n" +
                "           <tr>\n" +
                "               <th>ID</th>\n" +
                "               <th>Fullname</th>\n" +
                "               <th>Part</th>\n" +
                "               <th>Split</th>\n" +
                "               <th>Mobile No</th>\n" +
                "               <th>eMail</th>\n" +
                "               <th>Friendship Grp</th>\n" +
                "               <th>Wedding</th>\n" +
                "           </tr>\n" +
                "       </thead>\n"; //Headers declared. Now lets do the content
                total = members.Count();
                foreach (var itm in members)
                {
                    content +=
                   "<tbody>\n" +
                        "<tr>\n" +
                   "        <td>" + itm.MemberId + "</td>\n" + //itm.EntryDate.ToString("dd-MMM-yyy HH:mm") see date formating here oo
                   "        <td>" + itm.Fullname + "</td>\n" +
                   "        <td>" + itm.Part + "</td>\n" +
                   "        <td>" + itm.Split + "</td>\n" +
                   "        <td>" + itm.MobileNo + "</td>\n" +
                   "        <td>" + itm.eMail + "</td>\n" +
                   "        <td>" + itm.Friendship_Group + "</td>\n" +
                   "        <td>" + getmonth(itm.WeddingMonth) + " " + itm.WeddingDay.ToString() + "</td>\n" +
                   "    </tr>\n" +
                   "</tbody>";
                }

            }
            else
            {
                content = "<div class=\"alert-warning\"><h4>No members found.</h4></div>";
            }
            return Json(new
            {
                page = page + 1,
                details = content,
                totalmember = "<h4><b>Members in list</b>: " + total + "</h4>",
                akole = title
            });
        }
        //Finissssssssshed...11/04/2014 23:40.. Na wa oo. no be small thing.
        // Now i got to face Friendship group.

        //
        // GET: /Report/
        public ActionResult Index()
        {
            ViewBag.Splits = new SelectList(db.ChoirSplits.OrderBy(g => g.Description), "SplitId", "Description");
            ViewBag.Parts = new SelectList(db.ChoirParts.OrderBy(p => p.Part), "sn", "Part");
            ViewBag.Months = (from HsmBI.collections.Months m in Enum.GetValues(typeof(HsmBI.collections.Months))
                              select new SelectListItem { Text = m.ToString(), Value = Convert.ToUInt16(m).ToString() });
            ViewBag.wmonths = (from HsmBI.collections.Months m in Enum.GetValues(typeof(HsmBI.collections.Months))
                              select new SelectListItem { Text = m.ToString(), Value = Convert.ToUInt16(m).ToString() });
            var list = from v in db.vwMembersList_General
                       select v;

            return View( db.vwMembersList_General.ToList());
        }
         
        public ViewResult List(string alpha , int page = 1)
        {
            ViewData["currentPage"] = alpha ?? string.Empty;
            //Check if to list all
            var list = String.IsNullOrEmpty(alpha) ?
                db.vwMembersList_General :
                from v in db.vwMembersList_General
                where v.Fullname.StartsWith(alpha )
                select v;

            return View(list.ToList());
        }

        public ViewResult Carousel()
        {
            //This will be built later
            var list = from v in db.vwMembersList_General
                select v;

            return View(list.ToList());
        }
        //
        // GET: /Report/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Report/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Report/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Report/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Report/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Report/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Report/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
