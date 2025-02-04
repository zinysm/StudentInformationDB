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

        if (student == null)
        {
            Console.WriteLine("Studentas nerastas.");
            return;
        }

        if (newDepartment == null)
        {
            Console.WriteLine("Naujas fakultetas nerastas.");
            return;
        }

        // Pašalinamos paskaitos, kurios nepriklauso naujam fakultetui
        var validLectures = student.Lectures.Where(l => newDepartment.Lectures.Contains(l)).ToList();
        student.Lectures = validLectures;

        Console.WriteLine($"Studentas perkeltas į fakultetą: {newDepartment.Name}");

        // Leidžiama pasirinkti naujas paskaitas
        var newLectures = newDepartment.Lectures.Where(l => !student.Lectures.Contains(l)).ToList();

        if (newLectures.Any())
        {
            Console.WriteLine("Galimos naujos paskaitos šiame fakultete:");
            for (int i = 0; i < newLectures.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {newLectures[i].Title}");
            }

            Console.Write("Įveskite pasirinktų paskaitų numerius (atskirkite kableliais) arba palikite tuščią, jei nenorite priskirti naujų paskaitų: ");
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input))
            {
                var selectedIndexes = input.Split(',')
                    .Select(s => int.TryParse(s.Trim(), out int index) ? index - 1 : -1)
                    .Where(index => index >= 0 && index < newLectures.Count)
                    .ToList();

                foreach (var index in selectedIndexes)
                {
                    student.Lectures.Add(newLectures[index]);
                }
            }
        }

        _studentRepository.UpdateStudent(student);
        Console.WriteLine("Studentas sėkmingai perkeltas ir paskaitos atnaujintos.");
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
