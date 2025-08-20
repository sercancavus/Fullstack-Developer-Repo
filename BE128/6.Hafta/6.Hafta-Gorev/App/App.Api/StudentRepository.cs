using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

public class StudentRepository
{
    private readonly AppDbContext _context;
    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<Student> GetAll() => _context.Students.ToList();

    public Student? GetById(int id) => _context.Students.Find(id);

    public bool IsStudentNumberUnique(int studentNumber, int? excludeId = null)
    {
        return !_context.Students.Any(s => s.StudentNumber == studentNumber && (!excludeId.HasValue || s.Id != excludeId.Value));
    }

    public Student Add(Student student)
    {
        if (!IsStudentNumberUnique(student.StudentNumber))
            throw new InvalidOperationException("Öðrenci numarasý benzersiz olmalýdýr.");
        _context.Students.Add(student);
        _context.SaveChanges();
        return student;
    }

    public bool Update(int id, Student student)
    {
        var existing = _context.Students.Find(id);
        if (existing == null) return false;
        if (!IsStudentNumberUnique(student.StudentNumber, id))
            throw new InvalidOperationException("Öðrenci numarasý benzersiz olmalýdýr.");
        existing.FirstName = student.FirstName;
        existing.LastName = student.LastName;
        existing.StudentNumber = student.StudentNumber;
        existing.BirthDate = student.BirthDate;
        _context.SaveChanges();
        return true;
    }

    public bool Delete(int id)
    {
        var student = _context.Students.Find(id);
        if (student == null) return false;
        _context.Students.Remove(student);
        _context.SaveChanges();
        return true;
    }
}
