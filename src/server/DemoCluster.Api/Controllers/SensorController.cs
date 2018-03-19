using System.Collections.Generic;
using System.Threading.Tasks;
using DemoCluster.DAL;
using DemoCluster.DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoCluster.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class SensorController : Controller
    {
        private readonly IConfigurationStorage storage;
        public SensorController(IConfigurationStorage store)
        {
            storage = store;
        }

        [HttpGet]
        public async Task<IEnumerable<SensorConfig>> Get()
        {
            return await storage.GetSensorListAsync();
        }

        [HttpGet("{sensorId}")]
        public async Task<IActionResult> Get(int sensorId)
        {
            return Ok(await storage.GetSensorAsync(sensorId));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] SensorConfig config)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            SensorConfig result = await storage.SaveSensorAsync(config);

            return CreatedAtAction("Get", new { sensorId = result.SensorId }, result);
        }

        [HttpDelete("{sensorId}")]
        public async Task<IActionResult> Delete(int sensorId)
        {
            await storage.RemoveSensorAsync(sensorId);
            return Ok();
        }
    }
}