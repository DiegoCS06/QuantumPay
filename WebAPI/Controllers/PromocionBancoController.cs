using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Authorize(Roles = "Admin,Cliente,InstitucionBancaria,CuentaComercio")]
    [Route("api/[controller]")]
    [ApiController]
    public class PromocionBancoController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> Create(PromocionBanco promocion)
        {
            try
            {
                var manager = new PromocionBancoManager();
                await manager.Crear(promocion);
                return Ok(promocion);
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
                var pm = new PromocionBancoManager();
                var lst = pm.RetrieveAll();
                return Ok(lst);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveById")]
        public ActionResult RetrieveById(int id)
        {
            try
            {
                var pm = new PromocionBancoManager();
                var promocion = pm.OrdenarPorId(id);
                if (promocion == null)
                {
                    return Ok(new List<object>());
                }

                return Ok(new List<object> { promocion });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]
        public ActionResult Update(PromocionBanco promocion)
        {
            try
            {
                var pm = new PromocionBancoManager();
                pm.Actualizar(promocion);
                return Ok(promocion);
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
                var pm = new PromocionBancoManager();
                pm.Eliminar(id);
                return Ok(new { Message = $"Promoción con ID {id} eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
