using System;
using System.Collections.Generic;

using Android.App;
using Android.OS;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MaydSchedulerApp
{
    [Activity(Label = "Schedule", Theme = "@android:style/Theme.Material", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait, WindowSoftInputMode = SoftInput.AdjustPan)]
    public class ScheduleActivity : Activity
    {
        private ListView pickWeekView;
        public Week pickedWeek;
        private PickWeek pickWeek;
        private Button weeklySubmit, staffingSubmit;
        private bool weeklySubmitChanged = false;
        private bool staffingSubmitChanged = false;
        public bool availPassoff = false;
        public EmployeeScheduleWrapper employee;
        //This mode will determine which screen it is currently on allowing you to go back
        private int mode = 0;
        //This is for changing availability allowing for the passoff between activities.
        private int selectedEmp = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ChooseWeek();
            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);
            MainActivity.scheduler = this;
        }

        /// <summary>
        /// This handles changing submit button text back to normal when its been changed
        /// </summary>
        /// <param name="ev"></param>
        /// <returns></returns>
        public override bool DispatchTouchEvent(MotionEvent ev)
        {
            if(weeklySubmitChanged)
            {
                weeklySubmit.Text = "Submit";
                weeklySubmitChanged = false;
            }
            if(staffingSubmitChanged)
            {
                staffingSubmit.Text = "Submit";
                staffingSubmitChanged = false;
            }
            return base.DispatchTouchEvent(ev);
        }

        /// <summary>
        /// This catches the back button on the actionbar
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
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

        /// <summary>
        /// This overrides onbackpressed so we can navigate within the activity
        /// </summary>
        public override void OnBackPressed()
        {
            switch(mode)
            {
                case 0:
                    base.OnBackPressed();
                    break;
                case 1:
                    base.OnBackPressed();
                    break;
                case 2:
                    ChooseWeek();
                    break;
                case 3:
                    WeeklyConfig();
                    break;
            }
        }

        /// <summary>
        /// mode 1
        /// </summary>
        private void ChooseWeek()
        {
            mode = 1;
            SetContentView(Resource.Layout.PickWeekLayout);
            this.Title = "Choose Week";
            pickWeek = new PickWeek();
            pickWeekView = FindViewById<ListView>(Resource.Id.chooseWeekListView);
            List<string> weekList = pickWeek.FindWeeks();
            weekList.Add("Load More");
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, weekList);

            pickWeekView.Adapter = adapter;

            pickWeekView.ItemClick += PickWeekView_ItemClick;
        }

        private void PickWeekView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            if (e.Position == pickWeek.currentCount)//Means they hit load moar
            {
                List<string> weekList = pickWeek.LoadMore();
                weekList.Add("Load More");
                ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, weekList);
                pickWeekView.Adapter = adapter;
            }
            else
            {
                pickedWeek = pickWeek.weekList[e.Position];
                WeeklyConfig();
            }
        }

        /// <summary>
        /// Mode 2
        /// </summary>
        private void WeeklyConfig()
        {
            mode = 2;
            this.Title = "Weekly Facility Hours";
            SetContentView(Resource.Layout.WeeklyConfig);
            SetupWeeklyObjects();
            if (CheckIfDefaultsExist())
                UseDefaultsPopup();

            weeklySubmit = FindViewById<Button>(Resource.Id.btnWeeklySubmit);
            weeklySubmit.Click += WeeklySubmit_Check;
        }

        private void UseDefaultsPopup()
        {
            new AlertDialog.Builder(this)
            .SetPositiveButton("Yes", (sender, args) =>
            {
                pickedWeek.SetWeek(int.Parse(sunOpen.Text), int.Parse(monOpen.Text), int.Parse(tueOpen.Text), int.Parse(wedOpen.Text), int.Parse(thuOpen.Text),
                int.Parse(friOpen.Text), int.Parse(satOpen.Text), int.Parse(sunClose.Text), int.Parse(monClose.Text), int.Parse(tueClose.Text),
                int.Parse(wedClose.Text), int.Parse(thuClose.Text), int.Parse(friClose.Text), int.Parse(satClose.Text), sunToggle.Checked, monToggle.Checked,
                tueToggle.Checked, wedToggle.Checked, thuToggle.Checked, friToggle.Checked, satToggle.Checked);
                GeneratePositionLists();
                StaffingNeeds();
            })
            .SetNegativeButton("No", (sender, args) =>
            {
                //do nothing
            })
            .SetMessage("Do you want to use default facility hours?")
            .SetTitle("Use Defaults")
            .Show();
        }

        EditText sunOpen, sunClose, monOpen, monClose, tueOpen, tueClose, wedOpen, wedClose, thuOpen, thuClose, friOpen, friClose, satOpen, satClose;
        CheckBox sunToggle, monToggle, tueToggle, wedToggle, thuToggle, friToggle, satToggle;
        bool defaultsLoaded = false;

        private void SetupWeeklyObjects()
        {
            sunOpen = FindViewById<EditText>(Resource.Id.inputSunOpen);
            monOpen = FindViewById<EditText>(Resource.Id.inputMonOpen);
            tueOpen = FindViewById<EditText>(Resource.Id.inputTueOpen);
            wedOpen = FindViewById<EditText>(Resource.Id.inputWedOpen);
            thuOpen = FindViewById<EditText>(Resource.Id.inputThuOpen);
            friOpen = FindViewById<EditText>(Resource.Id.inputFriOpen);
            satOpen = FindViewById<EditText>(Resource.Id.inputSatOpen);
            sunClose = FindViewById<EditText>(Resource.Id.inputSunClose);
            monClose = FindViewById<EditText>(Resource.Id.inputMonClose);
            tueClose = FindViewById<EditText>(Resource.Id.inputTueClose);
            wedClose = FindViewById<EditText>(Resource.Id.inputWedClose);
            thuClose = FindViewById<EditText>(Resource.Id.inputThuClose);
            friClose = FindViewById<EditText>(Resource.Id.inputFriClose);
            satClose = FindViewById<EditText>(Resource.Id.inputSatClose);
            sunToggle = FindViewById<CheckBox>(Resource.Id.chkSunday);
            monToggle = FindViewById<CheckBox>(Resource.Id.chkMonday);
            tueToggle = FindViewById<CheckBox>(Resource.Id.chkTuesday);
            wedToggle = FindViewById<CheckBox>(Resource.Id.chkWednesday);
            thuToggle = FindViewById<CheckBox>(Resource.Id.chkThursday);
            friToggle = FindViewById<CheckBox>(Resource.Id.chkFriday);
            satToggle = FindViewById<CheckBox>(Resource.Id.chkSaturday);
        }

        private bool CheckIfDefaultsExist()
        {
            bool exist = SystemSettings.facilityDefaults;
            if (exist)
            {
                sunOpen.Text = SystemSettings.GetIntPref("suO").ToString();
                monOpen.Text = SystemSettings.GetIntPref("mO").ToString();
                tueOpen.Text = SystemSettings.GetIntPref("tuO").ToString();
                wedOpen.Text = SystemSettings.GetIntPref("wO").ToString();
                thuOpen.Text = SystemSettings.GetIntPref("thO").ToString();
                friOpen.Text = SystemSettings.GetIntPref("fO").ToString();
                satOpen.Text = SystemSettings.GetIntPref("saO").ToString();
                sunClose.Text = SystemSettings.GetIntPref("suC").ToString();
                monClose.Text = SystemSettings.GetIntPref("mC").ToString();
                tueClose.Text = SystemSettings.GetIntPref("tuC").ToString();
                wedClose.Text = SystemSettings.GetIntPref("wC").ToString();
                thuClose.Text = SystemSettings.GetIntPref("thC").ToString();
                friClose.Text = SystemSettings.GetIntPref("fC").ToString();
                satClose.Text = SystemSettings.GetIntPref("saC").ToString();
                defaultsLoaded = true;
                if (sunToggle.Checked && (sunOpen.Text == "-1" || sunClose.Text == "-1") || monToggle.Checked && (monOpen.Text == "-1" || monClose.Text == "-1") ||
                    tueToggle.Checked && (tueOpen.Text == "-1" || tueClose.Text == "-1") || sunToggle.Checked && (wedOpen.Text == "-1" || wedClose.Text == "-1") ||
                    sunToggle.Checked && (thuOpen.Text == "-1" || thuClose.Text == "-1") || sunToggle.Checked && (friOpen.Text == "-1" || friClose.Text == "-1") ||
                    sunToggle.Checked && (satOpen.Text == "-1" || satClose.Text == "-1"))
                {//This means at least one of the preferences didnt load correctly and now there will be an issue.
                    FacHoursLoadErrorAlert();
                }
            }
            return exist;
        }

        /// <summary>
        /// This pops if there is an issue loading the default facility preferences
        /// </summary>
        private void FacHoursLoadErrorAlert()
        {
            new AlertDialog.Builder(this)
            .SetPositiveButton("Okay", (sender, args) =>
            {
                //We dont want to do anything, just make sure they know there was an error
            })
            .SetMessage("We would like to apologize but you have run into an error.  Please check all of your hours as at least one of them did not load correctly.")
            .SetTitle("Facility Preference Loading Error!")
            .Show();
        }

        /// <summary>
        /// This will check all fields before allowing the user to continue on to the next page.
        /// </summary>
        private void WeeklySubmit_Check(object sender, EventArgs e)
        {
            //this is huge and it sucks but its neccesary to check if any of the fields are empty
            if (sunToggle.Checked && (sunOpen.Text == "" || sunClose.Text == "") || monToggle.Checked && (monOpen.Text == "" || monClose.Text == "") || 
                tueToggle.Checked && (tueOpen.Text == "" || tueClose.Text == "") || sunToggle.Checked && (wedOpen.Text == "" || wedClose.Text == "") ||
                sunToggle.Checked && (thuOpen.Text == "" || thuClose.Text == "") || sunToggle.Checked && (friOpen.Text == "" || friClose.Text == "") ||
                sunToggle.Checked && (satOpen.Text == "" || satClose.Text == ""))
            {
                weeklySubmit.Text = "Please fill out all fields!";
                weeklySubmitChanged = true;
            }
            //At least one day must be on
            else if(!sunToggle.Checked && !sunToggle.Checked && !sunToggle.Checked && !sunToggle.Checked && 
                !sunToggle.Checked && !sunToggle.Checked && !sunToggle.Checked)
            {
                weeklySubmit.Text = "At least one day MUST be enabled!";
                weeklySubmitChanged = true;
            }
            else
            {
                WeeklyConfigDefaultSettings();
            }
        }

        /// <summary>
        /// This is a prompt to ask if they want to set these facility hours as their default, if they do it changes the setting, if not it continues no changes
        /// </summary>
        private void WeeklyConfigDefaultSettings()
        {
            new AlertDialog.Builder(this)
            .SetCancelable(false)
            .SetPositiveButton("Yes", (sender, args) =>
            {
                SystemSettings.SetupFacilityPreferences(int.Parse(sunOpen.Text), int.Parse(monOpen.Text), int.Parse(tueOpen.Text), int.Parse(wedOpen.Text),
                    int.Parse(thuOpen.Text), int.Parse(friOpen.Text), int.Parse(satOpen.Text), int.Parse(sunClose.Text), int.Parse(monClose.Text),
                    int.Parse(tueClose.Text), int.Parse(wedClose.Text), int.Parse(thuClose.Text), int.Parse(friClose.Text), int.Parse(satClose.Text));
                pickedWeek.SetWeek(int.Parse(sunOpen.Text), int.Parse(monOpen.Text), int.Parse(tueOpen.Text), int.Parse(wedOpen.Text), int.Parse(thuOpen.Text),
                int.Parse(friOpen.Text), int.Parse(satOpen.Text), int.Parse(sunClose.Text), int.Parse(monClose.Text), int.Parse(tueClose.Text),
                int.Parse(wedClose.Text), int.Parse(thuClose.Text), int.Parse(friClose.Text), int.Parse(satClose.Text), sunToggle.Checked, monToggle.Checked,
                tueToggle.Checked, wedToggle.Checked, thuToggle.Checked, friToggle.Checked, satToggle.Checked);
                GeneratePositionLists();
                StaffingNeeds();
            })
            .SetNegativeButton("No", (sender, args) =>
            {
                pickedWeek.SetWeek(int.Parse(sunOpen.Text), int.Parse(monOpen.Text), int.Parse(tueOpen.Text), int.Parse(wedOpen.Text), int.Parse(thuOpen.Text),
                int.Parse(friOpen.Text), int.Parse(satOpen.Text), int.Parse(sunClose.Text), int.Parse(monClose.Text), int.Parse(tueClose.Text),
                int.Parse(wedClose.Text), int.Parse(thuClose.Text), int.Parse(friClose.Text), int.Parse(satClose.Text), sunToggle.Checked, monToggle.Checked,
                tueToggle.Checked, wedToggle.Checked, thuToggle.Checked, friToggle.Checked, satToggle.Checked);
                GeneratePositionLists();
                StaffingNeeds();
            })
            .SetMessage(PromptChoice())
            .SetTitle("Default Facility Hours")
            .Show();
        }

        /// <summary>
        /// This changes the text in the popup based on whether defaults were loaded in or not
        /// </summary>
        private string PromptChoice()
        {
            if (defaultsLoaded)
                return "Would you like to modify your existing default hours to these?";
            else
                return "Would you like to make these your default facility hours?";
        }

        //Setup vars for this section
        EditText sunSOpen, sunSClose, monSOpen, monSClose, tueSOpen, tueSClose, wedSOpen, wedSClose, thuSOpen, thuSClose, friSOpen, friSClose, satSOpen, satSClose;
        Button titleButton;
        List<int> activePosList;
        int posListIterator = 0;
        private int currentPosition = 0;

        /// <summary>
        /// This figures out if we have any employees in the positions we are asking for shifts on.
        /// </summary>
        private void GeneratePositionLists()
        {
            activePosList = new List<int>();

            for (int i = 0; i < EmployeeStorage.employeeList.Count; i++)//Sort employees into the lists made above.
            {
                if (activePosList.Contains(EmployeeStorage.employeeList[i].position))
                { }//do nothing
                else
                    activePosList.Add(EmployeeStorage.employeeList[i].position);
            }
            currentPosition = activePosList[0];
        }

        /// <summary>
        /// mode 3
        /// </summary>
        private void StaffingNeeds()
        {
            mode = 3;
            Title = "Weekly Staffing Needs";
            SetContentView(Resource.Layout.StaffingNeeds);
            sunSOpen = FindViewById<EditText>(Resource.Id.inputSSunOpen);
            monSOpen = FindViewById<EditText>(Resource.Id.inputSMonOpen);
            tueSOpen = FindViewById<EditText>(Resource.Id.inputSTueOpen);
            wedSOpen = FindViewById<EditText>(Resource.Id.inputSWedOpen);
            thuSOpen = FindViewById<EditText>(Resource.Id.inputSThuOpen);
            friSOpen = FindViewById<EditText>(Resource.Id.inputSFriOpen);
            satSOpen = FindViewById<EditText>(Resource.Id.inputSSatOpen);
            sunSClose = FindViewById<EditText>(Resource.Id.inputSSunClose);
            monSClose = FindViewById<EditText>(Resource.Id.inputSMonClose);
            tueSClose = FindViewById<EditText>(Resource.Id.inputSTueClose);
            wedSClose = FindViewById<EditText>(Resource.Id.inputSWedClose);
            thuSClose = FindViewById<EditText>(Resource.Id.inputSThuClose);
            friSClose = FindViewById<EditText>(Resource.Id.inputSFriClose);
            satSClose = FindViewById<EditText>(Resource.Id.inputSSatClose);
            titleButton = FindViewById<Button>(Resource.Id.btnStaffingTitle);
            staffingSubmit = FindViewById<Button>(Resource.Id.btnStaffingSubmit);
            staffingSubmit.Click += StaffingSubmit_Check;
            titleButton.Text = SystemSettings.GetPositionName(currentPosition);
        }

        /// <summary>
        /// This resets the view for the next position
        /// </summary>
        private void StaffingPositionReset()
        {
            sunSOpen.Text = "";
            monSOpen.Text = "";
            tueSOpen.Text = "";
            wedSOpen.Text = "";
            thuSOpen.Text = "";
            friSOpen.Text = "";
            satSOpen.Text = "";
            sunSClose.Text = "";
            monSClose.Text = "";
            tueSClose.Text = "";
            wedSClose.Text = "";
            thuSClose.Text = "";
            friSClose.Text = "";
            satSClose.Text = "";
            posListIterator++;
            currentPosition = activePosList[posListIterator];
            titleButton.Text = SystemSettings.GetPositionName(currentPosition);
        }

        /// <summary>
        /// This checks to make sure that none of the fields are empty
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StaffingSubmit_Check(object sender, EventArgs e)
        {
            if (pickedWeek.sunday.activeDay && (sunSOpen.Text == "" || sunSClose.Text == "") || pickedWeek.monday.activeDay && (monSOpen.Text == "" || monSClose.Text == "") ||
               pickedWeek.tuesday.activeDay && (tueSOpen.Text == "" || tueSClose.Text == "") || pickedWeek.wednesday.activeDay && (wedSOpen.Text == "" || wedSClose.Text == "") ||
               pickedWeek.thursday.activeDay && (thuSOpen.Text == "" || thuSClose.Text == "") || pickedWeek.friday.activeDay && (friSOpen.Text == "" || friSClose.Text == "") ||
               pickedWeek.saturday.activeDay && (satSOpen.Text == "" || satSClose.Text == ""))
            {
                staffingSubmit.Text = "Please fill out all fields!";
                staffingSubmitChanged = true;
            }
            else
            {
                pickedWeek.SetNeeds(currentPosition, GenOpenDict(), GenCloseDict());
                int count = activePosList.Count;
                if (currentPosition < count - 1)
                {//If we need info for more positions
                    //FIXTHIS
                    StaffingPositionReset();
                }
                else
                {
                    AvailChangePrompt();
                }
            }
        }

        /// <summary>
        /// This generates a dictionary of the open shifts.
        /// </summary>
        /// <returns></returns>
        private SerializableDictionary<int, int> GenOpenDict()
        {
            SerializableDictionary<int, int> newDict = new SerializableDictionary<int, int>();
            newDict.Add(0, int.Parse(sunSOpen.Text));
            newDict.Add(1, int.Parse(monSOpen.Text));
            newDict.Add(2, int.Parse(tueSOpen.Text));
            newDict.Add(3, int.Parse(wedSOpen.Text));
            newDict.Add(4, int.Parse(thuSOpen.Text));
            newDict.Add(5, int.Parse(friSOpen.Text));
            newDict.Add(6, int.Parse(satSOpen.Text));
            return newDict;
        }

        //Not working yet
        /// <summary>
        /// This generates a dictionary of the mid shifts
        /// </summary>
        /// <returns></returns>
        private SerializableDictionary<int, int> GenMidDict()
        {
            SerializableDictionary<int, int> newDict = new SerializableDictionary<int, int>();
            newDict.Add(0, int.Parse(sunSOpen.Text));
            newDict.Add(1, int.Parse(monSOpen.Text));
            newDict.Add(2, int.Parse(tueSOpen.Text));
            newDict.Add(3, int.Parse(wedSOpen.Text));
            newDict.Add(4, int.Parse(thuSOpen.Text));
            newDict.Add(5, int.Parse(friSOpen.Text));
            newDict.Add(6, int.Parse(satSOpen.Text));
            return newDict;
        }

        /// <summary>
        /// This generates a dictionary of the closing shifts
        /// </summary>
        /// <returns></returns>
        private SerializableDictionary<int, int> GenCloseDict()
        {
            SerializableDictionary<int, int> newDict = new SerializableDictionary<int, int>();
            newDict.Add(0, int.Parse(sunSClose.Text));
            newDict.Add(1, int.Parse(monSClose.Text));
            newDict.Add(2, int.Parse(tueSClose.Text));
            newDict.Add(3, int.Parse(wedSClose.Text));
            newDict.Add(4, int.Parse(thuSClose.Text));
            newDict.Add(5, int.Parse(friSClose.Text));
            newDict.Add(6, int.Parse(satSClose.Text));
            return newDict;
        }
        
        private void AvailChangePrompt()
        {
            new AlertDialog.Builder(this)
            .SetPositiveButton("Yes", (sender, args) =>
            {
                AvailabilityChange();
            })
            .SetNegativeButton("No", (sender, args) =>
            {
                GenerateSchedule();
            })
            .SetMessage("Do any Employees have Availability changes? (Time Off)")
            .SetTitle("Availability Changes")
            .Show();
        }

        private void AvailabilityChange()
        {
            GenerateWrapperList();
            EmpListWindow();
        }

        private void EmpListWindow()
        {
            SetContentView(Resource.Layout.AvailChange);
            Title = "Active Employees for " + pickedWeek.startDate.ToShortDateString();
            ListView empListView = FindViewById<ListView>(Resource.Id.availChangeListView);
            List<string> empStringList = new List<string>();
            for(int i = 0; i < pickedWeek.empList.Count;i++)
            {
                empStringList.Add(pickedWeek.empList[i].lName + ", " + pickedWeek.empList[i].fName);
            }
            ArrayAdapter<string> adapter = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleListItem1, empStringList);
            empListView.Adapter = adapter;

            Button availSubmit = FindViewById<Button>(Resource.Id.btnAvailChangeSubmit);
            Button availCancel = FindViewById<Button>(Resource.Id.btnAvailChangeCancel);

            availSubmit.Click += AvailSubmit_Click;
            availCancel.Click += AvailCancel_Click;
            empListView.ItemClick += EmpListView_ItemClick;
        }

        private void AvailCancel_Click(object sender, EventArgs e)
        {
            GenerateSchedule();
        }

        private void AvailSubmit_Click(object sender, EventArgs e)
        {
            GenerateSchedule();
        }

        private void EmpListView_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            availPassoff = true;
            employee = pickedWeek.empList[e.Position];
            selectedEmp = e.Position;
            Intent intent = new Intent(this, typeof(EmpMgmtActivity));
            StartActivity(intent);
        }

        public void AvailabilitySubmit(Availability avail)
        {
            pickedWeek.empList[selectedEmp].SetTempAvailability(avail);
        }

        /// <summary>
        /// This method accepts a list of employees, then adds all active employees to a wrapper for more information to be placed on top of them without affecting the base saved data
        /// </summary>
        /// <param name="empList">Unsorted list of all employees in the system</param>
        public void GenerateWrapperList()
        {
            List<Employee> empList = EmployeeStorage.employeeList;
            for (int i = 0; i < empList.Count; i++)
            {
                if (empList[i].active)//Check if the employee is active
                {
                    EmployeeScheduleWrapper newWrapper = new EmployeeScheduleWrapper(empList[i]);
                    pickedWeek.empList.Add(newWrapper);
                }
            }
        }

        private void GenerateSchedule()
        {
            GenerateWrapperList();
            MainActivity.week = pickedWeek;
            MainActivity.historyActivityPassoff = true;
            SchedulingAlgorithm.StartScheduleGen();
            Finish();
        }
    }
}