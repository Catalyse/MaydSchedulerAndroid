using System;
using System.Collections.Generic;
using Android.App;

namespace MaydSchedulerApp
{
    public class PickWeek : DialogFragment
    {
        private int weekDistance = 10;//This is how many weeks forward it will check/generate
        private int loadMoreCount = 5;
        public int currentCount = 0;
        public DateTime date;
        public List<Week> weekList = new List<Week>();

        /// <summary>
        /// This method will using the current date and find all weeks moving forward within a limited timeframe.
        /// </summary>
        public List<string> FindWeeks()
        {
            date = DateTime.Now.Date;
            currentCount = weekDistance;
            for (int i = 0; i < weekDistance; i++)
            {
                int daysToFirstWeek = DayOfWeekValue(date.DayOfWeek);
                date = date.AddDays(daysToFirstWeek);
                Week tempWeek;
                tempWeek = new Week(date);
                weekList.Add(tempWeek);
            }

            List<string> weekListString = new List<string>();
            for(int i = 0; i < weekList.Count; i++)
            {
                weekListString.Add(weekList[i].startDate.ToShortDateString());
            }

            return weekListString;
        }

        public List<string> LoadMore()
        {
            weekList.Clear();
            date = DateTime.Now.Date;
            currentCount += loadMoreCount;
            for (int i = 0; i < currentCount; i++)
            {
                int daysToFirstWeek = DayOfWeekValue(date.DayOfWeek);
                date = date.AddDays(daysToFirstWeek);
                Week tempWeek;
                tempWeek = new Week(date);
                weekList.Add(tempWeek);
            }

            List<string> weekListString = new List<string>();
            for (int i = 0; i < weekList.Count; i++)
            {
                weekListString.Add(weekList[i].startDate.ToShortDateString());
            }

            return weekListString;
        }

        //This is generic
        private int DayOfWeekValue(DayOfWeek day)
        {
            switch (day)
            {
                case DayOfWeek.Sunday:
                    return 7;
                case DayOfWeek.Monday:
                    return 6;
                case DayOfWeek.Tuesday:
                    return 5;
                case DayOfWeek.Wednesday:
                    return 4;
                case DayOfWeek.Thursday:
                    return 3;
                case DayOfWeek.Friday:
                    return 2;
                case DayOfWeek.Saturday:
                    return 1;
                default:
                    //Console.WriteLine("Default case thrown in switch || ChooseWeek.cs || DayOfWeekValue()");
                    return -1;
            }
        }
    }
}