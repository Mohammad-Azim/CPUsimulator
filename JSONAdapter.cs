using System.Text.Json;
//using System.Collections.Generic;

namespace Simulator
{
    class JSONAdapter
    {
        public int cpuNumber { get; set; }
        public JsonElement Tasks { get; set; }
        public Dictionary<string, Dictionary<int, List<CPUTask>>>? AllDicTasks { get; set; }

        public JSONAdapter(string path)
        {  //./Tasks.json
            string data = File.ReadAllText(@"" + path);

            JsonDocument doc = JsonDocument.Parse(data);
            JsonElement root = doc.RootElement;

            this.cpuNumber = int.Parse(root.GetProperty("cpuNumber").ToString());
            this.Tasks = root.GetProperty("Tasks");
            Converter(Tasks);
        }





        public Dictionary<string, Dictionary<int, List<CPUTask>>> Converter(JsonElement json_Tasks)
        {

            Dictionary<string, Dictionary<int, List<CPUTask>>> All_Tasks =
                            new Dictionary<string, Dictionary<int, List<CPUTask>>>();

            Dictionary<int, List<CPUTask>> high_tasks = new Dictionary<int, List<CPUTask>>();
            Dictionary<int, List<CPUTask>> low_tasks = new Dictionary<int, List<CPUTask>>();

            All_Tasks.Add("high", high_tasks);
            All_Tasks.Add("low", low_tasks);

            for (int x = 0; x < json_Tasks.GetArrayLength(); x++)
            {
                var currentTask = json_Tasks[x];
                CPUTask NewTask = new CPUTask
                {
                    Id = currentTask.GetProperty("id").ToString(),
                    CreationTime = Int32.Parse(currentTask.GetProperty("CreationTime").ToString()),
                    RequestedTime = Int32.Parse(currentTask.GetProperty("RequestedTime").ToString()),
                    Priority = currentTask.GetProperty("Priority").ToString(),
                    CompletionTime = 0,
                    State = "waiting",
                    ProcessedTime = 0
                };

                if (NewTask.Priority == "high")
                {
                    if (All_Tasks["high"].ContainsKey(NewTask.CreationTime))
                    {
                        All_Tasks["high"][NewTask.CreationTime].Add(NewTask);
                    }
                    else
                    {
                        List<CPUTask> TasksSameCreationTimeList = new List<CPUTask>();
                        All_Tasks["high"].Add(NewTask.CreationTime, TasksSameCreationTimeList);
                        All_Tasks["high"][NewTask.CreationTime].Add(NewTask);
                    }

                }
                else
                {
                    if (All_Tasks["low"].ContainsKey(NewTask.CreationTime))
                    {
                        All_Tasks["low"][NewTask.CreationTime].Add(NewTask);
                    }
                    else
                    {
                        List<CPUTask> TasksSameCreationTimeList = new List<CPUTask>();
                        All_Tasks["low"].Add(NewTask.CreationTime, TasksSameCreationTimeList);
                        All_Tasks["low"][NewTask.CreationTime].Add(NewTask);
                    }
                }

            }



            this.AllDicTasks = All_Tasks;
            return All_Tasks;
        }

    }

}