using BaseManager;
using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransaccionController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]

        public async Task<ActionResult> Create(Transaccion transaccion)
        {
            try
            {
                var tm = new TransaccionManager();
                await Create(transaccion);
                return Ok(transaccion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("RetrieveByBanco")]
        public ActionResult RetrieveByBanco(int idBanco)
        {
            try
            {
                var tm = new TransaccionManager();
                var lstResults = tm.ObtenerPorBanco(idBanco);
                return Ok(lstResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveByComercio")]
        public ActionResult RetrieveByComercio(int idComercio)
        {
            try
            {
                var tm = new TransaccionManager();
                var result = tm.ObtenerPorComercio(idComercio);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
