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
using Android.Content.PM;

namespace SCaR_Arcade
{
    [Activity(
        Label = "TowerOfHanoiActivity",
        MainLauncher = true)
    ]
    public class TowerOfHanoiActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "Tower of Hanoi" layout resource
            SetContentView(Resource.Layout.TowerOfHanoi);
            Button dragBtn = FindViewById<Button>(Resource.Id.dragBtn);
        }
    }
}