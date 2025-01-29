public class Department
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Student> Students { get; set; } = new();
    public List<Lecture> Lectures { get; set; } = new();
}
