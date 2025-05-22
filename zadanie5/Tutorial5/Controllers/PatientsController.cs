using Microsoft.AspNetCore.Mvc;
using Tutorial5.Services;

namespace Tutorial5.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IDbService _dbService;

    public PatientsController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPatient(int id)
    {
        var result = await _dbService.GetPatientAsync(id);
        if (result == null)
            return NotFound();
        return Ok(result);
    }
}