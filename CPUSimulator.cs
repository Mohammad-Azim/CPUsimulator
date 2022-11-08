
namespace Simulator
{
    class CPUSimulator
    {
        public int ClockCycleNow = 0;


        public CPUSimulator(string path)
        {

            JSONAdapter myAdapter = new JSONAdapter(path);

            Console.WriteLine(myAdapter.cpuNumber);

            Console.WriteLine(myAdapter.Tasks[0].GetProperty("id"));

        }


        public void StartSimulate()
        {


            while (!Scheduler.isFinish)
            {

                ClockCycleNow += 1;
                if (ClockCycleNow > 30)
                {
                    Scheduler.isFinish = true;
                }
            }

        }
    }
}
