namespace GSP_by_GeneticAlgorithmFramework
{
    public class Work
    {
        public string Name { set; get; }
        public double WorkTime { get; set; }

        public Work(string name, double workTime)
        {
            Name = name;
            WorkTime = workTime;
        }

        public double AssignVarianceForFactories()
        {
            double Max = SearchForMax();
            double min = SearchForMin();
            return Max - min;
        }

        public double SearchForMax()
        {
            double max = 0.0;
            for (int i = 0; i < Program.ListOfFactories.Count; i++)
            {
                if (max < Program.ListOfFactories[i].localTotalTime)
                    max = Program.ListOfFactories[i].localTotalTime;
            }
            return max;
        }

        public double SearchForMin()
        {
            double min = 100.0;
            for (int i = 0; i < Program.ListOfFactories.Count; i++)
            {
                if (min > Program.ListOfFactories[i].localTotalTime)
                    min = Program.ListOfFactories[i].localTotalTime;
            }
            return min;
        }

        public override string ToString()
        {
            return "Task: " + Name + ", Duration: " + WorkTime;
        }
    }
}