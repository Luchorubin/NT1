using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AppNt.Models
{
    public class Confederation
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } //Tengo que ponerlo por BD cual es. Salvo que nos enseñen a trabajar con ENUMS.

        public ICollection<League> leagues { get; set; }

    }
}
