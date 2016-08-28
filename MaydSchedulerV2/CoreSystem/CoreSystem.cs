﻿using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;

namespace MaydSchedulerApp
{   
    /// <summary>
    /// This class serves as the central control point for the program
    /// Or reference point rather since it doesnt really have much direct control
    /// </summary>
    public static class CoreSystem
    {
        public static CoreSaveType coreSave;
        public static Activity currentActivity;
        public static ScheduleActivity scheduler;
        public static bool coreSettingsLoaded = false;
        public static bool coreSaveLoaded = false;
        public static SerializableDictionary<DateTime, Week> weekList = new SerializableDictionary<DateTime, Week>();
        public static List<string> savedFileList = new List<string>();
        public static string GenerationDate;

        //Networking Section
        public static bool networkLoading = false;
        public static bool networkFailure = false;

        //This section is for thread management of the scheduling algorithm
        public static Week week;
        public static bool currentlyProcessing = false;//This is for unity update loops to check if the subthread for the schedule is still running.
        public static bool loadingCoreSave = false;

        /// <summary>
        /// This method is used to load default system settings from file
        /// </summary>
        /*public static void LoadCoreSettings()
        {
            if (MainActivity.testingMode)
            {
                coreSettings = TempData.CoreSettings;
                defaultShift = coreSettings.defaultShift;
                defaultOpenAvail = coreSettings.defaultOpenAvail;
                defaultCloseAvail = coreSettings.defaultCloseAvail;
                minShift = coreSettings.minShift;
                maxShift = coreSettings.maxShift;
                skillLevelCap = coreSettings.skillLevelCap;
                positionList = coreSettings.positionList;
                savedFileList = coreSettings.savedFileList;
                GenerationDate = DateTime.Now.ToString();
                coreSettingsLoaded = true;
                Console.WriteLine("Core Settings File Loaded!");
            }
            else
            {
                if (FileManager.CheckIfFileExists("CoreSettings"))
                {
                    coreSettings = new CoreSettingsType();
                    coreSettings = FileManager.DeserializeFile<CoreSettingsType>("CoreSettings");
                    defaultShift = coreSettings.defaultShift;
                    defaultOpenAvail = coreSettings.defaultOpenAvail;
                    defaultCloseAvail = coreSettings.defaultCloseAvail;
                    minShift = coreSettings.minShift;
                    maxShift = coreSettings.maxShift;
                    skillLevelCap = coreSettings.skillLevelCap;
                    positionList = coreSettings.positionList;
                    savedFileList = coreSettings.savedFileList;
                    GenerationDate = DateTime.Now.ToString();
                    coreSettingsLoaded = true;
                    Console.WriteLine("Core Settings File Loaded!");
                }
                else
                {
                    coreSettingsLoaded = false;
                    Console.WriteLine("Core Settings File not found!");
                    coreSettings = new CoreSettingsType();
                    coreSettings.GenerationDate = DateTime.Now.ToString();//THis sets the gen date, although if a save is never completed on a weeklist then the gen date wont be saved
                }
            }
        }*/

        /*public static void CoreSettingsChanged()
        {
            
        }*/

        /*public static void CoreSettingsLoaded(CoreSettingsType settings)
        {
            coreSettings = new CoreSettingsType();
            coreSettings = settings;
            defaultShift = coreSettings.defaultShift;
            defaultOpenAvail = coreSettings.defaultOpenAvail;
            defaultCloseAvail = coreSettings.defaultCloseAvail;
            minShift = coreSettings.minShift;
            maxShift = coreSettings.maxShift;
            skillLevelCap = coreSettings.skillLevelCap;
            positionList = coreSettings.positionList;
            savedFileList = coreSettings.savedFileList;
            GenerationDate = DateTime.Now.ToString();
            coreSettingsLoaded = true;
            Console.WriteLine("Core Settings File Loaded!");
        }*/

        public static void LoadCoreSave()
        {
            if (FileManager.CheckIfFileExists("CoreSave"))
            {
                coreSave = new CoreSaveType();
                FileManager.DeserializeCoreSave("CoreSettings");
                coreSaveLoaded = true;
            }
            else
            {
                coreSaveLoaded = false;
                Console.WriteLine("Core Save File not found!");
                coreSave = new CoreSaveType();
                coreSave.GenerationDate = DateTime.Now.ToString(); //THis sets the gen date, although if a save is never completed on a weeklist then the gen date wont be saved
            }
        }

