public class LectureService
{
    private readonly LectureRepository _lectureRepository;

    public LectureService(LectureRepository lectureRepository)
    {
        _lectureRepository = lectureRepository;
    }

    public void CreateLecture(string title)
    {
        try
        {
            var lectureExists = _lectureRepository.GetAllLectures().Any(l => l.Title == title);
            if (lectureExists)
            {
                Console.WriteLine("Error: Paskaita jau egzistuoja.");
                return; // Nutraukiame vykdymą, jei paskaita jau egzistuoja
            }

            var lecture = new Lecture { Title = title };
            _lectureRepository.AddLecture(lecture);
            Console.WriteLine("Paskaita sukurta!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    public List<Lecture> GetAllLectures()
    {
        return _lectureRepository.GetAllLectures();
    }

    public Lecture GetLectureById(int id)
    {
        return _lectureRepository.GetLectureById(id);
    }
}
