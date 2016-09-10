using System;
using System.Collections.Generic;
using Android.App;
using Android.Views;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Gms.Analytics;

namespace MaydSchedulerApp
{
    [Activity(Label = "Mayd Scheduler", Theme = "@android:style/Theme.Material", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Icon = "@drawable/icon", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class MainActivity : Activity
    {
        #region Analytics
        private string TrackerId = "UA-18006016-7";
        public static Tracker tracker;
        #endregion Analytics

        //Temp
        public static bool testingMode = false;
        
        private int count;
        public static int clickedIndex;
        public static bool weekClicked = false, schedulerPassoff = false;
        public static Week week;//This is just a global storage location for this

        #region InterfaceVars
        private ListView recentView;
        private Button BtnNewWeek, BtnQuickWeek, BtnHistory, BtnEmpMgmt, BtnAbout, BtnSettings;
        #endregion InterfaceVars

        public static Activity currentActivity;
        public static ScheduleActivity scheduler;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            currentActivity = this;
            EmployeeStorage.Start();
            SystemSettings.SystemStartup();
            if (!SystemSettings.loaded)
                SettingsAlert();
            SetContentView(Resource.Layout.Main);

            BtnNewWeek = FindViewById<Button>(Resource.Id.newWeekBtn);
            BtnQuickWeek = FindViewById<Button>(Resource.Id.quickWeekBtn);
            BtnHistory = FindViewById<Button>(Resource.Id.historyBtn);
            BtnEmpMgmt = FindViewById<Button>(Resource.Id.empMgmtBtn);
            BtnAbout = FindViewById<Button>(Resource.Id.aboutBtn);
            BtnSettings = FindViewById<Button>(Resource.Id.settingsBtn);

            BtnNewWeek.Click += BtnNewWeek_Click;
            BtnQuickWeek.Click += BtnQuickWeek_Click;
            BtnHistory.Click += BtnHistory_Click;
            BtnEmpMgmt.Click += BtnEmpMgmt_Click;
            BtnAbout.Click += BtnAbout_Click;
            BtnSettings.Click += BtnSettings_Click;

            StartAnalytics();
            FillRecentList();
            SetupButtons();
        }

        #region OVERRIDE
        protected override void OnRestart()
        {
            if (schedulerPassoff)
            {
                schedulerPassoff = false;
                SchedulerPassoff();
                base.OnRestart();
            }
            else
            {
                recentView.ItemClick -= RecentView_ItemClick;
                FillRecentList();
                SetupButtons();
                base.OnRestart();
            }
        }

        protected override void OnDestroy()
        {
            EmployeeStorage.OnSaveList();
            base.OnDestroy();
        }
        #endregion OVERRIDE

        private void StartAnalytics()
        {
            string UUID;
            if(SystemSettings.UUIDLoaded)
            {
                UUID = SystemSettings.UUID;
            }
            else
            {
                UUID = Java.Util.UUID.RandomUUID().ToString();
                SystemSettings.SetUUID(UUID);
            }
            tracker = GoogleAnalytics.GetInstance(Application.Context).NewTracker(TrackerId);
            tracker.SetScreenName("MaydScheduler");
            tracker.SetClientId(UUID);
            tracker.Send(new HitBuilders.ScreenViewBuilder().Build());
            tracker.EnableAutoActivityTracking(true);
            tracker.EnableExceptionReporting(true);
        }

        private void SetupButtons()
        {
            if(SystemSettings.weeksLoaded)
            {
                string recentWeek = SystemSettings.weekList[0].startDate.ToShortDateString();
                BtnQuickWeek.Enabled = true;
                BtnQuickWeek.Text = "Quick Week Based on: " + recentWeek;
            }
            else
            {//Since we have nothing to base a quick week on.
                BtnQuickWeek.Enabled = false;
            }
        }

        private void FillRecentList()
        {
            recentView = FindViewById<ListView>(Resource.Id.homeRecentWeeks);
            List<string> lastFive = new List<string>();
            if (SystemSettings.weekList.Count <= 0)
            {
                lastFive.Add("Nothing here yet!");
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, lastFive);

                recentView.Adapter = adapter;
            }
            else
            {
                if (SystemSettings.weekList.Count > 5)
                    count = 5;
                else
                    count = SystemSettings.weekList.Count;
                for (int i = 0; i < count; i++)
                {
                    lastFive.Add("Schedule for " + SystemSettings.weekList[i].startDate.ToShortDateString());
                }
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, lastFive);
                recentView.Adapter = adapter;

                recentView.ItemClick += RecentView_ItemClick;
            }
        }

