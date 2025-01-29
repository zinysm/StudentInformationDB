public class LectureService
{
    private readonly LectureRepository _lectureRepository;

    public LectureService(LectureRepository lectureRepository)
    {
        _lectureRepository = lectureRepository;
    }

    public void CreateLecture(string title)
    {
        var lecture = new Lecture { Title = title };
        _lectureRepository.AddLecture(lecture);
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
