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

        private bool onChooseWeek = false;
        private int count;
        public static int clickedIndex;
        public static bool weekClicked = false, historyActivityPassoff = false;
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

            SetupMainScreen();

            StartAnalytics();
            FillRecentList();
            SetupButtons();
        }

        private void SetupMainScreen()
        {
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
        }

        #region OVERRIDE
        protected override void OnRestart()
        {
            SetupMainScreen();
            recentView.ItemClick -= RecentView_ItemClick;
            FillRecentList();
            SetupButtons();
            base.OnRestart();
        }

        protected override void OnDestroy()
        {
            EmployeeStorage.OnSaveList();
            base.OnDestroy();
        }

        public override void OnBackPressed()
        {
            if(onChooseWeek)
            {
                SetupMainScreen();
            }
            else
                base.OnBackPressed();
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
            QuickWeekPrompt();
        }

        private void QuickWeekPrompt()
        {
            new AlertDialog.Builder(this)
            .SetPositiveButton("Copy Week Settings", (sender, args) =>
            {
                copyAll = false;
                ChooseWeek();
            })
            .SetNegativeButton("Dupulicate Week", (sender, args) =>
            {
                copyAll = true;
                ChooseWeek();
            })
            .SetMessage("You can either: \nMake a copy of the previous week or: \nCopy last weeks settings and generate new shifts.")
            .SetTitle("Quick Week")
            .Show();
        }

        private void NoEmployeesWarning()
        {
            new AlertDialog.Builder(this)
            .SetPositiveButton("Okay", (sender, args) =>
            {
                copyAll = false;
                ChooseWeek();
            })
            .SetMessage("You currently have no employees to schedule,\nPlease add some to get started.")
            .SetTitle("No Employees to Schedule!")
            .Show();
        }

        private PickWeek pickWeek;
        ListView pickWeekView;
        private bool copyAll = false;

        private void ChooseWeek()
        {
            onChooseWeek = true;
            SetContentView(Resource.Layout.PickWeekLayout);
            this.Title = "Choose Week";
            pickWeek = new PickWeek();
            pickWeekView = FindViewById<ListView>(Resource.Id.chooseWeekListView);
            List<string> weekList = pickWeek.FindWeeks();
            weekList.Add("Load More");
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, weekList);

            pickWeekView.Adapter = adapter;

            pickWeekView.ItemClick += PickWeekView_ItemClick;
        }

        private void PickWeekView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (e.Position == pickWeek.currentCount)
            {
                List<string> weekList = pickWeek.LoadMore();
                weekList.Add("Load More");
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, weekList);

                pickWeekView.Adapter = adapter;
            }
            else
            {
                week = new Week(SystemSettings.weekList[0], pickWeek.weekList[e.Position].startDate, copyAll);
                onChooseWeek = false;
                if (copyAll)
                {
                    SystemSettings.SaveWeek(week);
                    SchedulerPassoff();
                }
                else
                {
                    GenerateWrapperList();
                    SchedulingAlgorithm.StartScheduleGen();
                }
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

        #region Buttons
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
            if (EmployeeStorage.loaded)
            {
                Intent intent = new Intent(this, typeof(ScheduleActivity));
                this.StartActivity(intent);
            }
            else
            {
                NoEmployeesWarning();
            }
        }
        #endregion Buttons

        public static void ScheduleGenerationComplete(Week w)
        {
            week = w;
            SystemSettings.SaveWeek(w);
            MainActivity main = (MainActivity)currentActivity;
            main.SchedulerPassoff();
        }
    }
}

