using System.Collections.Generic;

namespace RoxyBusinessPortolioApp.Models
{
    public class Portfolio
    {
        public int? Id { get; set; }
        public string? Title { get; set; }
        public string? ShortDesc { get; set; }
        public string? Image { get; set; }
        public string? Link { get; set; }
        public List<PortfolioSkill>? PorfolioSkills { get; set; }
    }
}
