using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MaydSchedulerApp
{
    /// <summary>
    /// This class simplifies the datetime protocol by basing the date from the year 2000 and allowing for comparison of ints, versus DateTime
    /// </summary>
    public static class DateConversion
    {
        public static int Convert(DateTime input)
        {
            int yearCount = input.Year - 2000;
            int leapCount = yearCount / 4;//calc no of leap years
            leapCount++;//add the year 2000
            yearCount -= leapCount;//remove leap years from yearcount
            return ((366 * leapCount) + (365 * yearCount) + input.DayOfYear);
        }
    }
}