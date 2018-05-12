using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _15DH110184_HoangVi.Models;
using PagedList;
using PagedList.Mvc;

namespace _15DH110184_HoangVi.Controllers
{
    public class BookStoreController : Controller
    {
        //
        // GET: /BookStore/
        dbQLBanSachDataContext data = new dbQLBanSachDataContext();

        private List<SACH> Laysachmoi(int count)
        {
            return data.SACHes.OrderByDescending(a => a.Ngaycapnhat).Take(count).ToList();
        }
        public ActionResult Index(int ? page)
        {
            int pageSize = 8;
            int pageNum = (page ?? 1);
            var sachmoi = Laysachmoi(24);
            return View(sachmoi.ToPagedList(pageNum,pageSize));
        }
        public ActionResult ChuDe()
        {
            var chude = from cd in data.CHUDEs select cd;
            return PartialView(chude);
        }
        public ActionResult NhaXuatBan()
        {
            var nhaxuatban = from nxb in data.NHAXUATBANs select nxb;
            return PartialView(nhaxuatban);
        }
        public ActionResult SanPhamTheoChuDe(int id)
        {
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return PartialView(sach);
        }
        public ActionResult SanPhamTheoNXB(int id)
        {
            var sach = from s in data.SACHes where s.MaCD == id select s;
            return PartialView(sach);
        }
        public ActionResult Details(int id)
        {
            var sach = from s in data.SACHes
                       where s.Masach == id
                       select s;
            return View(sach.Single());
        }
    }
}