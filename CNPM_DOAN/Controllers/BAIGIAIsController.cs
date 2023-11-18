using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CNPM_DOAN.Models;
using Microsoft.Ajax.Utilities;

namespace CNPM_DOAN.Controllers
{
    public class BAIGIAIsController : Controller
    {
        private CNPM_DOANEntities db = new CNPM_DOANEntities();

        // GET: BAIGIAIs
        public ActionResult Index()
        {
            var bAIGIAIs = db.BAIGIAIs.Include(b => b.BAITAP).Include(b => b.NGUOIDUNG);
            return View(bAIGIAIs.ToList());
        }

        // GET: BAIGIAIs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BAIGIAI bAIGIAI = db.BAIGIAIs.Find(id);
            if (bAIGIAI == null)
            {
                return HttpNotFound();
            }
            return View(bAIGIAI);
        }

        // GET: BAIGIAIs/Create
        public ActionResult Create()
        {
            ViewBag.IDBaiTap = new SelectList(db.BAITAPs, "IDBaiTap", "IDNhom");
            ViewBag.IDNguoiDung = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung");
            return View();
        }

        // POST: BAIGIAIs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDBaiGiai,TenBaiGiai,DuongDan,LoaiTep,SoDiem,NgayHoanThanh,IDNguoiDung,IDBaiTap")] BAIGIAI bAIGIAI)
        {
            if (ModelState.IsValid)
            {
                db.BAIGIAIs.Add(bAIGIAI);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDBaiTap = new SelectList(db.BAITAPs, "IDBaiTap", "IDNhom", bAIGIAI.IDBaiTap);
            ViewBag.IDNguoiDung = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", bAIGIAI.IDNguoiDung);
            return View(bAIGIAI);
        }

        // GET: BAIGIAIs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BAIGIAI bAIGIAI = db.BAIGIAIs.Find(id);
            if (bAIGIAI == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDBaiTap = new SelectList(db.BAITAPs, "IDBaiTap", "IDNhom", bAIGIAI.IDBaiTap);
            ViewBag.IDNguoiDung = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", bAIGIAI.IDNguoiDung);
            return View(bAIGIAI);
        }

        // POST: BAIGIAIs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDBaiGiai,TenBaiGiai,DuongDan,LoaiTep,SoDiem,NgayHoanThanh,IDNguoiDung,IDBaiTap")] BAIGIAI bAIGIAI)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bAIGIAI).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDBaiTap = new SelectList(db.BAITAPs, "IDBaiTap", "IDNhom", bAIGIAI.IDBaiTap);
            ViewBag.IDNguoiDung = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", bAIGIAI.IDNguoiDung);
            return View(bAIGIAI);
        }

        // GET: BAIGIAIs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BAIGIAI bAIGIAI = db.BAIGIAIs.Find(id);
            if (bAIGIAI == null)
            {
                return HttpNotFound();
            }
            return View(bAIGIAI);
        }

        // POST: BAIGIAIs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BAIGIAI bAIGIAI = db.BAIGIAIs.Find(id);
            db.BAIGIAIs.Remove(bAIGIAI);
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
        public bool KTBaiGiai(string iduser, string idbaitap)
        {
            var data = db.BAIGIAIs.Where(s => s.IDBaiTap == idbaitap && s.IDNguoiDung == iduser).ToList();
            if (data.Count == 0) return false;
            else return true;
        }
        public ActionResult ChiTietBaiGiai(string idbaitap, string iduser)
        {
            if (idbaitap != null && iduser != null)
            {
                Session["IDBAITAP"] = idbaitap;
                var data = db.BAIGIAIs.Where(s => s.IDBaiTap == idbaitap && s.IDNguoiDung == iduser).FirstOrDefault();
                if (KTBaiGiai(iduser, idbaitap) == true)
                {
                    data = db.BAIGIAIs.Where(s => s.IDBaiTap == idbaitap && s.IDNguoiDung == iduser).FirstOrDefault();
                    return PartialView(data);
                }
                else
                {
                    return PartialView();
                }
            }
            else return RedirectToAction("Index","Home");
        }
        public ActionResult NopBaiTap()
        {
            return PartialView();
        }
        [HttpPost]
        public ActionResult NopBaiTap(HttpPostedFileBase bg,string idnguoinop, string idbt) 
        {
            if (bg != null && bg.ContentLength > 0)
            {
                byte[] data = new byte[bg.ContentLength];
                bg.InputStream.Read(data, 0, data.Length);
                var baigiai = new BAIGIAI
                {
                    TenBaiGiai = bg.FileName,
                    DuongDan = data,
                    LoaiTep = bg.ContentType,
                    IDBaiGiai = "BG" + new RANDOMID().GenerateRandomString(3),
                    NgayHoanThanh = DateTime.Now,
                    IDNguoiDung =idnguoinop,
                    IDBaiTap= idbt,
                    SoDiem=null
                };
                db.BAIGIAIs.Add(baigiai);
                var baitap=db.BAITAPs.Find(idbt);
                baitap.TrangThai = "Đã nộp";
                db.Entry(baitap).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = CNPM_DOAN.Resources.Language.Nộp_bài_tập_thành_công_;
                return RedirectToAction("showBaiTap", "BAITAPs", new { iduser = baigiai.IDNguoiDung });
            }
            return View();
        }
        public ActionResult ChinhSuaBaiNop(string idbaigiai)
        {
            if (Request.IsAjaxRequest())
            {
                var data = db.BAIGIAIs.Find(idbaigiai);
                Session["IDBAIGIAI"] = data.IDBaiGiai;
                return PartialView(data);
            }
            else return PartialView("Error");
        }
        [HttpPost]
        public ActionResult ChinhSuaBaiNop(HttpPostedFileBase bg,string idbaigiai)
        {
            var baigiai = db.BAIGIAIs.Find(idbaigiai);
            if (bg != null && bg.ContentLength > 0)
            {
                byte[] data = new byte[bg.ContentLength];
                bg.InputStream.Read(data, 0, data.Length);
                baigiai.DuongDan = data;
                baigiai.LoaiTep = bg.ContentType;
                baigiai.TenBaiGiai = bg.FileName;
                baigiai.SoDiem = null;
                baigiai.NgayHoanThanh=DateTime.Now;
                db.Entry(baigiai).State=EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = CNPM_DOAN.Resources.Language.Chỉnh_sửa_thành_công;
                return RedirectToAction("showBaiTap", "BAITAPs", new { iduser = baigiai.IDNguoiDung });
            }
            return View();
        }
        public ActionResult showBaiGiai_PH(string idbaitap, string idnguoinhan)
        {
            var data = db.BAIGIAIs.Where(s => s.IDBaiTap == idbaitap && s.IDNguoiDung == idnguoinhan).FirstOrDefault();
            if (data!=null) { return View(data); }
            else
            {
                return View();
            }
        }
        public ActionResult TaiBG(string idbaigiai)
        {
            var baigiai = db.BAIGIAIs.Find(idbaigiai);
            if (baigiai != null)
            {
                return File(baigiai.DuongDan, baigiai.LoaiTep, baigiai.TenBaiGiai);
            }
            return HttpNotFound();
        }
        [HttpPost]
        public ActionResult ChamDiem(double diem, string idbt, string idnguoilam)
        {
            var data = db.BAIGIAIs.Where(s => s.IDBaiTap == idbt && s.IDNguoiDung == idnguoilam).FirstOrDefault();
            data.SoDiem = diem;
            db.Entry(data).State=EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("showBaiGiai_PH", "BAIGIAIs", new { idbaitap=idbt,idnguoinhan=idnguoilam});
        }
    }
}
