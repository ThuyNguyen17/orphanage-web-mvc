using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class HoaDonTaiTro
{
    public int MaSoPhieuHoaDon { get; set; }

    public decimal? SoTien { get; set; }

    public DateTime? NgayLap { get; set; }

    public int MaSoNtt { get; set; }

    public virtual NhaTaiTro? MaSoNttNavigation { get; set; }

}
