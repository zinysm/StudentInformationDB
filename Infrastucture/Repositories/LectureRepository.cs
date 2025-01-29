using Microsoft.EntityFrameworkCore;

public class LectureRepository
{
    private readonly StudentDB _context;

    public LectureRepository(StudentDB context)
    {
        _context = context;
    }

    public List<Lecture> GetAllLectures()
    {
        return _context.Lectures
            .Include(l => l.Students)
            .Include(l => l.Departments)
            .ToList();
    }

    public Lecture GetLectureById(int id)
    {
        return _context.Lectures
            .Include(l => l.Students)
            .Include(l => l.Departments)
            .FirstOrDefault(l => l.Id == id);
    }

    public void AddLecture(Lecture lecture)
    {
        _context.Lectures.Add(lecture);
        _context.SaveChanges();
    }

    public void UpdateLecture(Lecture lecture)
    {
        _context.Lectures.Update(lecture);
        _context.SaveChanges();
    }
}
