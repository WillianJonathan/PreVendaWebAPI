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
    public class ProdutosController : Controller
    {
        #region Construtores

        public ProdutosController()
        {
            _produtoRepository = new ProdutoRepository();
        }

        #endregion

        #region Propriedades

        private ProdutoRepository _produtoRepository;

        #endregion

        #region Métodos

        // GET api/produtos
        [HttpGet]
        public IActionResult Get()
        {
            try
            {

                //List<Produto> _produtos = new List<Produto>();

                //for (int i = 0; i < 10; i++)
                //{
                //    _produtos.Add(new Produto()
                //    {
                //        ProdutoId = i,
                //        Nome = "Melancia",
                //        Descricao = "Melancia fatiada na tijela",
                //        Grupo = new ProdutoGrupo() { Descricao = "Frutas", ProdutoGrupoId = 1, Ativo = true },
                //        Valor = i * 12
                //    });
                //}

                //return new ObjectResult(_produtos);
                return new ObjectResult(_produtoRepository.GetAll());
            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Erro ao retornar lista de produtos." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);

            }

        }

        // GET api/produtos/5
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                return new ObjectResult(_produtoRepository.GetItem(id));
            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Erro ao retornar um produto." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // GET api/produtos/grupos/5
        [HttpGet("grupos/{id:int}")]
        public IActionResult GetPorGrupoId(int id)
        {
            try
            {
                //List<Produto> _produtos = new List<Produto>();
                //List<string> _frutas = new List<string>();
                //_frutas.Add("Manga");
                //_frutas.Add("Morango");
                //_frutas.Add("Maça");
                //_frutas.Add("Melancia");
                //_frutas.Add("Maracujá");
                //_frutas.Add("Melão");
                //_frutas.Add("Ameixa");
                //_frutas.Add("Abacate");
                //_frutas.Add("Laranja");
                //_frutas.Add("Uva");

                //int _count = 0;

                //for (int i = 0; i < 30; i++)
                //{
                //    if (_count == 10)
                //        _count = 0;

                //    _produtos.Add(new Produto()
                //    {
                //        ProdutoId = i,
                //        Nome = _frutas[_count],
                //        Descricao = "Melancia fatiada na tijela",
                //        Grupo = new ProdutoGrupo() { Descricao = "Frutas", ProdutoGrupoId = id, Ativo = true },
                //        Valor = i * 12
                //    });

                //    _count += 1;
                //}

                //return new ObjectResult(_produtos);
                return new ObjectResult(_produtoRepository.RetornarTodosPorProdutoGrupoId(id));
            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Erro ao retornar um produto." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // POST api/ProdutoGrupo
        [HttpPost]
        public IActionResult Post([FromBody]Produto produto)
        {
            try
            {
                if (produto == null)
                {
                    return BadRequest(ModelState);
                }

                if (ModelState.IsValid)
                {
                    produto.ProdutoId = 0;
                    produto.ProdutoId = _produtoRepository.Add(produto);

                    return CreatedAtAction("Get", new { Id = produto.ProdutoId }, produto);
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao criar um produto." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // PUT api/produto/5
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody]Produto produto)
        {

            try
            {
                if (produto == null || produto.ProdutoId != id)
                {
                    return BadRequest(ModelState);
                }

                var _produto = _produtoRepository.GetItem(id);

                if (_produto.ProdutoId == 0)
                    return NotFound();


                if (ModelState.IsValid)
                {
                    _produtoRepository.Add(produto);

                    return new NoContentResult();
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao atualizar um produto." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // DELETE api/produto/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {

            try
            {
                if (id == 0)
                {
                    return NotFound();
                }

                var _produtoGrupo = _produtoRepository.GetItem(id);

                if (_produtoGrupo.ProdutoId == 0)
                    return NotFound();


                _produtoRepository.Remove(id);

                return new NoContentResult();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao excluir um produto." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }
        }

        #endregion       
    }
}
