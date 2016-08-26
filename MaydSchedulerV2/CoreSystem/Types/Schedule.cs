using System.Collections;
using System.Collections.Generic;

namespace MaydSchedulerApp
{
    public class Schedule
    {
        public Week scheduledWeek;
        public List<Employee> employeeList = new List<Employee>();
        //Dictionary format: 0=sunday, 1=monday, 2=tuesday, 3=wednesday, 4=thursday, 5=friday, 6=saturday
        //public Dictionary<int, List<ScheduledEmployee>> weeklySchedule = new Dictionary<int, List<ScheduledEmployee>>();

        public Schedule() { }//Default for serialization;
    }
}