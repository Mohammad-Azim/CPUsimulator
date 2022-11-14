
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


        public void SetAllTasks(List<CPUTask>? TasksList)
        {

            for (int x = 0; x < TasksList?.Count; x++)
            {

                var currentTask = TasksList[x];
                CPUTask NewTask = new CPUTask
                {
                    Id = currentTask.Id?.ToString(),
                    CreationTime = Int32.Parse(currentTask.CreationTime.ToString()),
                    RequestedTime = Int32.Parse(currentTask.RequestedTime.ToString()),
                    Priority = currentTask.Priority?.ToString(),
                    CompletionTime = 0,
                    State = "waiting",
                    ProcessedTime = 0
                };
                this.AllTasksList.Add(NewTask);

            }
            AllTasksList.Sort((x, y) => x.CreationTime.CompareTo(y.CreationTime));
        }

        public TasksManager(List<CPUTask> TasksList)
        {
            this.SetAllTasks(TasksList);

        }
    }



}