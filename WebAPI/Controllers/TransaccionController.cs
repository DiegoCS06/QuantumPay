using BaseManager;
using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
                await Task.Run(() => tm.Registrar(transaccion));
                return Ok(transaccion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Transaccion transaccion)
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
                return StatusCode(500, ex.Message);
            }
        }


        [HttpGet]
        [Route("RetrieveAll")]
        public ActionResult RetrieveAll()
        {
            try
            {
                var tm = new TransaccionManager();
                var lstResults = tm.RetrieveAll();
                return Ok(lstResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveByBanco")]
        public ActionResult RetrieveByBanco(string iban)
        {
            try
            {
                var tm = new TransaccionManager();
                var lstResults = tm.ObtenerPorBanco(iban);
                if (lstResults == null)
                {
                    return Ok(new List<object>());
                }

                return Ok(new List<object> { lstResults });
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

                if (result == null)
                {
                    return Ok(new List<object>());
                }

                return Ok(new List<object> { result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
