using Microsoft.EntityFrameworkCore;

public class StudentRepository
{
    private readonly StudentDB _context;

    public StudentRepository(StudentDB context)
    {
        _context = context;
    }

    public Student GetStudentById(int id)
    {
        return _context.Students.Include(s => s.Lectures).FirstOrDefault(s => s.Id == id);
    }

    public void AddStudent(Student student)
    {
        if (_context.Students.Any(s => s.Name == student.Name))
        {
            throw new Exception("Studentas jau egzistuoja");
        }

        _context.Students.Add(student);
        _context.SaveChanges();
    }

    public void UpdateStudent(Student student)
    {
        _context.Students.Update(student);
        _context.SaveChanges();
    }
}
