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
    [Activity(Label = "Employee Management", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@android:style/Theme.Material", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class EmpMgmtActivity : Activity
    {
        private Button menu1, menu2, action1, action2;
        private ListView empListView;
        private EmpMgmtAdapter empListAdapter;
        private int selected;
        private bool inEditor = false, submitChanged = false;

        #region override
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //These two methods enable the back button
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            GenerateEmployeeListScreen();
        }

        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            if (submitChanged)
            {
                submit.Text = "Submit";
                submitChanged = false;
            }
            return base.DispatchTouchEvent(ev);
        }

        public override void OnBackPressed()
        {
            if(inEditor)
            {
                GenerateEmployeeListScreen();
                inEditor = false;
            }
            else
                base.OnBackPressed();
        }


        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
            selected = info.Position;

            menu.Add(0, 0, 0, "Edit");
            menu.Add(1, 1, 1, "Delete");
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            if(item.ItemId == 0)
            {
                OnEditEmployee(EmployeeStorage.employeeList[selected]);
            }
            else
            {//delete
                RemovalAlert();
            }
            Console.WriteLine(item.ItemId);
            return base.OnContextItemSelected(item);
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
                case Resource.Id.action_button1:
                    OnAddEmployee();
                    return true;
                case Resource.Id.action_button2:

                    return true;
                case Android.Resource.Id.Home:
                    OnBackPressed();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }
        #endregion override

        private void RemovalAlert()
        {
            new AlertDialog.Builder(this)
            .SetPositiveButton("Yes", (sender, args) =>
            {
                EmployeeStorage.RemoveEmployee(selected);
            })
            .SetNegativeButton("No", (sender, args) =>
            {
                //do nothing
            })
            .SetMessage("Are you sure you want to remove " + EmployeeStorage.employeeList[selected].empLastName + ", " + EmployeeStorage.employeeList[selected].empFirstName + " from the employee list?")
            .SetTitle("Remove Employee?")
            .Show();
        }

        private void GenerateEmployeeListScreen()
        {
            SetContentView(Resource.Layout.EmpMgmtLayout);
            empListView = FindViewById<ListView>(Resource.Id.EmpMgmtList);

            GenerateEmpList(EmployeeStorage.employeeList);

            RegisterForContextMenu(empListView);
        }

        EditText firstName, lastName, position, id;
        EditText sunOpen, sunClose, monOpen, monClose, tueOpen, tueClose, wedOpen, wedClose, thuOpen, thuClose, friOpen, friClose, satOpen, satClose;
        CheckBox sunToggle, monToggle, tueToggle, wedToggle, thuToggle, friToggle, satToggle;
        Button cancel, submit;

        public void OnAddEmployee()
        {
            inEditor = true;
            SetContentView(Resource.Layout.EmployeeView);

            cancel = FindViewById<Button>(Resource.Id.btnAvailCancel);
            submit = FindViewById<Button>(Resource.Id.btnAvailSubmit);
            firstName = FindViewById<EditText>(Resource.Id.empPageFirstName);
            firstName.Text = "";
            firstName.Focusable = true;
            lastName = FindViewById<EditText>(Resource.Id.empPageLastName);
            lastName.Focusable = true;
            lastName.Text = "";
            position = FindViewById<EditText>(Resource.Id.empPagePosition);
            position.Focusable = true;
            position.Text = "";
            id = FindViewById<EditText>(Resource.Id.empPageID);
            id.Focusable = true;

            sunOpen = FindViewById<EditText>(Resource.Id.inputAvailSunOpen);
            monOpen = FindViewById<EditText>(Resource.Id.inputAvailMonOpen);
            tueOpen = FindViewById<EditText>(Resource.Id.inputAvailTueOpen);
            wedOpen = FindViewById<EditText>(Resource.Id.inputAvailWedOpen);
            thuOpen = FindViewById<EditText>(Resource.Id.inputAvailThuOpen);
            friOpen = FindViewById<EditText>(Resource.Id.inputAvailFriOpen);
            satOpen = FindViewById<EditText>(Resource.Id.inputAvailSatOpen);
            sunClose = FindViewById<EditText>(Resource.Id.inputAvailSunClose);
            monClose = FindViewById<EditText>(Resource.Id.inputAvailMonClose);
            tueClose = FindViewById<EditText>(Resource.Id.inputAvailTueClose);
            wedClose = FindViewById<EditText>(Resource.Id.inputAvailWedClose);
            thuClose = FindViewById<EditText>(Resource.Id.inputAvailThuClose);
            friClose = FindViewById<EditText>(Resource.Id.inputAvailFriClose);
            satClose = FindViewById<EditText>(Resource.Id.inputAvailSatClose);
            sunToggle = FindViewById<CheckBox>(Resource.Id.chkAvailSunday);
            monToggle = FindViewById<CheckBox>(Resource.Id.chkAvailMonday);
            tueToggle = FindViewById<CheckBox>(Resource.Id.chkAvailTuesday);
            wedToggle = FindViewById<CheckBox>(Resource.Id.chkAvailWednesday);
            thuToggle = FindViewById<CheckBox>(Resource.Id.chkAvailThursday);
            friToggle = FindViewById<CheckBox>(Resource.Id.chkAvailFriday);
            satToggle = FindViewById<CheckBox>(Resource.Id.chkAvailSaturday);
            sunToggle.CheckedChange += Toggle_CheckedChange;
            monToggle.CheckedChange += Toggle_CheckedChange;
            tueToggle.CheckedChange += Toggle_CheckedChange;
            wedToggle.CheckedChange += Toggle_CheckedChange;
            thuToggle.CheckedChange += Toggle_CheckedChange;
            friToggle.CheckedChange += Toggle_CheckedChange;
            satToggle.CheckedChange += Toggle_CheckedChange;
            submit.Click += SubmitAdd_Click;
            cancel.Click += Cancel_Click;
        }

        public void OnEditEmployee(Employee emp)
        {
            inEditor = true;
            SetContentView(Resource.Layout.EmployeeView);

            cancel = FindViewById<Button>(Resource.Id.btnAvailCancel);
            submit = FindViewById<Button>(Resource.Id.btnAvailSubmit);
            firstName = FindViewById<EditText>(Resource.Id.empPageFirstName);
            firstName.Focusable = false;
            lastName = FindViewById<EditText>(Resource.Id.empPageLastName);
            lastName.Focusable = false;
            position = FindViewById<EditText>(Resource.Id.empPagePosition);
            position.Focusable = false;
            id = FindViewById<EditText>(Resource.Id.empPageID);
            id.Focusable = false;
            sunOpen = FindViewById<EditText>(Resource.Id.inputAvailSunOpen);
            monOpen = FindViewById<EditText>(Resource.Id.inputAvailMonOpen);
            tueOpen = FindViewById<EditText>(Resource.Id.inputAvailTueOpen);
            wedOpen = FindViewById<EditText>(Resource.Id.inputAvailWedOpen);
            thuOpen = FindViewById<EditText>(Resource.Id.inputAvailThuOpen);
            friOpen = FindViewById<EditText>(Resource.Id.inputAvailFriOpen);
            satOpen = FindViewById<EditText>(Resource.Id.inputAvailSatOpen);
            sunClose = FindViewById<EditText>(Resource.Id.inputAvailSunClose);
            monClose = FindViewById<EditText>(Resource.Id.inputAvailMonClose);
            tueClose = FindViewById<EditText>(Resource.Id.inputAvailTueClose);
            wedClose = FindViewById<EditText>(Resource.Id.inputAvailWedClose);
            thuClose = FindViewById<EditText>(Resource.Id.inputAvailThuClose);
            friClose = FindViewById<EditText>(Resource.Id.inputAvailFriClose);
            satClose = FindViewById<EditText>(Resource.Id.inputAvailSatClose);
            sunToggle = FindViewById<CheckBox>(Resource.Id.chkAvailSunday);
            monToggle = FindViewById<CheckBox>(Resource.Id.chkAvailMonday);
            tueToggle = FindViewById<CheckBox>(Resource.Id.chkAvailTuesday);
            wedToggle = FindViewById<CheckBox>(Resource.Id.chkAvailWednesday);
            thuToggle = FindViewById<CheckBox>(Resource.Id.chkAvailThursday);
            friToggle = FindViewById<CheckBox>(Resource.Id.chkAvailFriday);
            satToggle = FindViewById<CheckBox>(Resource.Id.chkAvailSaturday);
            FillData(emp);
            sunToggle.CheckedChange += Toggle_CheckedChange;
            monToggle.CheckedChange += Toggle_CheckedChange;
            tueToggle.CheckedChange += Toggle_CheckedChange;
            wedToggle.CheckedChange += Toggle_CheckedChange;
            thuToggle.CheckedChange += Toggle_CheckedChange;
            friToggle.CheckedChange += Toggle_CheckedChange;
            satToggle.CheckedChange += Toggle_CheckedChange;
            submit.Click += Submit_Click;
            cancel.Click += Cancel_Click;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            OnBackPressed();
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            Availability avail = new Availability();
            if(sunToggle.Checked)
            {
                avail.sunday.available = true;
                avail.sunday.startTime = int.Parse(sunOpen.Text);
                avail.sunday.endTime = int.Parse(sunClose.Text);
            }
            else
                avail.sunday.available = false;
            if (monToggle.Checked)
            {
                avail.monday.available = true;
                avail.monday.startTime = int.Parse(monOpen.Text);
                avail.monday.endTime = int.Parse(monClose.Text);
            }
            else
                avail.monday.available = false;
            if (tueToggle.Checked)
            {
                avail.tuesday.available = true;
                avail.tuesday.startTime = int.Parse(tueOpen.Text);
                avail.tuesday.endTime = int.Parse(tueClose.Text);
            }
            else
                avail.tuesday.available = false;
            if (wedToggle.Checked)
            {
                avail.wednesday.available = true;
                avail.wednesday.startTime = int.Parse(wedOpen.Text);
                avail.wednesday.endTime = int.Parse(wedClose.Text);
            }
            else
                avail.wednesday.available = false;
            if (thuToggle.Checked)
            {
                avail.thursday.available = true;
                avail.thursday.startTime = int.Parse(thuOpen.Text);
                avail.thursday.endTime = int.Parse(thuClose.Text);
            }
            else
                avail.thursday.available = false;
            if (friToggle.Checked)
            {
                avail.friday.available = true;
                avail.friday.startTime = int.Parse(friOpen.Text);
                avail.friday.endTime = int.Parse(friClose.Text);
            }
            else
                avail.friday.available = false;
            if (satToggle.Checked)
            {
                avail.saturday.available = true;
                avail.saturday.startTime = int.Parse(satOpen.Text);
                avail.saturday.endTime = int.Parse(satClose.Text);
            }
            else
                avail.saturday.available = false;

            EmployeeStorage.employeeList[selected].availability = avail;
            OnBackPressed();
        }

        private void SubmitAdd_Click(object sender, EventArgs e)
        {
            #region avail
            Availability avail = new Availability();
            if (sunToggle.Checked)
            {
                avail.sunday.available = true;
                avail.sunday.startTime = int.Parse(sunOpen.Text);
                avail.sunday.endTime = int.Parse(sunClose.Text);
            }
            else
                avail.sunday.available = false;
            if (monToggle.Checked)
            {
                avail.monday.available = true;
                avail.monday.startTime = int.Parse(monOpen.Text);
                avail.monday.endTime = int.Parse(monClose.Text);
            }
            else
                avail.monday.available = false;
            if (tueToggle.Checked)
            {
                avail.tuesday.available = true;
                avail.tuesday.startTime = int.Parse(tueOpen.Text);
                avail.tuesday.endTime = int.Parse(tueClose.Text);
            }
            else
                avail.tuesday.available = false;
            if (wedToggle.Checked)
            {
                avail.wednesday.available = true;
                avail.wednesday.startTime = int.Parse(wedOpen.Text);
                avail.wednesday.endTime = int.Parse(wedClose.Text);
            }
            else
                avail.wednesday.available = false;
            if (thuToggle.Checked)
            {
                avail.thursday.available = true;
                avail.thursday.startTime = int.Parse(thuOpen.Text);
                avail.thursday.endTime = int.Parse(thuClose.Text);
            }
            else
                avail.thursday.available = false;
            if (friToggle.Checked)
            {
                avail.friday.available = true;
                avail.friday.startTime = int.Parse(friOpen.Text);
                avail.friday.endTime = int.Parse(friClose.Text);
            }
            else
                avail.friday.available = false;
            if (satToggle.Checked)
            {
                avail.saturday.available = true;
                avail.saturday.startTime = int.Parse(satOpen.Text);
                avail.saturday.endTime = int.Parse(satClose.Text);
            }
            else
                avail.saturday.available = false;
            #endregion avail

        }

        private bool FieldValidation()
        {
            if (sunToggle.Checked)
            {
                if(sunOpen.Text == "" || sunClose.Text == "")
                {
                    ValidationFailure();
                    return false;
                }
            }
            if (monToggle.Checked)
            {
                if (monOpen.Text == "" || monClose.Text == "")
                {
                    ValidationFailure();
                    return false;
                }
            }
            if (tueToggle.Checked)
            {
                if (tueOpen.Text == "" || tueClose.Text == "")
                {
                    ValidationFailure();
                    return false;
                }
            }
            if (wedToggle.Checked)
            {
                if (wedOpen.Text == "" || wedClose.Text == "")
                {
                    ValidationFailure();
                    return false;
                }
            }
            if (thuToggle.Checked)
            {
                if (thuOpen.Text == "" || thuClose.Text == "")
                {
                    ValidationFailure();
                    return false;
                }
            }
            if (friToggle.Checked)
            {
                if (friOpen.Text == "" || friClose.Text == "")
                {
                    ValidationFailure();
                    return false;
                }
            }
            if (satToggle.Checked)
            {
                if (satOpen.Text == "" || satClose.Text == "")
                {
                    ValidationFailure();
                    return false;
                }
            }
            return true;
        }

        private void ValidationFailure()
        {
            submit.Text = "Fill out all fields!";
            submitChanged = true;
        }

        /// <summary>
        /// This catches all events from the toggles and enables or disabled the respective edittext boxes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Toggle_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            CheckBox obj = (CheckBox)sender;
            switch(obj.Text)
            {
                case "Sunday":
                    if(!sunToggle.Checked)
                    {
                        sunOpen.Enabled = false;
                        sunClose.Enabled = false;
                    }
                    else
                    {
                        sunOpen.Enabled = true;
                        sunClose.Enabled = true;
                    }
                    break;
                case "Monday":
                    if (!monToggle.Checked)
                    {
                        monOpen.Enabled = false;
                        monClose.Enabled = false;
                    }
                    else
                    {
                        monOpen.Enabled = true;
                        monClose.Enabled = true;
                    }
                    break;
                case "Tuesday":
                    if (!tueToggle.Checked)
                    {
                        tueOpen.Enabled = false;
                        tueClose.Enabled = false;
                    }
                    else
                    {
                        tueOpen.Enabled = true;
                        tueClose.Enabled = true;
                    }
                    break;
                case "Wednesday":
                    if (!wedToggle.Checked)
                    {
                        wedOpen.Enabled = false;
                        wedClose.Enabled = false;
                    }
                    else
                    {
                        wedOpen.Enabled = true;
                        wedClose.Enabled = true;
                    }
                    break;
                case "Thursday":
                    if (!thuToggle.Checked)
                    {
                        thuOpen.Enabled = false;
                        thuClose.Enabled = false;
                    }
                    else
                    {
                        thuOpen.Enabled = true;
                        thuClose.Enabled = true;
                    }
                    break;
                case "Friday":
                    if (!friToggle.Checked)
                    {
                        friOpen.Enabled = false;
                        friClose.Enabled = false;
                    }
                    else
                    {
                        friOpen.Enabled = true;
                        friClose.Enabled = true;
                    }
                    break;
                case "Saturday":
                    if (!satToggle.Checked)
                    {
                        satOpen.Enabled = false;
                        satClose.Enabled = false;
                    }
                    else
                    {
                        satOpen.Enabled = true;
                        satClose.Enabled = true;
                    }
                    break;
            }
        }

        /// <summary>
        /// This populates the screen with existing employee data.
        /// </summary>
        /// <param name="emp"></param>
        private void FillData(Employee emp)
        {
            firstName.Text = emp.empFirstName;
            lastName.Text = emp.empLastName;
            position.Text = SystemSettings.positionList[emp.position];
            id.Text = emp.empID.ToString();
            sunToggle.Checked = emp.availability.sunday.available;
            monToggle.Checked = emp.availability.monday.available;
            tueToggle.Checked = emp.availability.tuesday.available;
            wedToggle.Checked = emp.availability.wednesday.available;
            thuToggle.Checked = emp.availability.thursday.available;
            friToggle.Checked = emp.availability.friday.available;
            satToggle.Checked = emp.availability.saturday.available;
            if (sunToggle.Checked)
            {
                sunOpen.Text = emp.availability.sunday.startTime.ToString();
                sunClose.Text = emp.availability.sunday.endTime.ToString();
            }
            else
            {
                sunOpen.Text = "";
                sunClose.Text = "";
            }
            if (monToggle.Checked)
            {
                monOpen.Text = emp.availability.monday.startTime.ToString();
                monClose.Text = emp.availability.monday.endTime.ToString();
            }
            else
            {
                monOpen.Text = "";
                monClose.Text = "";
            }
            if (tueToggle.Checked)
            {
                tueOpen.Text = emp.availability.tuesday.startTime.ToString();
                tueClose.Text = emp.availability.tuesday.endTime.ToString();
            }
            else
            {
                tueOpen.Text = "";
                tueClose.Text = "";
            }
            if (wedToggle.Checked)
            {
                wedOpen.Text = emp.availability.wednesday.startTime.ToString();
                wedClose.Text = emp.availability.wednesday.endTime.ToString();
            }
            else
            {
                wedOpen.Text = "";
                wedClose.Text = "";
            }
            if (thuToggle.Checked)
            {
                thuOpen.Text = emp.availability.thursday.startTime.ToString();
                thuClose.Text = emp.availability.thursday.endTime.ToString();
            }
            else
            {
                thuOpen.Text = "";
                thuClose.Text = "";
            }
            if (friToggle.Checked)
            {
                friOpen.Text = emp.availability.friday.startTime.ToString();
                friClose.Text = emp.availability.friday.endTime.ToString();
            }
            else
            {
                friOpen.Text = "";
                friClose.Text = "";
            }
            if (satToggle.Checked)
            {
                satOpen.Text = emp.availability.saturday.startTime.ToString();
                satClose.Text = emp.availability.saturday.endTime.ToString();
            }
            else
            {
                satOpen.Text = "";
                satClose.Text = "";
            }
        }

        public void GenerateEmpList(List<Employee> empList)
        {
            if (empList.Count > 0)
            {
                empListAdapter = new EmpMgmtAdapter(this, empList);

                empListView.Adapter = empListAdapter;
            }
        }
    }
}