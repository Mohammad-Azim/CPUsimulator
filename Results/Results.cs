namespace Simulator
{
    class Results
    {
        public static string[] SimulatorResults = { "--------CPU Simulator--------" };
        public static void PrintResultToFile(string[] lines)
        {
            var myUniqueFileName = $@"./Results/{Guid.NewGuid()}.txt";

            File.WriteAllLinesAsync(myUniqueFileName, lines);
        }


        public static void PrintResultInCMD(CPUTask task)
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
    }
}

