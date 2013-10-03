using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Knockout.Controllers
{
    public class TreeController : Controller
    {
        //
        // GET: /Tree/

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetData()
        {
            List<Books> list = new List<Books>();
            list.Add(new Books() { BookId = 1, label = "Maths", ParentId = -1 });
            list.Add(new Books() { BookId = 2, label = "Physics", ParentId = -1 });
            list.Add(new Books() { BookId = 3, label = "English", ParentId = -1 });

            list.Add(new Books() { BookId = 11, label = "English Grammer", ParentId = 3 });
            list.Add(new Books() { BookId = 12, label = "Active & Passive Voice", ParentId = 3 });
            list.Add(new Books() { BookId = 13, label = "Direct & Indirect", ParentId = 3 });
            list.Add(new Books() { BookId = 14, label = "Comprehension", ParentId = 3 });
            list.Add(new Books() { BookId = 15, label = "Tenses", ParentId = 3 });
            list.Add(new Books() { BookId = 16, label = "Proverbs", ParentId = 3 });
            list.Add(new Books() { BookId = 17, label = "Easy Writing", ParentId = 3 });

            list.Add(new Books() { BookId = 8, label = "Mechanics", ParentId = 2 });
            list.Add(new Books() { BookId = 9, label = "Thermodynamics", ParentId = 2 });
            list.Add(new Books() { BookId = 10, label = "Quantum Physics", ParentId = 2 });

            list.Add(new Books() { BookId = 4, label = "Geometry", ParentId = 1 });
            list.Add(new Books() { BookId = 5, label = "Calculus", ParentId = 1 });
            list.Add(new Books() { BookId = 6, label = "Algebra", ParentId = 1 });
            list.Add(new Books() { BookId = 7, label = "Trignometry", ParentId = 1 });

            list.Add(new Books() { BookId = 18, label = "Computer", ParentId = -1 });
            list.Add(new Books() { BookId = 19, label = "Web Designing", ParentId = 18 });
            list.Add(new Books() { BookId = 20, label = "Html", ParentId = 19 });
            list.Add(new Books() { BookId = 21, label = "Html4", ParentId = 20 });
            list.Add(new Books() { BookId = 22, label = "Html5", ParentId = 20 });

            List<Books> listRefined = new List<Books>();
            listRefined = list.Where(p => p.ParentId == -1).ToList();

            foreach (var item in listRefined)
            {
                item.children = GetBooks(item.BookId, list);
            }

            return Json(listRefined, JsonRequestBehavior.AllowGet);
        }

        public List<Books> GetBooks(int BookId, List<Books> list)
        {
            List<Books> listChild = list.Where(p => p.ParentId == BookId).ToList();

            if (listChild.Count > 0)
                foreach (var item in listChild)
                    item.children = GetBooks(item.BookId, list);    
            return listChild;
        }
    }

    public class Books
    {
        public int BookId { get; set; }
        public string label { get; set; }
        public int ParentId { get; set; }
        public List<Books> children { get; set; }
    }
}
