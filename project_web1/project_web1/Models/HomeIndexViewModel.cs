namespace project_web1.Models
{
    public class HomeIndexViewModel
    {
        public int TotalTreTrongTam { get; set; }
        public int TotalTreDaNhanNuoi { get; set; }
        public decimal TotalSoTien { get; set; }
        public List<TodoItem> TodoList { get; set; }
        public List<Tre> TreList { get; set; }
    }
}
