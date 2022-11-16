using Xunit;

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





}