using Xunit;

namespace Simulator
{

    public class FilesManagerTests
    {

        [Fact]
        public void testStartReading()
        {
            FilesManager myFileManager = new FilesManager();
            var data = myFileManager.StartFilesManaging("../../../tests/TestTasks.json");
            Assert.Equal(2, data.Item1);
            Assert.Equal(4, data.Item2.Count);
        }


        [Fact]
        public void testCreateFileWithResults()
        {
            FilesManager myFileManager = new FilesManager();
            string[] stringArray = { "this is test", "from test" };
            myFileManager.CreateFileWithResults(stringArray);
            string text = File.ReadAllText(@"/home/mohammad/CSharp/CPUsimulator/FilesManager/results(1).txt");
            Assert.Equal("this is test\nfrom test\n", text);
        }
    }

}