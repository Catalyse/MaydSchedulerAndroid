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
        private Button menu1, menu2, menu3, action1, action2;
        ListView empListView;
        EmpMgmtAdapter empListAdapter;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.EmpMgmtLayout);
            //These two methods enable the back button
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            empListView = FindViewById<ListView>(Resource.Id.EmpMgmtList);

            GenerateEmpList(EmployeeStorage.employeeList);

            RegisterForContextMenu(empListView);
        }

        private void Menu1_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Menu2_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            ListView lv = (ListView)v;
            AdapterView.AdapterContextMenuInfo acmi = (AdapterView.AdapterContextMenuInfo)menuInfo;
            var clicked = lv.GetItemAtPosition(acmi.Position);

            menu.Add(0, 0, 0, "Edit");
            menu.Add(1, 1, 1, "Delete");
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            int index = info.Position;
            if(item.ItemId == 0)
            {
                //edit
            }
            else
            {
                //delete
            }
            Console.WriteLine(item.ItemId);
            return base.OnContextItemSelected(item);
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
            action1 = FindViewById<Button>(Resource.Id.action_button1);
            action2 = FindViewById<Button>(Resource.Id.action_button2);
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