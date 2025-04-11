using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class Tre
{
    public int MaSoTre { get; set; }

    public string? HoTen { get; set; }

    public DateTime? NgaySinh { get; set; }

    public string? GioiTinh { get; set; }

    public DateTime? NgayNhapTrungTam { get; set; }

    public string? TrangThai { get; set; }

    public int MaNhanVien { get; set; }

    public string? AnhDaiDien { get; set; }

    public string? Quote { get; set; }

    public virtual ICollection<DonThuoc> DonThuocs { get; set; } = new List<DonThuoc>();

    public virtual ICollection<HoSoSucKhoe> HoSoSucKhoes { get; set; } = new List<HoSoSucKhoe>();

    public virtual NguoiChamSoc MaNhanVienNavigation { get; set; } = null!;

    public virtual ICollection<NguoiNhanNuoi> NguoiNhanNuois { get; set; } = new List<NguoiNhanNuoi>();

    public virtual ICollection<PhieuKhamBenh> PhieuKhamBenhs { get; set; } = new List<PhieuKhamBenh>();

    public virtual PhieuTiemChung? PhieuTiemChung { get; set; }
}
