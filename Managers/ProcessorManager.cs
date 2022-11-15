namespace Simulator
{
    class ProcessorsManager
    {
        public List<Processor> idleProcessors = new List<Processor>();
        public List<Processor> busyProcessors = new List<Processor>();


        public void CreateProcessors(int ProcessorsNumber)
        {
            for (int x = 1; x <= ProcessorsNumber; x++)
            {
                Processor NewProcessor = new Processor();

                NewProcessor.Id = x;
                NewProcessor.state = AllEnum.state.idle;
                NewProcessor.CurrentTask = null;

                this.idleProcessors.Add(NewProcessor);
            }
        }

        public Processor? GetProcessorWithLowTask()
        {
            foreach (Processor processors in this.busyProcessors)
            {
                Processor currentProcessor = processors;

                if (currentProcessor.CurrentTask!.Priority == AllEnum.Priority.low)
                {
                    return currentProcessor;
                }
            }
            return null;
        }

        public ProcessorsManager(int cpuNumber)
        {
            this.CreateProcessors(cpuNumber);
        }
    }
}