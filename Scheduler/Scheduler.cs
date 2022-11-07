namespace Simulator{


    class Scheduler{

        public static bool isFinish = false;
        public int CPUNumber;

        public static List<Processor>? CPUList;
        public List<Task>? HighTasks;

        public List<Task>? LowTasks;

        public List<Task>? InterruptedTasks;

        

        public void AddToCPU(Task task, Processor cpuID){
            
            

        }

        public int CheckEmptyCPU(){
            foreach(Processor CPU in CPUList){
                if (CPU.state == "idle"){
                    return CPU.Id;
                }
            }
            return 0;
        }

        public void ManageAllCPU(){

        }
    }
}