        private void RecentView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            weekClicked = true;
            week = SystemSettings.weekList[e.Position];
            Intent intent = new Intent(this, typeof(HistoryActivity));
            this.StartActivity(intent);
        }

        private void SchedulerPassoff()
        {
            weekClicked = true;
            Intent intent = new Intent(this, typeof(HistoryActivity));
            this.StartActivity(intent);
        }

        private void SettingsAlert()
        {
            new AlertDialog.Builder(this)
            .SetCancelable(false)
            .SetPositiveButton("Okay!", (sender, args) =>
            {
                Intent intent = new Intent(this, typeof(SettingsActivity));
                this.StartActivity(intent);
            })
            .SetMessage("Thank you for trying out the Mayd Scheduler! We hope that it becomes an important tool in your day to day operations. \nNow please set up your default settings so we can get you making schedules as fast as possible!")
            .SetTitle("Welcome")
            .Show();
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(AboutActivity));
            this.StartActivity(intent);
        }

        private void BtnQuickWeek_Click(object sender, EventArgs e)
        {
            QuickPrompt();
        }

        private void QuickPrompt()
        {
            new AlertDialog.Builder(this)
            .SetCancelable(false)
            .SetPositiveButton("New Week same settings", (sender, args) =>
            {
                copyAll = false;
                ChooseWeek();
            })
            .SetNegativeButton("Copy Week", (sender, args) =>
            {
                copyAll = true;
                ChooseWeek();
            })
            .SetMessage("You can either: \nMake a copy of the previous week or: \nCopy last weeks settings and generate new shifts.")
            .SetTitle("Quick Week")
            .Show();
        }

        PickWeek pickWeek;
        ListView pickWeekView;
        private bool copyAll = false;

        private void ChooseWeek()
        {
            //put onbackpressed bool here //FIXTHIS
            SetContentView(Resource.Layout.PickWeekLayout);
            this.Title = "Choose Week";
            pickWeek = new PickWeek();
            pickWeekView = FindViewById<ListView>(Resource.Id.chooseWeekListView);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, pickWeek.FindWeeks());

            pickWeekView.Adapter = adapter;

            pickWeekView.ItemClick += PickWeekView_ItemClick;
        }

        private void PickWeekView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            week = new Week(SystemSettings.weekList[0], pickWeek.weekList[e.Position].startDate, copyAll);
            if(copyAll)
            {

            }
            else
            {

            }
        }

        private void GenerateWrapperList()
        {
            List<Employee> empList = EmployeeStorage.employeeList;
            for (int i = 0; i < empList.Count; i++)
            {
                if (empList[i].active)//Check if the employee is active
                {
                    EmployeeScheduleWrapper newWrapper = new EmployeeScheduleWrapper(empList[i]);
                    week.empList.Add(newWrapper);
                }
            }
        }

        private void GenerateSchedule()
        {
            GenerateWrapperList();
            SchedulingAlgorithm.StartScheduleGen();
        }

        private void BtnSettings_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(SettingsActivity));
            this.StartActivity(intent);
        }

        private void BtnEmpMgmt_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(EmpMgmtActivity));
            this.StartActivity(intent);
        }

        private void BtnHistory_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(HistoryActivity));
            this.StartActivity(intent);
        }

        private void BtnNewWeek_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ScheduleActivity));
            this.StartActivity(intent);
        }
    }
}

