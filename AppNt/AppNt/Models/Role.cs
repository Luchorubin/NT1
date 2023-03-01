using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace AppNt.Models
 
{
    //0 es votante, 1 es admin
    public enum Role
    {

        VOTANTE,
        ADMINISTRADOR
       
}
}
