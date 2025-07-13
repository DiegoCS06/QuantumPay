using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]

        public async Task<ActionResult> Create(Admin admin)
        {
            try
            {
                var am = new AdminManager();
                await am.Create(admin);
                return Ok(admin);
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
                var am = new AdminManager();
                var lstResults = am.RetrieveAll();
                return Ok(lstResults);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveById")]
        public ActionResult RetrieveById(int Id)
        {
            try
            {
                var am = new AdminManager();
                var result = am.RetrieveById(Id);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveByUserName")]
        public ActionResult Retrieve(string userName)
        {
            try
            {
                var am = new AdminManager();
                var result = am.Retrieve(userName);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        [HttpPut]
        [Route("Update")]
        public ActionResult Update(Admin admin)
        {
            try
            {
                var cm = new AdminManager();
                var update = cm.Update(admin);
                return Ok(update);
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
                var cm = new AdminManager();
                var existing = cm.RetrieveById(id);
                cm.Delete(id);
                return Ok(new { Message = $"Admin con ID {id} eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
