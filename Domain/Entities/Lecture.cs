public class Lecture
{
    public int Id { get; set; }
    public string Title { get; set; }
    public List<Department> Departments { get; set; } = new();
    public List<Student> Students { get; set; } = new();
}