using Microsoft.AspNetCore.Mvc;
using challenge.Application.DTOs;
using challenge.Application.Services;
using challenge.Domain.Enums;

namespace challenge.Api.Controllers
{
    /// <summary>
    /// Controller para gerenciamento de veículos
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json")]
    public class VehiclesController : ControllerBase
    {
        private readonly VehicleService _vehicleService;

        public VehiclesController(VehicleService vehicleService)
        {
            _vehicleService = vehicleService ?? throw new ArgumentNullException(nameof(vehicleService));
        }

        /// <summary>
        /// Obtém todos os veículos
        /// </summary>
        /// <returns>Lista de veículos</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAllVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetAllVehiclesAsync();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um veículo por ID
        /// </summary>
        /// <param name="id">ID do veículo</param>
        /// <returns>Dados do veículo</returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<VehicleDto>> GetVehicleById(string id)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
                if (vehicle == null)
                    return NotFound(new { message = "Veículo não encontrado" });

                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém um veículo por placa
        /// </summary>
        /// <param name="licensePlate">Placa do veículo</param>
        /// <returns>Dados do veículo</returns>
        [HttpGet("by-license-plate/{licensePlate}")]
        public async Task<ActionResult<VehicleDto>> GetVehicleByLicensePlate(string licensePlate)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleByLicensePlateAsync(licensePlate);
                if (vehicle == null)
                    return NotFound(new { message = "Veículo não encontrado" });

                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém veículos disponíveis para locação
        /// </summary>
        /// <returns>Lista de veículos disponíveis</returns>
        [HttpGet("available")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetAvailableVehicles()
        {
            try
            {
                var vehicles = await _vehicleService.GetAvailableVehiclesAsync();
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Obtém veículos por status
        /// </summary>
        /// <param name="status">Status do veículo</param>
        /// <returns>Lista de veículos com o status especificado</returns>
        [HttpGet("by-status/{status}")]
        public async Task<ActionResult<IEnumerable<VehicleDto>>> GetVehiclesByStatus(VehicleStatus status)
        {
            try
            {
                var vehicles = await _vehicleService.GetVehiclesByStatusAsync(status);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Cria um novo veículo
        /// </summary>
        /// <param name="createDto">Dados do veículo a ser criado</param>
        /// <returns>Dados do veículo criado</returns>
        [HttpPost]
        public async Task<ActionResult<VehicleDto>> CreateVehicle([FromBody] CreateVehicleDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var vehicle = await _vehicleService.CreateVehicleAsync(createDto);
                return CreatedAtAction(nameof(GetVehicleById), new { id = vehicle.Id }, vehicle);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Atualiza um veículo existente
        /// </summary>
        /// <param name="id">ID do veículo</param>
        /// <param name="updateDto">Dados a serem atualizados</param>
        /// <returns>Dados do veículo atualizado</returns>
        [HttpPut("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<VehicleDto>> UpdateVehicle(string id, [FromBody] UpdateVehicleDto updateDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var vehicle = await _vehicleService.UpdateVehicleAsync(id, updateDto);
                return Ok(vehicle);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Altera o status de um veículo
        /// </summary>
        /// <param name="id">ID do veículo</param>
        /// <param name="statusDto">Novo status do veículo</param>
        /// <returns>Dados do veículo atualizado</returns>
        [HttpPatch("{id}/status")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult<VehicleDto>> ChangeVehicleStatus(string id, [FromBody] VehicleStatusDto statusDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var vehicle = await _vehicleService.ChangeVehicleStatusAsync(id, statusDto.Status);
                return Ok(vehicle);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }

        /// <summary>
        /// Remove um veículo
        /// </summary>
        /// <param name="id">ID do veículo</param>
        /// <returns>Confirmação da remoção</returns>
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<ActionResult> DeleteVehicle(string id)
        {
            try
            {
                await _vehicleService.DeleteVehicleAsync(id);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro interno do servidor", details = ex.Message });
            }
        }
    }
}

