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
    public class BAITAPsController : Controller
    {
        private CNPM_DOANEntities db = new CNPM_DOANEntities();
        // GET: BAITAPs
        public ActionResult Index()
        {
            var bAITAPs = db.BAITAPs.Include(b => b.NGUOIDUNG).Include(b => b.NGUOIDUNG1);
            return View(bAITAPs.ToList());
        }

        // GET: BAITAPs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BAITAP bAITAP = db.BAITAPs.Find(id);
            if (bAITAP == null)
            {
                return HttpNotFound();
            }
            return View(bAITAP);
        }

        // GET: BAITAPs/Create
        public ActionResult Create()
        {
            ViewBag.IDNguoiNhan = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung");
            ViewBag.IDNguoiTao = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung");
            return View();
        }

        // POST: BAITAPs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDBaiTap,TenBT,NgayGiao,HanNop,IDNhom,IDNguoiTao,IDNguoiNhan")] BAITAP bAITAP)
        {
            if (ModelState.IsValid)
            {
                db.BAITAPs.Add(bAITAP);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDNguoiNhan = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", bAITAP.IDNguoiNhan);
            ViewBag.IDNguoiTao = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", bAITAP.IDNguoiTao);
            return View(bAITAP);
        }

        // GET: BAITAPs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BAITAP bAITAP = db.BAITAPs.Find(id);
            if (bAITAP == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDNguoiNhan = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", bAITAP.IDNguoiNhan);
            ViewBag.IDNguoiTao = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", bAITAP.IDNguoiTao);
            return View(bAITAP);
        }

        // POST: BAITAPs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDBaiTap,TenBT,NgayGiao,HanNop,IDNguoiTao,IDNguoiNhan,DuongDan,LoaiTep")] BAITAP bAITAP)
        {
            if (ModelState.IsValid)
            {
                db.Entry(bAITAP).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDNguoiNhan = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", bAITAP.IDNguoiNhan);
            ViewBag.IDNguoiTao = new SelectList(db.NGUOIDUNGs, "IDNguoiDung", "TenNguoiDung", bAITAP.IDNguoiTao);
            return View(bAITAP);
        }

        // GET: BAITAPs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BAITAP bAITAP = db.BAITAPs.Find(id);
            if (bAITAP == null)
            {
                return HttpNotFound();
            }
            return View(bAITAP);
        }

        // POST: BAITAPs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            BAITAP bAITAP = db.BAITAPs.Find(id);
            db.BAITAPs.Remove(bAITAP);
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
        public ActionResult showBaiTap(string iduser)
        {
            
            if(iduser.Contains("HS"))
            {
                var data = db.BAITAPs.Where(s => s.IDNguoiNhan == iduser);
                return View(data.ToList());
            }
            else
            {
                return RedirectToAction("showUserBaitap", "BAITAPs", new {idquanly=iduser});
            }
        }
        public bool KTNopBai()
        {
            
            return false;
        }
        public ActionResult showUserBaitap(string idquanly)
        {
            var data = db.NGUOIDUNGs.Where(s => s.IDQuanLy == idquanly && s.IDNguoiDung != idquanly);
            return View(data.ToList());
        }
        public ActionResult showBaiTap_PH(string iduser)
        {
            var data = db.BAITAPs.Where(s => s.IDNguoiNhan == iduser);
            Session["IDNGUOINHAN"] = iduser;
            ViewBag.NguoiNhan = iduser;
            return View(data.ToList());
        }
        public ActionResult TaoBTMoi()
        {
            return PartialView();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoBTMoi(HttpPostedFileBase bt, DateTime hanchot,string idnguoitao, string idnguoinhan)
        {
            if(bt !=null&&bt.ContentLength>0)
            {
                byte[]data= new byte[bt.ContentLength];
                    if(hanchot<DateTime.Now)
                    {
                        ModelState.AddModelError("day", "Hạn chót phải lớn hơn ngày hôm nay");
                        return PartialView();
                    }
                    else
                    {
                        bt.InputStream.Read(data, 0, data.Length);
                        TimeSpan currentTime = DateTime.Now.TimeOfDay;
                        var baitap = new BAITAP
                        {
                            TenBT = bt.FileName,
                            DuongDan = data,
                            LoaiTep = bt.ContentType,
                            IDBaiTap = "BT" + new RANDOMID().GenerateRandomString(3),
                            NgayGiao = DateTime.Now,
                            HanNop = hanchot + currentTime,
                            IDNguoiTao = idnguoitao,
                            IDNguoiNhan = idnguoinhan,
                            TrangThai = "Chưa nộp"
                        };
                        db.BAITAPs.Add(baitap);
                        db.SaveChanges();
                        TempData["message"] = CNPM_DOAN.Resources.Language.Tạo_thành_công_;
                        return Json(new { success = true, redirectUrl = Url.Action("showBaiTap_PH", "BAITAPs", new { iduser = baitap.IDNguoiNhan }) });
                    }
                
            }
            return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
        }
        public ActionResult TaiBT(string idbaitap)
        {
            var baitap=db.BAITAPs.Find(idbaitap);
            if (baitap != null) 
            {
                return File(baitap.DuongDan, baitap.LoaiTep, baitap.TenBT);
            }
            return HttpNotFound();
        }
        public ActionResult DeleteBaiTap(string idbaitap,string iduser)
        {
            var data = db.BAITAPs.Find(idbaitap);
            var baigiai=db.BAIGIAIs.Where(s=>s.IDBaiTap==idbaitap).ToList();
            if(baigiai.Count>0)
            {
                foreach (var item in baigiai)
                {
                    db.BAIGIAIs.Remove(item);
                }
            }
            db.BAITAPs.Remove(data);
            db.SaveChanges();
            string id = iduser;
            TempData["message"] = CNPM_DOAN.Resources.Language.Xóa_thành_công_;
            return RedirectToAction("showBaiTap_PH", "BAITAPs", new {iduser=id});
        }
        public ActionResult updateBaiTap(string idbaitap)
        {
            var data = db.BAITAPs.Find(idbaitap);
            Session["IDBAITAP"] = idbaitap;
            Session["IDNGUOINHAN"] = data.IDNguoiNhan;
            return PartialView();
        }
        [HttpPost]
        public ActionResult updateBaiTap(HttpPostedFileBase bt, DateTime hanchot,string idbaitap,string idnguoinhan )
        {
            if (bt != null && bt.ContentLength > 0)
            {
                byte[] data = new byte[bt.ContentLength];
                if (hanchot < DateTime.Now)
                {
                    ModelState.AddModelError("day", "Hạn chót phải lớn hơn ngày hôm nay");
                    return PartialView();
                }
                else
                {
                    var baitap = db.BAITAPs.Find(idbaitap);
                    bt.InputStream.Read(data, 0, data.Length);
                    baitap.TenBT = bt.FileName;
                    baitap.DuongDan = data;
                    baitap.LoaiTep = bt.ContentType;
                    baitap.HanNop = hanchot;
                    db.Entry(baitap).State = EntityState.Modified;
                    db.SaveChanges();
                    return Json(new { success = true, redirectUrl = Url.Action("showBaiTap_PH", "BAITAPs", new { iduser = baitap.IDNguoiNhan }) });
                }

            }
            return Json(new { success = true, redirectUrl = Url.Action("Index", "Home") });
            
        }
        public ActionResult showBieuDoBT_PH(string idnguoinhan)
        {
            var baitap = db.BAITAPs.Where(s=>s.IDNguoiNhan== idnguoinhan).ToList();
            var hoanthanh= db.BAIGIAIs.Where(s => s.IDNguoiDung == idnguoinhan).ToList();
            int tongBT = 0;
            int ht = 0;
            int cht = 0;
            foreach(var item in baitap)
            {
                tongBT++;
            }
            foreach(var item in hoanthanh)
            {
                ht++;
            }
            cht = tongBT - ht;
            var DataValues = new[] { ht, cht };
            var DataColors = new[] { "#36A2EB","#FF6384" };
            ViewBag.DataColors = DataColors;
            ViewBag.DataValues= DataValues;
            return View();
        }
    }
}
