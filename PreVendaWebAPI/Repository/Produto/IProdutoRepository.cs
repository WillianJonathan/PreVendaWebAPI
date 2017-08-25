using CoreNetFramework.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreNetFramework.Repository
{
    interface IProdutoRepository : IRepository<Produto>
    {
        List<Produto> RetornarTodosPorProdutoGrupoId(int produtoGrupoId);
    }
}
