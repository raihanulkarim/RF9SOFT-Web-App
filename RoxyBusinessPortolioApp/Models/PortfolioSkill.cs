namespace RoxyBusinessPortolioApp.Models
{
    public class PortfolioSkill
    {
        public int Id { get; set; }
        public string? SkillName { get; set; }

        // Foreign key to Portfolio
        public int PortfolioId { get; set; }
        public Portfolio? Portfolio { get; set; }

        public static implicit operator string(PortfolioSkill v)
        {
            throw new NotImplementedException();
        }
    }
}
