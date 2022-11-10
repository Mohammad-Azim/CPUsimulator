namespace Simulator
{
    class TasksManager
    {
        public static Queue<CPUTask> HighTasksWaiting = new Queue<CPUTask>();
        public static Queue<CPUTask> LowTasksWaiting = new Queue<CPUTask>();
        public static Queue<CPUTask> InterruptedTasks = new Queue<CPUTask>();


        public static bool IsThereTaskInDictionary(JSONAdapter JsonAdapter)
        {
            // call it only after the Converter method (if you will call it in the constructor) 
            // because of the Nullable reference types
            return (JsonAdapter.AllDicTasks!["high"].Any() || JsonAdapter.AllDicTasks!["low"].Any());
        }

        public static bool IsTaskDone(CPUTask task)
        {
            // check if task in cpu is done
            return (task.ProcessedTime == task.RequestedTime);
        }


        public static void SetTasksToWaitingBasedOnPriority(Dictionary<string, Dictionary<int, List<CPUTask>>> AllTasks)
        {
            // Call every cycle

            int currentCycle = Scheduler.ClockCycleNow;


            if (AllTasks["high"].ContainsKey(currentCycle))
            {

                foreach (CPUTask task in AllTasks["high"][currentCycle])
                {
                    task.State = "waiting";
                    TasksManager.HighTasksWaiting.Enqueue(task);



                }
                AllTasks["high"].Remove(currentCycle);
            }

            if (AllTasks["low"].ContainsKey(currentCycle))
            {
                foreach (var task in AllTasks["low"][currentCycle])
                {
                    task.State = "waiting";
                    TasksManager.LowTasksWaiting.Enqueue(task);
                }
                AllTasks["low"].Remove(currentCycle);
            }
        }

    }



}