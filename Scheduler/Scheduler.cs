using System.Linq;

namespace Simulator
{


    class Scheduler
    {
        public static int ClockCycleNow;

        Queue<CPUTask> HighTasksWaiting = new Queue<CPUTask>();
        Queue<CPUTask> LowTasksWaiting = new Queue<CPUTask>();

        Queue<CPUTask> InterruptedTasks = new Queue<CPUTask>();

        static Queue<Processor> idleProcessors = new Queue<Processor>();
        static Dictionary<int, Processor> busyProcessors = new Dictionary<int, Processor>();


        public void CreateProcessors(int ProcessorsNumber)
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

        public void SetTasksToWaitingBasedOnPriority(Dictionary<string, Dictionary<int, List<CPUTask>>> AllTasks)
        {
            // Call every cycle

            int currentCycle = Scheduler.ClockCycleNow;


            if (AllTasks["high"].ContainsKey(currentCycle))
            {

                foreach (CPUTask task in AllTasks["high"][currentCycle])
                {
                    task.State = "waiting";
                    this.HighTasksWaiting.Enqueue(task);



                }
                AllTasks["high"].Remove(currentCycle);
            }

            if (AllTasks["low"].ContainsKey(currentCycle))
            {
                foreach (var task in AllTasks["low"][currentCycle])
                {
                    task.State = "waiting";
                    this.LowTasksWaiting.Enqueue(task);
                }
                AllTasks["low"].Remove(currentCycle);
            }
        }

        public void AddTaskToProcessor(CPUTask Task)
        {
            Processor processor = idleProcessors.Dequeue();
            busyProcessors.Add(processor.Id, processor);


            processor.CurrentTask = Task;
            processor.state = "busy";

            Task.State = "executing";
            Task.CreationTime = Scheduler.ClockCycleNow;

        }

        public void AddTaskToBusyProcessor(Processor processor, CPUTask task)
        {


            CPUTask oldTask = processor.CurrentTask!;

            processor.CurrentTask = task;
            InterruptedTasks.Enqueue(oldTask);
        }

        public bool IsTaskDone(CPUTask task)
        {
            // check if task in cpu is done

            return (task.ProcessedTime == task.RequestedTime);
        }

        public void RemoveTaskFromProcessor(Processor processor)
        {
            CPUTask task = processor.CurrentTask!;

            task.State = "completed";
            task.CompletionTime = Scheduler.ClockCycleNow;


            processor.CurrentTask = null;
            busyProcessors.Remove(processor.Id);
            idleProcessors.Enqueue(processor);


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




        public void ProcessorRunningOneClock()
        {


            if (Scheduler.busyProcessors.Count > 0)
            {

                foreach (int processorsKey in busyProcessors.Keys)
                {
                    Processor currentProcessor = busyProcessors[processorsKey];
                    if (IsTaskDone(currentProcessor.CurrentTask!))
                    {
                        RemoveTaskFromProcessor(currentProcessor);
                    }
                    else
                    {

                        currentProcessor.CurrentTask!.ProcessedTime += 1;

                    }

                }
            }
        }

        public void ProcessorsManagement()
        {
            if (this.HighTasksWaiting.Count > 0 || this.LowTasksWaiting.Count > 0 || this.InterruptedTasks.Count > 0)
            {
                while ((idleProcessors.Count > 0 && (this.HighTasksWaiting.Count > 0 || this.LowTasksWaiting.Count > 0 || this.InterruptedTasks.Count > 0)) || (IsThereProcessorWithLowTask() != null && this.HighTasksWaiting.Count > 0))
                {

                    if (this.HighTasksWaiting.Count > 0)
                    {
                        var ProcessorWithLowTask = IsThereProcessorWithLowTask();

                        if (idleProcessors.Count > 0)
                        {
                            CPUTask task = this.HighTasksWaiting.Dequeue();
                            AddTaskToProcessor(task);
                        }
                        else if (ProcessorWithLowTask != null)
                        {
                            CPUTask task = this.HighTasksWaiting.Dequeue();
                            AddTaskToBusyProcessor(ProcessorWithLowTask, task);
                        }
                    }
                    else if (idleProcessors.Count > 0)
                    {

                        if (this.InterruptedTasks.Count > 0)
                        {

                            CPUTask task = this.InterruptedTasks.Dequeue();
                            AddTaskToProcessor(task);
                        }
                        else if (this.LowTasksWaiting.Count > 0)
                        {
                            CPUTask task = this.LowTasksWaiting.Dequeue();
                            AddTaskToProcessor(task);
                        }
                    }


                }
            }

        }

        public Processor? IsThereProcessorWithLowTask()
        {
            foreach (int processorsKey in busyProcessors.Keys)
            {
                Processor currentProcessor = busyProcessors[processorsKey];

                if (currentProcessor.CurrentTask!.Priority == "low")
                {
                    return currentProcessor;
                }

            }
            return null;
        }


        public bool IsThereTaskInDictionary(JSONAdapter JsonAdapter)
        {
            // call it only after the Converter method (if you will call it in the constructor) 
            // because of the Nullable reference types
            return (JsonAdapter.AllDicTasks!["high"].Any() || JsonAdapter.AllDicTasks!["low"].Any());

        }

        public void ClockCycleRunner(JSONAdapter JsonAdapter)
        {
            this.CreateProcessors(JsonAdapter.cpuNumber);
            Results MyResults = new Results();

            while (this.HighTasksWaiting.Count > 0 || this.LowTasksWaiting.Count > 0 || this.InterruptedTasks.Count > 0 || IsThereTaskInDictionary(JsonAdapter) || Scheduler.busyProcessors.Count > 0)
            {


                ProcessorRunningOneClock();
                this.SetTasksToWaitingBasedOnPriority(JsonAdapter.AllDicTasks!);



                this.ProcessorsManagement();



                Scheduler.ClockCycleNow += 1;



            }
            Console.WriteLine("------------------- TheEnd -------------------");
            Results.PrintResultToFile(Results.SimulatorResults);
        }


        public Scheduler()
        {
            JSONAdapter myAdapter = new JSONAdapter("./Tasks.json");
            this.ClockCycleRunner(myAdapter);

        }


    }
}