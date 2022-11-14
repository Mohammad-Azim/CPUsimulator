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
                Priority = "high",
                CreationTime = 5,
                State = "waiting",
                CompletionTime = 0,
                ProcessedTime = 0

            };
            Assert.Equal("1", myTask.Id);
            Assert.Equal(4, myTask.RequestedTime);
            Assert.Equal("high", myTask.Priority);
            Assert.Equal(5, myTask.CreationTime);
            Assert.Equal("waiting", myTask.State);
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
                Priority = "high",
                CreationTime = 5,
                State = "waiting",
                CompletionTime = 0,
                ProcessedTime = 0

            };
            Processor myProcessor = new Processor()
            {
                Id = 1,
                state = "idle",
                CurrentTask = myTask

            };
            Assert.Equal(1, myProcessor.Id);
            Assert.Equal("idle", myProcessor.state);
            Assert.Equal(4, myProcessor.CurrentTask.RequestedTime);
            Assert.Equal("high", myProcessor.CurrentTask.Priority);
        }



        public class TasksManagerTest
        {
            static CPUTask myTask1 = new CPUTask()
            {
                Id = "1",
                RequestedTime = 4,
                Priority = "high",
                CreationTime = 5,
                State = "waiting",
                CompletionTime = 0,
                ProcessedTime = 0

            };

            static CPUTask myTask2 = new CPUTask()
            {
                Id = "2",
                RequestedTime = 8,
                Priority = "low",
                CreationTime = 0,
                State = "waiting",
                CompletionTime = 0,
                ProcessedTime = 0

            };
            static CPUTask myTask3 = new CPUTask()
            {
                Id = "3",
                RequestedTime = 4,
                Priority = "high",
                CreationTime = 5,
                State = "waiting",
                CompletionTime = 0,
                ProcessedTime = 0

            };
            static public List<CPUTask> AllTasksList = new List<CPUTask>{
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

            // [Fact]
            // public void TestSetAllTasks()
            // {
            //     myTaskManager.SetAllTasks(AllTasksList)
            //     Assert.Equal(myTaskManager.AllTasksList[0], );
            // }


        }


    }
}