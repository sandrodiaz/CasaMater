using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProEventos.API.Extensions;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Persistence.Models;

namespace ProEventos.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SolicitantesController : ControllerBase
    {

        private readonly ISolicitanteService _solicitanteService;
        private readonly IWebHostEnvironment _hostEnvironment;
        private readonly IAccountService _accountService;

        public SolicitantesController(ISolicitanteService solicitanteService,
                                      IWebHostEnvironment hostEnvironment,
                                      IAccountService accountService)
        {
            _hostEnvironment = hostEnvironment;
            _accountService = accountService;
            _solicitanteService = solicitanteService;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll([FromQuery]PageParams pageParams)
        {
            try
            {
                var solicitantes = await _solicitanteService.GetAllSolicitantesAsync(pageParams);
                if (solicitantes == null) return NoContent();

                Response.AddPagination(solicitantes.CurrentPage,
                                       solicitantes.PageSize,
                                       solicitantes.TotalCount,
                                       solicitantes.TotalPages);

                return Ok(solicitantes);
            }
            catch ( Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                  $"Erro ao tentar recuperar solicitantes. Err {ex.Message}");
            }
        }

        [HttpGet("{solicitanteId}")]
        public async Task<IActionResult> GetSolicitantes(int solicitanteId)
        {
            try
            {
                var solicitante = await _solicitanteService.GetSolicitanteByIdAsync(solicitanteId);
                if(solicitante == null) return NoContent();

                return Ok(solicitante);

            }
            catch ( Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                   $"Erro ao tentar recuperar solicitantes. Erro {ex.Message}");
            }
        }

        [HttpPut("{solicitanteID}")]
        public async Task<IActionResult> Put(int solicitanteID, SolicitanteDto model)
        {
            try
            {
                var SolicitateRetorno = await _solicitanteService.UpdateSolicitante(solicitanteID, model);
                if (SolicitateRetorno == null) return NoContent();

                return Ok(SolicitateRetorno);

            }
            catch ( Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar solicitante. Erro: {ex.Message}");                
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post(SolicitanteDto model)
        {
            try
            {
                var solicitante = await _solicitanteService.AddSolicitante(model);
                if (solicitante == null) return NoContent();

                return Ok(solicitante);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar adicionar solicitantes. Erro: {ex.Message}");
            }
        }

        [HttpDelete("{solicitanteId}")]

        public async Task<IActionResult> Delete(int solicitanteId)
        {
            try
            {
                var solicitante = await _solicitanteService.GetSolicitanteByIdAsync(solicitanteId);
                if (solicitante == null) return NoContent();

                return await _solicitanteService.DeleteSolicitante(solicitanteId)
                   ? Ok(new { message = "Solicitante Deletado"})
                   : throw new Exception("Ocorreu um problema não específico ao tentar deletar solicitante.");

            }
            catch ( Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar deletar solicitantes. Erro: {ex.Message}");
            }


        }
        
        
    }
}