using Microsoft.AspNetCore.Mvc;
using Mini_Project_ASMC4_NET1041_PH53971.Models;

namespace Mini_Project_ASMC4_NET1041_PH53971.Controllers
{
    public class BaseController : Controller
    {
        private readonly MiniProjectAsmc4Net1041Ph53971Context _dbContext;

        public BaseController(MiniProjectAsmc4Net1041Ph53971Context dbContext)
        {
            _dbContext = dbContext;
        }

        protected string GetVaiTroByMaNguoiDung(int? maNguoiDung)
        {
            if (maNguoiDung == null) return null;

            // Truy xuất vai trò từ cơ sở dữ liệu
            var user = _dbContext.NguoiDungs.FirstOrDefault(u => u.MaNguoiDung == maNguoiDung);
            return user?.VaiTro; // Giả sử trường VaiTro lưu vai trò
        }
    }
}
