namespace Simulator
{

    public record CPUTask
    {

        public string? Id { get; init; }
        public int RequestedTime { get; init; }
        public string? Priority { get; init; }

        //  the below can be modified
        public int CreationTime { get; set; }
        public string? State { get; set; } = "waiting";
        public int CompletionTime { get; set; } = 0;

        public int ProcessedTime { get; set; } = 0;

    }
}