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
    [Activity(Label = "System Settings", Theme = "@android:style/Theme.Material", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class SettingsActivity : Activity
    {
        EditText defaultShift, minShift, maxShift, skillCap, partTime, fullTime;
        private Button cancelButton, submitButton, positionButton;
        private bool submitChanged = false;
        private bool settingsSet = true;
        private bool onPosScreen = false;
        public bool editPosition = false;
        public int clickedIndex;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            SettingsScreen();
        }

        private void SettingsScreen()
        {
            SetContentView(Resource.Layout.settings);

            defaultShift = FindViewById<EditText>(Resource.Id.inputDefaultShift);
            minShift = FindViewById<EditText>(Resource.Id.inputMinShift);
            maxShift = FindViewById<EditText>(Resource.Id.inputMaxShift);
            skillCap = FindViewById<EditText>(Resource.Id.inputSkillCap);
            partTime = FindViewById<EditText>(Resource.Id.inputPartTime);
            fullTime = FindViewById<EditText>(Resource.Id.inputFullTime);
            LoadSettings();

            cancelButton = FindViewById<Button>(Resource.Id.btnSettingsCancel);
            cancelButton.Click -= CancelButton_Click;
            cancelButton.Click += CancelButton_Click;

            submitButton = FindViewById<Button>(Resource.Id.btnSettingsSubmit);
            submitButton.Click -= SubmitButton_Click;
            submitButton.Click += SubmitButton_Click;

            positionButton = FindViewById<Button>(Resource.Id.btnPositionEdit);
            positionButton.Click -= PositionButton_Click;
            positionButton.Click += PositionButton_Click;
        }

        #region OVERRIDE
        public override void OnBackPressed()
        {
            if(onPosScreen)
            {
                SettingsScreen();
                onPosScreen = false;
            }
            else if (!settingsSet)
            {
                SettingsNotLoadedAlert();
            }
            else
                base.OnBackPressed();
        }

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            if (submitChanged)
            {
                submitButton.Text = "Submit";
                submitChanged = false;
            }
            return base.DispatchTouchEvent(ev);
        }

        #if DEBUG
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.testing_menu, menu);
            return true;
        }
        #endif

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            #if DEBUG
            switch (item.ItemId)
            {
                case Resource.Id.testing_button1:
                    EmployeeStorage.TestingModeLoad();
                    EmpListSerializer.SerializeEmpList(EmployeeStorage.employeeList);
                    return true;
                case Resource.Id.testing_button2:
                    SystemSettings.LoadTestingSettings();
                    LoadSettings();
                    return true;
            #else
            switch(item.ItemId)
            {
            #endif
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            ListView lv = (ListView)v;
            AdapterView.AdapterContextMenuInfo acmi = (AdapterView.AdapterContextMenuInfo)menuInfo;
            clickedIndex = acmi.Position;

            menu.Add(0, 0, 0, "Edit");
            menu.Add(1, 1, 1, "Delete");
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            if (item.ItemId == 0)
            {
                editPosition = true;
                FragmentTransaction transaction = FragmentManager.BeginTransaction();
                PositionAdd posAdd = new PositionAdd();
                posAdd.Show(transaction, "posAddFragment");
            }
            else
            {//Delete
                ConfirmDeletePosition();
            }
            return base.OnContextItemSelected(item);
        }
        #endregion OVERRIDE

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if(defaultShift.Text == "" || minShift.Text == "" || maxShift.Text == "" || skillCap.Text == "" || partTime.Text == "" || fullTime.Text == "")
            {
                submitButton.Text = "Please fill out all settings!";
                submitChanged = true;
            }
            else
            {
                SystemSettings.InitialSetup(int.Parse(defaultShift.Text), int.Parse(minShift.Text), int.Parse(maxShift.Text),
                    int.Parse(skillCap.Text), int.Parse(partTime.Text), int.Parse(fullTime.Text));
                settingsSet = true;
                Finish();
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (!settingsSet)
            {
                SettingsNotLoadedAlert();
            }
            else
                Finish();
        }

        Button positionCancel, positionAdd;
        ListView positionList;

        private void PositionButton_Click(object sender, EventArgs e)
        {
            onPosScreen = true;
            SetContentView(Resource.Layout.PositionManager);
            positionAdd = FindViewById<Button>(Resource.Id.btnPosMgrAdd);
            positionCancel = FindViewById<Button>(Resource.Id.btnPosMgrCancel);
            positionAdd.Click += PositionAdd_Click;
            positionCancel.Click += PositionCancel_Click;

            positionList = FindViewById<ListView>(Resource.Id.posListView);
            LoadPositionList();
            RegisterForContextMenu(positionList);
        }

        public void LoadPositionList()
        {
            List<string> tempList = new List<string>();
            for (int i = 0; i < SystemSettings.positionList.Count; i++)
            {
                tempList.Add(SystemSettings.positionList[i]);
            }
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, tempList);

            positionList.Adapter = adapter;
        }

        private void PositionCancel_Click(object sender, EventArgs e)
        {
            OnBackPressed();
        }

        private void PositionAdd_Click(object sender, EventArgs e)
        {
            FragmentTransaction transaction = FragmentManager.BeginTransaction();
            PositionAdd posAdd = new PositionAdd();
            posAdd.Show(transaction, "posAddFragment");
        }

        private void SettingsNotLoadedAlert()
        {
            new AlertDialog.Builder(this)
            .SetPositiveButton("Okay", (sender, args) =>
            {
                //DO NOTHING
            })
            .SetNegativeButton("Close Application", (sender, args) =>
            {
                Process.KillProcess(Process.MyPid());
                //Java.Lang.JavaSystem.Exit(0);
            })
            .SetMessage("The system cannot run without default settings!")
            .SetTitle("System Settings")
            .Show();
        }

        private void ConfirmDeletePosition()
        {
            new AlertDialog.Builder(this)
            .SetPositiveButton("Delete", (sender, args) =>
            {
                SystemSettings.RemovePosition(clickedIndex);
                LoadPositionList();
            })
            .SetNegativeButton("Cancel", (sender, args) =>
            {
                //Do nothing
            })
            .SetMessage("If you delete this position you will also delete all employees in the position as well.\nIf you want to move your employees to a new position do that first then delete the position.")
            .SetTitle("Delete Position?")
            .Show();
        }

        /// <summary>
        /// This will load the existing settings in to the editText boxes
        /// </summary>
        private void LoadSettings()
        {
            if (SystemSettings.loaded)
            {
                defaultShift.Text = SystemSettings.defaultShift.ToString();
                minShift.Text = SystemSettings.minShift.ToString();
                maxShift.Text = SystemSettings.maxShift.ToString();
                partTime.Text = SystemSettings.partTimeHours.ToString();
                fullTime.Text = SystemSettings.fullTimeHours.ToString();
                skillCap.Text = SystemSettings.skillLevelCap.ToString();
                settingsSet = true;
            }
            else
                settingsSet = false;
        }
    }
}