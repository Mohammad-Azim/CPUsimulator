using Newtonsoft.Json;

//using System.Collections.Generic;

namespace Simulator
{

    class MyDeserializer
    {
        public int cpuNumber { get; set; }
        public List<CPUTask>? Tasks { get; set; }
    }

    class FilesManager
    {
        // public Dictionary<string, Dictionary<int, List<CPUTask>>>? AllDicTasks { get; set; }

        public string[] SimulatorResults = { "--------CPU Simulator--------" };

        public void CreateFileWithResults(string[] lines)
        {
            var myUniqueFileName = @"/home/mohammad/CSharp/CPUsimulator/FilesManager/results(1).txt";
            File.WriteAllLinesAsync(myUniqueFileName, lines);
        }

        public (int, List<CPUTask>) StartFilesManaging(string path)
        {  //./Tasks.json
            string jsonString = File.ReadAllText(@"" + path);
            MyDeserializer? filesManager =
               JsonConvert.DeserializeObject<MyDeserializer>(jsonString);
            return (filesManager!.cpuNumber, filesManager.Tasks!);
        }


    }

}