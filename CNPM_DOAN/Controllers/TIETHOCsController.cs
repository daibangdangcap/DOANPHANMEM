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
    public class TIETHOCsController : Controller
    {
        private CNPM_DOANEntities db = new CNPM_DOANEntities();

        // GET: TIETHOCs
        public ActionResult Index()
        {
            var tIETHOCs = db.TIETHOCs.Include(t => t.NGUOIDUNG).Include(t => t.THOIKHOABIEU);
            return View(tIETHOCs.ToList());
        }

        // GET: TIETHOCs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIETHOC tIETHOC = db.TIETHOCs.Find(id);
            if (tIETHOC == null)
            {
                return HttpNotFound();
            }
            return View(tIETHOC);
        }

        // GET: TIETHOCs/Create
        public ActionResult Create()
        {
            ViewBag.IDNGUOITAO = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung");
            ViewBag.IDTKB = new SelectList(db.THOIKHOABIEUx, "IDTKB", "TenTKB");
            return View();
        }

        // POST: TIETHOCs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDMonHoc,TenMH,Thu,TietBatDau,TietKetThuc,IDTKB,IDNGUOITAO")] TIETHOC tIETHOC)
        {
            if (ModelState.IsValid)
            {
                db.TIETHOCs.Add(tIETHOC);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDNGUOITAO = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", tIETHOC.IDNGUOITAO);
            ViewBag.IDTKB = new SelectList(db.THOIKHOABIEUx, "IDTKB", "TenTKB", tIETHOC.IDTKB);
            return View(tIETHOC);
        }

        // GET: TIETHOCs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIETHOC tIETHOC = db.TIETHOCs.Find(id);
            if (tIETHOC == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDNGUOITAO = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", tIETHOC.IDNGUOITAO);
            ViewBag.IDTKB = new SelectList(db.THOIKHOABIEUx, "IDTKB", "TenTKB", tIETHOC.IDTKB);
            return View(tIETHOC);
        }

        // POST: TIETHOCs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDMonHoc,TenMH,Thu,TietBatDau,TietKetThuc,IDTKB,IDNGUOITAO")] TIETHOC tIETHOC)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tIETHOC).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDNGUOITAO = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", tIETHOC.IDNGUOITAO);
            ViewBag.IDTKB = new SelectList(db.THOIKHOABIEUx, "IDTKB", "TenTKB", tIETHOC.IDTKB);
            return View(tIETHOC);
        }

        // GET: TIETHOCs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TIETHOC tIETHOC = db.TIETHOCs.Find(id);
            if (tIETHOC == null)
            {
                return HttpNotFound();
            }
            return View(tIETHOC);
        }

        // POST: TIETHOCs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TIETHOC tIETHOC = db.TIETHOCs.Find(id);
            db.TIETHOCs.Remove(tIETHOC);
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
        public ActionResult showTietHoc(string idtkb)
        {
            var data = db.TIETHOCs.Where(s => s.IDTKB == idtkb).OrderBy(s => s.Thu).ThenBy(s => s.TietBatDau); ;
            Session["IDTKB"] = idtkb;
            return View(data.ToList());
        }
        public ActionResult themTietHocMoi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult themTietHocMoi(FormCollection form)
        {
            string idtkb = form["idtkbb"];
            string thu = form["day"];
            var findData = db.TIETHOCs.Where(mh=> mh.IDTKB == idtkb && mh.Thu == thu).ToList();
            short start = short.Parse(form["start"]);
            short end= short.Parse(form["end"]);
            if (end < start)
            {
                ModelState.AddModelError("day", CNPM_DOAN.Resources.Language.Tiết_kết_thúc_phải_lớn_hơn_hoặc_bằng_tiết_bắt_đầu);
                return View();
            }
            if(findData.Count==0)
            {
                TIETHOC tIETHOC = new TIETHOC();
                tIETHOC.TenMH = form["monhoc"];
                tIETHOC.Thu = form["day"];
                tIETHOC.TietBatDau = short.Parse(form["start"]);
                tIETHOC.TietKetThuc = short.Parse(form["end"]);
                tIETHOC.IDTKB = form["idtkbb"];
                tIETHOC.IDMonHoc = "MH" + new RANDOMID().GenerateRandomString(2);
                tIETHOC.IDNGUOITAO = form["iduser"];
                db.TIETHOCs.Add(tIETHOC);
                db.SaveChanges();
                TempData["message"] = CNPM_DOAN.Resources.Language.Tạo_thành_công_;
                return RedirectToAction("showTietHoc_PH", "TIETHOCs", new { idtkb = form["idtkbb"] });
            }
            else
            {
                bool duplicate = true;
                foreach (TIETHOC mh in findData)
                {
                    if ((mh.TietBatDau < start && mh.TietKetThuc < start && mh.TietBatDau<end &&mh.TietKetThuc<end) || (mh.TietBatDau > start && mh.TietKetThuc > start && mh.TietBatDau > end && mh.TietKetThuc > end))
                    {
                        duplicate = false;
                    }
                    else
                    {
                        ModelState.AddModelError("day", CNPM_DOAN.Resources.Language.Tiết_học_trùng_với_tiết_học_của_một_môn_khác__Vui_lòng_kiểm_tra_lại_thời_khóa_biểu_của_bạn_);
                        return View();
                    }
                }
                if (duplicate == false)
                {
                    TIETHOC tIETHOC = new TIETHOC();
                    tIETHOC.TenMH = form["monhoc"];
                    tIETHOC.Thu = form["day"];
                    tIETHOC.TietBatDau = short.Parse(form["start"]);
                    tIETHOC.TietKetThuc = short.Parse(form["end"]);
                    tIETHOC.IDTKB = form["idtkbb"];
                    tIETHOC.IDMonHoc = "MH" + new RANDOMID().GenerateRandomString(2);
                    tIETHOC.IDNGUOITAO = form["iduser"];
                    db.TIETHOCs.Add(tIETHOC);
                    db.SaveChanges();
                    TempData["message"] = CNPM_DOAN.Resources.Language.Tạo_thành_công_;
                    return RedirectToAction("showTietHoc_PH", "TIETHOCs", new { idtkb = form["idtkbb"] });
                }
            }
            return RedirectToAction("showTietHoc_PH", "TIETHOCs", new {idtkb=form["idtkbb"] });
        }
        public ActionResult showTietHoc_PH(string idtkb)
        {
            var data = db.TIETHOCs.Where(s => s.IDTKB == idtkb).OrderBy(s=> s.Thu).ThenBy(s=>s.TietBatDau);
            Session["IDTKB"] = idtkb;
            return View(data.ToList());
        }

        public ActionResult deleteTietHoc(string idtiethoc, string tkb)
        {
            var data = db.TIETHOCs.Find(idtiethoc);
            db.TIETHOCs.Remove(data);
            db.SaveChanges();
            TempData["message"] = CNPM_DOAN.Resources.Language.Xóa_thành_công_;
            return RedirectToAction("showTietHoc_PH", "TIETHOCs", new {idtkb=tkb});
        }

        public ActionResult editTietHoc(string idtiethoc, string tkb)
        {
            Session["IDTKB"] = tkb;
            Session["IDTIETHOC"] = idtiethoc;
            return View();
        }

        [HttpPost]
        public ActionResult editTietHoc(FormCollection form)
        {
            string idtkb = form["idtkb"];
            string thu = form["day"];
            var findData = db.TIETHOCs.Where(mh => mh.IDTKB == idtkb && mh.Thu == thu).ToList();
            short start = short.Parse(form["start"]);
            short end = short.Parse(form["end"]);
            if (end < start)
            {
                ModelState.AddModelError("day", CNPM_DOAN.Resources.Language.Tiết_kết_thúc_phải_lớn_hơn_hoặc_bằng_tiết_bắt_đầu);
                return View();
            }
            if (findData.Count == 0)
            {
                var tIETHOC = db.TIETHOCs.Find(form["idtiethoc"]);
                tIETHOC.TenMH = form["monhoc"];
                tIETHOC.Thu = form["day"];
                tIETHOC.TietBatDau = short.Parse(form["start"]);
                tIETHOC.TietKetThuc = short.Parse(form["end"]);
                db.Entry(tIETHOC).State = EntityState.Modified;
                db.SaveChanges();
                TempData["message"] = CNPM_DOAN.Resources.Language.Tạo_thành_công_;
                return RedirectToAction("showTietHoc_PH", "TIETHOCs", new { idtkb = form["idtkb"] });
            }
            else
            {
                bool duplicate = true;
                foreach (TIETHOC mh in findData)
                {
                    if ((mh.TietBatDau < start && mh.TietKetThuc < start && mh.TietBatDau < end && mh.TietKetThuc < end) || (mh.TietBatDau > start && mh.TietKetThuc > start && mh.TietBatDau > end && mh.TietKetThuc > end))
                    {
                        duplicate = false;
                    }
                    else
                    {
                        ModelState.AddModelError("day", CNPM_DOAN.Resources.Language.Tiết_học_trùng_với_tiết_học_của_một_môn_khác__Vui_lòng_kiểm_tra_lại_thời_khóa_biểu_của_bạn_);
                        return View();
                    }
                }
                if (duplicate == false)
                {
                    var tIETHOC = db.TIETHOCs.Find(form["idtiethoc"]);
                    tIETHOC.TenMH = form["monhoc"];
                    tIETHOC.Thu = form["day"];
                    tIETHOC.TietBatDau = short.Parse(form["start"]);
                    tIETHOC.TietKetThuc = short.Parse(form["end"]);
                    db.Entry(tIETHOC).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["message"] = CNPM_DOAN.Resources.Language.Tạo_thành_công_;
                    return RedirectToAction("showTietHoc_PH", "TIETHOCs", new { idtkb = form["idtkb"] });
                }
            }
            return RedirectToAction("showTietHoc_PH", "TIETHOCs", new { idtkb = form["idtkb"] });
        }
    }
}
