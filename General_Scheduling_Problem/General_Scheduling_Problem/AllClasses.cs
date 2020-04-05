using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General_Scheduling_Problem
{
    public class Task
    {
        public char job;
        public int duration;
        public Task(char _j, int _dr)
        {
            job = _j;
            duration = _dr;
        }
        public override string ToString()
        {
            return "Task: { Job: " + job + ", Duration: " + duration + " }";
        }
    }

    public class Way
    {
        public string combination;
        public int worktime;
        public int index;
        public string history;
        public Way(string _c, int _w, int _i, string _h)
        {
            combination = _c;
            worktime = _w;
            index = _i;
            history = _h;
        }
        public override string ToString()
        {
            return "Way: { Combination: " + combination + ", Worktime: " + worktime + ", Index: " + index + " }";
        }
    }

    public class Factory : Task
    {
        public string name;
        public bool isWorking;
        public int progress;
        public Factory(char _j, int _dr, string _n, bool _i, int _p) : base(_j, _dr)
        {
            name = _n;
            isWorking = _i;
            progress = _p;
        }
        public override string ToString()
        {
            if (isWorking == false)
                return "Factory: {\n" +
                    "\tName: " + name + "\n" +
                    "\tState: IS NOT WORKING\n" +
                    "}";
            else
                return "Factory: {\n" +
                    "\tName: " + name + "\n" +
                    "\tState: IS WORKING {\n" +
                    "\t\tJob: " + job + "\n" +
                    "\t\tProgress: " + progress + " / " + duration + "\n" +
                    "\t}\n}";
        }
    }
}
