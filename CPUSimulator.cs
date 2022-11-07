
namespace Simulator{
    class CPUSimulator{
        public int ClockCycleNow = 0;


         public CPUSimulator(List<Processor> CPUList,List<Task> HighTasks ,List<Task> LowTasks ){
            
        }


        public void StartSimulate(){
            

            while (!Scheduler.isFinish){


            



                ClockCycleNow +=1;
                if (ClockCycleNow > 30){
                    Scheduler.isFinish = true;
                }
            }

        }
    }
}
