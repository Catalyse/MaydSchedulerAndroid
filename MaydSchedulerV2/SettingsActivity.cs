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
    [Activity(Label = "System Settings")]
    public class SettingsActivity : Activity
    {
        private Button cancelButton, submitButton;
        private bool submitChanged = false;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            SetContentView(Resource.Layout.settings);

            cancelButton = FindViewById<Button>(Resource.Id.btnSettingsCancel);
            cancelButton.Click += CancelButton_Click;

            submitButton = FindViewById<Button>(Resource.Id.btnSettingsSubmit);
            submitButton.Click += SubmitButton_Click;
        }

        private void SubmitButton_Click(object sender, EventArgs e)
        {
            EditText defaultShift, minShift, maxShift, defaultOpen, defaultClose, skillCap;
            defaultShift = FindViewById<EditText>(Resource.Id.inputDefaultShift);
            minShift = FindViewById<EditText>(Resource.Id.inputMinShift);
            maxShift = FindViewById<EditText>(Resource.Id.inputMaxShift);
            defaultOpen = FindViewById<EditText>(Resource.Id.inputDefaultOpen);
            defaultClose = FindViewById<EditText>(Resource.Id.inputDefaultClose);
            skillCap = FindViewById<EditText>(Resource.Id.inputSkillCap);
            if(defaultShift.Text == "" || minShift.Text == "" || maxShift.Text == "" || defaultOpen.Text == "" || defaultClose.Text == "" || skillCap.Text == "")
            {
                submitButton.Text = "Please fill out all settings!";
                submitChanged = true;
            }
            else
            {
                SystemSettings.InitialSetup(int.Parse(defaultShift.Text), int.Parse(minShift.Text), int.Parse(maxShift.Text),
                    int.Parse(skillCap.Text), int.Parse(defaultOpen.Text), int.Parse(defaultClose.Text));
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
            if (!SystemSettings.CheckIfLoaded())
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

        private void LoadSettings()
        {
            if(SystemSettings.CheckIfLoaded())
            {

            }
            else
            {//settings not loaded

            }
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