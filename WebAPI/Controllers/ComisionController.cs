using BaseManager;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "Admin,InstitucionBancaria,CuentaComercio")]
    [ApiController]
    [Route("api/[controller]")]
    public class ComisionController : ControllerBase
    {
        private readonly ComisionManager _cm;

        public ComisionController()
        {
            _cm = new ComisionManager();
        }

        [HttpGet]
        [Route("RetrieveAll")]
        public ActionResult<IEnumerable<Comision>> RetrieveAll()
        {
            try
            {
                var lista = _cm.Listar();
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveById/{id}")]
        public ActionResult<Comision> RetrieveById(int id)
        {
            try
            {
                var comision = _cm.Obtener(id);
                if (comision == null) return NotFound();
                return Ok(comision);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<Comision>> Create([FromBody] Comision comision)
        {
            try
            {
                await _cm.Create(comision);
                return CreatedAtAction(nameof(RetrieveById), new { id = comision.Id }, comision);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult Update([FromBody] Comision comision)
        {
            try
            {
                var existing = _cm.Obtener(comision.Id);
                if (existing == null) return NotFound();
                _cm.Actualizar(comision);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public ActionResult Delete(int id)
        {
            try
            {
                var existing = _cm.Obtener(id);
                if (existing == null) return NotFound();
                _cm.Eliminar(existing);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
