namespace Simulator
{
    class Scheduler
    {
        public int ClockCycleNow;
        ProcessorsManager? processorsManager;
        TasksManager? tasksManager;

        public string[] SimulatorData = { "--------CPU Simulator--------" };


        public void IncreaseProcessedTimeForTaskInProcessors()
        {
            if (processorsManager!.busyProcessors.Count > 0)
            {
                List<Processor> busy = new List<Processor>();
                busy.AddRange(processorsManager.busyProcessors);

                foreach (Processor processors in busy)
                {
                    if (tasksManager!.IsTaskDone(processors.CurrentTask!))
                    {
                        RemoveTaskFromProcessor(processors);
                    }
                    else
                    {
                        processors.CurrentTask!.ProcessedTime += 1;
                    }
                }
            }
        }

        //SetTasksToWaitingPriorityQueue
        public void SetTasksToWaitingPriorityQueue()
        {
            // Call every cycle

            int currentCycle = this.ClockCycleNow;

            while (tasksManager!.AllTasksList.Count > 0)
            {
                CPUTask currentTask = tasksManager.AllTasksList[0];
                int lowOrHigh = 1;

                if (currentTask.CreationTime > currentCycle)
                {
                    break;
                }

                if (currentTask.Priority == AllEnum.Priority.high)
                {
                    lowOrHigh = 0;
                }
                tasksManager.WaitingPriorityQueue.Enqueue(tasksManager.AllTasksList[0], lowOrHigh);
                tasksManager.AllTasksList.Remove(currentTask);
            }


        }

        public void RemoveTaskFromProcessor(Processor processor)
        {
            CPUTask task = processor.CurrentTask!;

            task.State = AllEnum.TaskState.completed;
            task.CompletionTime = this.ClockCycleNow;


            processor.CurrentTask = null;
            processorsManager!.busyProcessors.Remove(processor);
            processorsManager.idleProcessors.Add(processor);


            string[] NewTaskResults = {
                "New Task Finished",$"task Id: {task.Id}",
                $"Task State: {task.State}",
                $"Task CreationTime: {task.CreationTime}",
                $"Task CompletionTime: {task.CompletionTime}",
                $"Task ProcessedTime: {task.ProcessedTime}",
                $"Task RequestedTime: {task.RequestedTime}",
                ""
                };

            SimulatorData = SimulatorData.Concat(NewTaskResults).ToArray();
        }


        public void AddTaskToProcessor(CPUTask Task)
        {
            Processor processor = this.processorsManager!.idleProcessors[0];
            processorsManager!.busyProcessors.Add(processor);
            processorsManager.idleProcessors.Remove(processor);

            processor.CurrentTask = Task;
            processor.state = AllEnum.state.busy;

            Task.State = AllEnum.TaskState.executing;
        }


        public void AddTaskToBusyProcessor(Processor processor, CPUTask task)
        {
            CPUTask oldTask = processor.CurrentTask!;

            processor.CurrentTask = task;
            // always one because we only interrupt low tasks
            tasksManager!.WaitingPriorityQueue.Enqueue(oldTask, 1);
        }


        public void ProcessorsTasksManagement()
        {

            if (tasksManager!.WaitingPriorityQueue.Count > 0)
            {

                while ((processorsManager!.idleProcessors.Count > 0 && tasksManager.WaitingPriorityQueue.Count > 0))
                {
                    if (processorsManager.idleProcessors.Count > 0 && tasksManager.WaitingPriorityQueue.Count > 0)
                    {

                        CPUTask task = tasksManager.WaitingPriorityQueue.Dequeue();
                        this.AddTaskToProcessor(task);
                    }
                    else if (processorsManager.idleProcessors.Count == 0 && tasksManager.WaitingPriorityQueue.Count > 0 && tasksManager.WaitingPriorityQueue.Peek().Priority == AllEnum.Priority.high)
                    {
                        var ProcessorWithLowTask = processorsManager.IsThereProcessorWithLowTask();
                        if (ProcessorWithLowTask != null)
                        {
                            CPUTask task = tasksManager.WaitingPriorityQueue.Dequeue();
                            this.AddTaskToBusyProcessor(ProcessorWithLowTask, task);
                        }
                    }

                }
            }
        }


        public void ClockCycleRunner()
        {
            // processorsManager.CreateProcessors(JsonAdapter.cpuNumber);

            while (tasksManager!.WaitingPriorityQueue.Count > 0 || tasksManager.AllTasksList.Count > 0 || processorsManager!.busyProcessors.Count > 0)
            {
                this.IncreaseProcessedTimeForTaskInProcessors();
                this.SetTasksToWaitingPriorityQueue();
                this.ProcessorsTasksManagement();
                this.ClockCycleNow += 1;
            }
            Console.WriteLine("------------------- Simulating is Over -------------------");
        }


        public void scheduling(TasksManager tasksManager, ProcessorsManager processorsManager)
        {
            this.tasksManager = tasksManager;
            this.processorsManager = processorsManager;
            this.ClockCycleRunner();
        }

    }
}