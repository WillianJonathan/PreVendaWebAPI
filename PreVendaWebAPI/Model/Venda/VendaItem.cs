using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreNetFramework.Model
{
    public class VendaItem
    {

        public int VendaItemId { get; set; }

        public int VendaId { get; set; }

        public Produto Produto { get; set; }

        public decimal Quantidade { get; set; }

        public decimal ValorUnitario { get; set; }

        public decimal Desconto { get; set; }

    }
}
