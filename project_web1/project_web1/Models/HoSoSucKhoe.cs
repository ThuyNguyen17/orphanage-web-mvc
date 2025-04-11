using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class HoSoSucKhoe
{
    public int MaHoSo { get; set; }

    public string? TinhTrangSucKhoe { get; set; }

    public string? TienSuBenh { get; set; }

    public string? DiUng { get; set; }

    public double? CanNang { get; set; }

    public double? ChieuCao { get; set; }

    public string? KetLuan { get; set; }

    public string? GhiChu { get; set; }

    public int Idnvyte { get; set; }

    public int MaSoTre { get; set; }

    public virtual NhanVienYte IdnvyteNavigation { get; set; } = null!;

    public virtual Tre MaSoTreNavigation { get; set; } = null!;
}
