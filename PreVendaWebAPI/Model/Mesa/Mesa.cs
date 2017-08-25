using CoreNetFramework.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreNetFramework.Model
{
    public class Mesa
    {

        public int MesaId { get; set; }

        [Required]
        [Range(1,9999)]
        public int Numero { get; set; }

        [Required]
        [Range(1,4)]
        public MesaStatus Status { get; set; }

    }
}
