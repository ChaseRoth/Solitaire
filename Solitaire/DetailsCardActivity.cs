﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Syncfusion.SfKanban.Android;
using Solitaire.Lang;

namespace Solitaire
{
    [Activity(Label = "DetailsCardActivity")]
    public class DetailsCardActivity : AppCompatActivity
    {
        long kanbanModelId;
        KanbanModelWrapper clickedKanbanModel;
        private const int EDIT_ACTIVITY_CODE = 3;
        ListView contributorsListView;
        TextView cardNameTextView, cardDescriptionTextView, cardLeaderTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.details_card_activity);

            var toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            toolbar.Title = "Card Details";
            SetSupportActionBar(toolbar);

            // Getting the Id of the kanbanModel so we can setup a pointer to it
            kanbanModelId =  this.Intent.GetLongExtra("kanbanModelId", -1);
            
            // We will try and find the kanbanModel but if we can't for some odd reason
            try
            {
                clickedKanbanModel = UseBoardActivity.kanbanModels.Single(kanbanModel => kanbanModel.ID == kanbanModelId);
            }
            // Just finish the activity
            catch
            {
                Finish();
            }

            // Setting the textviews to display the information about the kanbanModel
            contributorsListView         = FindViewById<ListView>(Resource.Id.contributorsListView);
            cardNameTextView             = FindViewById<TextView>(Resource.Id.cardNameTextView);
            cardDescriptionTextView      = FindViewById<TextView>(Resource.Id.cardDescriptionTextView);
            cardLeaderTextView           = FindViewById<TextView>(Resource.Id.cardLeaderTextView);
            cardDescriptionTextView.Text = clickedKanbanModel.Description;
            cardNameTextView.Text        = clickedKanbanModel.Title;
            cardLeaderTextView.Text      = clickedKanbanModel.Leader == null ? "N/A" : $"{clickedKanbanModel.Leader.Name} | {clickedKanbanModel.Leader.Email}";

            // Gets list of contributors using email as our primary key, also null check
            contributorsListView.Adapter = new ContributorsAdapter(
                AssetManager.contributors.Where(contributor => {
                    if (clickedKanbanModel.Tags != null) {
                        return clickedKanbanModel.Tags.Contains(contributor.Email);
                    }
                    return false;
                }).ToList(), this);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.details_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // Comparing the title of the toolbar btns to a string to determine which was clicked
            switch (item.ItemId)
            {
                case Resource.Id.edit:
                    Intent editCardActivity = new Intent(this, typeof(EditCardActivity));
                    editCardActivity.PutExtra("kanbanModelId", kanbanModelId);                        
                    StartActivityForResult(editCardActivity, EDIT_ACTIVITY_CODE);
                    break;
                default:
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }

        /// 
        /// 
        ///     When the EditCardActivity finishe we will check to see if the values were saved, therefore we must signal the UseBoardActivity to update its UI
        ///       
        /// 
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            // If the resultCode is equal to Result.Ok then we will manually tell the UI to refresh
            if (requestCode == EDIT_ACTIVITY_CODE && resultCode == Result.Ok)
            {
                SetResult(Result.Ok);
            }            
        }
        

        // When this resumes we re-assign the views the proper values
        protected override void OnResume()
        {
            base.OnResume();
            cardNameTextView.Text = clickedKanbanModel.Title;
            cardDescriptionTextView.Text = clickedKanbanModel.Description;
            cardLeaderTextView.Text = clickedKanbanModel.Leader == null ? "N/A" : $"{clickedKanbanModel.Leader.Name} | {clickedKanbanModel.Leader.Email}";
                            
            contributorsListView.Adapter = new ContributorsAdapter(
                AssetManager.contributors.Where(contributor => {
                    if (clickedKanbanModel.Tags != null)
                    {
                        return clickedKanbanModel.Tags.Contains(contributor.Email);
                    }
                    return false;
                }).ToList(), this);

            var test = clickedKanbanModel.Leader;
        }
    }
}