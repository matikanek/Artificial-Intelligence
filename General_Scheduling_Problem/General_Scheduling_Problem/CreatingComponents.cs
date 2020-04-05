using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General_Scheduling_Problem
{
    class CreatingComponents
    {
        public static List<Factory> CreateFactories()
        {
            List<Factory> list = new List<Factory>();
            for (int i = 0; i < Parameters.K; i++)
            {
                string name = Parameters.names[i];
                Factory tmp = new Factory('X', -1, name, false, -1);
                list.Add(tmp);
            }
            return list;
        }

        public static List<Task> CreateTasks()
        {
            List<Task> list = new List<Task>();
            for (int i = 0; i < Parameters.N; i++)
            {
                char alf = Parameters.alphabet[i];
                Task tmp = new Task(alf, Parameters.Duration[i]);
                list.Add(tmp);
            }
            return list;
        }
    }
}
