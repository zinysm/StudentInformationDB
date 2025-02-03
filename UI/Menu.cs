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
        _lectureService = serviceProvider.GetRequiredService<LectureService>();
    }

    public void Show()
    {
        while (true)
        {
            DisplayMenuOptions();
            Console.Write("\nĮvesk savo pasirinkimą ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1": CreateDepartment(); break;
                case "2": AddStudentToDepartment(); break;
                case "3": AddLectureToDepartment(); break;
                case "4": CreateStudentAndEnroll(); break;
                case "5": CreateLecture(); break;
                case "6": TransferStudent(); break;
                case "7": DisplayStudentsInDepartment(); break;
                case "8": DisplayLecturesInDepartment(); break;
                case "9": DisplayLecturesForStudent(); break;
                case "10": DisplayAllLectures(); break;
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
        Console.WriteLine("4. Sukurti studentą ir priskirti paskaitas");
        Console.WriteLine("5. Sukurti paskaitas");
        Console.WriteLine("6. Perkelti studentą į kitą fakultetą");
        Console.WriteLine("7. Parodyti visus studentus fakultete");
        Console.WriteLine("8. Parodyti paskaitas fakultete");
        Console.WriteLine("9. Parodyti studento paskaitas");
        Console.WriteLine("10. Parodyti visas paskaitas");
        Console.WriteLine("0. Pabaigas");
    }

    private void CreateDepartment()
    {
        Console.Write("Įvesk fakulteto pavadinimą: ");
        var departmentName = Console.ReadLine();
        _departmentService.CreateDepartment(departmentName, new List<Student>(), new List<Lecture>());
        Console.WriteLine("Fakultetas sukurtas!");
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

    private void AddLectureToDepartment()
    {
        Console.Write("Įvessk fakulteto ID: ");
        if (!int.TryParse(Console.ReadLine(), out int departmentId))
        {
            Console.WriteLine("Neteisingai įvesta. Įvesk teisingą reikšmę");
            return;
        }
        Console.Write("Įvesk paskaitos pavadinimą: ");
        var lectureTitle = Console.ReadLine();
        var lecture = new Lecture { Title = lectureTitle };
        _departmentService.AddLectureToDepartment(departmentId, lecture);
        Console.WriteLine("Paskaitos pridėtos departamentui");
    }

    private void CreateStudentAndEnroll()
    {
        Console.Write("Įvesk studento vardą pavardę: ");
        var studentName = Console.ReadLine();
        Console.Write("Įvessk fakulteto ID: ");
        if (!int.TryParse(Console.ReadLine(), out int departmentId))
        {
            Console.WriteLine("Neteisingai įvesta. Įvesk teisingą reikšmę");
            return;
        }
        Console.Write("Įvesk paskaitų ID, atskirk kableliais ");
        var lectureIdsInput = Console.ReadLine();
        var lectureIds = lectureIdsInput.Split(',').Select(s => int.TryParse(s, out int id) ? id : -1).Where(id => id != -1).ToList();
        _studentService.CreateStudent(studentName, departmentId, lectureIds);
        Console.WriteLine("Studentas sukurtas ir pridėtos paskaitos");
    }

    private void CreateLecture()
    {
        Console.Write("Įvesk paskaitos pavadinimą: ");
        var lectureTitle = Console.ReadLine();
        _lectureService.CreateLecture(lectureTitle);
        Console.WriteLine("Paskaitą sukurta");
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
        Console.WriteLine("Studentas perkeltas sėkmingai");
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
}
