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
        Label = "Tower Of Hanoi",
        ScreenOrientation = ScreenOrientation.Landscape,
        Theme = "@android:style/Theme.NoTitleBar")
    ]
    public class TowerOfHanoiActivity : Activity
    {
        //---------------------------------------------------------------------
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // Set our view from the "Tower of Hanoi" layout resource
            SetContentView(Resource.Layout.TowerOfHanoi);

            StartGame();


        }
        //-------------------------------------------------------------------------------------------------
        protected void Button_Long_Click(Object sender, View.LongClickEventArgs lg) {
            // Generate clip data package to attach it to the drag
            var data = ClipData.NewPlainText("name", "Element 1");
            // Start dragging and pass data
            ((sender) as ImageButton).StartDrag(data, new View.DragShadowBuilder(((sender) as ImageButton)), null, 0);
        }
        //--------------------------------------------
        void StartGame()
        {
            //setup game
            bool gameOn = true;
            



            //run game
            while (gameOn)
            {







                //end game
                gameOn = false;


            }


        }
    }
}