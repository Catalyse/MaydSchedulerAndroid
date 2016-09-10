using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace MaydSchedulerApp
{
    public class PositionAdd : DialogFragment
    {
        Button submit, cancel;
        EditText text;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.PositionAddFragment, container, false);

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            Dialog.Window.RequestFeature(WindowFeatures.NoTitle);
            base.OnActivityCreated(savedInstanceState);
            submit = View.FindViewById<Button>(Resource.Id.btnPosFragSubmit);
            cancel = View.FindViewById<Button>(Resource.Id.btnPosFragCancel);
            text = View.FindViewById<EditText>(Resource.Id.inputPosFrag);

            submit.Click += Submit_Click;
            cancel.Click += Cancel_Click;
            text.Click += Text_Click;
        }

        private void Text_Click(object sender, EventArgs e)
        {//This is kinda over the top but I needed a way to reset the text if the submit says no because the position already exists
            submit.Text = "Submit";
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Dismiss();
        }

        private void Submit_Click(object sender, EventArgs e)
        {
            if (SystemSettings.CheckIfPositionExists(text.Text))
            {
                submit.Text = "Position Already Exists!";
            }
            else
            {
                SystemSettings.AddPosition(text.Text);
                SettingsActivity act = (SettingsActivity)Activity;
                act.LoadPositionList();
                Dismiss();
            }
        }
    }
}