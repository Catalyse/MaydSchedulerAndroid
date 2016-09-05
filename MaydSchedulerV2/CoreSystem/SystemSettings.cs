using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

using Android.App;
using Android.Content;
using Android.Preferences;

namespace MaydSchedulerApp
{
    public static class SystemSettings
    {
        public static int defaultShift, minShift, maxShift;//Need to make config window
        public static int defaultOpenAvail, defaultCloseAvail;
        public static int skillLevelCap;
        public static Dictionary<int, string> positionList = new Dictionary<int, string>();
        public static List<DateTime> weekIndex = new List<DateTime>();
        public static List<Week> weekList = new List<Week>();
        public static int sunOpenPref, monOpenPref, tueOpenPref, wedOpenPref, thuOpenPref, friOpenPref, satOpenPref;
        public static int sunClosePref, monClosePref, tueClosePref, wedClosePref, thuClosePref, friClosePref, satClosePref;
        public static bool loaded, facilityDefaults, positionsCreated, weeksLoaded, UUIDLoaded;
        public static string UUID;

        public static void SystemStartup()
        {
            loaded = CheckIfSettingsExist();
            if (loaded)
                InitialLoad();
            facilityDefaults = CheckFacDefaults();
            positionsCreated = CheckPositionsExist();
            if (positionsCreated)
                GetPositionList();
            weeksLoaded = CheckSavedWeeks();
            if (weeksLoaded)
                LoadWeeks();
            UUIDLoaded = CheckUUID();
            if (UUIDLoaded)
                GetUUID();
        }

        private static bool CheckIfSettingsExist()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            return prefs.GetBoolean("loaded", false);
        }

