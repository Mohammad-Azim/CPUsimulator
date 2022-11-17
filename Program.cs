
namespace Simulator
{
    class Program
    {
        static void Main()
        {
            FilesManager filesManager = new FilesManager();

            var data = filesManager.StartFilesManaging("./Tasks.json");
            TasksManager tasksManager = new TasksManager(data.Item2);
            ProcessorsManager processorsManager = new ProcessorsManager(data.Item1);

            Scheduler mySc = new Scheduler();
            mySc.scheduling(tasksManager, processorsManager);
            filesManager.CreateFileWithResults("./FilesManager/results(1).txt", mySc.SimulatorData);

        }
    }
}