using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_Project_ASMC4_NET1041_PH53971.Models;

namespace Mini_Project_ASMC4_NET1041_PH53971.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        private readonly MiniProjectAsmc4Net1041Ph53971Context _context = new MiniProjectAsmc4Net1041Ph53971Context(); // Thay thế "YourDbContext" bằng DbContext của bạn.

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string matKhau)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(matKhau))
            {
                ViewBag.Error = "Email và mật khẩu không được để trống.";
                return View();
            }

            
            var nguoiDung = _context.NguoiDungs
                .FirstOrDefault(nd => nd.Email == email && nd.MatKhau == matKhau);

            if (nguoiDung != null)
            {
                
                if (!nguoiDung.TrangThai)
                {
                    ViewBag.Error = "Tài khoản của bạn đã bị vô hiệu hóa.";
                    return View();
                }

                
                HttpContext.Session.SetString("TenNguoiDung", nguoiDung.Ten);
                HttpContext.Session.SetInt32("MaNguoiDung", nguoiDung.MaNguoiDung);

               
                if (nguoiDung.VaiTro == "KhachHang")
                {
                    return RedirectToAction("Index", "KhachHang"); 
                }
                else if (nguoiDung.VaiTro == "Admin")
                {
                    return RedirectToAction("Index", "Admin"); 
                }
                else
                {
                    
                    ViewBag.Error = "Vai trò người dùng không hợp lệ.";
                    return View();
                }
            }

            ViewBag.Error = "Email hoặc mật khẩu không chính xác.";
            return View();
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); 
            return RedirectToAction("Index", "Home");
        }




        [HttpGet]
        public IActionResult DangKy()
        {
            var model = new NguoiDung(); // Khởi tạo model để tránh null
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> DangKy(NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra trùng email
                var existingUser = await _context.NguoiDungs.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ViewBag.Error = "Email này đã được sử dụng.";
                    return View(model);
                }

                // Cập nhật VaiTro và TrangThai
                model.VaiTro = "KhachHang";
                model.TrangThai = true;

                // Thêm vào cơ sở dữ liệu
                await _context.NguoiDungs.AddAsync(model);
                await _context.SaveChangesAsync();

                return RedirectToAction("Login", "Login");
            }

            return View(model); // Trả lại View nếu ModelState không hợp lệ
        }





        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(string email)
        {
            var user = _context.NguoiDungs.FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                ViewBag.Error = "Email không tồn tại trong hệ thống.";
                return View();
            }

            ViewBag.Password = user.MatKhau; // Hiển thị mật khẩu trực tiếp (chỉ để học, không an toàn thực tế)
            return View();
        }




    }
}
