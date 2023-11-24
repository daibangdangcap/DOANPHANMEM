using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CNPM_DOAN.Models;

namespace CNPM_DOAN.Controllers
{
    public class TAILIEUxController : Controller
    {
        private CNPM_DOANEntities db = new CNPM_DOANEntities();

        // GET: TAILIEUx
        public ActionResult Index()
        {
            var tAILIEUx = db.TAILIEUx.Include(t => t.NGUOIDUNG);
            return View(tAILIEUx.ToList());
        }

        // GET: TAILIEUx/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAILIEU tAILIEU = db.TAILIEUx.Find(id);
            if (tAILIEU == null)
            {
                return HttpNotFound();
            }
            return View(tAILIEU);
        }

        // GET: TAILIEUx/Create
        public ActionResult Create()
        {
            ViewBag.IDNguoiTao = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung");
            return View();
        }

        // POST: TAILIEUx/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDTaiLieu,TenTaiLieu,TenDuongDan,LoaiTep,IDNguoiTao")] TAILIEU tAILIEU)
        {
            if (ModelState.IsValid)
            {
                db.TAILIEUx.Add(tAILIEU);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDNguoiTao = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", tAILIEU.IDNguoiTao);
            return View(tAILIEU);
        }

        // GET: TAILIEUx/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAILIEU tAILIEU = db.TAILIEUx.Find(id);
            if (tAILIEU == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDNguoiTao = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", tAILIEU.IDNguoiTao);
            return View(tAILIEU);
        }

        // POST: TAILIEUx/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDTaiLieu,TenTaiLieu,TenDuongDan,LoaiTep,IDNguoiTao")] TAILIEU tAILIEU)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tAILIEU).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDNguoiTao = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", tAILIEU.IDNguoiTao);
            return View(tAILIEU);
        }

        // GET: TAILIEUx/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TAILIEU tAILIEU = db.TAILIEUx.Find(id);
            if (tAILIEU == null)
            {
                return HttpNotFound();
            }
            return View(tAILIEU);
        }

        // POST: TAILIEUx/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TAILIEU tAILIEU = db.TAILIEUx.Find(id);
            db.TAILIEUx.Remove(tAILIEU);
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


        public ActionResult showUserTaiLieuHocTap_PH(string iduser)
        {
            var data=db.NGUOIDUNGs.Where(s=>s.IDQuanLy==iduser);
            return View(data.ToList());
        }

        public ActionResult showTaiLieuHocTap_PH(string iduser)
        {
            var data=db.TAILIEUx.Where(s=>s.IDNguoiTao==iduser);
            return View(data.ToList());
        }

        public ActionResult createNewTaiLieu()
        {
            return PartialView();
        }

        [HttpPost]
        public ActionResult createNewTaiLieu(HttpPostedFileBase fileTaiLieu, string idnguoinhan)
        {
            if (fileTaiLieu == null)
            {
                ModelState.AddModelError("null", "Yêu cầu nhập file tài liệu");
                return PartialView();
            }
            else
            {
                byte[] data = new byte[fileTaiLieu.ContentLength];
                var tailieu = new TAILIEU();
                tailieu.IDTaiLieu = "TL" + new RANDOMID().GenerateRandomString(5);
                tailieu.TenTaiLieu = fileTaiLieu.FileName;
                tailieu.TenDuongDan = data;
                tailieu.LoaiTep = fileTaiLieu.ContentType;
                tailieu.IDNguoiTao = idnguoinhan;
                db.TAILIEUx.Add(tailieu);
                TempData["message"] = CNPM_DOAN.Resources.Language.Tạo_tài_liệu_thành_công;
                db.SaveChanges();
                return Json(new { success = true, redirectUrl = Url.Action("showTaiLieuHocTap_PH", "TAILIEUx", new { iduser = idnguoinhan }) });
            }
        }

        public ActionResult editTaiLieu(string id)
        {
            if(Request.IsAjaxRequest())
            {
                Session["IDTAILIEU"] = id;
                return PartialView();
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult editTaiLieu(HttpPostedFileBase fileTaiLieu, string idnguoitao, string idtailieu)
        {
            if (fileTaiLieu == null)
            {
                ModelState.AddModelError("null", "Yêu cầu nhập file tài liệu");
                return PartialView();
            }
            else
            {
                var data = db.TAILIEUx.Find(idtailieu);
                data.TenTaiLieu = fileTaiLieu.FileName;
                data.TenDuongDan = new byte[fileTaiLieu.ContentLength];
                data.LoaiTep = fileTaiLieu.ContentType;
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = CNPM_DOAN.Resources.Language.Chỉnh_sửa_tài_liệu_thành_công;
                return RedirectToAction("showTaiLieuHocTap_PH", "TAILIEUx", new {iduser=idnguoitao});
            }
        }

        public ActionResult deleteTaiLieu(string idtailieu, string idnguoinhan)
        {
            var data = db.TAILIEUx.Find(idtailieu);
            db.TAILIEUx.Remove(data);
            db.SaveChanges();
            TempData["message"] = CNPM_DOAN.Resources.Language.Xóa_tài_liệu_thành_công;
            return RedirectToAction("showTaiLieuHocTap_PH", "TAILIEUx", new { iduser = idnguoinhan });
        }

        public ActionResult showTaiLieuHocTap(string idhocsinh)
        {
            var hocsinh=db.NGUOIDUNGs.Find(idhocsinh);
            string idPH = hocsinh.IDQuanLy;
            var data = db.TAILIEUx.Where(s => s.IDNguoiTao == idPH);
            return View(data.ToList());
        }

        public ActionResult taiTaiLieu(string idtailieu)
        {
            var tailieu = db.TAILIEUx.Find(idtailieu);
            if (tailieu != null)
            {
                return File(tailieu.TenDuongDan, tailieu.LoaiTep, tailieu.TenTaiLieu);
            }
            return HttpNotFound();
        }
    }
}
