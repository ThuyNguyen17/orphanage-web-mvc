using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class NhanVienYte
{
    public int Idnvyte { get; set; }

    public string? ChucVu { get; set; }

    public string HoTen { get; set; } = null!;

    public string? DonVi { get; set; }

    public string? Loai { get; set; }

    public virtual ICollection<HoSoSucKhoe> HoSoSucKhoes { get; set; } = new List<HoSoSucKhoe>();

    public virtual ICollection<PhieuKhamBenh> PhieuKhamBenhs { get; set; } = new List<PhieuKhamBenh>();
}
