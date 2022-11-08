namespace Simulator
{


    class Scheduler
    {
        public static int ClockCycleNow;

        Queue<CPUTask> HighTasksWaiting = new Queue<CPUTask>();
        Queue<CPUTask> LowTasksWaiting = new Queue<CPUTask>();

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

            Console.Write(task.Id);
            Console.Write(task.State);
            Console.Write(task.CreationTime);
            Console.Write(task.CompletionTime);
            Console.Write(task.ProcessedTime);
            Console.Write(task.RequestedTime);
            Console.WriteLine("another Task down");

        }


        public void ProcessorRunningOneClock()
        {


            if (Scheduler.busyProcessors.Any())
            {

                foreach (int processorsKey in busyProcessors.Keys)
                {

                    Processor currentProcessor = busyProcessors[processorsKey];

                    currentProcessor.CurrentTask!.ProcessedTime += 1;

                    if (IsTaskDone(currentProcessor.CurrentTask))
                    {

                        RemoveTaskFromProcessor(currentProcessor);
                    }
                }
            }
        }

        public void ProcessorsManagement()
        {
            /// wrong should be while until fill all processors 

            if (this.HighTasksWaiting.Count > 0 || this.LowTasksWaiting.Count > 0)
            {
                for (int z = 0; Scheduler.idleProcessors.Count > z; z++)
                {
                    if (this.HighTasksWaiting.Count > 0)
                    {

                        CPUTask task = this.HighTasksWaiting.Dequeue();
                        AddTaskToProcessor(task);
                        idleProcessors.Dequeue();

                    }
                    else if (this.LowTasksWaiting.Count > 0)
                    {
                        CPUTask task = this.LowTasksWaiting.Dequeue();
                        AddTaskToProcessor(task);
                        idleProcessors.Dequeue();
                    }
                }
            }

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



            while (this.HighTasksWaiting.Any() || this.LowTasksWaiting.Any() || IsThereTaskInDictionary(JsonAdapter))
            {


                ProcessorRunningOneClock();
                this.SetTasksToWaitingBasedOnPriority(JsonAdapter.AllDicTasks!);



                this.ProcessorsManagement();


                Scheduler.ClockCycleNow += 1;

            }
        }


        public Scheduler()
        {

            JSONAdapter myAdapter = new JSONAdapter("./Tasks.json");
            this.ClockCycleRunner(myAdapter);

            // Console.WriteLine(myAdapter.cpuNumber);

            // Console.WriteLine(myAdapter.AllDicTasks?["high"][5][0]);
            // Console.WriteLine(myAdapter.AllDicTasks?["high"][5][0]);
        }


    }
}