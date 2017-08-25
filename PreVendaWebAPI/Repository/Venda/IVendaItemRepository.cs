using CoreNetFramework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreNetFramework.Repository
{
    interface IVendaItemRepository: IRepository<VendaItem>
    {
        List<VendaItem> RetornarTodosPorVendaId(int vendaId);
    }
}
