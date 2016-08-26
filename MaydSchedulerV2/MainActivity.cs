using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using System.Collections.Generic;

namespace MaydSchedulerApp
{
    [Activity(Label = "Mayd Scheduler", Theme = "@android:style/Theme.Material", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static bool testingMode = true;
        private Button BtnNewWeek, BtnEditWeek, BtnHistory, BtnEmpMgmt, BtnSettings;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            StartupProcess();
            SetContentView(Resource.Layout.Main);

            BtnNewWeek = FindViewById<Button>(Resource.Id.newWeekBtn);
            BtnEditWeek = FindViewById<Button>(Resource.Id.editWeekBtn);
            BtnHistory = FindViewById<Button>(Resource.Id.historyBtn);
            BtnEmpMgmt = FindViewById<Button>(Resource.Id.empMgmtBtn);
            BtnSettings = FindViewById<Button>(Resource.Id.settingsBtn);

            BtnNewWeek.Click += BtnNewWeek_Click;
            BtnEditWeek.Click += BtnEditWeek_Click;
            BtnHistory.Click += BtnHistory_Click;
            BtnEmpMgmt.Click += BtnEmpMgmt_Click;
            BtnSettings.Click += BtnSettings_Click;
        }

        private void StartupProcess()
        {
            EmployeeStorage.Start();
            CoreSystem.LoadCoreSettings();
            CoreSystem.LoadCoreSave();
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
            throw new NotImplementedException();
        }

        private void BtnEditWeek_Click(object sender, EventArgs e)
        {
            //SetContentView(Resource.Layout.HorizListViewTest);
            List<string> newlist = new List<string>();
            for (int i = 0; i < 100; i++)
            {
                newlist.Add("This is a test for line " + i + " ||||||||||||||||||||||||||||||||||||||||||||||||||||||||||");
            }

            //ListView list = FindViewById<ListView>(Resource.Id.mylist);

            //ArrayAdapter<string> testAdapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, newlist);

            //list.Adapter = testAdapter;
        }

        private void BtnNewWeek_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ScheduleActivity));
            this.StartActivity(intent);
        }
    }
}

