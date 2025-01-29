// UI/Menu.cs
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
            Console.WriteLine("\nMenu:");
            Console.WriteLine("1. Create Department");
            Console.WriteLine("2. Add Student to Department");
            Console.WriteLine("3. Add Lecture to Department");
            Console.WriteLine("4. Create Student and Enroll in Lectures");
            Console.WriteLine("5. Create Lecture");
            Console.WriteLine("6. Transfer Student to Another Department");
            Console.WriteLine("7. Display All Students in Department");
            Console.WriteLine("8. Display All Lectures in Department");
            Console.WriteLine("9. Display All Lectures for a Student");
            Console.WriteLine("10. Display All Lectures");
            Console.WriteLine("0. Exit");

            Console.Write("\nEnter your choice: ");
            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter Department Name: ");
                    var departmentName = Console.ReadLine();
                    _departmentService.CreateDepartment(departmentName, new List<Student>(), new List<Lecture>());
                    Console.WriteLine("Department created successfully!");
                    break;

                case "2":
                    Console.Write("Enter Department ID: ");
                    var departmentId = int.Parse(Console.ReadLine());
                    Console.Write("Enter Student Name: ");
                    var studentName = Console.ReadLine();
                    var student = new Student { Name = studentName };
                    _departmentService.AddStudentToDepartment(departmentId, student);
                    Console.WriteLine("Student added to department successfully!");
                    break;

                case "3":
                    Console.Write("Enter Department ID: ");
                    var deptId = int.Parse(Console.ReadLine());
                    Console.Write("Enter Lecture Title: ");
                    var lectureTitle = Console.ReadLine();
                    var lecture = new Lecture { Title = lectureTitle };
                    _departmentService.AddLectureToDepartment(deptId, lecture);
                    Console.WriteLine("Lecture added to department successfully!");
                    break;

                case "4":
                    Console.Write("Enter Student Name: ");
                    var newStudentName = Console.ReadLine();
                    Console.Write("Enter Department ID: ");
                    var deptIdForStudent = int.Parse(Console.ReadLine());
                    Console.Write("Enter Lecture IDs (comma-separated): ");
                    var lectureIdsInput = Console.ReadLine();
                    var lectureIds = lectureIdsInput.Split(',').Select(int.Parse).ToList();
                    _studentService.CreateStudent(newStudentName, deptIdForStudent, lectureIds);
                    Console.WriteLine("Student created and enrolled in lectures successfully!");
                    break;

                case "5":
                    Console.Write("Enter Lecture Title: ");
                    var newLectureTitle = Console.ReadLine();
                    _lectureService.CreateLecture(newLectureTitle);
                    Console.WriteLine("Lecture created successfully!");
                    break;

                case "6":
                    Console.Write("Enter Student ID: ");
                    var studentId = int.Parse(Console.ReadLine());
                    Console.Write("Enter New Department ID: ");
                    var newDeptId = int.Parse(Console.ReadLine());
                    _studentService.TransferStudent(studentId, newDeptId);
                    Console.WriteLine("Student transferred successfully!");
                    break;

                case "7":
                    Console.Write("Enter Department ID: ");
                    var deptIdToDisplay = int.Parse(Console.ReadLine());
                    var studentsInDept = _departmentService.GetStudentsInDepartment(deptIdToDisplay);
                    Console.WriteLine("Students in Department:");
                    foreach (var s in studentsInDept)
                        Console.WriteLine($"- {s.Id}: {s.Name}");
                    break;

                case "8":
                    Console.Write("Enter Department ID: ");
                    var deptIdToDisplayLectures = int.Parse(Console.ReadLine());
                    var lecturesInDept = _departmentService.GetLecturesInDepartment(deptIdToDisplayLectures);
                    Console.WriteLine("Lectures in Department:");
                    foreach (var l in lecturesInDept)
                        Console.WriteLine($"- {l.Id}: {l.Title}");
                    break;

                case "9":
                    Console.Write("Enter Student ID: ");
                    var studentIdToDisplayLectures = int.Parse(Console.ReadLine());
                    var lecturesForStudent = _studentService.GetLecturesForStudent(studentIdToDisplayLectures);
                    Console.WriteLine("Lectures for Student:");
                    foreach (var l in lecturesForStudent)
                        Console.WriteLine($"- {l.Id}: {l.Title}");
                    break;

                case "10":
                    var allLectures = _lectureService.GetAllLectures();
                    Console.WriteLine("All Lectures:");
                    foreach (var l in allLectures)
                        Console.WriteLine($"- {l.Id}: {l.Title}");
                    break;

                case "0":
                    Console.WriteLine("Exiting the program. Goodbye!");
                    return;

                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }
}
