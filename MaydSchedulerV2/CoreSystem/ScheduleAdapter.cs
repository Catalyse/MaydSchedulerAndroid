using System;
using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MaydSchedulerApp
{
    class ScheduleAdapter : BaseAdapter<EmployeeScheduleWrapper>
    {
        private List<EmployeeScheduleWrapper> mItems;
        private Context mContext;

        public ScheduleAdapter(Context context, List<EmployeeScheduleWrapper> items)
        {
            mItems = items;
            mContext = context;
        }

        public override int Count
        {
            get
            {
                return mItems.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override EmployeeScheduleWrapper this[int position]
        {
            get
            {
                return mItems[position];
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View row = convertView;

            if (row == null)
            {
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.ScheduleListView, null, false);
            }

            TextView txtName = row.FindViewById<TextView>(Resource.Id.txtScheduleName);
            txtName.Text = mItems[position].lName + ", " + mItems[position].fName;

            TextView txtPosition = row.FindViewById<TextView>(Resource.Id.txtSchedulePosition);
            txtPosition.Text = CoreSystem.GetPositionName(mItems[position].position);

            TextView txtSunday = row.FindViewById<TextView>(Resource.Id.txtSun);
            TextView txtMonday = row.FindViewById<TextView>(Resource.Id.txtMon);
            TextView txtTuesday = row.FindViewById<TextView>(Resource.Id.txtTue);
            TextView txtWednesday = row.FindViewById<TextView>(Resource.Id.txtWed);
            TextView txtThursday = row.FindViewById<TextView>(Resource.Id.txtThu);
            TextView txtFriday = row.FindViewById<TextView>(Resource.Id.txtFri);
            TextView txtSaturday = row.FindViewById<TextView>(Resource.Id.txtSat);
            txtSunday.Text = "Off";
            txtMonday.Text = "Off";
            txtTuesday.Text = "Off";
            txtWednesday.Text = "Off";
            txtThursday.Text = "Off";
            txtFriday.Text = "Off";
            txtSaturday.Text = "Off";
            for (int i = 0; i < mItems[position].shiftList.Count; i++)
            {
                switch (mItems[position].shiftList[i].date)
                {
                    case DayOfWeek.Sunday:
                        txtSunday.Text = mItems[position].shiftList[i].startShift.ToString() + "00 - " + mItems[position].shiftList[i].endShift.ToString() + "00";
                        break;
                    case DayOfWeek.Monday:
                        txtMonday.Text = mItems[position].shiftList[i].startShift.ToString() + "00 - " + mItems[position].shiftList[i].endShift.ToString() + "00";
                        break;
                    case DayOfWeek.Tuesday:
                        txtTuesday.Text = mItems[position].shiftList[i].startShift.ToString() + "00 - " + mItems[position].shiftList[i].endShift.ToString() + "00";
                        break;
                    case DayOfWeek.Wednesday:
                        txtWednesday.Text = mItems[position].shiftList[i].startShift.ToString() + "00 - " + mItems[position].shiftList[i].endShift.ToString() + "00";
                        break;
                    case DayOfWeek.Thursday:
                        txtThursday.Text = mItems[position].shiftList[i].startShift.ToString() + "00 - " + mItems[position].shiftList[i].endShift.ToString() + "00";
                        break;
                    case DayOfWeek.Friday:
                        txtFriday.Text = mItems[position].shiftList[i].startShift.ToString() + "00 - " + mItems[position].shiftList[i].endShift.ToString() + "00";
                        break;
                    case DayOfWeek.Saturday:
                        txtSaturday.Text = mItems[position].shiftList[i].startShift.ToString() + "00 - " + mItems[position].shiftList[i].endShift.ToString() + "00";
                        break;
                }
            }
            return row;
        }
    }
}