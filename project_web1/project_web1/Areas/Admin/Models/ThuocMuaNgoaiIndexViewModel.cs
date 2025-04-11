using project_web1.Models;

namespace project_web1.Areas.Admin.Models
{
    public class ThuocMuaNgoaiIndexViewModel
    {
        public List<ThuocMuaNgoai> DanhSachPhieu { get; set; } = new();
        public decimal TongChi { get; set; }
        public ThuocMuaNgoai PhieuMoi { get; set; } = new();
    }
}