        private static bool CheckFacDefaults()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            return prefs.GetBoolean("facilityDefaults", false);
        }

        private static bool CheckPositionsExist()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            return prefs.GetBoolean("positionsCreated", false);
        }

        private static bool CheckSavedWeeks()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            return prefs.GetBoolean("positionsCreated", false);
        }

        private static bool CheckUUID()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            return prefs.GetBoolean("UUIDSet", false);
        }

        private static string GetUUID()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            return prefs.GetString("UUID", "null");
        }

        public static void SetUUID(string id)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString("UUID", id);
            editor.PutBoolean("UUID", true);
            UUIDLoaded = true;
            UUID = id.ToString();
            editor.Apply();
        }

        public static bool FindWeek(DateTime weekStartDate)
        {
            if (weekIndex.Contains(weekStartDate))
                return true;
            else
                return false;
        }

        public static void LoadTestingSettings()
        {
            InitialSetup(8, 6, 10, 10, 8, 22);
            SetupFacilityPreferences(10, 10, 10, 10, 10, 10, 10, 20, 20, 20, 20, 20, 20, 20);
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutBoolean("positionsCreated", true);
            editor.PutInt("positionCount", 2);
            editor.PutString("0", "Solutions Specialist");
            editor.PutString("1", "Experience Specialist");
            editor.Apply();
            InitialLoad();
            GetPositionList();
        }

        public static void InitialSetup(int _defaultShift, int _minShift, int _maxShift, int _skillCap, int _defaultOpenAvail, int _defaultCloseAvail)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            ISharedPreferencesEditor editor = prefs.Edit();
            loaded = true;
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

        public static bool InitialLoad()
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            defaultShift = prefs.GetInt("defaultShift", -1);
            minShift = prefs.GetInt("minShift", -1);
            maxShift = prefs.GetInt("maxShift", -1);
            defaultOpenAvail = prefs.GetInt("defaultOpenAvail", -1);
            defaultCloseAvail = prefs.GetInt("defaultCloseAvail", -1);
            skillLevelCap = prefs.GetInt("skillCap", -1);
            if (defaultShift == -1 || minShift == -1 || maxShift == -1 || defaultOpenAvail == -1 || defaultCloseAvail == -1 || skillLevelCap == -1)
                return false;
            else
                return true;
        }

        public static void SaveWeek(Week week)
        {
            StringWriter writer = new StringWriter();
            XmlSerializer serializer = new XmlSerializer(typeof(Week));

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            ISharedPreferencesEditor editor = prefs.Edit();
            int count = prefs.GetInt("savedWeekCount", 0);
            if (!weekIndex.Contains(week.startDate))
            {
                week.saveIndex = count;
                serializer.Serialize(writer, week);
                string serializedWeek = writer.ToString();
                editor.PutString("week" + count.ToString(), serializedWeek);
                editor.PutInt("savedWeekCount", count + 1);
                editor.PutBoolean("weeksSaved", true);
                editor.Apply();
                LoadWeeks();
            }
            else
            {
                serializer.Serialize(writer, week);
                string serializedWeek = writer.ToString();
                editor.PutString("week" + week.saveIndex.ToString(), serializedWeek);
                editor.PutBoolean("weeksSaved", true);
                editor.Apply();
            }
        }

        public static void LoadWeeks()
        {
            weekList = new List<Week>();
            weekIndex = new List<DateTime>();
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            if(weeksLoaded)
            {
                int count = prefs.GetInt("savedWeekCount", 0);
                if(count < 1)//This likely means nothing is saved and the bool is fucked up
                {
                    Console.WriteLine("LoadWeeks() Failure, weeksSaved bool reports we have weeks saved but count reports zero, unable to load");
                }
                else
                {//We have a set of weeks
                    for(int i = 0; i < count; i++)
                    {
                        string loadedWeek = prefs.GetString("week" + i, "null");
                        StringReader reader = new StringReader(loadedWeek);
                        XmlSerializer serializer = new XmlSerializer(typeof(Week));
                        Week des = (Week)serializer.Deserialize(reader);
                        weekList.Add(des);
                        weekIndex.Add(des.startDate);
                    }
                    SortList();
                }
            }
        }

        private static void SortList()
        {
            //Starts false so if list is already in order do nothing
            bool sortAgain = false;
            Week current;
            Week next;
            do
            {
                if (weekList.Count > 1)
                {
                    sortAgain = false;
                    for (int i = 0; i < weekList.Count - 1; i++)
                    {
                        current = weekList[i];
                        next = weekList[i + 1];
                        if (DateTime.Compare(current.startDate, next.startDate) < 0)
                        {
                            weekList[i] = next;
                            weekList[i + 1] = current;
                            sortAgain = true;
                        }
                    }
                }
            }
            while (sortAgain == true);
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
            editor.PutBoolean("facilityDefaults", true);
            editor.Apply();
            facilityDefaults = true;
        }

        public static int GetIntPref(string prefName)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            return prefs.GetInt(prefName, -1);
        }

        public static bool GetPositionList()
        {
            positionList = new SerializableDictionary<int, string>();
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            if (!prefs.GetBoolean("positionsCreated", false))
                return false;
            else
            {
                int count = prefs.GetInt("positionCount", -1);
                if(count <= 0)
                {
                    Console.WriteLine("Position Count Returned -1, please reset the applications data");
                    return false;
                }
                else
                {
                    for(int i = 0; i < count; i++)
                    {
                        positionList.Add(i, prefs.GetString(i.ToString(), "ERROR RESET DATA"));
                    }
                    return true;
                }
            }
        }

        public static bool AddPosition(string positionName)
        {
            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(CoreSystem.currentActivity);
            ISharedPreferencesEditor editor = prefs.Edit();
            if (!prefs.GetBoolean("positionsCreated", false))
            {//No positions made, first added
                editor.PutBoolean("positionsCreated", true);
                editor.PutInt("positionCount", 1);
                editor.PutString("0", positionName);
                positionList.Add(0, positionName);
                editor.Apply();
                return true;
            }
            else
            {
                if (prefs.Contains(positionName))
                    return false;//The position already exists
                else
                {//Another position has been created, so we increment the count
                    int count = prefs.GetInt("positionCount", -1);
                    count++;
                    editor.PutInt("positionCount", count);
                    editor.PutString(count.ToString(), positionName);
                    positionList.Add(count, positionName);
                    editor.Apply();
                    return true;
                }
            }
        }
    }
}