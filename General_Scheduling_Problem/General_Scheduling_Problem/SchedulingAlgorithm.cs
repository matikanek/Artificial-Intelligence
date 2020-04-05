using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace General_Scheduling_Problem
{
    class SchedulingAlgorithm
    {
        public static int AssignDuration(string combination, List<Task> listT, int k, List<Factory> listF)
        {
            int day = 0;
            char[] works = combination.ToCharArray();
            Task work = new Task('X', -1);
            Factory factory = new Factory('X', -1, "Lazy Factory", false, -1);
            while (IsThereAnyWork(works))
            {
                work = PullWork(works, listT);
                works = works.Skip(1).ToArray();
                if (IsThereALazyFactory(listF))
                {
                    factory = GetFreeFactory(listF);
                    factory.job = work.job;
                    factory.duration = work.duration;
                    factory.progress = 0;
                    factory.isWorking = true;
                }
                else
                {
                    while (!CheckFactories(listF))
                    {
                        Parameters.animation += PrintingMethods.print(day, listF);
                        Sunset(listF);
                        day++;
                    }
                    factory = GetFreeFactory(listF);
                    factory.job = work.job;
                    factory.duration = work.duration;
                    factory.progress = 0;
                    factory.isWorking = true;
                }
            }
            while (IsThereAnyLazyFactory(listF))
            {
                Parameters.animation += PrintingMethods.print(day, listF);
                Sunset(listF);
                day++;
            }
            Parameters.animation += PrintingMethods.print(day, listF);
            return day;
        }

        public static bool IsThereAnyLazyFactory(List<Factory> listF)
        {
            int numberOfWorkingFactories = Parameters.K;
            for (int i = 0; i < Parameters.K; i++)
            {
                if (listF[i].progress == listF[i].duration || listF[i].isWorking == false)
                {
                    listF[i].isWorking = false;
                    numberOfWorkingFactories--;
                }
            }
            if (numberOfWorkingFactories > 0)
                return true;
            else
                return false;
        }

        public static void Sunset(List<Factory> listF)
        {
            for (int i = 0; i < Parameters.K; i++) listF[i].progress++;
        }

        public static bool CheckFactories(List<Factory> listF)
        {
            for (int i = 0; i < Parameters.K; i++)
            {
                if (listF[i].progress == listF[i].duration) listF[i].isWorking = false;
                if (listF[i].isWorking == false) return true;
            }
            return false;
        }

        public static Factory GetFreeFactory(List<Factory> listF)
        {
            int i = 0;
            while (listF[i].isWorking == true) i++;
            return listF[i];
        }

        public static bool IsThereALazyFactory(List<Factory> listF)
        {
            for (int i = 0; i < Parameters.K; i++)
            {
                if (listF[i].isWorking == false)
                    return true;
            }
            return false;
        }

        public static bool IsThereAnyWork(char[] works)
        {
            if (works.Length == 0) return false;
            else return true;
        }

        public static Task PullWork(char[] works, List<Task> listT)
        {
            char sign = works[0];
            int i = 0;
            Task work = new Task('X', -1);
            while (listT[i].job != sign) i++;
            work.job = listT[i].job;
            work.duration = listT[i].duration;
            return work;
        }
    }
}
