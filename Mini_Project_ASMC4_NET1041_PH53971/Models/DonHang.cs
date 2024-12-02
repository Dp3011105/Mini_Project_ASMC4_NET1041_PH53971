using System;
using System.Collections.Generic;

namespace Mini_Project_ASMC4_NET1041_PH53971.Models;

public partial class DonHang
{
    public int MaDonHang { get; set; }

    public DateTime ThoiGianDat { get; set; }

    public string? TrangThai { get; set; }

    public double TongTien { get; set; }

    public int? MaNguoiDung { get; set; }

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual NguoiDung? MaNguoiDungNavigation { get; set; }
}
