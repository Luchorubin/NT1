using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;




namespace AppNt.Models
{
    public class User //Esto es valido tanto para el estudiante como para el Admin.
    {
        public int Id { get; set; }

        [Required(ErrorMessage="Por favor, ingrese un nombre")]
        public string Name { get; set; }

        [Range(0, 99999999, ErrorMessage = "Por favor ingrese el DNI con el formato correcto")]
        [Required(ErrorMessage = "Por favor, ingrese un DNI")]
        public int IdentificationNumber { get; set; }

        [Required(ErrorMessage = "Por favor, ingrese una contraseña")]
        public string Password { get; set; }

        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Por favor, ingrese un apellido")]
        public string Lastname { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Por favor, ingrese un email")]
        public string Email { get; set; }

        [Range(18, 120, ErrorMessage = "Por favor ingrese una edad entre 18 y 120")]
        [Required(ErrorMessage = "Por favor, ingrese una edad")]
        public int Age { get; set; }

        [EnumDataType(typeof(Role))]
        public Role Role { get; set; }
    }

        
}
