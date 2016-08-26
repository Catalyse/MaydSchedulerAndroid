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
    [Activity(Label = "Employee Management", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@android:style/Theme.Material")]
    public class EmpMgmtActivity : Activity
    {
        private Button menu1, menu2, menu3;
        ListView empListView;
        EmpMgmtAdapter empListAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EmpMgmtLayout);
            //These two methods enable the back button
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            menu1 = FindViewById<Button>(Resource.Id.action_button1);
            menu2 = FindViewById<Button>(Resource.Id.action_button2);
            menu3 = FindViewById<Button>(Resource.Id.action_button3);
            
            empListView = FindViewById<ListView>(Resource.Id.EmpMgmtList);

            GenerateEmpList(EmployeeStorage.employeeList);

            RegisterForContextMenu(empListView);

            /*Button btnpopupmenu = FindViewById<Button>(Resource.Id.btnpopupmenu);
            btnpopupmenu.Click += (s, arg) =>
            {
                PopupMenu menu = new PopupMenu(this, btnpopupmenu);
                menu.Inflate(Resource.Layout.popup_menu);
                menu.Show();
            };*/
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            ListView lv = (ListView)v;
            AdapterView.AdapterContextMenuInfo acmi = (AdapterView.AdapterContextMenuInfo)menuInfo;
            var clicked = lv.GetItemAtPosition(acmi.Position);

            menu.Add("one");
            menu.Add("two");
            menu.Add(clicked.Class.Name);
        }

        public void GenerateEmpList(List<Employee> empList)
        {
            if (empList.Count > 0)
            {
                empListAdapter = new EmpMgmtAdapter(this, empList);

                empListView.Adapter = empListAdapter;
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Layout.options_menu, menu);
            return true;
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