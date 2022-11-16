using Xunit;

namespace Simulator
{



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
}