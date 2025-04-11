using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class DonThuoc
{
    public int MaDonThuoc { get; set; }

    public string? ThoiGianDung { get; set; }

    public string? TrangThai { get; set; }

    public string? GhiChu { get; set; }

    public int MaSoTre { get; set; }

    public virtual ICollection<Ctdonthuoc> Ctdonthuocs { get; set; } = new List<Ctdonthuoc>();

    public virtual Tre MaSoTreNavigation { get; set; } = null!;
}
