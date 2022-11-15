namespace Simulator
{

    record Processor
    {
        public int Id;
        public ProcessorState state;

        public CPUTask? CurrentTask;
    }
}