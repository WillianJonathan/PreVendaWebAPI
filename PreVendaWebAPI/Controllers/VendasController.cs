using CoreNetFramework.Model;
using CoreNetFramework.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PreVendaWebAPI.Controllers
{
    [Route("api/[controller]")]
    public class VendasController : Controller
    {

        #region Construtores

        public VendasController()
        {
            _vendaRepository = new VendaRepository();
            _vendaItemRepository = new VendaItemRepository();
        }

        #endregion

        #region Propriedades

        private VendaRepository _vendaRepository;
        private VendaItemRepository _vendaItemRepository;

        #endregion

        #region Métodos

        // GET api/vendas
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return new ObjectResult(_vendaRepository.GetAll());
            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Erro ao retornar lista de vendas." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);

            }

        }

        // GET api/vendas/5
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {

                //var _vendaFake = new Venda();
                //_vendaFake.Cliente = new Cliente() { ClienteId = 1, DataCadastro = new DateTime(2017, 01, 1), DataNascimento = new DateTime(2017, 01, 1), Email = "ww@hotmail.com", Nome = "Jose" };
                //_vendaFake.DataCadastro = DateTime.Now;
                //_vendaFake.DataFaturamento = DateTime.Now;
                //_vendaFake.Status = CoreNetFramework.Model.Enums.VendaStatus.PENDENTE;
                //_vendaFake.Usuario = new Usuario() { Login = "teste", Senha = "123", UsuarioId = 1 };
                //_vendaFake.Valor = 1000;
                //_vendaFake.VendaId = 1;

                //for (int i = 1; i <= 30; i++)
                //{
                //    var item = new VendaItem()
                //    {
                //        VendaId = 1,
                //        Desconto = 0,
                //        Produto = new Produto() { Descricao = "abacate", Valor = 100, Grupo = new ProdutoGrupo() { Ativo = true, Descricao = "Frutas", ProdutoGrupoId = 1 }, Nome = "Abacate ao molho branco", ProdutoId = 1 },
                //        Quantidade = i * 1,
                //        ValorUnitario = i * 10,
                //        VendaItemId = i
                //    };

                //    _vendaFake.Itens.Add(item);
                //}


                //return new ObjectResult(_vendaFake);

                return new ObjectResult(_vendaRepository.GetItem(id));



            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Erro ao retornar uma venda." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // GET api/vendas/mesas/5       
        [HttpGet("mesas/{Id:int}")]
        public IActionResult GetPorMesaId(int Id)
        {
            try
            {
                return new ObjectResult(_vendaRepository.RetornarVendaPorMesaId(Id, CoreNetFramework.Model.Enums.VendaStatus.PENDENTE));
            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Erro ao retornar uma venda." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // POST api/vendas
        [HttpPost]
        public IActionResult Post([FromBody]Venda venda)
        {
            try
            {
                if (venda == null)
                {
                    return BadRequest(ModelState);
                }

                if (ModelState.IsValid)
                {
                    venda.VendaId = 0;
                    venda.VendaId = _vendaRepository.Add(venda);

                    return CreatedAtAction("Get", new { Id = venda.VendaId }, venda);
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao criar uma venda." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // POST api/vendas/1/vendaItem
        [HttpPost("{vendaId:int}/vendaItem")]
        public IActionResult vendaItemPost(int vendaId,[FromBody]VendaItem vendaItem)
        {
            try
            {
                if (vendaItem == null)
                {
                    return BadRequest(ModelState);
                }

                if (ModelState.IsValid)
                {
                    vendaItem.VendaItemId = 0;
                    vendaItem.VendaItemId = _vendaItemRepository.Add(vendaItem);

                    return CreatedAtAction("Get", new { Id = vendaItem.VendaItemId }, vendaItem);
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao criar uma vendaitem." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // PUT api/vendas/5
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody]Venda venda)
        {

            try
            {
                if (venda == null || venda.VendaId != id)
                {
                    return BadRequest(ModelState);
                }

                var _venda = _vendaRepository.GetItem(id);

                if (_venda.VendaId == 0)
                    return NotFound();


                if (ModelState.IsValid)
                {
                    _vendaRepository.Add(venda);

                    return new NoContentResult();
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao atualizar uma venda." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // DELETE api/vendas/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {

            try
            {
                if (id == 0)
                {
                    return NotFound();
                }

                var _venda = _vendaRepository.GetItem(id);

                if (_venda.VendaId == 0)
                    return NotFound();


                _vendaRepository.Remove(id);

                return new NoContentResult();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao excluir uma venda." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }
        }

        #endregion       
    }
}
