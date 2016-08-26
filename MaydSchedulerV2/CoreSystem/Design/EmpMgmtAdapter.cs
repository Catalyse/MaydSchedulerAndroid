using System.Collections.Generic;
using Android.Content;
using Android.Views;
using Android.Widget;

namespace MaydSchedulerApp
{
    class EmpMgmtAdapter : BaseAdapter<Employee>
    {
        private List<Employee> mItems;
        private Context mContext;

        public EmpMgmtAdapter(Context context, List<Employee> items)
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

        public override Employee this[int position]
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
                row = LayoutInflater.From(mContext).Inflate(Resource.Layout.EmpMgmtListView, null, false);
            }

            TextView txtFirst = row.FindViewById<TextView>(Resource.Id.txtFirst);
            txtFirst.Text = mItems[position].empFirstName;

            TextView txtLast = row.FindViewById<TextView>(Resource.Id.txtLast);
            txtLast.Text = mItems[position].empLastName;

            TextView txtPosition = row.FindViewById<TextView>(Resource.Id.txtPosition);
            txtPosition.Text = CoreSystem.GetPositionName(mItems[position].position);

            TextView txtEID = row.FindViewById<TextView>(Resource.Id.txtEID);
            txtEID.Text = mItems[position].empID.ToString();

            TextView txtPartFullTime = row.FindViewById<TextView>(Resource.Id.txtPartFullTime);
            if (mItems[position].hourTarget >= 40)
            {
                txtPartFullTime.Text = "Full Time";
            }
            else
                txtPartFullTime.Text = "Part Time";

            return row;
        }
    }
}