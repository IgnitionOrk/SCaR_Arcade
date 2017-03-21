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
        private ImageButton img;
        //---------------------------------------------------------------------
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "Tower of Hanoi" layout resource
            SetContentView(Resource.Layout.TowerOfHanoi);
            img = FindViewById<ImageButton>(Resource.Id.imgBtn);
            img.LongClick += Button_Long_Click;
        }
        //-------------------------------------------------------------------------------------------------
        protected void Button_Long_Click(Object sender, View.LongClickEventArgs lg) {
            // Generate clip data package to attach it to the drag
            var data = ClipData.NewPlainText("name", "Element 1");
            // Start dragging and pass data
            ((sender) as ImageButton).StartDrag(data, new View.DragShadowBuilder(((sender) as ImageButton)), null, 0);
        }
    }
}