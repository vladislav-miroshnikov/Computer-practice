namespace ExamSystemLib.LockFree
{
#pragma warning disable CS0659 
    public class LockFreeNode
#pragma warning restore CS0659 
    {
        public long StudentId { get; private set; }

        public long CourseId { get; private set; }

        private LockFreeNode next;

        public ref LockFreeNode GetNextRef()
        {
            return ref next;
        }

        public LockFreeNode GetNextValue()
        {
            return next;
        }

        public void SetNextValue(LockFreeNode node)
        {
            next = node;
        }

        public LockFreeNode(long studentId, long courseId)
        {
            StudentId = studentId;
            CourseId = courseId;
        }

        public bool IsRemovedLogically { get; set; }

        public override bool Equals(object obj)
        {
            if (obj as LockFreeNode == null)
            {
                return false;
            }
            LockFreeNode node = (LockFreeNode)obj;
            return (node.StudentId == this.StudentId && node.CourseId == this.CourseId);
        }
    }
}
