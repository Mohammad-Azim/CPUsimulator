namespace Simulator
{
    class ProcessorsManager
    {
        public Queue<Processor> idleProcessors = new Queue<Processor>();
        public Dictionary<int, Processor> busyProcessors = new Dictionary<int, Processor>();


        public void CreateProcessors(int ProcessorsNumber)
        {
            for (int x = 1; x <= ProcessorsNumber; x++)
            {
                Processor NewProcessor = new Processor();

                NewProcessor.Id = x;
                NewProcessor.state = "idle";
                NewProcessor.CurrentTask = null;

                this.idleProcessors.Enqueue(NewProcessor);
            }
        }

        public Processor? IsThereProcessorWithLowTask()
        {
            foreach (int processorsKey in this.busyProcessors.Keys)
            {
                Processor currentProcessor = this.busyProcessors[processorsKey];

                if (currentProcessor.CurrentTask!.Priority == "low")
                {
                    return currentProcessor;
                }

            }
            return null;
        }
    }
}