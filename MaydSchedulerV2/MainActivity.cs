using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Preferences;

namespace MaydSchedulerApp
{
    [Activity(Label = "Mayd Scheduler", Theme = "@android:style/Theme.Material", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static bool testingMode = false, weekClicked = false;
        private Button BtnNewWeek, BtnQuickWeek, BtnHistory, BtnEmpMgmt, BtnAbout, BtnSettings;
        private int count;
        public static int clickedIndex;
        private ListView recentView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            CoreSystem.currentActivity = this;
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

            FillRecentList();
            SetupButtons();
        }
        
        protected override void OnRestart()
        {
            recentView.ItemClick -= RecentView_ItemClick;
            FillRecentList();
            base.OnRestart();
        }

        private void SetupButtons()
        {
            if(SystemSettings.weeksLoaded)
            {

            }
            else
            {

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
            switch(e.Position)
            {
                case 0:
                    weekClicked = true;
                    clickedIndex = 1;
                    break;

                case 1:
                    weekClicked = true;
                    clickedIndex = 2;
                    break;
                case 2:
                    weekClicked = true;
                    clickedIndex = 3;
                    break;
                case 3:
                    weekClicked = true;
                    clickedIndex = 4;
                    break;
                case 4:
                    weekClicked = true;
                    clickedIndex = 5;
                    break;
            }
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
            .SetMessage("Thank you for trying out the Mayd Scheduler! We hope that it becomes an important tool in your day to day operations.  Now please set up your default settings so we can get you making schedules as fast as possible!")
            .SetTitle("Welcome")
            .Show();
        }

        private void BtnAbout_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void BtnQuickWeek_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
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

