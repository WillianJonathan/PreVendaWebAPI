using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreNetFramework.Model
{
    public class Produto
    {

        public int ProdutoId { get; set; }

        public string Nome { get; set; }

        public byte[] Imagem { get; set; }

        public string  Descricao { get; set; }

        public ProdutoGrupo Grupo { get; set; }

        public decimal Valor { get; set; }

    }
}
