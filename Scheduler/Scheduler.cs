namespace Simulator
{
    class Scheduler
    {
        public static int ClockCycleNow;
        public void ProcessorsManagement()
        {
            if (CPUTask.HighTasksWaiting.Count > 0 || CPUTask.LowTasksWaiting.Count > 0 || CPUTask.InterruptedTasks.Count > 0)
            {
                while ((Processor.idleProcessors.Count > 0 && (CPUTask.HighTasksWaiting.Count > 0 || CPUTask.LowTasksWaiting.Count > 0 || CPUTask.InterruptedTasks.Count > 0)) || (IsThereProcessorWithLowTask() != null && CPUTask.HighTasksWaiting.Count > 0))
                {

                    if (CPUTask.HighTasksWaiting.Count > 0)
                    {
                        var ProcessorWithLowTask = IsThereProcessorWithLowTask();

                        if (Processor.idleProcessors.Count > 0)
                        {
                            CPUTask task = CPUTask.HighTasksWaiting.Dequeue();
                            Processor.AddTaskToProcessor(task);
                        }
                        else if (ProcessorWithLowTask != null)
                        {
                            CPUTask task = CPUTask.HighTasksWaiting.Dequeue();
                            Processor.AddTaskToBusyProcessor(ProcessorWithLowTask, task);
                        }
                    }
                    else if (Processor.idleProcessors.Count > 0)
                    {

                        if (CPUTask.InterruptedTasks.Count > 0)
                        {

                            CPUTask task = CPUTask.InterruptedTasks.Dequeue();
                            Processor.AddTaskToProcessor(task);
                        }
                        else if (CPUTask.LowTasksWaiting.Count > 0)
                        {
                            CPUTask task = CPUTask.LowTasksWaiting.Dequeue();
                            Processor.AddTaskToProcessor(task);
                        }
                    }

                }
            }

        }

        public Processor? IsThereProcessorWithLowTask()
        {
            foreach (int processorsKey in Processor.busyProcessors.Keys)
            {
                Processor currentProcessor = Processor.busyProcessors[processorsKey];

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
            Processor.CreateProcessors(JsonAdapter.cpuNumber);
            Results MyResults = new Results();

            while (CPUTask.HighTasksWaiting.Count > 0 || CPUTask.LowTasksWaiting.Count > 0 || CPUTask.InterruptedTasks.Count > 0 || IsThereTaskInDictionary(JsonAdapter) || Processor.busyProcessors.Count > 0)
            {

                Processor.ProcessorRunningOneClock();
                CPUTask.SetTasksToWaitingBasedOnPriority(JsonAdapter.AllDicTasks!);
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