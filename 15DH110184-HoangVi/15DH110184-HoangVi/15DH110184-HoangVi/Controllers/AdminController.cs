using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _15DH110184_HoangVi.Models;
using PagedList;
using PagedList.Mvc;

namespace _15DH110184_HoangVi.Controllers
{
    public class AdminController : Controller
    {
        public dbQLBanSachDataContext data = new dbQLBanSachDataContext();
        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();

        }
        public ActionResult Sach(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 9;
            return View(data.SACHes.ToList().OrderBy(n => n.Masach).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult ChuDe(int? page)
        {
            return View(data.CHUDEs.ToList());
        }
        public ActionResult NhaXuatBan(int? page)
        {
            return View(data.NHAXUATBANs.ToList());
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {

            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
      
                if (ad != null)
                {
                    Session["TaiKhaonAdmin"] = ad;
                    return RedirectToAction("Index", "Admin");

                }
                else
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Themmoisach()
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            return View();
        }
        [HttpPost]
        public ActionResult Themmoisach(SACH sach, HttpPostedFileBase fileupload)
        {
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            //Kiểm tra hình ảnh đã tồn tại hay chưa
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();

            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Lưu tên file , lưu ý bổ sung thư viện using System.IO
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Lưu đường dẫn của file
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sach.Hinhminhhoa = fileName;
                    data.SACHes.InsertOnSubmit(sach);
                    data.SubmitChanges();
                }
                return RedirectToAction("Sach");
            }
        }
        public ActionResult Chitietsach(int id)
        {
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            {
                ViewBag.Masach = sach.Masach;
                if (sach == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(sach);
            }
        }
        [HttpGet]
        public ActionResult Xoasach(int id)
        {
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);

            ViewBag.ThongBao = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }   
            return View(sach);
        }
        [HttpPost,ActionName("Xoasach")]
        public ActionResult Xoasach(int id,CHUDE chude , NHAXUATBAN nxb)
        {
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Masach = sach.Masach;
            data.SACHes.DeleteOnSubmit(sach);
            data.SubmitChanges();
            return RedirectToAction("Sach");


        }
        [HttpGet]
        public ActionResult Suasach(int id)
        {
            SACH sach = data.SACHes.SingleOrDefault(n => n.Masach == id);
            ViewBag.Mota = sach.Mota;
            ViewBag.Masach = sach.Masach;
            if (sach == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");

            return View(sach);
        }
        [HttpPost]
        public ActionResult Suasach(SACH sach, HttpPostedFileBase fileupload)
        {
            SACH sach1 = data.SACHes.SingleOrDefault(n => n.Masach == sach.Masach);
            sach1.Masach = sach.Masach;
            data.SACHes.DeleteOnSubmit(sach1);
            ViewBag.MaCD = new SelectList(data.CHUDEs.ToList().OrderBy(n => n.TenChuDe), "MaCD", "TenChude");
            ViewBag.MaNXB = new SelectList(data.NHAXUATBANs.ToList().OrderBy(n => n.TenNXB), "MaNXB", "TenNXB");
            
            if (ModelState.IsValid)
            {
                if (fileupload != null)
                {
                    var filename = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), filename);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hinh anh da ton tai !";
              
                    }
                    else
                    {
                        fileupload.SaveAs(path);

                    }
                    sach.Hinhminhhoa = filename;
                    data.SACHes.InsertOnSubmit(sach);
                    data.SubmitChanges();
                }



            }
            return RedirectToAction("Sach");
            //var fileName = Path.GetFileName(fileupload.FileName);
            //var path = Path.Combine(Server.MapPath("~/images"), fileName);   
            //fileupload.SaveAs(path);
            //sach.Hinhminhhoa = fileName;
            //data.SACHes.InsertOnSubmit(sach);
            //data.SubmitChanges();
            //return RedirectToAction("Sach");

        }
        public ActionResult ThemChuDe()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemChuDe(CHUDE chude)
        {   
             if (ModelState.IsValid)
                    {
                        
                        data.CHUDEs.InsertOnSubmit(chude);
                        data.SubmitChanges();
                    }
                return RedirectToAction("ChuDe");                         
        }
        public ActionResult XoaChuDe(int ? id)
        {
            CHUDE chude = data.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            if (chude == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chude);
        }
        [HttpPost]
        public ActionResult XoaChuDe(int id)
        {
            CHUDE chude = data.CHUDEs.SingleOrDefault(n => n.MaCD == id);
            data.CHUDEs.DeleteOnSubmit(chude);
            data.SubmitChanges();
            return RedirectToAction("ChuDe");
        }
        public ActionResult ThemNhaXuatBan()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemNhaXuatBan(NHAXUATBAN nha)
        {
            if (ModelState.IsValid)
            {

                data.NHAXUATBANs.InsertOnSubmit(nha);
                data.SubmitChanges();
            }
            return RedirectToAction("NhaXuatBan");
        }
        public ActionResult XoaNhaXuatBan(int? id)
        {
            NHAXUATBAN nha = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nha == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nha);
        }
        [HttpPost]
        public ActionResult XoaNhaXuatBan(int id)
        {
            NHAXUATBAN nha = data.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            data.NHAXUATBANs.DeleteOnSubmit(nha);
            data.SubmitChanges();
            return RedirectToAction("NhaXuatBan");
        }
    }
}
