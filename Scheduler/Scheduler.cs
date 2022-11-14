namespace Simulator
{
    class Scheduler
    {
        public int ClockCycleNow;
        ProcessorsManager? processorsManager;
        TasksManager? tasksManager;

        public string[] SimulatorResults = { "--------CPU Simulator--------" };


        public void IncreaseProcessedTimeForTaskInProcessors()
        {
            if (processorsManager!.busyProcessors.Count > 0)
            {
                foreach (int processorsKey in processorsManager.busyProcessors.Keys)
                {
                    Processor currentProcessor = processorsManager.busyProcessors[processorsKey];
                    if (tasksManager!.IsTaskDone(currentProcessor.CurrentTask!))
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

        public void SetTasksToWaitingBasedOnPriority(Dictionary<string, Dictionary<int, List<CPUTask>>> AllTasks)
        {
            // Call every cycle

            int currentCycle = this.ClockCycleNow;


            if (AllTasks["high"].ContainsKey(currentCycle))
            {

                foreach (CPUTask task in AllTasks["high"][currentCycle])
                {
                    task.State = "waiting";
                    tasksManager!.HighTasksWaiting.Enqueue(task);



                }
                AllTasks["high"].Remove(currentCycle);
            }

            if (AllTasks["low"].ContainsKey(currentCycle))
            {
                foreach (var task in AllTasks["low"][currentCycle])
                {
                    task.State = "waiting";
                    tasksManager!.LowTasksWaiting.Enqueue(task);
                }
                AllTasks["low"].Remove(currentCycle);
            }
        }

        public void RemoveTaskFromProcessor(Processor processor)
        {
            CPUTask task = processor.CurrentTask!;

            task.State = "completed";
            task.CompletionTime = this.ClockCycleNow;


            processor.CurrentTask = null;
            processorsManager!.busyProcessors.Remove(processor.Id);
            processorsManager.idleProcessors.Enqueue(processor);


            string[] NewTaskResults = {
                "New Task Finished",$"task Id: {task.Id}",
                $"Task State: {task.State}",
                $"Task CreationTime: {task.CreationTime}",
                $"Task CompletionTime: {task.CompletionTime}",
                $"Task ProcessedTime: {task.ProcessedTime}",
                $"Task RequestedTime: {task.RequestedTime}",
                ""
                };

            SimulatorResults = SimulatorResults.Concat(NewTaskResults).ToArray();
        }


        public void AddTaskToProcessor(CPUTask Task)
        {
            Processor processor = processorsManager!.idleProcessors.Dequeue();
            processorsManager.busyProcessors.Add(processor.Id, processor);


            processor.CurrentTask = Task;
            processor.state = "busy";

            Task.State = "executing";
            Task.CreationTime = this.ClockCycleNow;
        }


        public void AddTaskToBusyProcessor(Processor processor, CPUTask task)
        {
            CPUTask oldTask = processor.CurrentTask!;

            processor.CurrentTask = task;
            tasksManager!.InterruptedTasks.Enqueue(oldTask);
        }


        public void ProcessorsTasksManagement()
        {
            if (tasksManager!.HighTasksWaiting.Count > 0 || tasksManager.LowTasksWaiting.Count > 0 || tasksManager.InterruptedTasks.Count > 0)
            {
                while ((processorsManager!.idleProcessors.Count > 0 && (tasksManager.HighTasksWaiting.Count > 0 || tasksManager.LowTasksWaiting.Count > 0 || tasksManager.InterruptedTasks.Count > 0)) || (processorsManager.IsThereProcessorWithLowTask() != null && tasksManager.HighTasksWaiting.Count > 0))
                {
                    if (tasksManager.HighTasksWaiting.Count > 0)
                    {
                        var ProcessorWithLowTask = processorsManager.IsThereProcessorWithLowTask();

                        if (processorsManager.idleProcessors.Count > 0)
                        {
                            CPUTask task = tasksManager.HighTasksWaiting.Dequeue();
                            this.AddTaskToProcessor(task);
                        }
                        else if (ProcessorWithLowTask != null)
                        {
                            CPUTask task = tasksManager.HighTasksWaiting.Dequeue();
                            this.AddTaskToBusyProcessor(ProcessorWithLowTask, task);
                        }
                    }
                    else if (processorsManager.idleProcessors.Count > 0)
                    {

                        if (tasksManager.InterruptedTasks.Count > 0)
                        {

                            CPUTask task = tasksManager.InterruptedTasks.Dequeue();
                            this.AddTaskToProcessor(task);
                        }
                        else if (tasksManager.LowTasksWaiting.Count > 0)
                        {
                            CPUTask task = tasksManager.LowTasksWaiting.Dequeue();
                            this.AddTaskToProcessor(task);
                        }
                    }
                }
            }
        }


        public void ClockCycleRunner(TasksManager tasksManager, ProcessorsManager processorsManager)
        {
            // processorsManager.CreateProcessors(JsonAdapter.cpuNumber);

            while (tasksManager.HighTasksWaiting.Count > 0 || tasksManager.LowTasksWaiting.Count > 0 || tasksManager.InterruptedTasks.Count > 0 || tasksManager.IsThereTaskInDictionary(tasksManager.AllDicTasks) || processorsManager.busyProcessors.Count > 0)
            {
                this.IncreaseProcessedTimeForTaskInProcessors();
                this.SetTasksToWaitingBasedOnPriority(tasksManager.AllDicTasks!);
                this.ProcessorsTasksManagement();
                this.ClockCycleNow += 1;
            }
            Console.WriteLine("------------------- Simulating is Over -------------------");
        }


        public void scheduling(TasksManager tasksManager, ProcessorsManager processorsManager)
        {
            this.tasksManager = tasksManager;
            this.processorsManager = processorsManager;
            this.ClockCycleRunner(this.tasksManager, this.processorsManager);
        }

    }
}