using CoreNetFramework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreNetFramework.Model
{
    public class Venda
    {

        public Venda()
        {
            _cliente = new Cliente();
            _usuario = new Usuario();
            _itens = new List<VendaItem>();
            _mesa = new Mesa();
        }

        private Mesa _mesa;
        private Cliente _cliente;
        private Usuario _usuario;
        private List<VendaItem> _itens;

        public int VendaId { get; set; }
        
        public Cliente Cliente
        {
            get { return _cliente; }
            set { _cliente = value; }
        }

        public DateTime DataCadastro { get; set; }

        public DateTime DataFaturamento { get; set; }

        public VendaStatus Status { get; set; }

        public decimal Valor { get; set; }

        public Usuario Usuario
        {
            get { return _usuario; }
            set { _usuario = value; }
        }

        public List<VendaItem> Itens
        {
            get { return _itens; }
            set { _itens = value; }
        }

        public Mesa Mesa
        {
            get { return _mesa; }
            set { _mesa = value; }

        }

    }
}
