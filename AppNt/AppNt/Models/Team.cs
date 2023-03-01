using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppNt.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int LeagueId { get; set; }

        public League League { get; set; }

        public ICollection<Vote> Vote { get; set; }



    }
}