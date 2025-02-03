public class StudentService
{
    private readonly StudentRepository _studentRepository;
    private readonly DepartmentRepository _departmentRepository;
    private readonly LectureRepository _lectureRepository;

    public StudentService(StudentRepository studentRepository, DepartmentRepository departmentRepository, LectureRepository lectureRepository)
    {
        _studentRepository = studentRepository;
        _departmentRepository = departmentRepository;
        _lectureRepository = lectureRepository;
    }

    public void CreateStudent(string name, int departmentId)
    {
        try
        {
            var studentExists = _studentRepository.GetAllStudents().Any(s => s.Name == name);
            if (studentExists)
            {
                Console.WriteLine("Klaida: Studentas jau egzistuoja.");
                return;
            }

            var department = _departmentRepository.GetDepartmentById(departmentId);
            if (department == null)
            {
                Console.WriteLine("Fakultetas nerastas.");
                return;
            }

            var student = new Student
            {
                Name = name,
                DepartmentId = departmentId,
                Department = department
            };

            _studentRepository.AddStudent(student);
            Console.WriteLine($"Studentas {name} sukurtas ir priskirtas fakultetui.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Klaida: {ex.Message}");
        }
    }

    public bool CheckIfStudentExists(string name)
    {
        return _studentRepository.GetAllStudents().Any(s => s.Name == name);
    }

    public void AssignLecturesToStudent(int studentId, List<int> lectureIds)
    {
        try
        {
            var student = _studentRepository.GetStudentById(studentId);
            if (student == null)
            {
                Console.WriteLine("Studentas nerastas.");
                return;
            }

            // Patikriname, ar studentas jau yra priskirtas fakultetui
            var department = student.Department;
            if (department == null)
            {
                Console.WriteLine("Studentas nėra priskirtas fakultetui.");
                return; // Negalima priskirti paskaitų, jei studentas nėra priskirtas fakultetui
            }

            // Patikriname, kurios paskaitos priklauso fakultetui
            var validLectures = new List<Lecture>();
            foreach (var lectureId in lectureIds)
            {
                var lecture = _lectureRepository.GetLectureById(lectureId);
                if (lecture != null && department.Lectures.Contains(lecture))  // Tikriname, ar paskaita priklauso fakultetui
                {
                    validLectures.Add(lecture);  // Pridedame tik tas paskaitas, kurios priklauso departamentui
                }
                else
                {
                    Console.WriteLine($"Paskaita su ID {lectureId} nepriklauso šiam fakultetui ir nebus priskirta studentui.");
                }
            }

            // Priskiriame tinkamas paskaitas studentui
            if (validLectures.Any())
            {
                student.Lectures.AddRange(validLectures);
                _studentRepository.UpdateStudent(student);
                Console.WriteLine("Paskaitos sėkmingai priskirtos studentui.");
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

    public void TransferStudent(int studentId, int newDepartmentId)
    {
        var student = _studentRepository.GetStudentById(studentId);
        var newDepartment = _departmentRepository.GetDepartmentById(newDepartmentId);

        if (student != null && newDepartment != null)
        {
            student.DepartmentId = newDepartmentId;
            student.Department = newDepartment;

            var validLectures = student.Lectures.Where(l => newDepartment.Lectures.Contains(l)).ToList();
            student.Lectures = validLectures;

            var newLectures = newDepartment.Lectures.Where(l => !student.Lectures.Contains(l)).ToList();

            if (newLectures.Any())
            {
                Console.WriteLine("Naujos paskaitos, kurias galima priskirti studentui:");
                foreach (var lecture in newLectures)
                {
                    Console.WriteLine($"- {lecture.Title}");
                }

                Console.Write("Ar norite priskirti šias paskaitas studentui? (y/n): ");
                var choice = Console.ReadLine();
                if (choice?.ToLower() == "y")
                {
                    student.Lectures.AddRange(newLectures);
                }
            }

            _studentRepository.UpdateStudent(student);
            Console.WriteLine("Studentas sėkmingai perkeltas su paskaitomis.");
        }
    }

    public List<Lecture> GetLecturesForStudent(int studentId)
    {
        var student = _studentRepository.GetStudentById(studentId);
        return student?.Lectures ?? new List<Lecture>();
    }

    public List<Student> GetAllStudents()
    {
        return _studentRepository.GetAllStudents();
    }
}
