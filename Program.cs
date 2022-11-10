
namespace Simulator
{
    class Program
    {
        static void Main()
        {
            // string data = File.ReadAllText(@"./Tasks.json");

            // using JsonDocument doc = JsonDocument.Parse(data);
            // JsonElement root = doc.RootElement;

            // Console.WriteLine(root.GetProperty("cpuNumber"));

            // Console.WriteLine(root.GetProperty("Tasks")[0].GetProperty("id"))

            // CPUSimulator mySimulator = new CPUSimulator("./Tasks.json");

            Scheduler mySc = new Scheduler();
        }
    }
}