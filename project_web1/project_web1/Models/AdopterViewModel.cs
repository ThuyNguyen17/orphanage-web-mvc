namespace project_web1.Models
{
    public class AdopterViewModel
    {
        public List<NguoiNhanNuoi> Adopters { get; set; }
        public List<Tre> Children { get; set; }
        public List<Tre> AvailableChildren { get; set; }
        public int TotalAdopters { get; set; }
        public int TotalAdoptedChildren { get; set; }
    }
}
