using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_Project_ASMC4_NET1041_PH53971.Models;

namespace Mini_Project_ASMC4_NET1041_PH53971.Controllers
{
    public class DonHangController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        private readonly MiniProjectAsmc4Net1041Ph53971Context _context;

        public DonHangController(MiniProjectAsmc4Net1041Ph53971Context context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult ThanhToan()
        {
            var nguoiDungId = HttpContext.Session.GetInt32("MaNguoiDung");
            if (nguoiDungId == null)
            {
                return RedirectToAction("Login", "NguoiDung");
            }

            var gioHang = _context.GioHangs
                .Where(gh => gh.MaNguoiDung == nguoiDungId)
                .Include(gh => gh.ChiTietGioHangs)
                .ThenInclude(ct => ct.MaMonAnNavigation)
                .FirstOrDefault();

            if (gioHang == null || !gioHang.ChiTietGioHangs.Any())
            {
                TempData["Message"] = "Giỏ hàng trống!";
                return RedirectToAction("Index", "GioHang");
            }

            // Tạo đơn hàng
            var donHang = new DonHang
            {
                ThoiGianDat = DateTime.Now,
                TrangThai = "Chua_Van_Chuyen",
                MaNguoiDung = nguoiDungId,
                TongTien = gioHang.ChiTietGioHangs.Sum(ct => ct.SoLuong * ct.Gia)
            };

            _context.DonHangs.Add(donHang);
            _context.SaveChanges();

            // Thêm chi tiết đơn hàng và trừ số lượng từ kho chỉ cho MonAn
            foreach (var chiTietGioHang in gioHang.ChiTietGioHangs)
            {
                // Thêm chi tiết đơn hàng
                var chiTietDonHang = new ChiTietDonHang
                {
                    MaDonHang = donHang.MaDonHang,
                    SoLuong = chiTietGioHang.SoLuong,
                    Gia = chiTietGioHang.Gia,
                    MaMonAn = chiTietGioHang.MaMonAn
                };
                _context.ChiTietDonHangs.Add(chiTietDonHang);

                // Trừ số lượng MonAn
                if (chiTietGioHang.MaMonAn != null)
                {
                    var monAn = _context.MonAns.Find(chiTietGioHang.MaMonAn);
                    if (monAn != null)
                    {
                        monAn.Soluong -= chiTietGioHang.SoLuong;
                    }
                }
            }

            _context.ChiTietGioHangs.RemoveRange(gioHang.ChiTietGioHangs);
            _context.SaveChanges();

            TempData["Message"] = "Đơn hàng đã được tạo thành công!";
            return RedirectToAction("Index", "DonHang");
        }


        [HttpPost]
        public IActionResult CapNhatTrangThai(int MaDonHang, string TrangThai)
        {
            try
            {
                // Lấy thông tin đơn hàng từ database
                var donHang = _context.DonHangs
                    .Include(dh => dh.ChiTietDonHangs)
                    .ThenInclude(ct => ct.MaMonAnNavigation)
                    .FirstOrDefault(dh => dh.MaDonHang == MaDonHang);

                if (donHang == null)
                {
                    // Trả về thông báo lỗi chi tiết
                    return Json(new { success = false, message = "Đơn hàng không tồn tại." });
                }

                // Kiểm tra trạng thái của đơn hàng trước khi cập nhật
                if (donHang.TrangThai == "Huy_Don" || donHang.TrangThai == "Hoan_Thanh")
                {
                    return Json(new { success = false, message = "Đơn hàng đã bị hủy hoặc đã hoàn thành, không thể thay đổi." });
                }

                // Kiểm tra trạng thái mới và cập nhật nếu hợp lệ
                if (TrangThai == "Huy_Don")
                {
                    donHang.TrangThai = "Huy_Don";

                    foreach (var chiTiet in donHang.ChiTietDonHangs)
                    {
                        if (chiTiet.MaMonAnNavigation != null)
                        {
                            chiTiet.MaMonAnNavigation.Soluong += chiTiet.SoLuong;
                        }
                    }

                    _context.SaveChanges();
                    return Json(new { success = true, message = "Đơn hàng đã được hủy thành công." });
                }
                if (TrangThai == "Da_Van_Chuyen" || TrangThai == "Hoan_Thanh")
                {
                    donHang.TrangThai = TrangThai;
                    _context.SaveChanges();
                    return Json(new { success = true, message = "Trạng thái đã được cập nhật." });
                }
                else
                {
                    return Json(new { success = false, message = "Trạng thái không hợp lệ." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi xảy ra: " + ex.Message); // Ghi ra console
                return Json(new { success = false, message = "Đã có lỗi xảy ra: " + ex.Message });
            }

        }







        public IActionResult DonHangCuaToi()
        {
            var nguoiDungId = HttpContext.Session.GetInt32("MaNguoiDung");
            if (nguoiDungId == null)
            {
                TempData["Message"] = "Vui lòng đăng nhập để xem đơn hàng của bạn.";
                return RedirectToAction("Login", "NguoiDung");
            }

            // Lấy danh sách đơn hàng của người dùng
            var donHangs = _context.DonHangs
                .Where(dh => dh.MaNguoiDung == nguoiDungId)
                .OrderByDescending(dh => dh.ThoiGianDat)
                .ToList();

            return View(donHangs);
        }



        public IActionResult ChiTietDonHang(int id)
        {
            var donHang = _context.DonHangs
                .Include(dh => dh.ChiTietDonHangs)
                .ThenInclude(ct => ct.MaMonAnNavigation)
                .FirstOrDefault(dh => dh.MaDonHang == id);

            if (donHang == null)
            {
                return NotFound();
            }

            return View(donHang);
        }


    }
}
