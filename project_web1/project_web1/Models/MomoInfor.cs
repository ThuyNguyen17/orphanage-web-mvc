using System.ComponentModel.DataAnnotations;

namespace project_web1.Models
{
    public class MomoInfor
    {
        [Key]
        public int Id { get; set; }
        public string OrderId { get; set; }
        public string Amount { get; set; }
        public string FullName { get; set; }
        public string OrderInfo { get; set; }

        public DateTime DatePaid { get; set; }
    }
}
