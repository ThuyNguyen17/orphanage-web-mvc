using System;
using System.Collections.Generic;

namespace project_web1.Models;

public partial class LoaiChiTieu
{
    public int ChiTieuId { get; set; }

    public string TenChiTieu { get; set; } = null!;

    public virtual ICollection<CtchiTieu> CtchiTieus { get; set; } = new List<CtchiTieu>();
}
