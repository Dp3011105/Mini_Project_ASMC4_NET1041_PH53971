using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_Project_ASMC4_NET1041_PH53971.Models;

namespace Mini_Project_ASMC4_NET1041_PH53971.Controllers
{
    public class KhachHangController : Controller
    {
        private readonly ILogger<KhachHangController> _logger;
        private readonly MiniProjectAsmc4Net1041Ph53971Context _context;

        public KhachHangController(ILogger<KhachHangController> logger, MiniProjectAsmc4Net1041Ph53971Context context)
        {
            _logger = logger;
            _context = context;
        }


        public IActionResult Index(string searchName, string selectedTag)
        {
            var query = _context.MonAns.AsQueryable();

            // Tìm kiếm theo tên
            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(m => m.TenMonAn.Contains(searchName));
            }

            // Tìm kiếm theo tag
            if (!string.IsNullOrEmpty(selectedTag))
            {
                query = query.Where(m => m.MaTag.ToString() == selectedTag);
            }

            // Chỉ lấy các món ăn có TrangThai = true
            query = query.Where(m => m.TrangThai == true);

            var result = query.ToList();

            // Để đưa danh sách các Tag vào View (để tạo dropdown filter)
            ViewBag.Tags = _context.Tags.ToList();

            return View(result);
        }


        public IActionResult Details(int id)
        {
            var product = _context.MonAns.FirstOrDefault(m => m.MaMonAn == id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }





        public IActionResult IndexComBo(string searchName, string selectedTag)
        {
            var query = _context.Combos.AsQueryable();

            // Tìm kiếm theo tên
            if (!string.IsNullOrEmpty(searchName))
            {
                query = query.Where(m => m.TenCombo.Contains(searchName));
            }

            // Lọc chỉ những combo có TrangThai = true
            query = query.Where(m => m.TrangThai == true);

            var result = query.ToList();

            return View(result);
        }




        public IActionResult DetailsComBo(int id)
        {
            // Lấy thông tin combo
            var combo = _context.Combos
                .Include(c => c.ChiTietCombos)
                .ThenInclude(ct => ct.MaMonAnNavigation)
                .FirstOrDefault(c => c.MaCombo == id);

            if (combo == null)
            {
                return NotFound();
            }

            // Chuẩn bị ViewModel
            var viewModel = new ComboDetailsViewModel
            {
                Combo = combo,
                MonAnsInCombo = combo.ChiTietCombos.ToList()
            };

            return View(viewModel);
        }



        // Action GET để hiển thị chi tiết người dùng
        [HttpGet]
        public IActionResult DetailsNguoiDung(int id)
        {
            // Lấy thông tin người dùng từ cơ sở dữ liệu
            var nguoiDung = _context.NguoiDungs.FirstOrDefault(n => n.MaNguoiDung == id);

            // Nếu không tìm thấy người dùng, trả về lỗi NotFound
            if (nguoiDung == null)
            {
                return NotFound();
            }

            // Trả về view Details với mô hình người dùng
            return View(nguoiDung);
        }




        [HttpGet]
        public IActionResult EditNguoiDung(int id)
        {
            var nguoiDung = _context.NguoiDungs.FirstOrDefault(n => n.MaNguoiDung == id);
            if (nguoiDung == null)
            {
                return NotFound();
            }

            // Trả về view cùng với thông tin người dùng
            return View(nguoiDung);
        }

        [HttpPost]
        public IActionResult EditNguoiDung(NguoiDung nguoiDung, string MatKhauCu, string MatKhauMoi)
        {
            if (ModelState.IsValid)
            {
                // Lấy thông tin người dùng từ cơ sở dữ liệu
                var currentUser = _context.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == nguoiDung.MaNguoiDung);
                if (currentUser == null)
                {
                    return NotFound(); // Người dùng không tồn tại
                }

                // Kiểm tra mật khẩu cũ
                if (!string.IsNullOrEmpty(MatKhauCu) && MatKhauCu == currentUser.MatKhau)
                {
                    // Nếu mật khẩu cũ đúng, cập nhật mật khẩu mới
                    if (!string.IsNullOrEmpty(MatKhauMoi))
                    {
                        currentUser.MatKhau = MatKhauMoi; // Cập nhật mật khẩu mới
                    }
                    else
                    {
                        ModelState.AddModelError("MatKhauMoi", "Mật khẩu mới không được để trống.");
                        return View(nguoiDung); // Trả về view với lỗi
                    }
                }
                else if (!string.IsNullOrEmpty(MatKhauCu))
                {
                    // Nếu mật khẩu cũ không đúng
                    ModelState.AddModelError("MatKhauCu", "Mật khẩu cũ không đúng.");
                    return View(nguoiDung); // Trả về view với lỗi
                }

                // Cập nhật thông tin người dùng
                currentUser.Ten = nguoiDung.Ten;
                currentUser.Email = nguoiDung.Email;

                // Lưu thay đổi vào cơ sở dữ liệu
                _context.Update(currentUser);
                _context.SaveChanges();

                // Chuyển hướng về trang danh sách người dùng
                return RedirectToAction("Index", "KhachHang");
            }

            // Nếu model không hợp lệ, quay lại view với thông tin đã nhập
            return View(nguoiDung);
        }




    }
}
