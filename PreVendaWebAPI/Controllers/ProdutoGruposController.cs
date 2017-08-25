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
    public class ProdutoGruposController : Controller
    {

        #region Construtores

        public ProdutoGruposController()
        {
            _produtoGrupoRepository = new ProdutoGrupoRepository();
        }

        #endregion

        #region Propriedades

        private ProdutoGrupoRepository _produtoGrupoRepository;

        #endregion

        #region Métodos

        // GET api/produtoGrupos
        [HttpGet]
        public IActionResult Get()
        {
            try
            {

                //List<ProdutoGrupo> _produtoGrupos = new List<ProdutoGrupo>();
                //for (int i = 0; i < 10; i++)
                //{
                //    _produtoGrupos.Add(new ProdutoGrupo()
                //    {
                //          ProdutoGrupoId = i,
                //          Descricao = "Bebidas",
                //          Ativo = true                          
                //    });
                //}

                //return new ObjectResult(_produtoGrupos);

                return new ObjectResult(_produtoGrupoRepository.GetAll());
            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Erro ao retornar lista de produto grupo." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);

            }

        }

        // GET api/produtoGrupos/5
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                return new ObjectResult(_produtoGrupoRepository.GetItem(id));
            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Erro ao retornar um produto grupo." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // POST api/ProdutoGrupo
        [HttpPost]
        public IActionResult Post([FromBody]ProdutoGrupo produtoGrupo)
        {
            try
            {
                if (produtoGrupo == null)
                {
                    return BadRequest(ModelState);
                }

                if (ModelState.IsValid)
                {
                    produtoGrupo.ProdutoGrupoId = 0;
                    produtoGrupo.ProdutoGrupoId = _produtoGrupoRepository.Add(produtoGrupo);

                    return CreatedAtAction("Get", new { Id = produtoGrupo.ProdutoGrupoId }, produtoGrupo);
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao criar um produto grupo." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // PUT api/ProdutoGrupo/5
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody]ProdutoGrupo produtoGrupo)
        {

            try
            {
                if (produtoGrupo == null || produtoGrupo.ProdutoGrupoId != id)
                {
                    return BadRequest(ModelState);
                }

                var _produtoGrupo = _produtoGrupoRepository.GetItem(id);

                if (_produtoGrupo.ProdutoGrupoId == 0)
                    return NotFound();


                if (ModelState.IsValid)
                {
                    _produtoGrupoRepository.Add(produtoGrupo);

                    return new NoContentResult();
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao atualizar um produto grupo." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // DELETE api/ProdutoGrupo/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {

            try
            {
                if (id == 0)
                {
                    return NotFound();
                }

                var _produtoGrupo = _produtoGrupoRepository.GetItem(id);

                if (_produtoGrupo.ProdutoGrupoId == 0)
                    return NotFound();


                _produtoGrupoRepository.Remove(id);

                return new NoContentResult();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao excluir um produto grupo." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }
        }

        #endregion       

    }
}
