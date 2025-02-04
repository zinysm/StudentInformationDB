using Microsoft.Extensions.DependencyInjection;

public class Menu
{
    private readonly DepartmentService _departmentService;
    private readonly StudentService _studentService;
    private readonly LectureService _lectureService;

    public Menu(IServiceProvider serviceProvider)
    {
        _departmentService = serviceProvider.GetRequiredService<DepartmentService>();
        _studentService = serviceProvider.GetRequiredService<StudentService>();
        _lectureService = serviceProvider.GetRequiredService<LectureService>();  // Užtikrinkite, kad LectureService yra injekuojamas
    }

    public void Show()
    {
        while (true)
        {
            DisplayMenuOptions();
            Console.Write("\nĮvesk savo pasirinkimą: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateDepartment();
                    break;
                case "2":
                    AddStudentToDepartment();
                    break;
                case "3":
                    AddLecturesToDepartment();
                    break;
                case "4":
                    CreateStudent();
                    break;
                case "5":
                    AssignLecturesToStudent();
                    break;
                case "6":
                    CreateLecture();  
                    break;
                case "7":
                    TransferStudent();
                    break;
                case "8":
                    DisplayStudentsInDepartment();
                    break;
                case "9":
                    DisplayLecturesInDepartment();
                    break;
                case "10":
                    DisplayLecturesForStudent();
                    break;
                case "11":
                    DisplayAllLectures();
                    break;
                case "12":
                    DisplayAllStudents();
                    break;
                case "13":
                    DisplayAllDepartments();
                    break;
                case "0":
                    Console.WriteLine("Programos pabaiga");
                    return;
                default:
                    Console.WriteLine("Neteisingas pasirinkimas");
                    break;
            }
        }
    }

    private void DisplayMenuOptions()
    {
        Console.WriteLine("\nMenu:");
        Console.WriteLine("1. Sukurti fakultetą");
        Console.WriteLine("2. Priskirti studentui fakultetą");
        Console.WriteLine("3. Pridėti paskaitą fakultetui");
        Console.WriteLine("4. Sukurti studentą");
        Console.WriteLine("5. Priskirti paskaitas studentui");
        Console.WriteLine("6. Sukurti paskaitą");
        Console.WriteLine("7. Perkelti studentą į kitą fakultetą");
        Console.WriteLine("8. Parodyti visus studentus fakultete");
        Console.WriteLine("9. Parodyti paskaitas fakultete");
        Console.WriteLine("10. Parodyti studento paskaitas");
        Console.WriteLine("11. Parodyti visas paskaitas");
        Console.WriteLine("12. Parodyti visus studentus");
        Console.WriteLine("13. Parodyti visus fakultetus");
        Console.WriteLine("0. Pabaiga");
    }

    private void CreateDepartment()
    {
        Console.Write("Įvesk fakulteto pavadinimą: ");
        var departmentName = Console.ReadLine();
        _departmentService.CreateDepartment(departmentName, new List<Student>(), new List<Lecture>());
    }

    private void AddStudentToDepartment()
    {
        Console.Write("Departamento ID: ");
        if (!int.TryParse(Console.ReadLine(), out int departmentId))
        {
            Console.WriteLine("Neteisingai įvesta. Įvesk teisingą reikšmę");
            return;
        }
        Console.Write("Įvesk studento vardą pavardę: ");
        var studentName = Console.ReadLine();
        var student = new Student { Name = studentName };
        _departmentService.AddStudentToDepartment(departmentId, student);
        Console.WriteLine("Studentas priskirtas fakultetui!");
    }

    private void AddLecturesToDepartment()
    {
        Console.Write("Įvesk fakulteto ID: ");
        if (!int.TryParse(Console.ReadLine(), out int departmentId))
        {
            Console.WriteLine("Neteisingai įvesta. Įvesk teisingą reikšmę");
            return;
        }

        Console.Write("Įvesk paskaitų ID (atskirtus kableliais): ");
        var lectureIdsInput = Console.ReadLine();
        var lectureIds = lectureIdsInput.Split(',')
                                        .Select(s => int.TryParse(s, out int id) ? id : -1)
                                        .Where(id => id != -1)
                                        .ToList();

        _departmentService.AddLecturesToDepartment(departmentId, lectureIds);
    }


