using System;
using System.Collections.Generic;

namespace Mini_Project_ASMC4_NET1041_PH53971.Models;

public partial class NguoiDung
{
    public int MaNguoiDung { get; set; }

    public string Ten { get; set; } = null!;

    public string MatKhau { get; set; } = null!;

    public string? Email { get; set; }

    public string? SoDienThoai { get; set; }

    public string? VaiTro { get; set; }

    public DateOnly? NgaySinh { get; set; }

    public bool TrangThai { get; set; }

    public virtual ICollection<DonHang> DonHangs { get; set; } = new List<DonHang>();

    public virtual ICollection<GioHang> GioHangs { get; set; } = new List<GioHang>();
}
