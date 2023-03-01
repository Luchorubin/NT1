using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AppNt.Models
{
    public class League
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public int ConfederationId { get; set; }
        public Confederation Confederation { get; set; }

        public ICollection<Team> Team { get; set; }
    }
}