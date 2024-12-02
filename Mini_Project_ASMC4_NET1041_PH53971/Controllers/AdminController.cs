using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mini_Project_ASMC4_NET1041_PH53971.Models;

namespace Mini_Project_ASMC4_NET1041_PH53971.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


        private readonly MiniProjectAsmc4Net1041Ph53971Context _context;

        public AdminController(MiniProjectAsmc4Net1041Ph53971Context context)
        {
            _context = context;
        }






        // Hiển thị danh sách người dùng
        public async Task<IActionResult> ListNguoiDung()
        {
            var users = await _context.NguoiDungs.ToListAsync();
            return View(users);
        }

        // Hiển thị form thêm người dùng mới
        public IActionResult CreateNguoiDung()
        {
            return View();
        }

        // Xử lý thêm người dùng mới
        [HttpPost]
        public async Task<IActionResult> CreateNguoiDung(NguoiDung model)
        {
            if (ModelState.IsValid)
            {
                _context.NguoiDungs.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListNguoiDung)); // Sau khi thêm xong, chuyển về trang danh sách người dùng
            }
            return View(model);
        }



        [HttpGet]
        public IActionResult EditNguoiDung(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID không hợp lệ.");
            }

            var nguoiDung = _context.NguoiDungs.FirstOrDefault(n => n.MaNguoiDung == id);
            if (nguoiDung == null)
            {
                return NotFound("Người dùng không tồn tại.");
            }

            return View(nguoiDung);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditNguoiDung(NguoiDung nguoiDung)
        {
            if (nguoiDung == null || nguoiDung.MaNguoiDung <= 0)
            {
                return BadRequest("Dữ liệu không hợp lệ.");
            }
            if (ModelState.IsValid)
            {
                var nguoiDungDb = _context.NguoiDungs.FirstOrDefault(n => n.MaNguoiDung == nguoiDung.MaNguoiDung);
                if (nguoiDungDb == null)
                {
                    return View("NotFound");
                }

                nguoiDungDb.Ten = nguoiDung.Ten;
                nguoiDungDb.Email = nguoiDung.Email;
                nguoiDungDb.SoDienThoai = nguoiDung.SoDienThoai;
                nguoiDungDb.VaiTro = nguoiDung.VaiTro;
                nguoiDungDb.NgaySinh = nguoiDung.NgaySinh;
                nguoiDungDb.TrangThai = nguoiDung.TrangThai;

                _context.SaveChanges();
                return RedirectToAction("ListNguoiDung", "Admin");
            }
            return View(nguoiDung);
        }


        // GET: NguoiDungs


        //// GET: NguoiDungs/Edit/5
        //public async Task<IActionResult> EditNguoiDung(int? id)
        //{
        //	if (id == null)
        //	{
        //		return NotFound();
        //	}

        //	var nguoiDung = await _context.NguoiDungs.FindAsync(id);
        //	if (nguoiDung == null)
        //	{
        //		return NotFound();
        //	}

        //	// Trả về view và truyền model (dữ liệu người dùng) vào view
        //	return View(nguoiDung);
        //}


        //// POST: NguoiDungs/Edit/5
        //// To protect from overposting attacks, enable the specific properties you want to bind to.
        //// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditNguoiDung(int id, [Bind("MaNguoiDung,Ten,MatKhau,Email,SoDienThoai,VaiTro,NgaySinh,TrangThai")] NguoiDung nguoiDung)
        //{
        //	if (id != nguoiDung.MaNguoiDung)
        //	{
        //		return NotFound();
        //	}

        //	if (ModelState.IsValid)
        //	{
        //		try
        //		{
        //			_context.Update(nguoiDung);
        //			await _context.SaveChangesAsync();
        //		}
        //		catch (DbUpdateConcurrencyException)
        //		{
        //			if (!NguoiDungExists(nguoiDung.MaNguoiDung))
        //			{
        //				return NotFound();
        //			}
        //			else
        //			{
        //				throw;
        //			}
        //		}
        //		return RedirectToAction(nameof(Index));
        //	}
        //	return View(nguoiDung); // Nếu có lỗi validation, trả lại view với dữ liệu người dùng
        //}



        //private bool NguoiDungExists(int id)
        //      {
        //          return _context.NguoiDungs.Any(e => e.MaNguoiDung == id);
        //      }
















        // Action hiển thị danh sách món ăn
        public async Task<IActionResult> ListMonAn()
        {
            // Lấy tất cả món ăn và các thông tin liên quan (bao gồm Tag) từ cơ sở dữ liệu
            var monAnList = _context.MonAns
                                    .Include(m => m.MaTagNavigation) // Lấy thông tin về tag liên quan
                                    .ToList();

            return View(monAnList);
        }

        // Action hiển thị form thêm món ăn
        public IActionResult CreateMonAn()
        {
            ViewBag.Tags = _context.Tags.ToList();  // Truyền danh sách tags cho view
            return View(new MonAn());
        }

        // Action xử lý form thêm món ăn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(MonAn monAn)
        {
            if (ModelState.IsValid)
            {
                _context.MonAns.Add(monAn);
                _context.SaveChanges();
                return RedirectToAction("ListMonAn", "Admin");
            }

            // Nếu có lỗi trong ModelState, truyền lại danh sách tags và Model về view
            ViewBag.Tags = _context.Tags.ToList();
            return View(monAn);
        }


        // Action hiển thị form sửa món ăn
        public async Task<IActionResult> EditMonAn(int id)
        {
            var monAn = await _context.MonAns.FindAsync(id);
            if (monAn == null)
            {
                return NotFound();
            }
            ViewBag.Tags = _context.Tags.ToList(); // Lấy danh sách các tag để chọn
            return View(monAn);
        }

        // Action xử lý form sửa món ăn
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditMonAn(int id, MonAn monAn)
        {
            if (id != monAn.MaMonAn)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(monAn);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.MonAns.Any(m => m.MaMonAn == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListMonAn)); // Sau khi sửa, quay lại danh sách món ăn
            }
            ViewBag.Tags = _context.Tags.ToList(); // Nếu có lỗi, truyền lại danh sách tags
            return View(monAn);
        }











        public IActionResult ListComBo()
        {
            // Lấy danh sách combo và chi tiết combo (bao gồm các món ăn liên quan)
            var comboList = _context.Combos
                                    .Include(c => c.ChiTietCombos)  // Lấy chi tiết combo
                                    .ThenInclude(ct => ct.MaMonAnNavigation)  // Lấy thông tin món ăn
                                    .ToList();

            return View(comboList);
        }




        public IActionResult CreateCombo()
        {
            ViewBag.MonAnList = _context.MonAns.ToList();
            return View(new Combo());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateCombo(Combo combo, List<int> selectedMonAnIds, List<string> selectedSoLuongs)
        {
            if (ModelState.IsValid)
            {
                // Lưu Combo
                _context.Combos.Add(combo);
                _context.SaveChanges();

                // Lưu Chi Tiết Combo
                for (int i = 0; i < selectedMonAnIds.Count; i++)
                {
                    // Lấy số lượng tương ứng với món ăn
                    int soLuong = int.Parse(selectedSoLuongs[i]);

                    // Chỉ lưu nếu số lượng > 0
                    if (soLuong > 0)
                    {
                        var chiTiet = new ChiTietCombo
                        {
                            MaCombo = combo.MaCombo,
                            MaMonAn = selectedMonAnIds[i],
                            SoLuong = soLuong
                        };
                        _context.ChiTietCombos.Add(chiTiet);
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("ListComBo", "Admin");
            }

            ViewBag.MonAnList = _context.MonAns.Where(m => m.TrangThai == true).ToList();
            return View(combo);
        }



        public IActionResult EditCombo(int id)
        {
            var combo = _context.Combos.Include(c => c.ChiTietCombos)
                                       .ThenInclude(ct => ct.MaMonAnNavigation)
                                       .FirstOrDefault(c => c.MaCombo == id);
            if (combo == null) return NotFound();

            ViewBag.MonAnList = _context.MonAns.ToList();
            return View(combo);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditCombo(int id, Combo updatedCombo, List<int> selectedMonAnIds, List<string> selectedSoLuongs)
        {
            if (id != updatedCombo.MaCombo) return NotFound();

            if (ModelState.IsValid)
            {
                var combo = _context.Combos.Include(c => c.ChiTietCombos).FirstOrDefault(c => c.MaCombo == id);
                if (combo == null) return NotFound();

                // Cập nhật thông tin cơ bản của combo
                combo.TenCombo = updatedCombo.TenCombo;
                combo.MoTa = updatedCombo.MoTa;
                combo.Gia = updatedCombo.Gia;
                combo.TrangThai = updatedCombo.TrangThai;
                combo.DuongDanHinh = updatedCombo.DuongDanHinh;

                // Xóa chi tiết combo cũ
                _context.ChiTietCombos.RemoveRange(combo.ChiTietCombos);

                // Thêm chi tiết combo mới
                for (int i = 0; i < selectedMonAnIds.Count; i++)
                {
                    int soLuong = int.Parse(selectedSoLuongs[i]);

                    // Chỉ thêm nếu số lượng > 0
                    if (soLuong > 0)
                    {
                        var chiTiet = new ChiTietCombo
                        {
                            MaCombo = id,
                            MaMonAn = selectedMonAnIds[i],
                            SoLuong = soLuong
                        };
                        _context.ChiTietCombos.Add(chiTiet);
                    }
                }

                _context.SaveChanges();
                return RedirectToAction("ListComBo", "Admin");
            }

            ViewBag.MonAnList = _context.MonAns.ToList();
            return View(updatedCombo);
        }




        // GET: Admin/ListDonHang
        public IActionResult ListDonHang()
        {
            var donHangs = _context.DonHangs
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.MaMonAnNavigation)
                .Include(d => d.ChiTietDonHangs)
                    .ThenInclude(ct => ct.MaComboNavigation)
                .ToList();

            return View(donHangs);
        }


       






        public IActionResult IndexTag()
        {
            var tags = _context.Tags.ToList();
            return View(tags);
        }
 
        // Hành động GET để hiển thị form thêm mới
        [HttpGet]
        public IActionResult CreateTag()
        {
            return View();
        }

        // Hành động POST để lưu Tag mới vào cơ sở dữ liệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTag(Tag tag)
        {
            if (ModelState.IsValid)
            {
                // Lưu tag mới vào cơ sở dữ liệu
                _context.Tags.Add(tag);
                _context.SaveChanges();
                return RedirectToAction(nameof(IndexTag)); // Quay lại trang danh sách
            }
            return View(tag);
        }


        // Hành động GET để hiển thị form sửa thông tin tag
        [HttpGet]
        public IActionResult EditTag(int id)
        {
            var tag = _context.Tags.FirstOrDefault(t => t.MaTag == id);
            if (tag == null)
            {
                return NotFound();
            }
            return View(tag);
        }

        // Hành động POST để cập nhật tag vào cơ sở dữ liệu
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTag(Tag tag)
        {
            if (ModelState.IsValid)
            {
                // Cập nhật tag vào cơ sở dữ liệu
                _context.Update(tag);
                _context.SaveChanges();
                return RedirectToAction(nameof(IndexTag)); // Quay lại trang danh sách
            }
            return View(tag);
        }


    }
}
