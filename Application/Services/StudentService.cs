public class StudentService
{
    private readonly StudentRepository _studentRepository;
    private readonly DepartmentRepository _departmentRepository;

    public StudentService(StudentRepository studentRepository, DepartmentRepository departmentRepository)
    {
        _studentRepository = studentRepository;
        _departmentRepository = departmentRepository;
    }

    public void CreateStudent(string name, int departmentId, List<int> lectureIds)
    {
        var department = _departmentRepository.GetDepartmentById(departmentId);
        var lectures = department?.Lectures.Where(l => lectureIds.Contains(l.Id)).ToList() ?? new List<Lecture>();

        var student = new Student
        {
            Name = name,
            DepartmentId = departmentId,
            Department = department,
            Lectures = lectures
        };

        _studentRepository.AddStudent(student);
    }

    public void TransferStudent(int studentId, int newDepartmentId)
    {
        var student = _studentRepository.GetStudentById(studentId);
        var newDepartment = _departmentRepository.GetDepartmentById(newDepartmentId);

        if (student != null && newDepartment != null)
        {
            student.DepartmentId = newDepartmentId;
            student.Department = newDepartment;
            _studentRepository.UpdateStudent(student);
        }
    }

    public List<Lecture> GetLecturesForStudent(int studentId)
    {
        var student = _studentRepository.GetStudentById(studentId);
        return student?.Lectures ?? new List<Lecture>();
    }
}
