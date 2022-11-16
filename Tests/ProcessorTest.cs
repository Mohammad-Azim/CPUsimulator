using Xunit;

namespace Simulator
{
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
}