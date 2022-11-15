namespace Simulator
{
    public class AllEnum
    {
        public enum state
        {
            idle,
            busy
        }

        public enum Priority
        {
            high,
            low
        }
        public enum TaskState
        {
            waiting,
            executing,
            completed,

        }
    }
}