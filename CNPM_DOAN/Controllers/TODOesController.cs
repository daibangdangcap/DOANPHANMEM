using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CNPM_DOAN.Models;

namespace CNPM_DOAN.Controllers
{
    public class TODOesController : Controller
    {
        private CNPM_DOANEntities db = new CNPM_DOANEntities();

        // GET: TODOes
        public ActionResult Index()
        {
            var tODOes = db.TODOes.Include(t => t.NGUOIDUNG);
            return View(tODOes.ToList());
        }

        // GET: TODOes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TODO tODO = db.TODOes.Find(id);
            if (tODO == null)
            {
                return HttpNotFound();
            }
            return View(tODO);
        }

        // GET: TODOes/Create
        public ActionResult Create()
        {
            ViewBag.IDNguoiDung = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung");
            return View();
        }

        // POST: TODOes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDToDo,NDToDo,NgayBatDau,NgayHoanThanh,HanChot,TrangThai,IDNguoiDung")] TODO tODO)
        {
            if (ModelState.IsValid)
            {
                db.TODOes.Add(tODO);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDNguoiDung = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", tODO.IDNguoiDung);
            return View(tODO);
        }

        // GET: TODOes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TODO tODO = db.TODOes.Find(id);
            if (tODO == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDNguoiDung = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", tODO.IDNguoiDung);
            return View(tODO);
        }

        // POST: TODOes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDToDo,NDToDo,NgayBatDau,NgayHoanThanh,HanChot,TrangThai,IDNguoiDung")] TODO tODO)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tODO).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDNguoiDung = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", tODO.IDNguoiDung);
            return View(tODO);
        }

        // GET: TODOes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TODO tODO = db.TODOes.Find(id);
            if (tODO == null)
            {
                return HttpNotFound();
            }
            return View(tODO);
        }

        // POST: TODOes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TODO tODO = db.TODOes.Find(id);
            db.TODOes.Remove(tODO);
            db.SaveChanges();
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
        [HttpGet]
        public ActionResult showToDo(string iduser)
        {
            if(iduser.Contains("HS"))
            {
                var data = db.TODOes.Where(s => s.IDNguoiDung.Equals(iduser) && (s.TrangThai.Equals("Còn hạn") || s.TrangThai.Equals("Qúa hạn"))).ToList();
                foreach (var item in data)
                {
                    if (checkUpdate(item) == false) updateTrangThai(item.IDToDo, item.IDNguoiDung);
                }
                return View(data);
            }
            else
            {
                return RedirectToAction("showUserToDo_PH", "TODOes", new {iduser });
            }
        }
        [HttpPost]
        public ActionResult TaoToDo(string id, string ndtodo)
        {
            if (ndtodo == null)
            {
                return RedirectToAction("showToDo", "TODOes", new { iduser = id });
            }
            if (ModelState.IsValid)
            {
                TODO todo = themMoiTODO(ndtodo, id);
                
                todo.IDToDo = id + "TD" + new RANDOMID().GenerateRandomString(2);
                db.TODOes.Add(todo);
                db.SaveChanges();
                return RedirectToAction("showToDo", "TODOes", new { iduser = id });
            }
            return View();
        }
        public ActionResult CompleteToDo(string IDToDo, string id)
        {
            submitTodo(IDToDo, id);
            return RedirectToAction("showToDo", "TODOes", new { iduser = id });
        }
        public ActionResult DeleteToDo(string IDToDo, string id)
        {
            var data = db.TODOes.Find(IDToDo);
            db.TODOes.Remove(data);
            db.SaveChanges();
            return RedirectToAction("showToDo", "TODOes", new { iduser = id });
        }
        [HttpPost]
        public ActionResult UpdateTrangThai(string IDToDo, string id)
        {
            updateTrangThai(IDToDo, id);
            return RedirectToAction("showToDo", "TODOes", new { iduser = id });
        }
        public ActionResult UpdateToDo(string IDToDo, string id, string newNDToDo)
        {
            updateTodo(IDToDo, newNDToDo);
            return RedirectToAction("showToDo", "TODOes", new { iduser = id });
        }
        public ActionResult showUserToDo_PH( string iduser) 
        {
            var data=db.NGUOIDUNGs.Where(s=>s.IDQuanLy==iduser&&s.IDNguoiDung!=iduser);
            return View(data.ToList());
        }
        public ActionResult showToDo_PH(string iduser)
        {
            var data=db.TODOes.Where(s=>s.IDNguoiDung==iduser).ToList();
            return View(data.ToList());
        }

        public TODO themMoiTODO(string NDTODO, string id)
        {
            TODO todo=new TODO();
            todo.NDToDo = NDTODO;
            todo.IDNguoiDung = id;
            var timenow = DateTime.Now;
            todo.NgayBatDau = timenow;
            todo.NgayHoanThanh = null;
            //DateTime.ParseExact($"{timenow.Month}/{timenow.Day}/{timenow.Year} {11 - timenow.Hour}:{59 - timenow.Minute}:{59 - timenow.Second} PM", "MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            //this.HanChot = timenow.AddDays(1).AddSeconds(-1);
            todo.HanChot = timenow.AddDays(0).AddMonths(0).AddYears(0).AddHours(23 - timenow.Hour).AddMinutes(59 - timenow.Minute).AddSeconds(59 - timenow.Second);
            todo.TrangThai = "Còn hạn";
            return todo;
        }
        public void submitTodo(string IDToDo, string id)
        {
            var data = db.TODOes.Find(IDToDo);
            data.TrangThai = "Đã hoàn thành";
            data.NgayHoanThanh = DateTime.Now;
            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void updateTrangThai(string IDToDo, string id)
        {
            var data = db.TODOes.Find(IDToDo);
            data.TrangThai = "Qúa hạn";
            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
        }
        public void updateTodo(string IDToDo, string newNDtodo)
        {
            var data = db.TODOes.Find(IDToDo);
            data.NDToDo = newNDtodo;
            TODO tODO = data;
            db.Entry(tODO).State = EntityState.Modified;
            db.SaveChanges();
        }
        public bool checkUpdate(TODO todo)
        {
            if (todo.HanChot < DateTime.Now) return false;
            else return true;
        }

    }
}
