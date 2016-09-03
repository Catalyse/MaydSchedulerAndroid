using System;
using System.Collections.Generic;
using Android.App;

namespace MaydSchedulerApp
{
    public class PickWeek : DialogFragment
    {
        private int weekDistance = 8;//This is how many weeks forward it will check/generate
        public DateTime date;
        public List<Week> weekList = new List<Week>();

        /// <summary>
        /// This method will using the current date and find all weeks moving forward within a limited timeframe.
        /// </summary>
        public List<string> FindWeeks()
        {
            date = DateTime.Now.Date;

            for (int i = 0; i < weekDistance; i++)
            {
                int daysToFirstWeek = DayOfWeekValue(date.DayOfWeek);
                date = date.AddDays(daysToFirstWeek);
                Week tempWeek = CoreSystem.FindWeek(date);
                if (tempWeek == null)//Week has not been created, generate
                {
                    tempWeek = new Week(date);
                    CoreSystem.weekList.Add(date, tempWeek);
                    weekList.Add(tempWeek);
                }
                else//Week has been generated, and we will replace it if chosen
                {
                    tempWeek = new Week(date);
                    weekList.Add(tempWeek);
                }
            }

            List<string> weekListString = new List<string>();
            for(int i = 0; i < weekList.Count; i++)
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