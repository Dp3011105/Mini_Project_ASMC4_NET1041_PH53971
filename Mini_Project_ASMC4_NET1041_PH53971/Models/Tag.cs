using System;
using System.Collections.Generic;

namespace Mini_Project_ASMC4_NET1041_PH53971.Models;

public partial class Tag
{
    public int MaTag { get; set; }

    public string TenTag { get; set; } = null!;

    public bool TrangThai { get; set; }

    public virtual ICollection<MonAn> MonAns { get; set; } = new List<MonAn>();
}
