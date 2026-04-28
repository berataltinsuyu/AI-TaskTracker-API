using AITaskTracker.API.Data;
using AITaskTracker.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace AITaskTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController : ControllerBase
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
      _context = context;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
      var tasks = await _context.TaskItems
            .OrderByDescending(x=> x.CreatedAt)
            .ToListAsync();
      return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
      var task = await _context.TaskItems.FindAsync(id);
      if (task is null)
      {
        return NotFound("Task not found.");
      }
      return Ok(task);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TaskItem taskItem)
    {
        taskItem.CreatedAt = DateTime.UtcNow;

        await _context.TaskItems.AddAsync(taskItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new{id=taskItem.Id}, taskItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id,TaskItem updatedTask)
    {
      var task = await _context.TaskItems.FindAsync(id);

      if (task is null)
      {
        return NotFound("Task not found");
      }

      task.Title = updatedTask.Title;
      task.Description = updatedTask.Description;
      task.IsCompleted = updatedTask.IsCompleted;

      await _context.SaveChangesAsync();
      return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete (int id)
    {
        var task = await _context.TaskItems.FindAsync(id);

        if (task is null)
      {
          return NotFound("Task not found.");
      }
      
      _context.TaskItems.Remove(task);
      await _context.SaveChangesAsync();

      return NoContent();
    }
}
