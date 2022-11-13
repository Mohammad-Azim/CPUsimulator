using System.Text.Json;

//using System.Collections.Generic;

namespace Simulator
{

    class FilesManager
    {

        public int cpuNumber { get; set; }
        public JsonElement Tasks { get; set; }
        // public Dictionary<string, Dictionary<int, List<CPUTask>>>? AllDicTasks { get; set; }

        public string[] SimulatorResults = { "--------CPU Simulator--------" };

        public void CreateFileWithResults(string[] lines)
        {
            string datee = DateTime.Now.ToString("yyyyMMddHHmmss");
            var myUniqueFileName = $@"./FilesManager/results({datee}).txt";


            File.WriteAllLinesAsync(myUniqueFileName, lines);
        }


        public void PrintResultInCMD(CPUTask task)
        {
            Console.WriteLine("New Task Finished");
            Console.WriteLine($"task Id: {task.Id}");
            Console.WriteLine($"Task State: {task.State}");
            Console.WriteLine($"Task CreationTime: {task.CreationTime}");
            Console.WriteLine($"Task CompletionTime: {task.CompletionTime}");
            Console.WriteLine($"Task ProcessedTime: {task.ProcessedTime}");
            Console.WriteLine($"Task RequestedTime: {task.RequestedTime}");
            Console.WriteLine("");
        }
        public FilesManager(string path)
        {  //./Tasks.json
            string data = File.ReadAllText(@"" + path);

            JsonDocument doc = JsonDocument.Parse(data);
            JsonElement root = doc.RootElement;

            this.cpuNumber = int.Parse(root.GetProperty("cpuNumber").ToString());
            this.Tasks = root.GetProperty("Tasks");
        }

    }

}