using System.Collections;

namespace MaydSchedulerApp
{
    public class Day
    {
        public bool available;
        public bool openAvail;
        public int startTime, endTime;

        /// <summary>
        /// If Day is instantiated using its default constructor it is assumed to be not available or false.
        /// </summary>
        public Day()
        {
            available = false;
            openAvail = false;
            startTime = 0;
            endTime = 0;
        }
        /// <summary>
        /// If day is instantiated without start and end times it is assumed to be the default times of 08-22
        /// </summary>
        /// <param name="on"></param>
        public Day(bool on)
        {
            available = on;
            if (available)
            {
                openAvail = true;
            }
            else
            {
                openAvail = false;
            }
        }
        /// <summary>
        /// If they can work but have limited availability
        /// </summary>
        /// <param name="s"></param>
        /// <param name="e"></param>
        public Day(int s, int e)
        {
            available = true;
            openAvail = false;
            startTime = s;
            endTime = e;
        }
        public Day(Day copy)
        {
            available = copy.available;
            openAvail = copy.openAvail;
            startTime = copy.startTime;
            endTime = copy.endTime;
        }
    }
}
