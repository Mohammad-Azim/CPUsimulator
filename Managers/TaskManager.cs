namespace Simulator
{
    class TasksManager
    {
        public Queue<CPUTask> HighTasksWaiting = new Queue<CPUTask>();
        public Queue<CPUTask> LowTasksWaiting = new Queue<CPUTask>();
        public Queue<CPUTask> InterruptedTasks = new Queue<CPUTask>();


        public bool IsThereTaskInDictionary(FilesManager JsonAdapter)
        {
            // call it only after the Converter method (if you will call it in the constructor) 
            // because of the Nullable reference types
            return (JsonAdapter.AllDicTasks!["high"].Any() || JsonAdapter.AllDicTasks!["low"].Any());
        }

        public bool IsTaskDone(CPUTask task)
        {
            // check if task in cpu is done
            return (task.ProcessedTime == task.RequestedTime);
        }
    }



}