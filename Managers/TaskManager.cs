
namespace Simulator
{
    class TasksManager
    {

        public List<CPUTask> AllTasksList = new List<CPUTask>();
        public PriorityQueue<CPUTask, int> WaitingPriorityQueue = new PriorityQueue<CPUTask, int>();


        public bool IsTaskDone(CPUTask task)
        {
            // check if task in cpu is done
            return (task.ProcessedTime == task.RequestedTime);
        }


        public void SetAllTasks(List<CPUTask> TasksList)
        {
            AllTasksList = TasksList!;
            AllTasksList?.Sort((x, y) => x.CreationTime.CompareTo(y.CreationTime));
        }

        public TasksManager(List<CPUTask> TasksList)
        {
            this.SetAllTasks(TasksList);

        }
    }



}