using System;
using System.Collections;
using System.Collections.Generic;

namespace MaydSchedulerApp
{
    /// <summary>
    /// TODO Add hourly availability
    /// </summary>
    public class Employee
    {
        public string empLastName, empFirstName;
        public int empID;
        public int position;
        public bool fullTime;
        public int shiftPreference;
        public int skillLevel;
        public bool active, overtimeAllowed;
        public Availability availability { get; set; }

        public Employee() 
        {
            availability = new Availability();
        }//Default for serialization

        public void SetEmployee(string lastName, string firstName, int id, int pos, bool time, int skill, Availability avail)
        {
            availability = new Availability(avail);
            active = true;
            overtimeAllowed = false;
            empLastName = lastName;
            empFirstName = firstName;
            empID = id;
            skillLevel = skill;
            position = pos;
            fullTime = time;
            shiftPreference = 8;
        }
        
        public void SetEmployee(string lastName, string firstName, int id, int pos, bool time, int skill, Availability avail, int shiftPref)
        {
            availability = new Availability(avail);
            active = true;
            overtimeAllowed = false;
            empLastName = lastName;
            empFirstName = firstName;
            empID = id;
            skillLevel = skill;
            position = pos;
            fullTime = time;
            shiftPreference = shiftPref;
        }

        public bool GetAvailability(int day)
        {
            switch (day)
            {
                case 0:
                    if (availability.sunday.available)
                        return true;
                    else
                        return false;
                case 1:
                    if (availability.monday.available)
                        return true;
                    else
                        return false;
                case 2:
                    if (availability.tuesday.available)
                        return true;
                    else
                        return false;
                case 3:
                    if (availability.wednesday.available)
                        return true;
                    else
                        return false;
                case 4:
                    if (availability.thursday.available)
                        return true;
                    else
                        return false;
                case 5:
                    if (availability.friday.available)
                        return true;
                    else
                        return false;
                case 6:
                    if (availability.saturday.available)
                        return true;
                    else
                        return false;
                default:
                    Console.WriteLine("Invalid case chosen! :: Employee.cs :: GetAvailability(int day): Invalid Value for Day Thrown! :: Returning false!");
                    break;
            }
            return false;
        }
    }
}
