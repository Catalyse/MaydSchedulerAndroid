using System;
using System.Collections;
using System.Collections.Generic;

namespace MaydSchedulerApp
{
    public class DailySchedule
    {
        public DateTime date;
        public bool activeDay;
        public int openTime, closeTime;
        public DayOfWeek dayOfWeek;
        public SerializableDictionary<int, int> openNeededShifts = new SerializableDictionary<int,int>();
        public SerializableDictionary<int, int> closeNeededShifts = new SerializableDictionary<int,int>();
        public SerializableDictionary<int, int> openScheduledShifts = new SerializableDictionary<int, int>();
        public SerializableDictionary<int, int> closeScheduledShifts = new SerializableDictionary<int, int>();
        //This was changed from being a list to a single value since you cannot have more than one shift in a day.

        public DailySchedule() { }

        public DailySchedule(DateTime day)
        {
            date = day;
            dayOfWeek = day.DayOfWeek;
        }

        public DailySchedule(DailySchedule day, DateTime _date)
        {
            date = _date;
            activeDay = day.activeDay;
            openTime = day.openTime;
            closeTime = day.closeTime;
            dayOfWeek = day.dayOfWeek;
            openNeededShifts = day.openNeededShifts;
            closeNeededShifts = day.closeNeededShifts;
            openScheduledShifts = day.openScheduledShifts;
            closeScheduledShifts = day.closeScheduledShifts;
        }

        public void SetBaseInfo(bool active)
        {
            activeDay = active;
            openTime = 0;
            closeTime = 0;
        }

        public void SetBaseInfo(bool active, int open, int close)
        {
            activeDay = active;
            openTime = open;
            closeTime = close;
        }

        public void SetOpenNeeded(int key, int value)
        {
            try
            {
                openNeededShifts.Add(key, value);
                openScheduledShifts.Add(key, 0);
            }
            catch
            {
                Console.WriteLine(key + " already existed in OpenNeeded dictionary");
                openNeededShifts.Remove(key);
                openNeededShifts.Add(key, value);
            }
        }

        /// <summary>
        /// This sets the number of employees needed to close, and initializes the scheduled dictionary
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void SetCloseNeeded(int key, int value)
        {
            try
            {
                closeNeededShifts.Add(key, value);
                closeScheduledShifts.Add(key, 0);
            }
            catch
            {
                Console.WriteLine(key + " already existed in CloseNeeded dictionary");
                closeNeededShifts.Remove(key);
                closeNeededShifts.Add(key, value);
            }
        }
    }
}
