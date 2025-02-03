public class DepartmentService
{
    private readonly DepartmentRepository _departmentRepository;
    private readonly LectureRepository _lectureRepository;

    public DepartmentService(DepartmentRepository departmentRepository, LectureRepository lectureRepository)
    {
        _departmentRepository = departmentRepository;
        _lectureRepository = lectureRepository;
    }

    public void CreateDepartment(string name, List<Student> students, List<Lecture> lectures)
    {
        try
        {
            var departmentExists = _departmentRepository.GetAllDepartments().Any(d => d.Name == name);
            if (departmentExists)
            {
                Console.WriteLine("Error: Fakultetas jau egzistuoja.");
                return;
            }

            var department = new Department
            {
                Name = name,
                Students = students,
                Lectures = lectures
            };
            _departmentRepository.AddDepartment(department);
            Console.WriteLine("Fakultetas sukurtas!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
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

    public void AddLecturesToDepartment(int departmentId, List<int> lectureIds)
    {
        try
        {
            var department = _departmentRepository.GetDepartmentById(departmentId);
            if (department == null)
            {
                Console.WriteLine("Fakultetas nerastas.");
                return;
            }

            var validLectures = new List<Lecture>();
            foreach (var lectureId in lectureIds)
            {
                var lecture = _lectureRepository.GetLectureById(lectureId);
                if (lecture != null && !department.Lectures.Contains(lecture))
                {
                    validLectures.Add(lecture);
                }
                else
                {
                    Console.WriteLine($"Paskaita su ID {lectureId} jau priskirta fakultetui arba neegzistuoja.");
                }
            }

            // Pridedame paskaitas
            if (validLectures.Any())
            {
                department.Lectures.AddRange(validLectures);
                _departmentRepository.UpdateDepartment(department);
                Console.WriteLine("Paskaitos sėkmingai priskirtos fakultetui.");
            }
            else
            {
                Console.WriteLine("Nepriskirta jokių paskaitų.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Klaida: {ex.Message}");
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

    public List<Department> GetAllDepartments()
    {
        return _departmentRepository.GetAllDepartments();
    }
}
