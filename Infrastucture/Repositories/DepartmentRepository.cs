using Microsoft.EntityFrameworkCore;

public class DepartmentRepository
{
    private readonly StudentDB _context;

    public DepartmentRepository(StudentDB context)
    {
        _context = context;
    }

    public List<Department> GetAllDepartments()
    {
        return _context.Departments
            .Include(d => d.Students)
            .Include(d => d.Lectures)
            .ToList();
    }

    public Department GetDepartmentById(int id)
    {
        return _context.Departments
            .Include(d => d.Students)
            .Include(d => d.Lectures)
            .FirstOrDefault(d => d.Id == id);
    }

    public void AddDepartment(Department department)
    {
        if (_context.Departments.Any(d => d.Name == department.Name))
        {
            throw new Exception("Fakultetas jau egzistuoja.");
        }

        _context.Departments.Add(department);
        _context.SaveChanges();
    }
    public void AddLectureToDepartment(int departmentId, Lecture lecture)
    {
        var department = _context.Departments.Include(d => d.Lectures).FirstOrDefault(d => d.Id == departmentId);
        if (department != null)
        {
            // Patikriname, ar paskaita jau nėra priskirta
            if (!department.Lectures.Contains(lecture))
            {
                department.Lectures.Add(lecture);
                _context.SaveChanges();
                Console.WriteLine($"Paskaita {lecture.Title} priskirta departamentui.");
            }
            else
            {
                Console.WriteLine("Paskaita jau priskirta šiam fakultetui.");
            }
        }
        else
        {
            Console.WriteLine("Departamentas nerastas.");
        }
    }

    public void UpdateDepartment(Department department)
    {
        _context.Departments.Update(department);
        _context.SaveChanges();
    }
}
