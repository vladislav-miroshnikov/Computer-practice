namespace ExamSystemLib.Coarse
{
    public class CoarseElement
    {
        public long StudentId { get; private set; }

        public long CourseId { get; private set; }

        public CoarseElement(long studentId, long courseId)
        {
            StudentId = studentId;
            CourseId = courseId;
        }
    }
}
