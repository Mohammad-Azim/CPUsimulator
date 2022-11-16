using System;
using Xunit;
using System.Text.Json;

namespace Simulator
{
    public class FilesManagerTests
    {

        [Fact]
        public void testStartReading()
        {
            FilesManager myFileManager = new FilesManager();
            myFileManager.StartFilesManaging("../../../Tests/TestTasks.json");
            Assert.Equal(2, myFileManager.cpuNumber);
            Assert.Equal(4, myFileManager.Tasks?.Count);
        }

    }

    public class CPUTaskTest
    {
        [Fact]
        public void testCPUTaskRecord()
        {
            CPUTask myTask = new CPUTask()
            {
                Id = "1",
                RequestedTime = 4,
                Priority = TaskPriority.high,
                CreationTime = 5,
                State = TaskState.waiting,
                CompletionTime = 0,
                ProcessedTime = 0

            };
            Assert.Equal("1", myTask.Id);
            Assert.Equal(4, myTask.RequestedTime);
            Assert.Equal(TaskPriority.high, myTask.Priority);
            Assert.Equal(5, myTask.CreationTime);
            Assert.Equal(TaskState.waiting, myTask.State);
            Assert.Equal(0, myTask.CompletionTime);
            Assert.Equal(0, myTask.ProcessedTime);
        }

    }


    public class ProcessorTest
    {
        [Fact]
        public void testProcessorRecord()
        {
            CPUTask myTask = new CPUTask()
            {
                Id = "1",
                RequestedTime = 4,
                Priority = TaskPriority.high,
                CreationTime = 5,
                State = TaskState.waiting,
                CompletionTime = 0,
                ProcessedTime = 0

            };
            Processor myProcessor = new Processor()
            {
                Id = 1,
                state = ProcessorState.busy,
                CurrentTask = myTask

            };
            Assert.Equal(1, myProcessor.Id);
            Assert.Equal(ProcessorState.busy, myProcessor.state);
            Assert.Equal(4, myProcessor.CurrentTask.RequestedTime);
            Assert.Equal(TaskPriority.high, myProcessor.CurrentTask.Priority);
        }





    }


    public class TasksManagerTest
    {
        public static CPUTask myTask1 = new CPUTask()
        {
            Id = "1",
            RequestedTime = 4,
            Priority = TaskPriority.high,
            CreationTime = 6,
            State = TaskState.waiting,
            CompletionTime = 0,
            ProcessedTime = 0

        };

        public static CPUTask myTask2 = new CPUTask()
        {
            Id = "2",
            RequestedTime = 8,
            Priority = TaskPriority.low,
            CreationTime = 0,
            State = TaskState.waiting,
            CompletionTime = 0,
            ProcessedTime = 0

        };
        public static CPUTask myTask3 = new CPUTask()
        {
            Id = "3",
            RequestedTime = 4,
            Priority = TaskPriority.high,
            CreationTime = 5,
            State = TaskState.waiting,
            CompletionTime = 0,
            ProcessedTime = 0

        };
        public static List<CPUTask> AllTasksList = new List<CPUTask>{
            myTask3,
            myTask1,
            myTask2
        };

        TasksManager myTaskManager = new TasksManager(AllTasksList);

        // Test Start Here
        [Fact]
        public void TestIsTaskDone()
        {
            Assert.False(myTaskManager.IsTaskDone(myTask1));
        }

        [Fact]
        public void TestSetAllTasks()
        {
            myTaskManager.SetAllTasks(AllTasksList);
            Assert.Equal(myTaskManager.AllTasksList[0], myTask2);
        }

    }


    public class ProcessorsManagerTest
    {
        public static CPUTask TaskToCPU1 = new CPUTask()
        {
            Id = "3",
            RequestedTime = 4,
            Priority = TaskPriority.low,
            CreationTime = 5,
            State = TaskState.waiting,
            CompletionTime = 0,
            ProcessedTime = 0

        };

        Processor cpu1 = new Processor
        {
            Id = 1,
            state = ProcessorState.idle,
            CurrentTask = TaskToCPU1
        };

        ProcessorsManager processorsManager = new ProcessorsManager(4);

        [Fact]
        public void TestCreateProcessors()
        {
            Assert.Equal(4, processorsManager.idleProcessors.Count);
        }

        [Fact]
        public void TestGetProcessorWithLowTask()
        {
            TasksManagerTest.myTask1.Priority = TaskPriority.low;
            processorsManager.busyProcessors.Add(cpu1);
            var myProcessor = processorsManager.GetProcessorWithLowTask();
            Assert.Equal(TaskPriority.low, myProcessor!.CurrentTask!.Priority);

        }

    }


    public class SchedulerTest
    {

        ProcessorsManager processorsManager = new ProcessorsManager(2);
        TasksManager tasksManager = new TasksManager(TasksManagerTest.AllTasksList);

        Scheduler scheduler = new Scheduler();

        static Processor myProcessor = new Processor()
        {
            Id = 1,
            state = ProcessorState.busy,
            CurrentTask = null

        };


        [Fact]
        public void TestAddTaskToBusyProcessor()
        {
            scheduler.processorsManager = processorsManager;
            scheduler.tasksManager = tasksManager;

            scheduler.AddTaskToBusyProcessor(myProcessor, TasksManagerTest.myTask1);
            Assert.Equal(TasksManagerTest.myTask1, myProcessor.CurrentTask);
        }

        [Fact]
        public void TestAddTaskToProcessor()
        {
            scheduler.processorsManager = processorsManager;
            scheduler.tasksManager = tasksManager;

            scheduler.AddTaskToProcessor(TasksManagerTest.myTask2);
            Assert.Equal(TasksManagerTest.myTask2, processorsManager.busyProcessors[0].CurrentTask);
        }


        [Fact]
        public void TestRemoveTaskFromProcessor()
        {
            scheduler.processorsManager = processorsManager;
            scheduler.tasksManager = tasksManager;
            scheduler.AddTaskToProcessor(TasksManagerTest.myTask2);// add then will test the remove

            Processor processorToTest = scheduler.processorsManager.busyProcessors[0];

            Assert.Equal(1, scheduler.processorsManager.busyProcessors.Count);
            scheduler.RemoveTaskFromProcessor(scheduler.processorsManager.busyProcessors[0]);
            Assert.Empty(scheduler.processorsManager.busyProcessors); // check if it was removed

        }

        [Fact]
        public void TestSetTasksToWaitingPriorityQueue()
        {
            scheduler.processorsManager = processorsManager;
            scheduler.tasksManager = tasksManager;
            scheduler.ClockCycleNow = 0;

            Assert.Equal(0, scheduler.tasksManager.WaitingPriorityQueue.Count);
            scheduler.SetTasksToWaitingPriorityQueue();
            Assert.Equal(1, scheduler.tasksManager.WaitingPriorityQueue.Count);
        }


        [Fact]
        public void TestIncreaseProcessedTimeForTaskInProcessors()
        {
            scheduler.processorsManager = processorsManager;
            scheduler.tasksManager = tasksManager;
            scheduler.AddTaskToProcessor(TasksManagerTest.myTask1);

            Assert.Equal(0, TasksManagerTest.myTask1.ProcessedTime);
            scheduler.IncreaseProcessedTimeForTaskInProcessors();
            Assert.Equal(1, TasksManagerTest.myTask1.ProcessedTime);
        }

        [Fact]
        public void TestProcessorsTasksManagement()
        {

        }




    }


}