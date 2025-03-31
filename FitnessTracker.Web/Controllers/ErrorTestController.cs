using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;
using FitnessTracker.DataAccess.Interfaces; // Repository interfaces
public class ErrorTestController : Controller
{
    private readonly ILogger<ErrorTestController> _logger;

    public ErrorTestController(ILogger<ErrorTestController> logger)
    {
        _logger = logger;
    }

    // Throw a generic exception
    public IActionResult ThrowGenericException()
    {
        throw new Exception("This is a test generic exception");
    }

    // Throw a specific type of exception
    public IActionResult ThrowArgumentException()
    {
        throw new ArgumentException("This is a test argument exception");
    }

    // Simulate a database-related exception
    public IActionResult SimulateDatabaseError()
    {
        throw new InvalidOperationException("Simulated database connection error");
    }

    // Simulate a validation error
    public IActionResult ValidationError()
    {
        ModelState.AddModelError("", "Simulated validation error");
        return BadRequest(ModelState);
    }
}