    private void CreateStudent()
    {
        Console.Write("Įvesk studento vardą pavardę: ");
        var studentName = Console.ReadLine();

        var studentExists = _studentService.CheckIfStudentExists(studentName);
        if (studentExists)
        {
            Console.WriteLine("Klaida: Studentas jau egzistuoja.");
            return;  // Jei studentas egzistuoja, nepraleisime įvedimo
        }

        Console.Write("Įvessk fakulteto ID: ");
        if (!int.TryParse(Console.ReadLine(), out int departmentId))
        {
            Console.WriteLine("Neteisingai įvesta. Įvesk teisingą reikšmę");
            return;
        }

        _studentService.CreateStudent(studentName, departmentId);
    }

    private void AssignLecturesToStudent()
    {
        Console.Write("Įvesk studento ID: ");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Neteisingai įvesta. Įvesk teisingą reikšmę");
            return;
        }

        Console.Write("Įvesk paskaitų ID, atskirk kableliais ");
        var lectureIdsInput = Console.ReadLine();
        var lectureIds = lectureIdsInput.Split(',').Select(s => int.TryParse(s, out int id) ? id : -1).Where(id => id != -1).ToList();

        _studentService.AssignLecturesToStudent(studentId, lectureIds);
    }

    private void CreateLecture()
    {
        Console.Write("Įvesk paskaitos pavadinimą: ");
        var lectureTitle = Console.ReadLine();
        _lectureService.CreateLecture(lectureTitle);  // Šis metodas kviečia `CreateLecture` iš `LectureService`
    }

    private void TransferStudent()
    {
        Console.Write("Įvesk studento ID: ");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Neteisingai įvesta. Įvesk teisingą reikšmę");
            return;
        }
        Console.Write("Įvesk fakulteto ID: ");
        if (!int.TryParse(Console.ReadLine(), out int newDepartmentId))
        {
            Console.WriteLine("Neteisingai įvesta. Įvesk teisingą reikšmę");
            return;
        }
        _studentService.TransferStudent(studentId, newDepartmentId);
    }

    private void DisplayStudentsInDepartment()
    {
        Console.Write("Įvesk fakulteto ID: ");
        if (!int.TryParse(Console.ReadLine(), out int departmentId))
        {
            Console.WriteLine("Neteisingai įvesta. Įvesk teisingą reikšmę");
            return;
        }
        var students = _departmentService.GetStudentsInDepartment(departmentId);
        Console.WriteLine("Studentai fakultete:");
        foreach (var student in students)
            Console.WriteLine($"- {student.Id}: {student.Name}");
    }

    private void DisplayLecturesInDepartment()
    {
        Console.Write("Įvesk fakulteto ID: ");
        if (!int.TryParse(Console.ReadLine(), out int departmentId))
        {
            Console.WriteLine("Neteisingai įvesta. Įvesk teisingą reikšmę");
            return;
        }
        var lectures = _departmentService.GetLecturesInDepartment(departmentId);
        Console.WriteLine("Paskaitos fakultete:");
        foreach (var lecture in lectures)
            Console.WriteLine($"- {lecture.Id}: {lecture.Title}");
    }

    private void DisplayLecturesForStudent()
    {
        Console.Write("Įvesk studento ID: ");
        if (!int.TryParse(Console.ReadLine(), out int studentId))
        {
            Console.WriteLine("Neteisingai įvesta. Įvesk teisingą reikšmę");
            return;
        }
        var lectures = _studentService.GetLecturesForStudent(studentId);
        Console.WriteLine("Studento paskaitos:");
        foreach (var lecture in lectures)
            Console.WriteLine($"- {lecture.Id}: {lecture.Title}");
    }

    private void DisplayAllLectures()
    {
        var lectures = _lectureService.GetAllLectures();
        Console.WriteLine("Visos paskaitos:");
        foreach (var lecture in lectures)
            Console.WriteLine($"- {lecture.Id}: {lecture.Title}");
    }

    private void DisplayAllStudents()
    {
        var students = _studentService.GetAllStudents();
        Console.WriteLine("Visi studentai:");
        foreach (var student in students)
        {
            Console.WriteLine($"- {student.Id}: {student.Name}");
        }
    }

    private void DisplayAllDepartments()
    {
        var departments = _departmentService.GetAllDepartments();
        Console.WriteLine("Visi fakultetai:");
        foreach (var department in departments)
        {
            Console.WriteLine($"- {department.Id}: {department.Name}");
        }
    }
}
