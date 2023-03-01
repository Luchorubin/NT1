using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AppNt.Models
{
    public class TeamVote
    {
        
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int QtyVotes { get; set; }
    }
}
