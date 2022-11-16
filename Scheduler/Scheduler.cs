namespace Simulator
{
    class Scheduler
    {
        public int ClockCycleNow = 0;
        public ProcessorsManager? processorsManager;
        public TasksManager? tasksManager;

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

        public void SetTasksToWaitingPriorityQueue()
        {
            // Call every cycle
            int currentCycle = this.ClockCycleNow;

            while (tasksManager!.AllTasksList.Count > 0)
            {
                CPUTask currentTask = tasksManager.AllTasksList[0];
                if (currentTask.CreationTime > currentCycle)
                {
                    break;
                }

                TaskPriority currentPriority = currentTask.Priority == TaskPriority.high ? TaskPriority.high : TaskPriority.low;
                tasksManager.WaitingPriorityQueue.Enqueue(tasksManager.AllTasksList[0], currentPriority);
                tasksManager.AllTasksList.Remove(currentTask);
            }


        }

        public void RemoveTaskFromProcessor(Processor processor)
        {
            CPUTask task = processor.CurrentTask!;

            task.State = TaskState.completed;
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
                $"Task Priority: {task.Priority}",
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
            processor.state = ProcessorState.busy;

            Task.State = TaskState.executing;
        }


        public void AddTaskToBusyProcessor(Processor processor, CPUTask task)
        {
            CPUTask oldTask = processor.CurrentTask!;

            processor.CurrentTask = task;
            // always low because we only interrupt low tasks
            tasksManager!.WaitingPriorityQueue.Enqueue(oldTask, TaskPriority.low);
        }


        public void ProcessorsTasksManagement()
        {

            while (tasksManager!.WaitingPriorityQueue.Count > 0 && (processorsManager!.idleProcessors.Count > 0 || (processorsManager!.GetProcessorWithLowTask() != null && tasksManager.WaitingPriorityQueue.Peek().Priority == TaskPriority.high)))
            {
                if (processorsManager!.idleProcessors.Count > 0)
                {
                    CPUTask task = tasksManager.WaitingPriorityQueue.Dequeue();
                    this.AddTaskToProcessor(task);
                }
                else
                {
                    var ProcessorWithLowTask = processorsManager.GetProcessorWithLowTask();
                    if (ProcessorWithLowTask != null)
                    {
                        CPUTask task = tasksManager.WaitingPriorityQueue.Dequeue();
                        this.AddTaskToBusyProcessor(ProcessorWithLowTask, task);
                    }
                }
            }
        }



        public void ClockCycleRunner()
        {
            while (tasksManager!.WaitingPriorityQueue.Count > 0 || tasksManager.AllTasksList.Count > 0 || processorsManager!.busyProcessors.Count > 0)
            {
                this.SetTasksToWaitingPriorityQueue();
                this.ProcessorsTasksManagement();
                this.IncreaseProcessedTimeForTaskInProcessors();
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