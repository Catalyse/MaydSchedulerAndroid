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
using Android.Preferences;

namespace MaydSchedulerApp
{
    public static class SystemSettings
    {
        public static int defaultShift, minShift, maxShift;//Need to make config window
        public static int defaultOpenAvail, defaultCloseAvail;
        public static int skillLevelCap;
        public static SerializableDictionary<int, string> positionList = new SerializableDictionary<int, string>();
        public static int sunOpenPref, monOpenPref, tueOpenPref, wedOpenPref, thuOpenPref, friOpenPref, satOpenPref;
        public static int sunClosePref, monClosePref, tueClosePref, wedClosePref, thuClosePref, friClosePref, satClosePref;

        public static bool CheckIfLoaded()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            return prefs.GetBoolean("loaded", false);
        }

        public static void InitialSetup(int _defaultShift, int _minShift, int _maxShift, int _skillCap, int _defaultOpenAvail, int _defaultCloseAvail)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutBoolean("loaded", true);
            defaultShift = _defaultShift;
            editor.PutInt("defaultShift", defaultShift);
            minShift = _minShift;
            editor.PutInt("minShift", minShift);
            maxShift = _maxShift;
            editor.PutInt("maxShift", maxShift);
            defaultOpenAvail = _defaultOpenAvail;
            editor.PutInt("defaultOpenAvail", defaultOpenAvail);
            defaultCloseAvail = _defaultCloseAvail;
            editor.PutInt("defaultCloseAvail", defaultCloseAvail);
            skillLevelCap = _skillCap;
            editor.PutInt("skillCap", skillLevelCap);
            editor.Apply();
        }

        public static void InitialLoad()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            defaultShift = prefs.GetInt("defaultShift", -1);
            minShift = prefs.GetInt("minShift", -1);
            maxShift = prefs.GetInt("maxShift", -1);
            defaultOpenAvail = prefs.GetInt("defaultOpenAvail", -1);
            defaultCloseAvail = prefs.GetInt("defaultCloseAvail", -1);
            skillLevelCap = prefs.GetInt("skillCap", -1);
        }

        public static void SetupFacilityPreferences(int suO, int mO, int tuO, int wO, int thO, int fO, int saO, 
                                                    int suC, int mC, int tuC, int wC, int thC, int fC, int saC)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            ISharedPreferencesEditor editor = prefs.Edit();
            sunOpenPref = suO;
            monOpenPref = mO;
            tueOpenPref = tuO;
            wedOpenPref = wO;
            thuOpenPref = thO;
            friOpenPref = fO;
            satOpenPref = saO;
            sunClosePref = suC;
            monClosePref = mC;
            tueClosePref = tuC;
            wedClosePref = wC;
            thuClosePref = thC;
            friClosePref = fC;
            satClosePref = saC;
            editor.PutInt("suO", suO);
            editor.PutInt("mO", mO);
            editor.PutInt("tuO", tuO);
            editor.PutInt("wO", wO);
            editor.PutInt("thO", thO);
            editor.PutInt("fO", fO);
            editor.PutInt("saO", saO);
            editor.PutInt("suC", suC);
            editor.PutInt("mC", mC);
            editor.PutInt("tuC", tuC);
            editor.PutInt("wC", wC);
            editor.PutInt("thC", thC);
            editor.PutInt("fC", fC);
            editor.PutInt("saC", saC);
            editor.Apply();
        }
    }
}