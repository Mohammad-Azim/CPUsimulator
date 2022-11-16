using Xunit;

namespace Simulator
{


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

        void SetTasksAndProcessorsManagers()
        {
            scheduler.processorsManager = processorsManager;
            scheduler.tasksManager = tasksManager;
        }


        [Fact]
        public void TestAddTaskToBusyProcessor()
        {

            SetTasksAndProcessorsManagers();
            scheduler.AddTaskToBusyProcessor(myProcessor, TasksManagerTest.myTask1);
            Assert.Equal(TasksManagerTest.myTask1, myProcessor.CurrentTask);
        }

        [Fact]
        public void TestAddTaskToProcessor()
        {
            SetTasksAndProcessorsManagers();

            scheduler.AddTaskToProcessor(TasksManagerTest.myTask2);
            Assert.Equal(TasksManagerTest.myTask2, processorsManager.busyProcessors[0].CurrentTask);
        }


        [Fact]
        public void TestRemoveTaskFromProcessor()
        {
            SetTasksAndProcessorsManagers();
            scheduler.AddTaskToProcessor(TasksManagerTest.myTask2);// add then will test the remove

            Processor processorToTest = scheduler.processorsManager!.busyProcessors[0];

            Assert.Single(scheduler.processorsManager.busyProcessors);
            scheduler.RemoveTaskFromProcessor(scheduler.processorsManager.busyProcessors[0]);
            Assert.Empty(scheduler.processorsManager.busyProcessors); // check if it was removed

        }

        [Fact]
        public void TestSetTasksToWaitingPriorityQueue()
        {
            SetTasksAndProcessorsManagers();
            scheduler.ClockCycleNow = 0;

            Assert.Equal(0, scheduler.tasksManager!.WaitingPriorityQueue.Count);
            scheduler.SetTasksToWaitingPriorityQueue();
            Assert.Equal(1, scheduler.tasksManager.WaitingPriorityQueue.Count);
        }


        [Fact]
        public void TestIncreaseProcessedTimeForTaskInProcessors()
        {
            SetTasksAndProcessorsManagers();
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