using System;
using System.Collections;

namespace MaydSchedulerApp
{
    public class Shift//This should be expanded to hold more data
    {
        public int employee;
        public int startShift, endShift;
        public DayOfWeek date;
        
        public Shift() { }
        
        public Shift(int emp, int start, int end, DayOfWeek d)//may need to float these or something to handle non hourly incremental shifts 
        {
            employee = emp;
            startShift = start;
            endShift = end;
            date = d;
        }
    }
}
