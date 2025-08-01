using BaseManager;
using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransaccionController : ControllerBase
    {
        [HttpPost("Create")]
        public async Task<ActionResult<Transaccion>> Create([FromBody] Transaccion transaccion)
        {
            try
            {
                var tm = new TransaccionManager();
                await Task.Run(() => tm.Registrar(transaccion));
                return Ok(transaccion);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        // Aquí eliminamos la ambigüedad: solo un atributo HTTP con ruta clara
        [HttpPut("Update/{id}")]
        public ActionResult<Transaccion> Update(int id, [FromBody] Transaccion transaccion)
        {
            try
            {
                transaccion.Id = id;
                var tm = new TransaccionManager();
                tm.Actualizar(transaccion);
                return Ok(transaccion);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("RetrieveAll")]
        public ActionResult<IEnumerable<Transaccion>> RetrieveAll()
        {
            try
            {
                var tm = new TransaccionManager();
                var lstResults = tm.RetrieveAll();
                return Ok(lstResults);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("RetrieveByBanco")]
        public ActionResult<List<Transaccion>> RetrieveByBanco([FromQuery] string iban)
        {
            try
            {
                var tm = new TransaccionManager();
                var lstResults = tm.ObtenerPorBanco(iban) ?? new List<Transaccion>();
                return Ok(lstResults);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("RetrieveByComercio")]
        public ActionResult<List<Transaccion>> RetrieveByComercio([FromQuery] int idComercio)
        {
            try
            {
                var tm = new TransaccionManager();
                var lstResults = tm.ObtenerPorComercio(idComercio) ?? new List<Transaccion>();
                return Ok(lstResults);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
