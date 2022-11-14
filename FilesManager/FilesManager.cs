using System.Text.Json;

//using System.Collections.Generic;

namespace Simulator
{

    class FilesManager
    {

        public int cpuNumber { get; set; }
        public List<CPUTask>? Tasks { get; set; }
        // public Dictionary<string, Dictionary<int, List<CPUTask>>>? AllDicTasks { get; set; }

        public string[] SimulatorResults = { "--------CPU Simulator--------" };

        public void CreateFileWithResults(string[] lines)
        {
            string date = DateTime.Now.ToString("yyyyMMddHHmmss");
            var myUniqueFileName = $@"./FilesManager/results({date}).txt";


            File.WriteAllLinesAsync(myUniqueFileName, lines);
        }

        public void StartFilesManaging(string path)
        {  //./Tasks.json
            string jsonString = File.ReadAllText(@"" + path);
            FilesManager? filesManager =
               JsonSerializer.Deserialize<FilesManager>(jsonString);
            this.cpuNumber = filesManager!.cpuNumber;
            this.Tasks = filesManager.Tasks;
        }


    }

}