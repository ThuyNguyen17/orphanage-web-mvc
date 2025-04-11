using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class NguoiChamSoc
{
    public int MaNhanVien { get; set; }

    public string HoTen { get; set; } = null!;

    public string? SoDienThoai { get; set; }

    public string? Email { get; set; }

    public string ImageUrl { get; set; } = null!;

    public virtual ICollection<PhieuChiTieu> PhieuChiTieus { get; set; } = new List<PhieuChiTieu>();

    public virtual ICollection<Tre> Tres { get; set; } = new List<Tre>();
}
