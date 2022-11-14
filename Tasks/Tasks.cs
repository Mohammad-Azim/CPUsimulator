namespace Simulator
{

    public record CPUTask
    {

        public string? Id { get; init; }
        public int RequestedTime { get; init; }

        public int CreationTime { get; set; }

        public int CompletionTime { get; set; } = 0;

        public int ProcessedTime { get; set; } = 0;

        public AllEnum.Priority Priority;

        public AllEnum.TaskState State;


    }
}