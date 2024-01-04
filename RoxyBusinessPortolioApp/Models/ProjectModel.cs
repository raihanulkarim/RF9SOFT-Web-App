namespace RoxyBusinessPortolioApp.Models
{
    public class ProjectModel
    {
        public int Id { get; set; }
        public string? Title{ get; set; }
        public string? Description { get; set; } = null;
        public int Sales { get; set; }
        public int Price { get; set; }
        public string? Link { get; set; }
        public string? Image{ get; set; }
    }
}
