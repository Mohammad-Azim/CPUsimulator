namespace Simulator
{


    class Scheduler
    {
        public static int ClockCycleNow;

        public void ProcessorRunningOneClock()
        {
            if (ProcessorsManager.busyProcessors.Count > 0)
            {
                foreach (int processorsKey in ProcessorsManager.busyProcessors.Keys)
                {
                    Processor currentProcessor = ProcessorsManager.busyProcessors[processorsKey];
                    if (TasksManager.IsTaskDone(currentProcessor.CurrentTask!))
                    {
                        ProcessorsManager.RemoveTaskFromProcessor(currentProcessor);
                    }
                    else
                    {
                        currentProcessor.CurrentTask!.ProcessedTime += 1;
                    }
                }
            }
        }

        public void ProcessorsTasksManagement()
        {
            if (TasksManager.HighTasksWaiting.Count > 0 || TasksManager.LowTasksWaiting.Count > 0 || TasksManager.InterruptedTasks.Count > 0)
            {
                while ((ProcessorsManager.idleProcessors.Count > 0 && (TasksManager.HighTasksWaiting.Count > 0 || TasksManager.LowTasksWaiting.Count > 0 || TasksManager.InterruptedTasks.Count > 0)) || (ProcessorsManager.IsThereProcessorWithLowTask() != null && TasksManager.HighTasksWaiting.Count > 0))
                {

                    if (TasksManager.HighTasksWaiting.Count > 0)
                    {
                        var ProcessorWithLowTask = ProcessorsManager.IsThereProcessorWithLowTask();

                        if (ProcessorsManager.idleProcessors.Count > 0)
                        {
                            CPUTask task = TasksManager.HighTasksWaiting.Dequeue();
                            ProcessorsManager.AddTaskToProcessor(task);
                        }
                        else if (ProcessorWithLowTask != null)
                        {
                            CPUTask task = TasksManager.HighTasksWaiting.Dequeue();
                            ProcessorsManager.AddTaskToBusyProcessor(ProcessorWithLowTask, task);
                        }
                    }
                    else if (ProcessorsManager.idleProcessors.Count > 0)
                    {

                        if (TasksManager.InterruptedTasks.Count > 0)
                        {

                            CPUTask task = TasksManager.InterruptedTasks.Dequeue();
                            ProcessorsManager.AddTaskToProcessor(task);
                        }
                        else if (TasksManager.LowTasksWaiting.Count > 0)
                        {
                            CPUTask task = TasksManager.LowTasksWaiting.Dequeue();
                            ProcessorsManager.AddTaskToProcessor(task);
                        }
                    }
                }
            }
        }


        public void ClockCycleRunner(JSONAdapter JsonAdapter)
        {
            ProcessorsManager.CreateProcessors(JsonAdapter.cpuNumber);
            Results MyResults = new Results();

            while (TasksManager.HighTasksWaiting.Count > 0 || TasksManager.LowTasksWaiting.Count > 0 || TasksManager.InterruptedTasks.Count > 0 || TasksManager.IsThereTaskInDictionary(JsonAdapter) || ProcessorsManager.busyProcessors.Count > 0)
            {
                ProcessorRunningOneClock();
                TasksManager.SetTasksToWaitingBasedOnPriority(JsonAdapter.AllDicTasks!);
                this.ProcessorsTasksManagement();
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