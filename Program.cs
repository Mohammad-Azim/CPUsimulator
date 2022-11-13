
namespace Simulator
{
    class Program
    {
        static void Main()
        {
            FilesManager filesManager = new FilesManager();
            filesManager.StartFilesManaging("./Tasks.json");
            TasksManager tasksManager = new TasksManager(filesManager.Tasks);
            ProcessorsManager processorsManager = new ProcessorsManager(filesManager.cpuNumber);

            Scheduler mySc = new Scheduler();
            mySc.scheduling(tasksManager, processorsManager);
            filesManager.CreateFileWithResults(mySc.SimulatorResults);

        }
    }
}