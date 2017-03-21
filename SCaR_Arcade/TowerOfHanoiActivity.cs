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

namespace SCaR_Arcade
{
    [Activity(Label = "TowerOfHanoiActivity")]
    public class TowerOfHanoiActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "Tower of Hanoi" layout resource
            SetContentView(Resource.Layout.TowerOfHanoi);
        }
   
    }
}