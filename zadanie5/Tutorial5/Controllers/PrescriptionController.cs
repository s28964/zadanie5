using Microsoft.AspNetCore.Mvc;
using Tutorial5.DTOs;
using Tutorial5.Services;

namespace Tutorial5.Controllers;


[Route("api/[controller]")]
[ApiController]
public class PrescriptionController : ControllerBase
{
    private readonly IDbService _dbService;

    public PrescriptionController(IDbService dbService)
    {
        _dbService = dbService;
    }

    [HttpGet]
    public async Task<IActionResult> AddPrescription([FromBody] AddPrescriptionDto prescription)
    {
        try
        {
            await _dbService.AddPrescriptionAsync(prescription);
            return Ok();
        }
        catch (ArgumentNullException e)
        {
            return BadRequest(e.Message);
        }
    }
}