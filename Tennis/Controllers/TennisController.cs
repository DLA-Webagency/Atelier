using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Metrics;
using Tennis.Models;
using Tennis.Repository;

namespace Tennis.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TennisController : ControllerBase
    {
        private readonly IReadJson<Player> _repo;
        public TennisController(IReadJson<Player> repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<IEnumerable<Player>?> GetAll()
        {
            return await _repo.GetAllPlayers();
        }

        [HttpGet]
        public async Task<Player?> GetById(int id)
        {
            return await _repo.Get(id);
        }

        [HttpGet]
        public async Task<IActionResult> GetStatistics()
        {
            var result = await _repo.GetStatistic();
            return Ok(new { Country = result.Country, IMC = result.IMC, Mediane = result.Mediane });
        }
    }
}
