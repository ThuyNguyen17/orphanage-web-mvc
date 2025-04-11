using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class Ctdonthuoc
{
    public int MaDonThuoc { get; set; }

    public int MaSoThuoc { get; set; }

    public int? SoLuong { get; set; }

    public virtual DonThuoc MaDonThuocNavigation { get; set; } = null!;

    public virtual Thuoc MaSoThuocNavigation { get; set; } = null!;
}
