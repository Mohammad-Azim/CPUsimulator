namespace Simulator
{

    public record CPUTask
    {

        public string? Id { get; init; }
        public int CreationTime { get; init; }
        public int RequestedTime { get; init; }
        public string? Priority { get; init; }

        //  the below can be modified
        public string? State { get; set; }
        public int CompletionTime { get; set; }



    }
}