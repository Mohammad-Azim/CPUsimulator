namespace Simulator
{
    class Results
    {


        public static string[] SimulatorResults = { "--------CPU Simulator--------" };
        public static void PrintResultToFile(string[] lines)
        {


            var myUniqueFileName = $@"./Results/{Guid.NewGuid()}.txt";

            File.WriteAllLinesAsync(myUniqueFileName, lines);

        }
    }
}

