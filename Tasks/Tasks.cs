namespace Simulator
{
    public class CPUTask
    {
        public static Queue<CPUTask> HighTasksWaiting = new Queue<CPUTask>();
        public static Queue<CPUTask> LowTasksWaiting = new Queue<CPUTask>();
        public static Queue<CPUTask> InterruptedTasks = new Queue<CPUTask>();

        public string? Id { get; init; }
        public int RequestedTime { get; init; }
        public string? Priority { get; init; }

        //  the below can be modified
        public int CreationTime { get; set; }
        public string? State { get; set; }
        public int CompletionTime { get; set; }

        public int ProcessedTime { get; set; }


        public static void SetTasksToWaitingBasedOnPriority(Dictionary<string, Dictionary<int, List<CPUTask>>> AllTasks)
        {
            // Call every cycle
            int currentCycle = Scheduler.ClockCycleNow;

            if (AllTasks["high"].ContainsKey(currentCycle))
            {
                foreach (CPUTask task in AllTasks["high"][currentCycle])
                {
                    task.State = "waiting";
                    CPUTask.HighTasksWaiting.Enqueue(task);
                }
                AllTasks["high"].Remove(currentCycle);
            }

            if (AllTasks["low"].ContainsKey(currentCycle))
            {
                foreach (var task in AllTasks["low"][currentCycle])
                {
                    task.State = "waiting";
                    CPUTask.LowTasksWaiting.Enqueue(task);
                }
                AllTasks["low"].Remove(currentCycle);
            }
        }

    }
}