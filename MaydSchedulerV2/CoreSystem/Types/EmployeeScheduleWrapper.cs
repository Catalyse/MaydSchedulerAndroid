using System;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace MaydSchedulerApp
{
    public class EmployeeScheduleWrapper
    {
        public string lName, fName;
        public int employee;
        public int position;
        public bool fullTime, overTime;
        public int hourTarget;
        public int skill;//This is to minimize callbacks to the main emp later
        public int scheduledHours;
        //The availability is copied onto this so that it can be modified without effecting the set availability in the employee type
        public Availability availability = new Availability();
        public bool availabilityModified = false;
        public List<Shift> shiftList = new List<Shift>();
        public List<DayOfWeek> workingDays = new List<DayOfWeek>();

        [XmlIgnore]//This makes references easier without worrying about complicated lookups. IT does require more in the heap but no extra in storage.
        public Employee empReference;

        public EmployeeScheduleWrapper() { }

        public EmployeeScheduleWrapper(Employee emp)
        {
            empReference = emp;
            lName = emp.empLastName;
            fName = emp.empFirstName;
            employee = emp.empID;
            position = emp.position;
            skill = emp.skillLevel;
            fullTime = emp.fullTime;
            overTime = emp.overtimeAllowed;
            if (emp.fullTime)
                hourTarget = SystemSettings.fullTimeHours;
            else
                hourTarget = SystemSettings.partTimeHours;
            scheduledHours = 0;
            availabilityModified = false;
        }

        public void SetTempAvailability(Availability avail)//TODO add check for when avail last changed to warn user if availability is no longer useful
        {
            availability = avail;
            availabilityModified = true;
        }

        public void PermanentAvailabilityChange(Availability avail)
        {
            availability = avail;
            availabilityModified = false;//since were using the permanent change
            EmployeeStorage.GetEmployee(employee).availability = avail;
        }

        public Availability GetEntireAvail()
        {
            if (availabilityModified)
                return availability;
            else
            {//This fixes the direct memory interaction when modifying temp availability
                availability = new Availability(EmployeeStorage.GetEmployee(employee).availability);
                return availability;
            }
        }

        public bool GetAvailability(int day)
        {
            if (availabilityModified)
            {
                if (availability.OpenAvailability)
                    return true;
                switch (day)
                {
                    case 0:
                        return availability.sunday.available;
                    case 1:
                        return availability.monday.available;
                    case 2:
                        return availability.tuesday.available;
                    case 3:
                        return availability.wednesday.available;
                    case 4:
                        return availability.thursday.available;
                    case 5:
                        return availability.friday.available;
                    case 6:
                        return availability.saturday.available;
                    default:
                        Console.WriteLine("Invalid case chosen! :: EmployeeManagement/Employee.cs :: GetAvailability(int day): Invalid Value for Day Thrown! :: Returning false!");
                        break;
                }
                return false;
            }
            else
            {
                return EmployeeStorage.GetEmployee(employee).GetAvailability(day);
            }
        }
    }
}
