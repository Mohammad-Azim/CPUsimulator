using System.Text.Json;
class JSONAdapter{
    public  int cpuNumber {get; set;}
    public  JsonElement Tasks { get; set; }




    public JSONAdapter(string path){  //./Tasks.json
        string data = File.ReadAllText(@""+path);
        
        JsonDocument doc =  JsonDocument.Parse(data);
        JsonElement root = doc.RootElement;

        this.cpuNumber = int.Parse(root.GetProperty("cpuNumber").ToString());
        this.Tasks = root.GetProperty("Tasks");
    }

}

