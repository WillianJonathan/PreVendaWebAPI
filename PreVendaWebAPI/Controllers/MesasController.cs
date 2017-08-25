using CoreNetFramework.Model;
using CoreNetFramework.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CoreNetFramework.Controllers
{
    [Route("api/[controller]")]  
    public class MesasController : Controller
    {
        #region Construtores

        public MesasController()
        {
            _mesaRepository = new MesaRepository();
        }

        #endregion

        #region Propriedades

        private MesaRepository _mesaRepository;

        #endregion

        #region Métodos

        // GET api/mesas
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                return new ObjectResult(_mesaRepository.GetAll());
            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Erro ao retornar lista de Mesas." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);

            }

        }

        // GET api/mesas/5
        [HttpGet("{id:int}")]
        public IActionResult Get(int id)
        {
            try
            {
                return new ObjectResult(_mesaRepository.GetItem(id));
            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Erro ao retornar uma Mesa." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // POST api/mesas
        [HttpPost]
        public IActionResult Post([FromBody]Mesa mesa)
        {
            try
            {
                if (mesa == null)
                {
                    return BadRequest(ModelState);
                }

                if (ModelState.IsValid)
                {
                    mesa.MesaId = 0;
                    mesa.MesaId = _mesaRepository.Add(mesa);

                    return CreatedAtAction("Get", new { Id = mesa.MesaId }, mesa);
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao criar uma Mesa." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // PUT api/mesas/5
        [HttpPut("{id:int}")]
        public IActionResult Put(int id, [FromBody]Mesa mesa)
        {

            try
            {
                if (mesa == null || mesa.MesaId != id)
                {
                    return BadRequest(ModelState);
                }

                var _mesa = _mesaRepository.GetItem(id);

                if (_mesa.MesaId == 0)
                    return NotFound();


                if (ModelState.IsValid)
                {
                    _mesaRepository.Add(mesa);

                    return new NoContentResult();
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao atualizar uma Mesa." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }

        }

        // DELETE api/mesas/5
        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {

            try
            {
                if (id == 0)
                {
                    return NotFound();
                }

                var _mesa = _mesaRepository.GetItem(id);

                if (_mesa.MesaId == 0)
                    return NotFound();


                _mesaRepository.Remove(id);

                return new NoContentResult();

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Erro ao excluir uma Mesa." + System.Environment.NewLine + " Detalhes:" + ex.Message);
                return BadRequest(ModelState);
            }
        }

        #endregion       
    }
}
