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
using Syncfusion.SfKanban.Android;

namespace Solitaire.Lang
{
    public class Board : SolitaireType
    {
        public Board(string _name, string _description) : base (_name, _description) { }        
        private SfKanban sfBoard;

        public SfKanban SfBoard { get { return sfBoard; } set { sfBoard = value; } }
    }    
}