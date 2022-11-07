using System.Text.Json;

class App
{
    static void Main()
    {
        // string data = File.ReadAllText(@"./Tasks.json");
        
        // using JsonDocument doc = JsonDocument.Parse(data);
        // JsonElement root = doc.RootElement;

        // Console.WriteLine(root.GetProperty("cpuNumber"));

        // Console.WriteLine(root.GetProperty("Tasks")[0].GetProperty("id"))
        
        JSONAdapter myAdapter = new JSONAdapter("./Tasks.json");

        Console.WriteLine(myAdapter.cpuNumber);

        Console.WriteLine(myAdapter.Tasks);

    }
}