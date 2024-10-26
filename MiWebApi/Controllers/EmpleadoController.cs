using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MiWebApi.Data;
using MiWebApi.Models;
namespace MiWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadoController : ControllerBase
    {
        private readonly EmpleadoData _empleadoData;
        public EmpleadoController(EmpleadoData empleadoData)
        {
            this._empleadoData = empleadoData;
        }
        [HttpGet]
        public async Task<ActionResult<List<Empleado>>> GetEmpleados()
        {
            List<Empleado> empleados = await _empleadoData.GetEmpleados();
            return StatusCode(StatusCodes.Status200OK, empleados);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Empleado>> GetEmpleado(int id)
        {
            var empleado = await _empleadoData.GetEmpleado(id);
            if (empleado.IdEmpleado == 0)
            {
                return NotFound();
            }
            return StatusCode(StatusCodes.Status200OK, empleado);
        }
        [HttpPost]
        public async Task<ActionResult> InsertEmpleado([FromBody] Empleado empleado)
        {
            bool result = await _empleadoData.InsertEmpleado(empleado);
            return StatusCode(StatusCodes.Status201Created, new { isSuccess = result });
        }
        [HttpPut]
        public async Task<ActionResult> UpdateEmpleado([FromBody] Empleado empleado)
        {
            bool result = await _empleadoData.UpdateEmpleado(empleado);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = result });
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteEmpleado(int id)
        {
            bool result = await _empleadoData.DeleteEmpleado(id);
            return StatusCode(StatusCodes.Status200OK, new { isSuccess = result });
        }

    }
}
