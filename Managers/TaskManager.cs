
namespace Simulator
{
    class TasksManager
    {
        public Dictionary<string, Dictionary<int, List<CPUTask>>>? AllDicTasks { get; set; }

        public Queue<CPUTask> HighTasksWaiting = new Queue<CPUTask>();
        public Queue<CPUTask> LowTasksWaiting = new Queue<CPUTask>();
        public Queue<CPUTask> InterruptedTasks = new Queue<CPUTask>();


        public bool IsThereTaskInDictionary(Dictionary<string, Dictionary<int, List<CPUTask>>>? AllDicTasks)
        {
            // call it only after the Converter method (if you will call it in the constructor) 
            // because of the Nullable reference types
            return (AllDicTasks!["high"].Any() || AllDicTasks!["low"].Any());
        }

        public bool IsTaskDone(CPUTask task)
        {
            // check if task in cpu is done
            return (task.ProcessedTime == task.RequestedTime);
        }


        public Dictionary<string, Dictionary<int, List<CPUTask>>> ConvertToTask(List<CPUTask>? json_Tasks)
        {


            Dictionary<string, Dictionary<int, List<CPUTask>>> All_Tasks = new Dictionary<string, Dictionary<int, List<CPUTask>>>();
            Dictionary<int, List<CPUTask>> high_tasks = new Dictionary<int, List<CPUTask>>();
            Dictionary<int, List<CPUTask>> low_tasks = new Dictionary<int, List<CPUTask>>();

            All_Tasks.Add("high", high_tasks);
            All_Tasks.Add("low", low_tasks);

            for (int x = 0; x < json_Tasks?.Count; x++)
            {
                var currentTask = json_Tasks[x];
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

                if (NewTask.Priority == "high")
                {
                    if (All_Tasks["high"].ContainsKey(NewTask.CreationTime))
                    {
                        All_Tasks["high"][NewTask.CreationTime].Add(NewTask);
                    }
                    else
                    {
                        List<CPUTask> TasksSameCreationTimeList = new List<CPUTask>();
                        All_Tasks["high"].Add(NewTask.CreationTime, TasksSameCreationTimeList);
                        All_Tasks["high"][NewTask.CreationTime].Add(NewTask);
                    }

                }
                else
                {
                    if (All_Tasks["low"].ContainsKey(NewTask.CreationTime))
                    {
                        All_Tasks["low"][NewTask.CreationTime].Add(NewTask);
                    }
                    else
                    {
                        List<CPUTask> TasksSameCreationTimeList = new List<CPUTask>();
                        All_Tasks["low"].Add(NewTask.CreationTime, TasksSameCreationTimeList);
                        All_Tasks["low"][NewTask.CreationTime].Add(NewTask);
                    }
                }

            }


            this.AllDicTasks = All_Tasks;

            return All_Tasks;
        }

        public TasksManager(List<CPUTask> json_Tasks)
        {
            this.ConvertToTask(json_Tasks);

        }
    }



}