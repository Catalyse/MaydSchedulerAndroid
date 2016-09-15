using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace MaydSchedulerApp
{
    [Activity(Label = "Employee Management", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, Theme = "@android:style/Theme.Material", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class EmpMgmtActivity : Activity
    {
        private Button action1, action2;
        private ListView empListView;
        private EmpMgmtAdapter empListAdapter;
        private int selected;
        private bool inEditor = false, submitChanged = false;
        private bool schedulerPassoff = false;

        #region override
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //These two methods enable the back button
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            if (MainActivity.scheduler.availPassoff)
            {
                schedulerPassoff = true;
                OnEditEmployee(MainActivity.scheduler.employee);
            }
            else
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
                InputMethodManager imm = (InputMethodManager)GetSystemService(Context.InputMethodService);
                imm.HideSoftInputFromWindow(MainActivity.currentActivity.CurrentFocus.WindowToken, HideSoftInputFlags.None);
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

        /// <summary>
        /// This confirms they want to remove an employee
        /// </summary>
        private void RemovalAlert()
        {
            new AlertDialog.Builder(this)
            .SetPositiveButton("Yes", (sender, args) =>
            {
                EmployeeStorage.RemoveEmployee(selected);
                GenerateEmployeeListScreen();
            })
            .SetNegativeButton("No", (sender, args) =>
            {
                //do nothing
            })
            .SetMessage("Are you sure you want to remove " + EmployeeStorage.employeeList[selected].empLastName + ", " + EmployeeStorage.employeeList[selected].empFirstName + " from the employee list?")
            .SetTitle("Remove Employee?")
            .Show();
        }

        /// <summary>
        /// Generates the employee list view
        /// </summary>
        private void GenerateEmployeeListScreen()
        {
            SetContentView(Resource.Layout.EmpMgmtLayout);
            empListView = FindViewById<ListView>(Resource.Id.EmpMgmtList);

            GenerateEmpList(EmployeeStorage.employeeList);

            RegisterForContextMenu(empListView);
        }

        EditText firstName, lastName, id;
        Spinner position;
        EditText sunOpen, sunClose, monOpen, monClose, tueOpen, tueClose, wedOpen, wedClose, thuOpen, thuClose, friOpen, friClose, satOpen, satClose, skillLevel, shiftPref;
        CheckBox sunToggle, monToggle, tueToggle, wedToggle, thuToggle, friToggle, satToggle, fullTime, overTime;
        CheckBox openSun, openMon, openTue, openWed, openThu, openFri, openSat;
        Button cancel, submit;

        /// <summary>
        /// This sets up the employee view when making a new employee
        /// </summary>
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
            position = FindViewById<Spinner>(Resource.Id.empPagePosition);
            position.Adapter = FillSpinner();
            position.Focusable = true;
            position.SetSelection(0);
            id = FindViewById<EditText>(Resource.Id.empPageID);
            id.Focusable = true;
            id.Text = "";
            fullTime = FindViewById<CheckBox>(Resource.Id.chkFullTime);
            fullTime.Checked = true;
            fullTime.Enabled = true;
            overTime = FindViewById<CheckBox>(Resource.Id.chkOvertime);
            overTime.Checked = false;
            overTime.Enabled = true;
            skillLevel = FindViewById<EditText>(Resource.Id.inputSkillLevel);
            skillLevel.Text = "";
            skillLevel.Focusable = true;
            shiftPref = FindViewById<EditText>(Resource.Id.inputShiftPref);
            shiftPref.Text = "";
            shiftPref.Focusable = true;
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
            openSun = FindViewById<CheckBox>(Resource.Id.chkOpenAvailSunday);
            openMon = FindViewById<CheckBox>(Resource.Id.chkOpenAvailMonday);
            openTue = FindViewById<CheckBox>(Resource.Id.chkOpenAvailTuesday);
            openWed = FindViewById<CheckBox>(Resource.Id.chkOpenAvailWednesday);
            openThu = FindViewById<CheckBox>(Resource.Id.chkOpenAvailThursday);
            openFri = FindViewById<CheckBox>(Resource.Id.chkOpenAvailFriday);
            openSat = FindViewById<CheckBox>(Resource.Id.chkOpenAvailSaturday);
            sunToggle.CheckedChange += Toggle_CheckedChange;
            monToggle.CheckedChange += Toggle_CheckedChange;
            tueToggle.CheckedChange += Toggle_CheckedChange;
            wedToggle.CheckedChange += Toggle_CheckedChange;
            thuToggle.CheckedChange += Toggle_CheckedChange;
            friToggle.CheckedChange += Toggle_CheckedChange;
            satToggle.CheckedChange += Toggle_CheckedChange;
            openSun.CheckedChange += Toggle_CheckedChange;
            openMon.CheckedChange += Toggle_CheckedChange;
            openTue.CheckedChange += Toggle_CheckedChange;
            openWed.CheckedChange += Toggle_CheckedChange;
            openThu.CheckedChange += Toggle_CheckedChange;
            openFri.CheckedChange += Toggle_CheckedChange;
            openSat.CheckedChange += Toggle_CheckedChange;
            submit.Click += SubmitAdd_Click;
            cancel.Click += Cancel_Click;
        }

        /// <summary>
        /// This takes a target int and converts it to an employee for the main method
        /// </summary>
        /// <param name="emp"></param>
        public void OnEditEmployee(EmployeeScheduleWrapper emp)
        {
            OnEditEmployee(emp.empReference);
        }

        /// <summary>
        /// THis sets up the employee view when editing an existing employee
        /// </summary>
        /// <param name="emp"></param>
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
            position = FindViewById<Spinner>(Resource.Id.empPagePosition);
            position.Adapter = FillSpinner();
            position.Focusable = false;
            position.SetSelection(emp.position+1);
            position.Enabled = false;
            id = FindViewById<EditText>(Resource.Id.empPageID);
            id.Focusable = false;
            fullTime = FindViewById<CheckBox>(Resource.Id.chkFullTime);
            fullTime.Checked = emp.fullTime;
            fullTime.Enabled = false;
            overTime = FindViewById<CheckBox>(Resource.Id.chkOvertime);
            overTime.Checked = emp.overtimeAllowed;
            overTime.Enabled = false;
            skillLevel = FindViewById<EditText>(Resource.Id.inputSkillLevel);
            skillLevel.Text = emp.skillLevel.ToString();
            skillLevel.Focusable = false;
            shiftPref = FindViewById<EditText>(Resource.Id.inputShiftPref);
            shiftPref.Text = emp.shiftPreference.ToString();
            shiftPref.Focusable = false;
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
            openSun = FindViewById<CheckBox>(Resource.Id.chkOpenAvailSunday);
            openMon = FindViewById<CheckBox>(Resource.Id.chkOpenAvailMonday);
            openTue = FindViewById<CheckBox>(Resource.Id.chkOpenAvailTuesday);
            openWed = FindViewById<CheckBox>(Resource.Id.chkOpenAvailWednesday);
            openThu = FindViewById<CheckBox>(Resource.Id.chkOpenAvailThursday);
            openFri = FindViewById<CheckBox>(Resource.Id.chkOpenAvailFriday);
            openSat = FindViewById<CheckBox>(Resource.Id.chkOpenAvailSaturday);
            FillData(emp);
            sunToggle.CheckedChange += Toggle_CheckedChange;
            monToggle.CheckedChange += Toggle_CheckedChange;
            tueToggle.CheckedChange += Toggle_CheckedChange;
            wedToggle.CheckedChange += Toggle_CheckedChange;
            thuToggle.CheckedChange += Toggle_CheckedChange;
            friToggle.CheckedChange += Toggle_CheckedChange;
            satToggle.CheckedChange += Toggle_CheckedChange;
            openSun.CheckedChange += Toggle_CheckedChange;
            openMon.CheckedChange += Toggle_CheckedChange;
            openTue.CheckedChange += Toggle_CheckedChange;
            openWed.CheckedChange += Toggle_CheckedChange;
            openThu.CheckedChange += Toggle_CheckedChange;
            openFri.CheckedChange += Toggle_CheckedChange;
            openSat.CheckedChange += Toggle_CheckedChange;
            submit.Click += Submit_Click;
            cancel.Click += Cancel_Click;
        }

        /// <summary>
        /// This method exits the employee view screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Cancel_Click(object sender, EventArgs e)
        {
            OnBackPressed();
        }

        /// <summary>
        /// This method modified the availability from the edit screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Submit_Click(object sender, EventArgs e)
        {
            if (!FieldValidation())
                return;
            Availability avail = GenerateAvailability();

            if (schedulerPassoff)
            {
                MainActivity.scheduler.AvailabilitySubmit(avail);
                Finish();
            }
            else
            {
                EmployeeStorage.employeeList[selected].availability = avail;
                OnBackPressed();
            }
        }

        /// <summary>
        /// This method generates a new employee type to be stored
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SubmitAdd_Click(object sender, EventArgs e)
        {
            if (!FieldValidation())
                return;

            Availability avail = GenerateAvailability();

            Employee newEmp = new Employee();
            newEmp.availability = avail;
            newEmp.empFirstName = firstName.Text;
            newEmp.empLastName = lastName.Text;
            newEmp.position = position.SelectedItemPosition-1;
            newEmp.empID = int.Parse(id.Text);
            newEmp.overtimeAllowed = overTime.Checked;
            newEmp.fullTime = fullTime.Checked;
            newEmp.shiftPreference = int.Parse(shiftPref.Text);
            newEmp.skillLevel = int.Parse(skillLevel.Text);
            newEmp.active = true;
            EmployeeStorage.AddEmployee(newEmp);
            OnBackPressed();
        }

        /// <summary>
        /// This method generates the availability type when submitting changes
        /// </summary>
        /// <returns></returns>
        private Availability GenerateAvailability()
        {
            Availability avail;
            if ((sunToggle.Checked && openSun.Checked) && (monToggle.Checked && openMon.Checked) && (tueToggle.Checked && openTue.Checked) && (wedToggle.Checked && openWed.Checked)
                && (thuToggle.Checked && openThu.Checked) && (friToggle.Checked && openFri.Checked) && (satToggle.Checked && openSat.Checked))
                avail = new Availability(true);
            else
            {
                avail = new Availability(false);

                if (sunToggle.Checked)
                {
                    if (openSun.Checked)
                    {
                        avail.sunday.openAvail = true;
                        avail.sunday.available = true;
                    }
                    else
                    {
                        avail.sunday.openAvail = false;
                        avail.sunday.available = true;
                        avail.sunday.startTime = int.Parse(sunOpen.Text);
                        avail.sunday.endTime = int.Parse(sunClose.Text);
                    }
                }
                else
                    avail.sunday = new Day(false);
                if (monToggle.Checked)
                {
                    if (openMon.Checked)
                    {
                        avail.monday.openAvail = true;
                        avail.monday.available = true;
                    }
                    else
                    {
                        avail.monday.openAvail = false;
                        avail.monday.available = true;
                        avail.monday.startTime = int.Parse(monOpen.Text);
                        avail.monday.endTime = int.Parse(monClose.Text);
                    }
                }
                else
                    avail.monday = new Day(false);
                if (tueToggle.Checked)
                {
                    if (openTue.Checked)
                    {
                        avail.tuesday.openAvail = true;
                        avail.tuesday.available = true;
                    }
                    else
                    {
                        avail.tuesday.openAvail = false;
                        avail.tuesday.available = true;
                        avail.tuesday.startTime = int.Parse(tueOpen.Text);
                        avail.tuesday.endTime = int.Parse(tueClose.Text);
                    }
                }
                else
                    avail.tuesday = new Day(false);
                if (wedToggle.Checked)
                {
                    if (openWed.Checked)
                    {
                        avail.wednesday.openAvail = true;
                        avail.wednesday.available = true;
                    }
                    else
                    {
                        avail.wednesday.openAvail = false;
                        avail.wednesday.available = true;
                        avail.wednesday.startTime = int.Parse(wedOpen.Text);
                        avail.wednesday.endTime = int.Parse(wedClose.Text);
                    }
                }
                else
                    avail.wednesday = new Day(false);
                if (thuToggle.Checked)
                {
                    if (openThu.Checked)
                    {
                        avail.thursday.openAvail = true;
                        avail.thursday.available = true;
                    }
                    else
                    {
                        avail.thursday.openAvail = false;
                        avail.thursday.available = true;
                        avail.thursday.startTime = int.Parse(thuOpen.Text);
                        avail.thursday.endTime = int.Parse(thuClose.Text);
                    }
                }
                else
                    avail.thursday = new Day(false);
                if (friToggle.Checked)
                {
                    if (openFri.Checked)
                    {
                        avail.friday.openAvail = true;
                        avail.friday.available = true;
                    }
                    else
                    {
                        avail.friday.openAvail = false;
                        avail.friday.available = true;
                        avail.friday.startTime = int.Parse(friOpen.Text);
                        avail.friday.endTime = int.Parse(friClose.Text);
                    }
                }
                else
                    avail.friday = new Day(false);
                if (satToggle.Checked)
                {
                    if (openSat.Checked)
                        avail.saturday.openAvail = true;
                    else
                    {
                        avail.saturday.openAvail = false;
                        avail.saturday.available = true;
                        avail.saturday.startTime = int.Parse(satOpen.Text);
                        avail.saturday.endTime = int.Parse(satClose.Text);
                    }
                }
                else
                    avail.saturday = new Day(false);
            }
            return avail;
        }

        /// <summary>
        /// This fills the employee view spinner from the position list
        /// </summary>
        /// <returns></returns>
        private ArrayAdapter<string> FillSpinner()
        {
            List<string> posList = new List<string>();
            posList.Add("Select One");
            posList.AddRange(SystemSettings.positionList.Values);
            var adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerItem, posList);
            adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            return adapter;
        }

        /// <summary>
        /// This validates that all fields are filled out properly
        /// </summary>
        /// <returns></returns>
        private bool FieldValidation()
        {
            if (firstName.Text == "" || lastName.Text == "" || position.SelectedItemPosition == 0 || id.Text == "" || shiftPref.Text == "" || skillLevel.Text == "")
            {
                ValidationFailure();
                return false;
            }
            if (!sunToggle.Checked && !monToggle.Checked && !tueToggle.Checked && !wedToggle.Checked && !thuToggle.Checked && !friToggle.Checked && !satToggle.Checked)
            {
                ValidationFailure();
                return false;
            }
            if (sunToggle.Checked)
            {
                if((sunOpen.Text == "" || sunClose.Text == "") && !openSun.Checked)
                {
                    ValidationFailure();
                    return false;
                }
            }
            if (monToggle.Checked)
            {
                if ((monOpen.Text == "" || monClose.Text == "") && !openMon.Checked)
                {
                    ValidationFailure();
                    return false;
                }
            }
            if (tueToggle.Checked)
            {
                if ((tueOpen.Text == "" || tueClose.Text == "") && !openTue.Checked)
                {
                    ValidationFailure();
                    return false;
                }
            }
            if (wedToggle.Checked)
            {
                if ((wedOpen.Text == "" || wedClose.Text == "") && !openWed.Checked)
                {
                    ValidationFailure();
                    return false;
                }
            }
            if (thuToggle.Checked)
            {
                if ((thuOpen.Text == "" || thuClose.Text == "") && !openThu.Checked)
                {
                    ValidationFailure();
                    return false;
                }
            }
            if (friToggle.Checked)
            {
                if ((friOpen.Text == "" || friClose.Text == "") && !openFri.Checked)
                {
                    ValidationFailure();
                    return false;
                }
            }
            if (satToggle.Checked)
            {
                if ((satOpen.Text == "" || satClose.Text == "") && !openSun.Checked)
                {
                    ValidationFailure();
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// If there is a field that is not properly filled out this is called and submitting is prevented
        /// </summary>
        private void ValidationFailure()
        {
            submit.Text = "Fill out all fields!";
            submitChanged = true;
        }

        /// <summary>
        /// This catches all events from the toggles and enables or disabled the respective edittext boxes
        /// Also: Sorry this method sucks but I had no choice
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Toggle_CheckedChange(object sender, CompoundButton.CheckedChangeEventArgs e)
        {
            CheckBox obj = (CheckBox)sender;
            if (obj.Id == sunToggle.Id)
            {
                if (!sunToggle.Checked)
                {
                    openSun.Enabled = false;
                    sunOpen.Enabled = false;
                    sunClose.Enabled = false;
                    sunOpen.Text = "Off";
                    sunClose.Text = "Off";
                }
                else if (openSun.Checked)
                {
                    openSun.Enabled = true;
                    sunOpen.Enabled = false;
                    sunClose.Enabled = false;
                    sunOpen.Text = "Open";
                    sunClose.Text = "Open";
                }
                else
                {
                    openSun.Enabled = true;
                    sunOpen.Enabled = true;
                    sunClose.Enabled = true;
                }
            }
            else if (obj.Id == monToggle.Id)
            {
                if (!monToggle.Checked)
                {
                    openMon.Enabled = false;
                    monOpen.Enabled = false;
                    monClose.Enabled = false;
                    monOpen.Text = "Off";
                    monClose.Text = "Off";
                }
                else if (openMon.Checked)
                {
                    openMon.Enabled = true;
                    monOpen.Enabled = false;
                    monClose.Enabled = false;
                    monOpen.Text = "Open";
                    monClose.Text = "Open";
                }
                else
                {
                    openMon.Enabled = true;
                    monOpen.Enabled = true;
                    monClose.Enabled = true;
                }
            }
            else if (obj.Id == tueToggle.Id)
            {
                if (!tueToggle.Checked)
                {
                    openTue.Enabled = false;
                    tueOpen.Enabled = false;
                    tueClose.Enabled = false;
                    tueOpen.Text = "Off";
                    tueClose.Text = "Off";
                }
                else if (openTue.Checked)
                {
                    openTue.Enabled = true;
                    tueOpen.Enabled = false;
                    tueClose.Enabled = false;
                    tueOpen.Text = "Open";
                    tueClose.Text = "Open";
                }
                else
                {
                    openTue.Enabled = true;
                    tueOpen.Enabled = true;
                    tueClose.Enabled = true;
                }
            }
            else if (obj.Id == wedToggle.Id)
            {
                if (!wedToggle.Checked)
                {
                    openWed.Enabled = false;
                    wedOpen.Enabled = false;
                    wedClose.Enabled = false;
                    wedOpen.Text = "Off";
                    wedClose.Text = "Off";
                }
                else if (openWed.Checked)
                {
                    openWed.Enabled = true;
                    wedOpen.Enabled = false;
                    wedClose.Enabled = false;
                    wedOpen.Text = "Open";
                    wedClose.Text = "Open";
                }
                else
                {
                    openWed.Enabled = true;
                    wedOpen.Enabled = true;
                    wedClose.Enabled = true;
                }
            }
            else if (obj.Id == thuToggle.Id)
            {
                if (!thuToggle.Checked)
                {
                    openThu.Enabled = false;
                    thuOpen.Enabled = false;
                    thuClose.Enabled = false;
                    thuOpen.Text = "Off";
                    thuClose.Text = "Off";
                }
                else if (openThu.Checked)
                {
                    openThu.Enabled = true;
                    thuOpen.Enabled = false;
                    thuClose.Enabled = false;
                    thuOpen.Text = "Open";
                    thuClose.Text = "Open";
                }
                else
                {
                    openThu.Enabled = true;
                    thuOpen.Enabled = true;
                    thuClose.Enabled = true;
                }
            }
            else if (obj.Id == friToggle.Id)
            {
                if (!friToggle.Checked)
                {
                    openFri.Enabled = false;
                    friOpen.Enabled = false;
                    friClose.Enabled = false;
                    friOpen.Text = "Off";
                    friClose.Text = "Off";
                }
                else if (openFri.Checked)
                {
                    openFri.Enabled = true;
                    friOpen.Enabled = false;
                    friClose.Enabled = false;
                    friOpen.Text = "Open";
                    friClose.Text = "Open";
                }
                else
                {
                    openFri.Enabled = true;
                    friOpen.Enabled = true;
                    friClose.Enabled = true;
                }
            }
            else if (obj.Id == satToggle.Id)
            {
                if (!satToggle.Checked)
                {
                    openSat.Enabled = false;
                    satOpen.Enabled = false;
                    satClose.Enabled = false;
                    satOpen.Text = "Off";
                    satClose.Text = "Off";
                }
                else if (openSat.Checked)
                {
                    openSat.Enabled = true;
                    satOpen.Enabled = false;
                    satClose.Enabled = false;
                    satOpen.Text = "Open";
                    satClose.Text = "Open";
                }
                else
                {
                    openSat.Enabled = true;
                    satOpen.Enabled = true;
                    satClose.Enabled = true;
                }
            }
            //Open availability check
            else if (obj.Id == openSun.Id)
            {
                if (openSun.Checked)
                {
                    sunOpen.Enabled = false;
                    sunClose.Enabled = false;
                    sunOpen.Text = "Open";
                    sunClose.Text = "Open";
                }
                else
                {
                    sunOpen.Enabled = true;
                    sunClose.Enabled = true;
                    sunOpen.Text = "Off";
                    sunClose.Text = "Off";
                }
            }
            else if (obj.Id == openMon.Id)
            {
                if (openMon.Checked)
                {
                    monOpen.Enabled = false;
                    monClose.Enabled = false;
                    monOpen.Text = "Open";
                    monClose.Text = "Open";
                }
                else
                {
                    monOpen.Enabled = true;
                    monClose.Enabled = true;
                    monOpen.Text = "Off";
                    monClose.Text = "Off";
                }
            }
            else if (obj.Id == openTue.Id)
            {
                if (openTue.Checked)
                {
                    tueOpen.Enabled = false;
                    tueClose.Enabled = false;
                    tueOpen.Text = "Open";
                    tueClose.Text = "Open";
                }
                else
                {
                    tueOpen.Enabled = true;
                    tueClose.Enabled = true;
                    tueOpen.Text = "Off";
                    tueClose.Text = "Off";
                }
            }
            else if (obj.Id == openWed.Id)
            {
                if (openWed.Checked)
                {
                    wedOpen.Enabled = false;
                    wedClose.Enabled = false;
                    wedOpen.Text = "Open";
                    wedClose.Text = "Open";
                }
                else
                {
                    wedOpen.Enabled = true;
                    wedClose.Enabled = true;
                    wedOpen.Text = "Off";
                    wedClose.Text = "Off";
                }
            }
            else if (obj.Id == openThu.Id)
            {
                if (openThu.Checked)
                {
                    thuOpen.Enabled = false;
                    thuClose.Enabled = false;
                    thuOpen.Text = "Open";
                    thuClose.Text = "Open";
                }
                else
                {
                    thuOpen.Enabled = true;
                    thuClose.Enabled = true;
                    thuOpen.Text = "Off";
                    thuClose.Text = "Off";
                }
            }
            else if (obj.Id == openFri.Id)
            {
                if (openFri.Checked)
                {
                    friOpen.Enabled = false;
                    friClose.Enabled = false;
                    friOpen.Text = "Open";
                    friClose.Text = "Open";
                }
                else
                {
                    friOpen.Enabled = true;
                    friClose.Enabled = true;
                    friOpen.Text = "Off";
                    friClose.Text = "Off";
                }
            }
            else if (obj.Id == openSat.Id)
            {
                if (openSat.Checked)
                {
                    satOpen.Enabled = false;
                    satClose.Enabled = false;
                    satOpen.Text = "Open";
                    satClose.Text = "Open";
                }
                else
                {
                    satOpen.Enabled = true;
                    satClose.Enabled = true;
                    satOpen.Text = "Off";
                    satClose.Text = "Off";
                }
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
            position.SetSelection(emp.position+1);
            id.Text = emp.empID.ToString();
            if (emp.availability.OpenAvailability)
            {
                sunToggle.Checked = true;
                monToggle.Checked = true;
                tueToggle.Checked = true;
                wedToggle.Checked = true;
                thuToggle.Checked = true;
                friToggle.Checked = true;
                satToggle.Checked = true;
                openSun.Checked = true;
                openMon.Checked = true;
                openTue.Checked = true;
                openWed.Checked = true;
                openThu.Checked = true;
                openFri.Checked = true;
                openSat.Checked = true;
                sunOpen.Text = "";
                sunClose.Text = "";
                sunOpen.Hint = "Open";
                sunClose.Hint = "Open";
                sunOpen.Enabled = false;
                sunClose.Enabled = false;
                monOpen.Text = "";
                monClose.Text = "";
                monOpen.Hint = "Open";
                monClose.Hint = "Open";
                monOpen.Enabled = false;
                monClose.Enabled = false;
                tueOpen.Text = "";
                tueClose.Text = "";
                tueOpen.Hint = "Open";
                tueClose.Hint = "Open";
                tueOpen.Enabled = false;
                tueClose.Enabled = false;
                wedOpen.Text = "";
                wedClose.Text = "";
                wedOpen.Hint = "Open";
                wedClose.Hint = "Open";
                wedOpen.Enabled = false;
                wedClose.Enabled = false;
                thuOpen.Text = "";
                thuClose.Text = "";
                thuOpen.Hint = "Open";
                thuClose.Hint = "Open";
                thuOpen.Enabled = false;
                thuClose.Enabled = false;
                friOpen.Text = "";
                friClose.Text = "";
                friOpen.Hint = "Open";
                friClose.Hint = "Open";
                friOpen.Enabled = false;
                friClose.Enabled = false;
                satOpen.Text = "";
                satClose.Text = "";
                satOpen.Hint = "Open";
                satClose.Hint = "Open";
                satOpen.Enabled = false;
                satClose.Enabled = false;
            }
            else
            {
                sunToggle.Checked = emp.availability.sunday.available;
                monToggle.Checked = emp.availability.monday.available;
                tueToggle.Checked = emp.availability.tuesday.available;
                wedToggle.Checked = emp.availability.wednesday.available;
                thuToggle.Checked = emp.availability.thursday.available;
                friToggle.Checked = emp.availability.friday.available;
                satToggle.Checked = emp.availability.saturday.available;
                openSun.Checked = emp.availability.sunday.openAvail;
                openMon.Checked = emp.availability.monday.openAvail;
                openTue.Checked = emp.availability.tuesday.openAvail;
                openWed.Checked = emp.availability.wednesday.openAvail;
                openThu.Checked = emp.availability.thursday.openAvail;
                openFri.Checked = emp.availability.friday.openAvail;
                openSat.Checked = emp.availability.saturday.openAvail;
                //If the open avail is checked then so is the toggle for being able to work that day.
                //FIXTHIS//This section can get rid of the elses if you clear out all the text before getting to the assignment(eg sunOpen.Text is blank when findviewbyid);
                if (openSun.Checked)
                {
                    sunOpen.Text = "";
                    sunClose.Text = "";
                    sunOpen.Hint = "Open";
                    sunClose.Hint = "Open";
                    sunOpen.Enabled = false;
                    sunClose.Enabled = false;
                }
                else if (sunToggle.Checked)
                {
                    sunOpen.Text = emp.availability.sunday.startTime.ToString();
                    sunClose.Text = emp.availability.sunday.endTime.ToString();
                    sunOpen.Enabled = true;
                    sunClose.Enabled = true;
                }
                else
                {
                    sunOpen.Text = "";
                    sunClose.Text = "";
                }
                if (openMon.Checked)
                {
                    monOpen.Text = "";
                    monClose.Text = "";
                    monOpen.Hint = "Open";
                    monClose.Hint = "Open";
                    monOpen.Enabled = false;
                    monClose.Enabled = false;
                }
                else if (monToggle.Checked)
                {
                    monOpen.Text = emp.availability.monday.startTime.ToString();
                    monClose.Text = emp.availability.monday.endTime.ToString();
                    monOpen.Enabled = true;
                    monClose.Enabled = true;
                }
                else
                {
                    monOpen.Text = "";
                    monClose.Text = "";
                }
                if (openTue.Checked)
                {
                    tueOpen.Text = "";
                    tueClose.Text = "";
                    tueOpen.Hint = "Open";
                    tueClose.Hint = "Open";
                    tueOpen.Enabled = false;
                    tueClose.Enabled = false;
                }
                else if (tueToggle.Checked)
                {
                    tueOpen.Text = emp.availability.tuesday.startTime.ToString();
                    tueClose.Text = emp.availability.tuesday.endTime.ToString();
                    tueOpen.Enabled = true;
                    tueClose.Enabled = true;
                }
                else
                {
                    tueOpen.Text = "";
                    tueClose.Text = "";
                }
                if (openWed.Checked)
                {
                    wedOpen.Text = "";
                    wedClose.Text = "";
                    wedOpen.Hint = "Open";
                    wedClose.Hint = "Open";
                    wedOpen.Enabled = false;
                    wedClose.Enabled = false;
                }
                else if (wedToggle.Checked)
                {
                    wedOpen.Text = emp.availability.wednesday.startTime.ToString();
                    wedClose.Text = emp.availability.wednesday.endTime.ToString();
                    wedOpen.Enabled = true;
                    wedClose.Enabled = true;
                }
                else
                {
                    wedOpen.Text = "";
                    wedClose.Text = "";
                }
                if (openThu.Checked)
                {
                    thuOpen.Text = "";
                    thuClose.Text = "";
                    thuOpen.Hint = "Open";
                    thuClose.Hint = "Open";
                    thuOpen.Enabled = false;
                    thuClose.Enabled = false;
                }
                else if (thuToggle.Checked)
                {
                    thuOpen.Text = emp.availability.thursday.startTime.ToString();
                    thuClose.Text = emp.availability.thursday.endTime.ToString();
                    thuOpen.Enabled = true;
                    thuClose.Enabled = true;
                }
                else
                {
                    thuOpen.Text = "";
                    thuClose.Text = "";
                }
                if (openFri.Checked)
                {
                    friOpen.Text = "";
                    friClose.Text = "";
                    friOpen.Hint = "Open";
                    friClose.Hint = "Open";
                    friOpen.Enabled = false;
                    friClose.Enabled = false;
                }
                else if (friToggle.Checked)
                {
                    friOpen.Text = emp.availability.friday.startTime.ToString();
                    friClose.Text = emp.availability.friday.endTime.ToString();
                    friOpen.Enabled = true;
                    friClose.Enabled = true;
                }
                else
                {
                    friOpen.Text = "";
                    friClose.Text = "";
                }
                if (openSat.Checked)
                {
                    satOpen.Text = "";
                    satClose.Text = "";
                    satOpen.Hint = "Open";
                    satClose.Hint = "Open";
                    satOpen.Enabled = false;
                    satClose.Enabled = false;
                }
                else if (satToggle.Checked)
                {
                    satOpen.Text = emp.availability.saturday.startTime.ToString();
                    satClose.Text = emp.availability.saturday.endTime.ToString();
                    satOpen.Enabled = true;
                    satClose.Enabled = true;
                }
                else
                {
                    satOpen.Text = "";
                    satClose.Text = "";
                }
            }
        }

        /// <summary>
        /// This generates the employee list adapter from a list of employees
        /// </summary>
        /// <param name="empList"></param>
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