using System;
using System.Collections;

namespace MaydSchedulerApp
{
    public class Availability
    {
        public bool OpenAvailability;
        public Day sunday { get; set; } 
        public Day monday { get; set; }
        public Day tuesday { get; set; }
        public Day wednesday { get; set; }
        public Day thursday{ get; set; }
        public Day friday { get; set; }
        public Day saturday { get; set; }

        //Default for serialization
        public Availability() { }

        public Availability(bool openAvail)
        {
            OpenAvailability = openAvail;
            if(openAvail)
            {
                //do nothing
            }
            else
            {
                sunday = new Day();
                monday = new Day();
                tuesday = new Day();
                wednesday = new Day();
                thursday = new Day();
                friday = new Day();
                saturday = new Day();
            }
        }

        //Copy Const
        public Availability(Availability copy)
        {
            sunday = new Day(copy.sunday);
            monday = new Day(copy.monday);
            tuesday = new Day(copy.tuesday);
            wednesday = new Day(copy.wednesday);
            thursday = new Day(copy.thursday);
            friday = new Day(copy.friday);
            saturday = new Day(copy.saturday);
        }
        
        //Index override
        public Day this[DayOfWeek d]
        {
            get
            {
                switch (d)
                {
                    case DayOfWeek.Sunday:
                        return sunday;
                    case DayOfWeek.Monday:
                        return monday;
                    case DayOfWeek.Tuesday:
                        return tuesday;
                    case DayOfWeek.Wednesday:
                        return wednesday;
                    case DayOfWeek.Thursday:
                        return thursday;
                    case DayOfWeek.Friday:
                        return friday;
                    case DayOfWeek.Saturday:
                        return saturday;
                    default:
                        return null;
                }
            }
            set
            {
                Console.WriteLine("Availability [] Operator Set Method Used. ERROR || Availability.cs || [] Operator");
            }
        }
    }
}
