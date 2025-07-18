﻿using BaseManager;
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
                await Task.Run(() => tm.Registrar(transaccion)); // If Registrar is not async
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
                transaccion.Id = id; // Asegúrate de asignar el id del path
                var tm = new TransaccionManager();
                tm.Actualizar(transaccion); // Debes tener este método en tu Manager
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
