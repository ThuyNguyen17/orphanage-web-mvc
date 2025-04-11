using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class PhieuKhamBenh
{
    public int MaPhieuKham { get; set; }

    public double? CanNang { get; set; }

    public double? ChieuCao { get; set; }

    public string? TinhTrangSucKhoe { get; set; }

    public DateTime? NgayGhi { get; set; }

    public int MaSoTre { get; set; }

    public int Idnvyte { get; set; }

    public virtual NhanVienYte IdnvyteNavigation { get; set; } = null!;

    public virtual Tre MaSoTreNavigation { get; set; } = null!;
}
