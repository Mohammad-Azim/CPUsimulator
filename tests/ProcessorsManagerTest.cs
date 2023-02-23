using Xunit;

namespace Simulator
{





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

}