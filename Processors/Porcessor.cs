namespace Simulator
{

    record Processor
    {
        public int Id;
        public AllEnum.state state;

        public CPUTask? CurrentTask;
    }
}