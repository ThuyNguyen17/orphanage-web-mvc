using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class CtchiTieu
{
    public int MaPhieuChiTieu { get; set; }

    public int ChiTieuId { get; set; }

    public decimal? SoTien { get; set; }

    public virtual LoaiChiTieu ChiTieu { get; set; } = null!;

    public virtual PhieuChiTieu MaPhieuChiTieuNavigation { get; set; } = null!;
}
