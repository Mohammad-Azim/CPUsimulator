
namespace Simulator
{
    class Program
    {
        static void Main()
        {
            FilesManager filesManager = new FilesManager("./Tasks.json");
            TasksManager tasksManager = new TasksManager(filesManager.Tasks);
            ProcessorsManager processorsManager = new ProcessorsManager(filesManager.cpuNumber);
            Scheduler mySc = new Scheduler(tasksManager, processorsManager, filesManager);

        }
    }
}