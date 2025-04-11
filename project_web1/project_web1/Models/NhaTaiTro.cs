using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class NhaTaiTro
{
    public int MaSoNtt { get; set; }

    public string Ten { get; set; } = null!;

    public string? ToChuc { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? DiaChi { get; set; }

    public string Sdt { get; set; } = null!;

    public string? GhiChu { get; set; }

    public virtual ICollection<HoaDonTaiTro> HoaDonTaiTros { get; set; } = new List<HoaDonTaiTro>();
}