        public static void CoreSaveChanged()
        {
            if (coreSave != null)
            {
                coreSave.weekList = weekList;
                coreSave.LastModified = DateTime.Now.ToString();
                //FileManager.SerializeCoreSave();
                //NetworkIO.SendCoreSave();
            }
            else
            {
                coreSave = new CoreSaveType();
                coreSave.weekList = weekList;
                coreSave.LastModified = DateTime.Now.ToString();
                //FileManager.SerializeCoreSave();
                //NetworkIO.SendCoreSave();
            }
        }

        public static void CoreSaveLoaded(CoreSaveType save)//This exists because the load is threaded
        {
            coreSave = save;
            coreSaveLoaded = true;
            weekList = coreSave.weekList;
            //NetworkIO.SendCoreSave();
        }

        public static Week FindWeek(DateTime weekStartDate)
        {
            if(weekList.ContainsKey(weekStartDate.Date))
                return weekList[weekStartDate.Date];
            else
                return null;
        }

        public static string GetPositionName(int type)
        {
            if (SystemSettings.positionList.ContainsKey(type))
            {
                return SystemSettings.positionList[type];
            }
            else
            {
                Console.WriteLine("Position that does not exist was queried for! || CoreSystem.cs || GetPositionName || TypeNo: " + type);
                return "ErrNotFound";
            }
        }

        /// <summary>
        /// Method to begin thread for schedule generation
        /// Also generates wrapper list due to temp availability
        /// </summary>
        /// <param name="w"></param>
        public static void GenerateSchedule(Week w)
        {
            Console.WriteLine("Starting Schedule Generation");
            currentlyProcessing = true;//This allows instanced objects to track whether the threaded generation is done or not
            week = w;
            Thread scheduleProcess = new Thread(new ThreadStart(SchedulingAlgorithm.StartScheduleGen));
            scheduleProcess.Start();
        }

        public static void GenerationComplete(Week w)
        {
            week = w;
            currentlyProcessing = false;
            if (weekList.ContainsKey(w.startDate))//This will overwrite the week in the event that it already existed and we are regenerating it.
            {
                weekList.Remove(w.startDate);
                weekList.Add(w.startDate, w);
            }
            else
            {
                weekList.Add(w.startDate, w);
            }
            CoreSaveChanged();
            scheduler.DrawSchedule();
        }

        //Random Common Methods ======================================================================================
        public static void ErrorCatch(string msg)
        {
            Console.WriteLine(msg);
            //SystemMessages.ThrowMessage(msg);
        }

        public static void ErrorCatch(string msg, Exception ex)
        {
            Console.WriteLine(msg);
            //SystemMessages.ThrowMessage(msg);
            Console.WriteLine(ex.Message + "\n" + ex.InnerException + "\n" + ex.StackTrace);
        }

        public static int RandomInt(int count)
        {
            Random gen = new Random();
            int returnVal = gen.Next(count);
            return returnVal;
        }

        public static bool RandomBool()
        {
            Random gen = new Random();
            int prob = gen.Next(100);
            if (prob < 50)
                return true;
            else
                return false;
        }

        public static int ConvertToMilitaryTime(int time)
        {
            if (time <= 12)
            {
                time += 12;
                return time;
            }
            else
                return time;
        }

        public static int ConvertDoWToInt(DayOfWeek d)
        {
            switch (d)
            {
                case DayOfWeek.Sunday:
                    return 0;
                case DayOfWeek.Monday:
                    return 1;
                case DayOfWeek.Tuesday:
                    return 2;
                case DayOfWeek.Wednesday:
                    return 3;
                case DayOfWeek.Thursday:
                    return 4;
                case DayOfWeek.Friday:
                    return 5;
                case DayOfWeek.Saturday:
                    return 6;
                default:
                    Console.WriteLine("ConvertDoWToInt() Error || Invalid DayOfWeek provided");
                    return -1;
            }
        }
    }
}
