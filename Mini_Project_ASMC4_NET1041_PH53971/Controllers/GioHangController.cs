using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_Project_ASMC4_NET1041_PH53971.Models;

namespace Mini_Project_ASMC4_NET1041_PH53971.Controllers
{
    public class GioHangController : Controller
    {



        private readonly MiniProjectAsmc4Net1041Ph53971Context _context;

        public GioHangController(MiniProjectAsmc4Net1041Ph53971Context context)
        {
            _context = context;
        }

        // Action để thêm món ăn vào giỏ hàng
        [HttpPost]
        public IActionResult AddMonAn(int maMonAn, int soLuong)
        {
            int? maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");

            if (maNguoiDung == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var gioHang = _context.GioHangs
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null)
            {
                gioHang = new GioHang { MaNguoiDung = maNguoiDung.Value };
                _context.GioHangs.Add(gioHang);
                _context.SaveChanges();
            }

            var chiTietGioHang = _context.ChiTietGioHangs
                .FirstOrDefault(c => c.MaGioHang == gioHang.MaGioHang && c.MaMonAn == maMonAn);

            if (chiTietGioHang == null)
            {
                chiTietGioHang = new ChiTietGioHang
                {
                    MaGioHang = gioHang.MaGioHang,
                    MaMonAn = maMonAn,
                    SoLuong = soLuong,
                    Gia = _context.MonAns.FirstOrDefault(m => m.MaMonAn == maMonAn).Gia,
                    Loai = "MonAn"
                };
                _context.ChiTietGioHangs.Add(chiTietGioHang);
            }
            else
            {
                chiTietGioHang.SoLuong += soLuong;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Action để thêm combo vào giỏ hàng
        [HttpPost]
        public IActionResult AddCombo(int maCombo, int soLuong)
        {
            int? maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");

            if (maNguoiDung == null)
            {
                return RedirectToAction("Login", "Login");
            }

            var gioHang = _context.GioHangs
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null)
            {
                gioHang = new GioHang { MaNguoiDung = maNguoiDung.Value };
                _context.GioHangs.Add(gioHang);
                _context.SaveChanges();
            }

            var chiTietGioHang = _context.ChiTietGioHangs
                .FirstOrDefault(c => c.MaGioHang == gioHang.MaGioHang && c.MaCombo == maCombo);

            if (chiTietGioHang == null)
            {
                chiTietGioHang = new ChiTietGioHang
                {
                    MaGioHang = gioHang.MaGioHang,
                    MaCombo = maCombo,
                    SoLuong = soLuong,
                    Gia = _context.Combos.FirstOrDefault(c => c.MaCombo == maCombo).Gia,
                    Loai = "Combo"
                };
                _context.ChiTietGioHangs.Add(chiTietGioHang);
            }
            else
            {
                chiTietGioHang.SoLuong += soLuong;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // Action để xem giỏ hàng
        public IActionResult Index()
        {
            // Kiểm tra người dùng đã đăng nhập chưa
            var maNguoiDung = HttpContext.Session.GetInt32("MaNguoiDung");

            if (maNguoiDung == null)
            {
                return RedirectToAction("Login", "User"); // Redirect nếu chưa đăng nhập
            }

            // Lấy giỏ hàng của người dùng từ database
            var gioHang = _context.GioHangs
                .Include(g => g.ChiTietGioHangs)
                .ThenInclude(ct => ct.MaMonAnNavigation)
                .Include(g => g.ChiTietGioHangs)
                .ThenInclude(ct => ct.MaComboNavigation)
                .FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            // Nếu không có giỏ hàng, tạo giỏ hàng mới
            if (gioHang == null)
            {
                gioHang = new GioHang
                {
                    MaNguoiDung = maNguoiDung.Value,
                    ChiTietGioHangs = new List<ChiTietGioHang>()
                };
            }

            // Trả về giỏ hàng cho view
            return View(gioHang);
        }





        // Phương thức xóa sản phẩm khỏi giỏ hàng
        [HttpPost]
        public IActionResult RemoveFromCart(int chiTietGioHangId)
        {
            var chiTietGioHang = _context.ChiTietGioHangs.FirstOrDefault(ct => ct.MaChiTietGioHang == chiTietGioHangId);

            if (chiTietGioHang != null)
            {
                _context.ChiTietGioHangs.Remove(chiTietGioHang);
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }

        // Phương thức cập nhật số lượng sản phẩm trong giỏ hàng
        [HttpPost]
        public IActionResult UpdateQuantity(int chiTietGioHangId, int newQuantity)
        {
            var chiTietGioHang = _context.ChiTietGioHangs.FirstOrDefault(ct => ct.MaChiTietGioHang == chiTietGioHangId);

            if (chiTietGioHang != null)
            {
                chiTietGioHang.SoLuong = newQuantity;
                _context.SaveChanges();
            }

            return RedirectToAction("Index");
        }




        //[HttpPost]
        //public IActionResult Checkout()
        //{
        //    var nguoiDungId = HttpContext.Session.GetInt32("MaNguoiDung");

        //    if (nguoiDungId == null)
        //    {
        //        TempData["Message"] = "Vui lòng đăng nhập trước khi thanh toán.";
        //        return RedirectToAction("Login", "NguoiDung");
        //    }

        //    // Lấy giỏ hàng của người dùng
        //    var gioHang = _context.GioHangs
        //        .Include(gh => gh.ChiTietGioHangs)
        //        .ThenInclude(ct => ct.MaMonAnNavigation)
        //        .FirstOrDefault(gh => gh.MaNguoiDung == nguoiDungId);

        //    if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
        //    {
        //        TempData["Message"] = "Giỏ hàng trống!";
        //        return RedirectToAction("Index");
        //    }

        //    // Logic tạo đơn hàng và trừ số lượng món ăn
        //    var donHang = new DonHang
        //    {
        //        MaNguoiDung = nguoiDungId,
        //        ThoiGianDat = DateTime.Now,
        //        TrangThai = "Chua_Van_Chuyen",
        //        TongTien = gioHang.ChiTietGioHangs.Sum(ct => ct.SoLuong * ct.Gia)
        //    };

        //    _context.DonHangs.Add(donHang);
        //    _context.SaveChanges();

        //    foreach (var chiTietGioHang in gioHang.ChiTietGioHangs)
        //    {
        //        var chiTietDonHang = new ChiTietDonHang
        //        {
        //            MaDonHang = donHang.MaDonHang,
        //            MaMonAn = chiTietGioHang.MaMonAn,
        //            SoLuong = chiTietGioHang.SoLuong,
        //            Gia = chiTietGioHang.Gia
        //        };
        //        _context.ChiTietDonHangs.Add(chiTietDonHang);

        //        // Trừ số lượng món ăn trong kho
        //        if (chiTietGioHang.MaMonAn != null)
        //        {
        //            var monAn = _context.MonAns.Find(chiTietGioHang.MaMonAn);
        //            if (monAn != null)
        //            {
        //                monAn.Soluong -= chiTietGioHang.SoLuong;
        //            }
        //        }
        //    }

        //    _context.ChiTietGioHangs.RemoveRange(gioHang.ChiTietGioHangs);
        //    _context.SaveChanges();

        //    TempData["Message"] = "Thanh toán thành công!";
        //    return RedirectToAction("ThanhToan", "DonHang");
        //}


        [HttpPost]
        public IActionResult Checkout()
        {
            var nguoiDungId = HttpContext.Session.GetInt32("MaNguoiDung");

            if (nguoiDungId == null)
            {
                TempData["Message"] = "Vui lòng đăng nhập trước khi thanh toán.";
                return RedirectToAction("Login", "Login");
            }

            // Lấy giỏ hàng của người dùng
            var gioHang = _context.GioHangs
                .Include(gh => gh.ChiTietGioHangs)
                .ThenInclude(ct => ct.MaMonAnNavigation)
                .FirstOrDefault(gh => gh.MaNguoiDung == nguoiDungId);

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
            {
                TempData["Message"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index");
            }

            // Tạo đơn hàng
            var donHang = new DonHang
            {
                MaNguoiDung = nguoiDungId,
                ThoiGianDat = DateTime.Now,
                TrangThai = "Chua_Van_Chuyen",
                TongTien = gioHang.ChiTietGioHangs.Sum(ct => ct.SoLuong * ct.Gia)
            };
            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            foreach (var chiTietGioHang in gioHang.ChiTietGioHangs)
            {
                // Tạo chi tiết đơn hàng
                var chiTietDonHang = new ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    MaMonAn = chiTietGioHang.MaMonAn,
                    SoLuong = chiTietGioHang.SoLuong,
                    Gia = chiTietGioHang.Gia
                };
                _context.ChiTietDonHangs.Add(chiTietDonHang);

                // Trừ số lượng món ăn trong kho
                if (chiTietGioHang.MaMonAn != null)
                {
                    var monAn = _context.MonAns.Find(chiTietGioHang.MaMonAn);
                    if (monAn != null)
                    {
                        if (monAn.Soluong < chiTietGioHang.SoLuong)
                        {
                            TempData["Message"] = $"Số lượng món ăn '{monAn.TenMonAn}' không đủ hàng.";
                            return RedirectToAction("Index");
                        }
                        monAn.Soluong -= chiTietGioHang.SoLuong;
                    }
                }
            }

            // Xóa giỏ hàng sau khi thanh toán
            _context.ChiTietGioHangs.RemoveRange(gioHang.ChiTietGioHangs);
            _context.SaveChanges();

            TempData["Message"] = "Thanh toán thành công!";
            return RedirectToAction("DonHangCuaToi", "DonHang");
        }



    }
}
