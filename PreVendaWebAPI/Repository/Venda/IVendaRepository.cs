using CoreNetFramework.Model;
using CoreNetFramework.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreNetFramework.Repository
{
    interface IVendaRepository:IRepository<Venda>
    {      
        Venda RetornarVendaPorMesaId(int mesaId, VendaStatus vendaStatus);
    }
}
