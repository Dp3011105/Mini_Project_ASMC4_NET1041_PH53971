using System;
using System.Collections.Generic;

namespace Mini_Project_ASMC4_NET1041_PH53971.Models;

public partial class Combo
{
    public int MaCombo { get; set; }

    public string TenCombo { get; set; } = null!;

    public string? MoTa { get; set; }

    public double Gia { get; set; }

    public bool? TrangThai { get; set; }

    public string? DuongDanHinh { get; set; }

    public virtual ICollection<ChiTietCombo> ChiTietCombos { get; set; } = new List<ChiTietCombo>();

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<ChiTietGioHang> ChiTietGioHangs { get; set; } = new List<ChiTietGioHang>();
}
