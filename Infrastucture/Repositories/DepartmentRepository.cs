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
        _context.Departments.Add(department);
        _context.SaveChanges();
    }

    public void UpdateDepartment(Department department)
    {
        _context.Departments.Update(department);
        _context.SaveChanges();
    }
}
