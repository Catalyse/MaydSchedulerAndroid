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
    [Activity(Label = "Schedule History", Theme = "@android:style/Theme.Material", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class HistoryActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ScheduleHistory);

            ListView historyView = FindViewById<ListView>(Resource.Id.historyListView);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, pickWeek.FindWeeks());
            // Create your application here
        }

        private List<string> ConvertToStringList()
        {

        }
    }
}