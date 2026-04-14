using System.Text.Json;
using SIS.Api.Models;

namespace SIS.Api.Services;

/// <summary>
/// Flat-file JSON database. All data files live next to the compiled binary
/// (AppContext.BaseDirectory = bin/Debug/net8.0/ or bin/Release/net8.0/).
/// Two files are managed:
///   students.json  — student records
///   users.json     — admin user accounts
/// </summary>
public class JsonDatabase
{
    private readonly string _studentsPath;
    private readonly string _usersPath;

    private static readonly JsonSerializerOptions _jsonOpts = new()
    {
        WriteIndented = true,
        PropertyNameCaseInsensitive = true
    };

    // ── Constructor: resolve paths relative to the binary ──────────────────
    public JsonDatabase()
    {
        string bin = AppContext.BaseDirectory;
        _studentsPath = Path.Combine(bin, "students.json");
        _usersPath    = Path.Combine(bin, "users.json");

        SeedUsers();
    }

    // ── STUDENTS ────────────────────────────────────────────────────────────

    public List<Student> GetAllStudents()
    {
        return ReadFile<List<Student>>(_studentsPath) ?? [];
    }

    public Student? GetStudentById(int id)
        => GetAllStudents().FirstOrDefault(s => s.Id == id);

    public Student CreateStudent(Student student)
    {
        var students = GetAllStudents();
        student.Id = students.Count > 0 ? students.Max(s => s.Id) + 1 : 1;
        student.CreatedAt = DateTime.UtcNow;
        students.Add(student);
        WriteFile(_studentsPath, students);
        return student;
    }

    public Student? UpdateStudent(int id, Student updated)
    {
        var students = GetAllStudents();
        int idx = students.FindIndex(s => s.Id == id);
        if (idx == -1) return null;

        updated.Id        = id;
        updated.CreatedAt = students[idx].CreatedAt;  // preserve original timestamp
        students[idx]     = updated;
        WriteFile(_studentsPath, students);
        return updated;
    }

    public bool DeleteStudent(int id)
    {
        var students = GetAllStudents();
        int removed  = students.RemoveAll(s => s.Id == id);
        if (removed == 0) return false;
        WriteFile(_studentsPath, students);
        return true;
    }

    // ── USERS ───────────────────────────────────────────────────────────────

    public User? FindUser(string username, string password)
    {
        var users = ReadFile<List<User>>(_usersPath) ?? [];
        return users.FirstOrDefault(u =>
            u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)
            && u.Password == password);
    }

    // ── HELPERS ─────────────────────────────────────────────────────────────

    private T? ReadFile<T>(string path)
    {
        if (!File.Exists(path)) return default;
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<T>(json, _jsonOpts);
    }

    private void WriteFile<T>(string path, T data)
    {
        string json = JsonSerializer.Serialize(data, _jsonOpts);
        File.WriteAllText(path, json);
    }

    /// <summary>Seed a default admin account if users.json does not exist.</summary>
    private void SeedUsers()
    {
        if (File.Exists(_usersPath)) return;

        var defaults = new List<User>
        {
            new() { Id = 1, Username = "admin", Password = "admin123" }
        };
        WriteFile(_usersPath, defaults);
    }
}
