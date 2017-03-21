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
        private Button dragBtn;
        //---------------------------------------------------------------------
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "Tower of Hanoi" layout resource
            SetContentView(Resource.Layout.TowerOfHanoi);
            dragBtn = FindViewById<Button>(Resource.Id.dragBtn);
            //dragBtn.SetOnTouchListener(new OnTouchListener());
            dragBtn.LongClick += Button_Long_Click;
        }
        //-------------------------------------------------------------------------------------------------
        protected void Button_Long_Click(Object sender, View.LongClickEventArgs lg) {
            // Generate clip data package to attach it to the drag
            var data = ClipData.NewPlainText("name", "Element 1");
            // Start dragging and pass data
            ((sender) as Button).StartDrag(data, new View.DragShadowBuilder(((sender) as Button)), null, 0);
        }


        private class OnTouchListener : View.IOnTouchListener {
            private IntPtr motion_Handle;
            public Boolean OnTouch(View vi, MotionEvent moev)
            { 
                return true;
            }
            public IntPtr Handle
            {
                get { return motion_Handle; }
            }

            public void Dispose()
            {

            }
        }
    }
}