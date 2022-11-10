namespace Simulator
{
    class ProcessorsManager
    {
        public static Queue<Processor> idleProcessors = new Queue<Processor>();
        public static Dictionary<int, Processor> busyProcessors = new Dictionary<int, Processor>();


        public static void CreateProcessors(int ProcessorsNumber)
        {
            for (int x = 1; x <= ProcessorsNumber; x++)
            {
                Processor NewProcessor = new Processor();

                NewProcessor.Id = x;
                NewProcessor.state = "idle";
                NewProcessor.CurrentTask = null;

                ProcessorsManager.idleProcessors.Enqueue(NewProcessor);
            }
        }

        public static void AddTaskToProcessor(CPUTask Task)
        {
            Processor processor = ProcessorsManager.idleProcessors.Dequeue();
            ProcessorsManager.busyProcessors.Add(processor.Id, processor);


            processor.CurrentTask = Task;
            processor.state = "busy";

            Task.State = "executing";
            Task.CreationTime = Scheduler.ClockCycleNow;
        }


        public static void AddTaskToBusyProcessor(Processor processor, CPUTask task)
        {
            CPUTask oldTask = processor.CurrentTask!;

            processor.CurrentTask = task;
            TasksManager.InterruptedTasks.Enqueue(oldTask);
        }


        public static Processor? IsThereProcessorWithLowTask()
        {
            foreach (int processorsKey in ProcessorsManager.busyProcessors.Keys)
            {
                Processor currentProcessor = ProcessorsManager.busyProcessors[processorsKey];

                if (currentProcessor.CurrentTask!.Priority == "low")
                {
                    return currentProcessor;
                }

            }
            return null;
        }


        public static void RemoveTaskFromProcessor(Processor processor)
        {
            CPUTask task = processor.CurrentTask!;

            task.State = "completed";
            task.CompletionTime = Scheduler.ClockCycleNow;


            processor.CurrentTask = null;
            ProcessorsManager.busyProcessors.Remove(processor.Id);
            ProcessorsManager.idleProcessors.Enqueue(processor);


            string[] NewTaskResults = {
                "New Task Finished",$"task Id: {task.Id}",
                $"Task State: {task.State}",
                $"Task CreationTime: {task.CreationTime}",
                $"Task CompletionTime: {task.CompletionTime}",
                $"Task ProcessedTime: {task.ProcessedTime}",
                $"Task RequestedTime: {task.RequestedTime}",
                ""
                };

            Results.SimulatorResults = Results.SimulatorResults.Concat(NewTaskResults).ToArray();

            Results.PrintResultInCMD(task);

        }


    }
}