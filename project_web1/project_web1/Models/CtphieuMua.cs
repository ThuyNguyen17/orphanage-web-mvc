using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class CtphieuMua
{
    public int MaSoThuoc { get; set; }

    public int MaSoMuaThuoc { get; set; }

    public int? SoLuong { get; set; }

    public decimal? Gia { get; set; }

    public virtual ThuocMuaNgoai MaSoMuaThuocNavigation { get; set; } = null!;

    public virtual Thuoc MaSoThuocNavigation { get; set; } = null!;
}
