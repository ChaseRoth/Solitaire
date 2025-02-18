﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Solitaire
{
    ///
    /// 
    ///     Provides a way for the user to choose whether they want to create a new contact or use a pre-existing contact 
    /// 
    ///
    public class ContributorOptionsDialog : Dialog
    {        
        Context callerActivity;

        public ContributorOptionsDialog(Context _context) : base(_context) 
        { 
            callerActivity = _context;
            this.Show();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            RequestWindowFeature((int)WindowFeatures.NoTitle);
            SetContentView(Resource.Layout.contributor_options_dialog);


            // Starts the activity to create a new contributor
            FindViewById<Button>(Resource.Id.addNewContributor).Click += delegate
            {
                new CreateContributorDialog(callerActivity);
                Dismiss();
            };
            FindViewById<Button>(Resource.Id.cancelBtn).Click += delegate
            {
                // new AddExistingContributorDialog(callerInstance);
                Dismiss();
            };
        }
    }
}