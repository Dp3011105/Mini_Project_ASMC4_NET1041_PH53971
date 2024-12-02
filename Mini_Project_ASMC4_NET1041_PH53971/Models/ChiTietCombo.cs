using System;
using System.Collections.Generic;

namespace Mini_Project_ASMC4_NET1041_PH53971.Models;

public partial class ChiTietCombo
{
    public int MaCombo { get; set; }

    public int MaMonAn { get; set; }

    public int SoLuong { get; set; }

    public virtual Combo MaComboNavigation { get; set; } = null!;

    public virtual MonAn MaMonAnNavigation { get; set; } = null!;
}
