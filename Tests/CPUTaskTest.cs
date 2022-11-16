using Xunit;

namespace Simulator
{


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
}