using Microsoft.AspNetCore.Mvc;
using SIS.Api.Models;
using SIS.Api.Services;

namespace SIS.Api.Controllers;

[ApiController]
[Route("api/students")]
public class StudentsController : ControllerBase
{
    private readonly JsonDatabase _db;

    public StudentsController(JsonDatabase db) => _db = db;

    // GET /api/students
    [HttpGet]
    public IActionResult GetAll()
    {
        var students = _db.GetAllStudents();
        return Ok(students);
    }

    // GET /api/students/{id}
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var student = _db.GetStudentById(id);
        return student is null ? NotFound(new { message = "Student not found." }) : Ok(student);
    }

    // POST /api/students
    [HttpPost]
    public IActionResult Create([FromBody] Student student)
    {
        if (string.IsNullOrWhiteSpace(student.Name) || string.IsNullOrWhiteSpace(student.Record))
            return BadRequest(new { message = "Student Name and ID Number are required." });

        var created = _db.CreateStudent(student);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // PUT /api/students/{id}
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Student student)
    {
        var updated = _db.UpdateStudent(id, student);
        return updated is null ? NotFound(new { message = "Student not found." }) : Ok(updated);
    }

    // DELETE /api/students/{id}
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        bool deleted = _db.DeleteStudent(id);
        return deleted ? Ok(new { deleted = true, id }) : NotFound(new { message = "Student not found." });
    }
}
