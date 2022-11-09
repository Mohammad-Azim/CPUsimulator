using System.ComponentModel.DataAnnotations;

namespace Simulator
{

    class Processor
    {
        public int Id;
        public string? state;

        public CPUTask? CurrentTask;

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

                idleProcessors.Enqueue(NewProcessor);
            }
        }


        public static void AddTaskToProcessor(CPUTask Task)
        {
            Processor processor = Processor.idleProcessors.Dequeue();
            Processor.busyProcessors.Add(processor.Id, processor);


            processor.CurrentTask = Task;
            processor.state = "busy";

            Task.State = "executing";
            Task.CreationTime = Scheduler.ClockCycleNow;

        }


        public static void AddTaskToBusyProcessor(Processor processor, CPUTask task)
        {
            CPUTask oldTask = processor.CurrentTask!;

            processor.CurrentTask = task;
            CPUTask.InterruptedTasks.Enqueue(oldTask);
        }

        public static bool IsTaskDone(CPUTask task)
        {
            // check if task in cpu is done

            return (task.ProcessedTime == task.RequestedTime);
        }


        public static void RemoveTaskFromProcessor(Processor processor)
        {
            CPUTask task = processor.CurrentTask!;

            task.State = "completed";
            task.CompletionTime = Scheduler.ClockCycleNow;


            processor.CurrentTask = null;
            Processor.busyProcessors.Remove(processor.Id);
            Processor.idleProcessors.Enqueue(processor);


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

            Console.WriteLine("New Task Finished");
            Console.WriteLine($"task Id: {task.Id}");
            Console.WriteLine($"Task State: {task.State}");
            Console.WriteLine($"Task CreationTime: {task.CreationTime}");
            Console.WriteLine($"Task CompletionTime: {task.CompletionTime}");
            Console.WriteLine($"Task ProcessedTime: {task.ProcessedTime}");
            Console.WriteLine($"Task RequestedTime: {task.RequestedTime}");
            Console.WriteLine("");
        }


        public static void ProcessorRunningOneClock()
        {


            if (Processor.busyProcessors.Count > 0)
            {

                foreach (int processorsKey in Processor.busyProcessors.Keys)
                {
                    Processor currentProcessor = Processor.busyProcessors[processorsKey];
                    if (Processor.IsTaskDone(currentProcessor.CurrentTask!))
                    {
                        Processor.RemoveTaskFromProcessor(currentProcessor);
                    }
                    else
                    {

                        currentProcessor.CurrentTask!.ProcessedTime += 1;

                    }

                }
            }
        }


    }
}