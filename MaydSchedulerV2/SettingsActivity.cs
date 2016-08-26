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
    [Activity(Label = "SettingsActivity")]
    public class SettingsActivity : Activity
    {
        private Button cancelButton; 

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            SetContentView(Resource.Layout.settings);

            cancelButton = FindViewById<Button>(Resource.Id.btnSettingsCancel);
            cancelButton.Click += CancelButton_Click;
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            Finish();
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