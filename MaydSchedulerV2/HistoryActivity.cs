using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace MaydSchedulerApp
{
    [Activity(Label = "Schedule History", Theme = "@android:style/Theme.Material", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class HistoryActivity : Activity
    {
        private Week selected;
        private EmployeeScheduleWrapper currentEmp;
        private bool weekSelected = false, editSubmitChanged = false, weekModified = false, inEditor = false;
        private int clickedIndex = 0;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            LoadWeekList();
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

        public override void OnBackPressed()
        {
            if (inEditor)
            {
                LoadWeek(selected);
                inEditor = false;
            }
            else if (weekSelected)
            {
                if(weekModified)
                {
                    SystemSettings.SaveWeek(selected);
                }
                LoadWeekList();
                weekSelected = false;
            }
            else
                base.OnBackPressed();
        }

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            if (editSubmitChanged)
            {
                editSubmit.Text = "Submit";
                editSubmitChanged = false;
            }
            return base.DispatchTouchEvent(ev);
        }

        private void LoadWeek(Week w)
        {
            weekSelected = true;
            this.Title = "Schedule for " + selected.startDate.ToShortDateString();
            SetContentView(Resource.Layout.ScheduleView);
            ScheduleAdapter adapter = new ScheduleAdapter(this, w.empList);
            ListView scheduleView = FindViewById<ListView>(Resource.Id.scheduleListView);
            scheduleView.Adapter = adapter;
            RegisterForContextMenu(scheduleView);
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
            if(item.ItemId == 0)
            {
                EditEmployeeShift(selected.empList[clickedIndex]);
            }
            else
            {
                selected.empList[clickedIndex].shiftList.Clear();
                weekModified = true;
                LoadWeek(selected);
            }
            return base.OnContextItemSelected(item);
        }

        EditText sunOpen, monOpen, tueOpen, wedOpen, thuOpen, friOpen, satOpen, sunClose, monClose, tueClose, wedClose, thuClose, friClose, satClose;
        Button editSubmit, editTitle, editCancel;

        private void EditEmployeeShift(EmployeeScheduleWrapper emp)
        {
            inEditor = true;
            currentEmp = emp;
            SetContentView(Resource.Layout.EditShift);
            editTitle = FindViewById<Button>(Resource.Id.btnEditTitle);
            editSubmit = FindViewById<Button>(Resource.Id.btnEditSubmit);
            editCancel = FindViewById<Button>(Resource.Id.btnEditCancel);
            sunOpen = FindViewById<EditText>(Resource.Id.editInputSSunOpen);
            monOpen = FindViewById<EditText>(Resource.Id.editInputSMonOpen);
            tueOpen = FindViewById<EditText>(Resource.Id.editInputSTueOpen);
            wedOpen = FindViewById<EditText>(Resource.Id.editInputSWedOpen);
            thuOpen = FindViewById<EditText>(Resource.Id.editInputSThuOpen);
            friOpen = FindViewById<EditText>(Resource.Id.editInputSFriOpen);
            satOpen = FindViewById<EditText>(Resource.Id.editInputSSatOpen);
            sunClose = FindViewById<EditText>(Resource.Id.editInputSSunClose);
            monClose = FindViewById<EditText>(Resource.Id.editInputSMonClose);
            tueClose = FindViewById<EditText>(Resource.Id.editInputSTueClose);
            wedClose = FindViewById<EditText>(Resource.Id.editInputSWedClose);
            thuClose = FindViewById<EditText>(Resource.Id.editInputSThuClose);
            friClose = FindViewById<EditText>(Resource.Id.editInputSFriClose);
            satClose = FindViewById<EditText>(Resource.Id.editInputSSatClose);
            editTitle.Text = emp.lName + ", " + emp.fName;
            for(int i = 0; i < emp.shiftList.Count; i++)
            {
                switch (emp.shiftList[i].date)
                {
                    case DayOfWeek.Sunday:
                        sunOpen.Text = emp.shiftList[i].startShift.ToString();
                        sunClose.Text = emp.shiftList[i].endShift.ToString();
                        break;
                    case DayOfWeek.Monday:
                        monOpen.Text = emp.shiftList[i].startShift.ToString();
                        monClose.Text = emp.shiftList[i].endShift.ToString();
                        break;
                    case DayOfWeek.Tuesday:
                        tueOpen.Text = emp.shiftList[i].startShift.ToString();
                        tueClose.Text = emp.shiftList[i].endShift.ToString();
                        break;
                    case DayOfWeek.Wednesday:
                        wedOpen.Text = emp.shiftList[i].startShift.ToString();
                        wedClose.Text = emp.shiftList[i].endShift.ToString();
                        break;
                    case DayOfWeek.Thursday:
                        thuOpen.Text = emp.shiftList[i].startShift.ToString();
                        thuClose.Text = emp.shiftList[i].endShift.ToString();
                        break;
                    case DayOfWeek.Friday:
                        friOpen.Text = emp.shiftList[i].startShift.ToString();
                        friClose.Text = emp.shiftList[i].endShift.ToString();
                        break;
                    case DayOfWeek.Saturday:
                        satOpen.Text = emp.shiftList[i].startShift.ToString();
                        satClose.Text = emp.shiftList[i].endShift.ToString();
                        break;
                }
            }
            editSubmit.Click += EditSubmit_Click;
            editCancel.Click += EditCancel_Click;
        }

        private void EditCancel_Click(object sender, EventArgs e)
        {
            OnBackPressed();
        }

        private void EditSubmit_Click(object sender, EventArgs e)
        {
            if(EditVerify())
            {
                currentEmp.shiftList.Clear();
                if (sunOpen.Text != "")
                {
                    Shift newShift = new Shift(currentEmp.employee, int.Parse(sunOpen.Text), int.Parse(sunClose.Text), DayOfWeek.Sunday);
                    currentEmp.shiftList.Add(newShift);
                }
                if (monOpen.Text != "")
                {
                    Shift newShift = new Shift(currentEmp.employee, int.Parse(monOpen.Text), int.Parse(monClose.Text), DayOfWeek.Monday);
                    currentEmp.shiftList.Add(newShift);
                }
                if (tueOpen.Text != "")
                {
                    Shift newShift = new Shift(currentEmp.employee, int.Parse(tueOpen.Text), int.Parse(tueClose.Text), DayOfWeek.Tuesday);
                    currentEmp.shiftList.Add(newShift);
                }
                if (wedOpen.Text != "")
                {
                    Shift newShift = new Shift(currentEmp.employee, int.Parse(wedOpen.Text), int.Parse(wedClose.Text), DayOfWeek.Wednesday);
                    currentEmp.shiftList.Add(newShift);
                }
                if (thuOpen.Text != "")
                {
                    Shift newShift = new Shift(currentEmp.employee, int.Parse(thuOpen.Text), int.Parse(thuClose.Text), DayOfWeek.Thursday);
                    currentEmp.shiftList.Add(newShift);
                }
                if (friOpen.Text != "")
                {
                    Shift newShift = new Shift(currentEmp.employee, int.Parse(friOpen.Text), int.Parse(friClose.Text), DayOfWeek.Friday);
                    currentEmp.shiftList.Add(newShift);
                }
                if (satOpen.Text != "")
                {
                    Shift newShift = new Shift(currentEmp.employee, int.Parse(satOpen.Text), int.Parse(satClose.Text), DayOfWeek.Saturday);
                    currentEmp.shiftList.Add(newShift);
                }
                weekModified = true;
                inEditor = false;
                LoadWeek(selected);
            }
            else
            {//Shifts must have a start and end time
                editSubmit.Text = "Please Complete Shifts!";
                editSubmitChanged = true;
            }
        }

        /// <summary>
        /// Since there isnt a set number of shifts an employee must work, all we have to check for is that if one of the shifts times are empty then both are
        /// </summary>
        /// <returns></returns>
        private bool EditVerify()
        {
            if (sunOpen.Text == "" && sunClose.Text != "" || sunOpen.Text != "" && sunClose.Text == "")
            {
                return false;
            }
            if (monOpen.Text == "" && monClose.Text != "" || monOpen.Text != "" && monClose.Text == "")
            {
                return false;
            }
            if (tueOpen.Text == "" && tueClose.Text != "" || tueOpen.Text != "" && tueClose.Text == "")
            {
                return false;
            }
            if (wedOpen.Text == "" && wedClose.Text != "" || wedOpen.Text != "" && wedClose.Text == "")
            {
                return false;
            }
            if (thuOpen.Text == "" && thuClose.Text != "" || thuOpen.Text != "" && thuClose.Text == "")
            {
                return false;
            }
            if (friOpen.Text == "" && friClose.Text != "" || friOpen.Text != "" && friClose.Text == "")
            {
                return false;
            }
            if (satOpen.Text == "" && satClose.Text != "" || satOpen.Text != "" && satClose.Text == "")
            {
                return false;
            }
            return true;
        }

        private void LoadWeekList()
        {
            SetContentView(Resource.Layout.ScheduleHistory);

            ListView historyView = FindViewById<ListView>(Resource.Id.historyListView);
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, ConvertToStringList());
            historyView.Adapter = adapter;

            historyView.ItemClick += HistoryView_ItemClick;//I gotta somehow stop this from being added multiple times
        }

        private void HistoryView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            selected = SystemSettings.weekList[e.Position];
            LoadWeek(selected);
        }

        private List<string> ConvertToStringList()
        {
            List<string> returnList = new List<string>();
            for(int i = 0; i < SystemSettings.weekList.Count; i++)
            {
                returnList.Add(SystemSettings.weekList[i].startDate.ToShortDateString());
            }
            return returnList;
        }
    }
}