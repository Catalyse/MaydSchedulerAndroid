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
        EditText defaultShift, minShift, maxShift, defaultOpen, defaultClose, skillCap;
        private Button cancelButton, submitButton;
        private bool submitChanged = false;
        private bool settingsSet = true;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            SetContentView(Resource.Layout.settings);

            defaultShift = FindViewById<EditText>(Resource.Id.inputDefaultShift);
            minShift = FindViewById<EditText>(Resource.Id.inputMinShift);
            maxShift = FindViewById<EditText>(Resource.Id.inputMaxShift);
            defaultOpen = FindViewById<EditText>(Resource.Id.inputDefaultOpen);
            defaultClose = FindViewById<EditText>(Resource.Id.inputDefaultClose);
            skillCap = FindViewById<EditText>(Resource.Id.inputSkillCap);

            LoadSettings();

            cancelButton = FindViewById<Button>(Resource.Id.btnSettingsCancel);
            cancelButton.Click += CancelButton_Click;

            submitButton = FindViewById<Button>(Resource.Id.btnSettingsSubmit);
            submitButton.Click += SubmitButton_Click;
        }

        public override void OnBackPressed()
        {
            if(!settingsSet)
            {
                SettingsNotLoadedAlert();
            }
            else
                base.OnBackPressed();
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            if(defaultShift.Text == "" || minShift.Text == "" || maxShift.Text == "" || defaultOpen.Text == "" || defaultClose.Text == "" || skillCap.Text == "")
            {
                submitButton.Text = "Please fill out all settings!";
                submitChanged = true;
            }
            else
            {
                SystemSettings.InitialSetup(int.Parse(defaultShift.Text), int.Parse(minShift.Text), int.Parse(maxShift.Text),
                    int.Parse(skillCap.Text), int.Parse(defaultOpen.Text), int.Parse(defaultClose.Text));
                settingsSet = true;
                Finish();
            }
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


        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (!settingsSet)
            {
                SettingsNotLoadedAlert();
            }
            else
                Finish();
        }

        private void SettingsNotLoadedAlert()
        {
            new AlertDialog.Builder(this)
            .SetPositiveButton("Okay", (sender, args) =>
            {
                Intent intent = new Intent(this, typeof(SettingsActivity));
                this.StartActivity(intent);
            })
            .SetNegativeButton("Close Application", (sender, args) =>
            {
                System.Environment.Exit(0);
            })
            .SetMessage("The system cannot run without default settings!")
            .SetTitle("System Settings")
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
                defaultOpen.Text = SystemSettings.defaultOpenAvail.ToString();
                defaultClose.Text = SystemSettings.defaultCloseAvail.ToString();
                skillCap.Text = SystemSettings.skillLevelCap.ToString();
                settingsSet = true;
            }
            else
                settingsSet = false;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
    }
}