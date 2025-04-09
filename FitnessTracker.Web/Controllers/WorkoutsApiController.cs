using Microsoft.AspNetCore.Mvc;
using FitnessTracker.DataAccess.Interfaces;
using FitnessTracker.Models;
[Route("api/[controller]")]
[ApiController]
public class WorkoutsApiController : ControllerBase
{
    private readonly IWorkoutRepository _repository;

    public WorkoutsApiController(IWorkoutRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Workout>> GetWorkouts()
    {
        return Ok(_repository.GetAllWorkouts());
    }

    [HttpGet("{id}")]
    public ActionResult<Workout> GetWorkout(int id)
    {
        var workout = _repository.GetWorkoutById(id);
        if (workout == null)
        {
            return NotFound();
        }
        return Ok(workout);
    }

}