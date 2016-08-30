using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Preferences;

namespace MaydSchedulerApp
{
    [Activity(Label = "Mayd Scheduler", Theme = "@android:style/Theme.Material", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        public static bool testingMode = false;
        private Button BtnNewWeek, BtnEditWeek, BtnHistory, BtnEmpMgmt, BtnSettings, action1, action2;

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

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.testing_menu, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.testing_button1:
                    EmployeeStorage.employeeList = FileManager.TestingModeLoad(); ;
                    EmpListSerializer.SerializeEmpList(EmployeeStorage.employeeList);
                    return true;
                case Resource.Id.testing_button2:
                    SystemSettings.LoadTestingSettings();
                    return true;
            }
            return base.OnOptionsItemSelected(item);
        }

        private void SettingsAlert()
        {
            new AlertDialog.Builder(this)
            .SetCancelable(false)
            .SetPositiveButton("Okay", (sender, args) =>
            {
                Intent intent = new Intent(this, typeof(SettingsActivity));
                this.StartActivity(intent);
            })
            .SetMessage("You need to set your default settings!")
            .SetTitle("System Settings")
            .Show();
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

        private void BtnEditWeek_Click(object sender, EventArgs e)
        {
            //HackSave();
        }

        private void BtnNewWeek_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(this, typeof(ScheduleActivity));
            this.StartActivity(intent);
        }
    }
}

