using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromocionController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult> Create(Promocion promocion)
        {
            try
            {
                var pm = new PromocionManager();
                await pm.Create(promocion);
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
                var pm = new PromocionManager();
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
                var pm = new PromocionManager();
                var promocion = pm.RetrieveById(id);
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
        public ActionResult Update(Promocion promocion)
        {
            try
            {
                var pm = new PromocionManager();
                pm.Update(promocion);
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
                var pm = new PromocionManager();
                pm.Delete(id);
                return Ok(new { Message = $"Promoción con ID {id} eliminada correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
