using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_Project_ASMC4_NET1041_PH53971.Models;
using System.Diagnostics;

namespace Mini_Project_ASMC4_NET1041_PH53971.Controllers
{
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;

        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        private readonly ILogger<HomeController> _logger;
        private readonly MiniProjectAsmc4Net1041Ph53971Context _context;

        public HomeController(ILogger<HomeController> logger, MiniProjectAsmc4Net1041Ph53971Context context)
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




        public IActionResult ComboList(string searchName)
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





        // Action để xem chi tiết combo và các món ăn trong combo
        public IActionResult ComboDetails(int id)
        {
            // Lấy thông tin combo
            var combo = _context.Combos
                .Where(c => c.MaCombo == id)
                .FirstOrDefault();

            if (combo == null)
            {
                return NotFound();
            }

            // Lấy danh sách các món ăn trong combo
            var monAnsInCombo = _context.ChiTietCombos
                .Where(c => c.MaCombo == id)
                .Include(c => c.MaMonAnNavigation)  // Đảm bảo lấy thông tin của món ăn
                .ToList();

            var model = new ComboDetailsViewModel
            {
                Combo = combo,
                MonAnsInCombo = monAnsInCombo
            };

            return View(model);
        }

       

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
