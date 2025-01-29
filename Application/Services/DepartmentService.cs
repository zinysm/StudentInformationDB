public class DepartmentService
{
    private readonly DepartmentRepository _departmentRepository;

    public DepartmentService(DepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public void CreateDepartment(string name, List<Student> students, List<Lecture> lectures)
    {
        var department = new Department
        {
            Name = name,
            Students = students,
            Lectures = lectures
        };
        _departmentRepository.AddDepartment(department);
    }

    public void AddStudentToDepartment(int departmentId, Student student)
    {
        var department = _departmentRepository.GetDepartmentById(departmentId);
        if (department != null)
        {
            department.Students.Add(student);
            _departmentRepository.UpdateDepartment(department);
        }
    }

    public void AddLectureToDepartment(int departmentId, Lecture lecture)
    {
        var department = _departmentRepository.GetDepartmentById(departmentId);
        if (department != null)
        {
            department.Lectures.Add(lecture);
            _departmentRepository.UpdateDepartment(department);
        }
    }

    public List<Student> GetStudentsInDepartment(int departmentId)
    {
        var department = _departmentRepository.GetDepartmentById(departmentId);
        return department?.Students ?? new List<Student>();
    }

    public List<Lecture> GetLecturesInDepartment(int departmentId)
    {
        var department = _departmentRepository.GetDepartmentById(departmentId);
        return department?.Lectures ?? new List<Lecture>();
    }
}
