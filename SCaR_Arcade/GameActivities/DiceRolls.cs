using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using System.Diagnostics;
using Android.Graphics;
using System.Collections.Generic;
using Android.Runtime;
using Android.Views;
using System.Threading.Tasks;
using Android.Hardware;
/// <summary>
/// Creator: Ryan Cunneen
/// Creator: Martin O'Connor
/// Student number: 3179234
/// Student number: 3279660
/// Date created: 25-Mar-17
/// Date modified: 10-Apr-17
/// </summary>
namespace SCaR_Arcade.GameActivities
{
    [Activity(
        Label = "",
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
        Theme = "@android:style/Theme.NoTitleBar"
    )]
    public class DiceRolls : Activity, IDialogInterfaceOnDismissListener, ISensorEventListener
    {
        // ----------------------------------------------------------------------------------------------------------------
        /*
         * sources
         * https://developer.xamarin.com/recipes/android/os_device_resources/accelerometer/get_accelerometer_readings/ 
        */
        private GameLogic.TowersOfHanoiLogic logic;
        private Chronometer chronometer;
        private TextView elapsedTime;
        private TextView txtVScore;
        private LinearLayout gameDisplay;
        private FrameLayout[] frameLayout;
        private LinearLayout[] linearLayout;
        private ImageView[] diceSlots;
        private int maxComponents = 1;
        private int numberOfMoves = 0;
        private int buffCount = 5;
        private int numOfGoodShakeCount = 5;
        private float x;
        private float y;
        private float z;
        // Used when a drag and drop event has occured to store data. 
        private View disk;
        private LinearLayout removedFromLinearLayout;
        private long pausedAt = 0;
        static readonly object _syncLock = new object();
        SensorManager _sensorManager;
        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.TowersOfHanoi);

                _sensorManager = (SensorManager)GetSystemService(SensorService);
                Button btnReplay = FindViewById<Button>(Resource.Id.btnReplay);
                Button btnQuit = FindViewById<Button>(Resource.Id.btnQuit);
                TextView txtOptimalNoOfMoves = FindViewById<TextView>(Resource.Id.txtViewOptNoOfMoves);
                chronometer = FindViewById<Chronometer>(Resource.Id.cTimer);
                elapsedTime = FindViewById<TextView>(Resource.Id.txtVElapsedTime);
                txtVScore = FindViewById<TextView>(Resource.Id.txtVScore);
                gameDisplay = FindViewById<LinearLayout>(Resource.Id.linLayGameDisplay);
                // Build the game display that the user will interact with;
                maxComponents = Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1);

                Game();

                // Initializing data for the game.
                logic = new GameLogic.TowersOfHanoiLogic(maxComponents, Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1));
                //txtOptimalNoOfMoves.Text = string.Format("{0}", "Optimal no. of moves: " + logic.calOptimalNoOfMoves(Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1)));
                txtVScore.Text = "No. of moves: " + 0;
                chronometer.Visibility = Android.Views.ViewStates.Invisible;

                // Event handlers:
                btnReplay.Click += btnReplayOnClick;
                btnQuit.Click += btnQuitOnClick;
                chronometer.ChronometerTick += chronometerOnTick;

                // Begin the timer;
                chronometer.Start();
            }
            catch
            {
                GlobalApp.Alert(this, 0);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // builds the game that the user will interact with at runtime. 
        // Initially the game had multiple instance variables for each ImageViews (Poles), and LinearLayouts (vertical)
        // By removing them from the axml file, and bulding them at runtime the Activity file's simplicity, and readability has been enhanced.
        private void Game()
        {
            createFrameLayouts();   // Will Allow both ImageViews (Poles) and ImageView (Disks) to be placed on top of eachother.
            createImageViews();     // Images of the poles that are displayed. 
            createLinearLayouts();  // (Vertical) Stacks that will hold the disks. 
            createDisks();          //  Movable disks that the user interacts with;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Initialize Layout manager (FrameLayout) as so to group an ImageView, and LinearLayout;
        // FrameLayout will allow us to place ImageViews, and LinearLayouts on top of each other. 
        private void createFrameLayouts()
        {
            frameLayout = new FrameLayout[maxComponents];

            LinearLayout.LayoutParams frameParameters = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.WrapContent,
                LinearLayout.LayoutParams.MatchParent,
                1
            );
            for (int i = 0; i < maxComponents; i++)
            {
                frameLayout[i] = new FrameLayout(this);
                frameLayout[i].Clickable = false;
                gameDisplay.AddView(frameLayout[i], frameParameters);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Images of the poles in the background.
        private void createImageViews()
        {
            LinearLayout.LayoutParams imageViewParameters = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MatchParent,
                LinearLayout.LayoutParams.MatchParent
            );
            diceSlots = new ImageView[maxComponents];
            for (int i = 0; i < maxComponents; i++)
            {
                diceSlots[i] = new ImageView(this);
                diceSlots[i].SetScaleType(ImageView.ScaleType.FitCenter);
                diceSlots[i].Enabled = false;
                frameLayout[i].AddView(diceSlots[i], imageViewParameters);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Creates the LinearLayouts (vertical) that will hold the ImageViews (disks).
        private void createLinearLayouts()
        {
            int paddingHeight = Resources.DisplayMetrics.HeightPixels / 40;

            linearLayout = new LinearLayout[maxComponents];
            LinearLayout.LayoutParams linearParameters = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MatchParent,
                LinearLayout.LayoutParams.MatchParent
            );
            for (int i = 0; i < maxComponents; i++)
            {
                linearLayout[i] = new LinearLayout(this);
                linearLayout[i].SetMinimumWidth(25);
                linearLayout[i].SetMinimumHeight(25);
                linearLayout[i].Orientation = Orientation.Vertical;
                linearLayout[i].SetGravity(Android.Views.GravityFlags.Center);
                frameLayout[i].AddView(linearLayout[i], linearParameters);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Builds all the disks, and adds then into the first LinearLayout;
        private void createDisks()
        {
            for (int i = 0; i < maxComponents; i++)
            {
                ImageView imgView = getResizedImage(i);

                // Add the view (imgView) into the linearlayout, with the added effect of the disks appearing in ascending order.
                linearLayout[i].AddView(imgView, 0);

                //Only the top disk is allowed to be clickable.
                //rollDice();
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns an imageView with a desired width;
        // Every imageView will be 5% shorter than its predecessor, so the user can 
        // differentiate between disks;
        private ImageView getResizedImage(int count)
        {
            // Why use Disk.png to determine the width, and height?
            // Disk.png is used because it has been calibrated to a desired shape (width, height).
            // So we simply form an imageView to the shape of Disk.png. 
            ImageView img = new ImageView(this);
            Bitmap bMapDisk = BitmapFactory.DecodeResource(Resources, Resource.Drawable.gameBase);

            // Scale the Bitmap image to the desired specs;
            Bitmap bMapDiskScaled = Bitmap.CreateScaledBitmap(bMapDisk, 400, 400, true);

            //Add the numbers to each Bitmap so the player can differentiate between disks, particularly if there are a large number of them.
            bMapDiskScaled = addNumbersToBitMap(bMapDiskScaled, count);

            img.SetImageBitmap(bMapDiskScaled);
            img.SetScaleType(ImageView.ScaleType.Center);
            

            return img;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Adds numbers to each of the disks, as some players may find it hard to differentiate between disks.
        // Particularly if there are alot of them.
        private Bitmap addNumbersToBitMap(Bitmap bMapDiskScaled, int count)
        {
            int number = Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1) - count;
            // The top left hand corner of the image of the number is specified by the (x,y) 
            // Different values were tested to find the best size.
            float x = (float)(bMapDiskScaled.Width - bMapDiskScaled.Width * .78);
            float y = (float)(bMapDiskScaled.Height - bMapDiskScaled.Height / 6);

            // The bitmap must be immutable otherwise it will through an exception, if changes are permitted. 
            bMapDiskScaled = bMapDiskScaled.Copy(Bitmap.Config.Argb8888, true);
            Canvas canvas = new Canvas(bMapDiskScaled);
            Paint paint = new Paint();
            paint.Color = Color.Black;

            // Again different values were tested to find the best size. 
            paint.TextSize = (int)(bMapDiskScaled.Height - (bMapDiskScaled.Height * 0.05));

            // Now draw the number at the specified x, and y coordinates. 
            canvas.DrawText(String.Format("{0}", number), x, y, paint);
            return bMapDiskScaled;
        }
        
        // ----------------------------------------------------------------------------------------------------------------
        // An listener issued for when an view has been (long) click;
        // Will execute a StartDrag event for when the user
        // wishes to move a view around the screen. 
        // Reference: https://blog.xamarin.com/android-tricks-supporting-drag-and-drop-in-an-app/
        public bool OnLongClick(View v)
        {
            var data = ClipData.NewPlainText("", "");

            // Initiates a drag event. 
            v.StartDrag(data, new View.DragShadowBuilder(v), null, 0);

            // After the drag event has finished we remove the disk from 
            // the linearlayout.
            removeDiskFromLinearLayout(v);
            return true;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Removes the @param v from its Parent view;
        private void removeDiskFromLinearLayout(View view)
        {
            // Save the view into the instance variable disk for one reason:
            // The lifetime of disk is the lifetime of the game (so no data is lost), when we 
            // try to insert the disk back into a LinearLayout. 
            disk = view;
            removedFromLinearLayout = (view.Parent) as LinearLayout;

            //Remove the view (disk) from its Parent. 
            removedFromLinearLayout.RemoveView(view);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Reloads the disk back on the pole;
        private void returnDiskToPlacement(DragEvent args)
        {
            //Check validity of Disk placement 
            if (!args.Result)
            {
                if (disk.Parent != null)
                {
                    (disk.Parent as ViewGroup).RemoveView(disk);
                }
                removedFromLinearLayout.AddView(disk, 0);
                disk.Invalidate();
                removedFromLinearLayout.Invalidate();
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines if the move is allowable
        // If the move is the disk is dropped into the desired dropzone.
        // Otherwise return the disk back from whence it came (removedFromLinearLayout).  
        private void allowableMove(View view)
        {
            if (logic != null)
            {
                int indexFrom = findLinearLayoutIndex(removedFromLinearLayout);
                int indexTo = findLinearLayoutIndex((view as LinearLayout));
                if (logic.canDropDisk(indexFrom, indexTo))
                {
                    //Essentially we now save the moves into the game logic object 'logic' for further use. 
                    logic.finalizeMove(indexFrom, indexTo);

                    // Add the new disk into the @param view.
                    addToNewDropzone(view);

                    // Now we set the appropriate properties of the disks for each LinearLayout.
                    //topDiskIsOnlyClickable();
                    numberOfMoves++;
                    txtVScore.Text = "No. of moves: " + numberOfMoves;
                }
                else
                {
                    // Show an alert.
                    Alert(0, 0);
                    // Adding the disk, that was just removed back into the linearlayout it came from at the top of the stack;
                    removedFromLinearLayout.AddView(disk, 0);
                }
                if (logic.ifWon())
                {
                    txtVScore.Text = "No. of moves: " + numberOfMoves;
                    end();
                }
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // The game has ended so the score, and time will be displayed.
        private void end()
        {
            try
            {
                chronometer.Stop();
                string playersScore = LeaderBoardInterface.formatLeaderBoardScore("", numberOfMoves.ToString(), Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1), chronometer.Text);
                BeginActivity(typeof(UserInputActivity), GlobalApp.getPlayersScoreVariable(), playersScore);
            }
            catch
            {
                GlobalApp.Alert(this, 0);
            }

        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event Handler: Will direct the player to the Game menu.
        public override void OnBackPressed()
        {
            determineResponse(false);
        }
        // ----------------------------------------------------------------------------------------------------------------
        /*
            INTERNAL ALERTS FOR TOWERS OF HANOI.        
        */
        // Display an error message for the invalid move.
        // Also stops the chronometer from continuing to count up.
        // http://stackoverflow.com/questions/42006181/chronometer-is-still-running-after-calling-stop
        // Helped with how to stop, and start the chronometer.s
        private void Alert(int iTitle, int iMessage)
        {
            string title = this.getAlertTitle(iTitle);
            string message = this.getAlertMessage(iMessage);

            // Stop the chronometer or the player will be timed for actually not playing the game.
            chronometer.Stop();

            // Say that time the chronometer was stopped so we can restart it. 
            pausedAt = chronometer.Base - SystemClock.ElapsedRealtime();

            // Now we build the Alert that will show the error message.
            AlertDialog.Builder adb = new AlertDialog.Builder(this);
            adb.SetMessage(message);
            adb.SetTitle(title);
            adb.SetOnDismissListener(this);
            adb.Show();


        }
        // ----------------------------------------------------------------------------------------------------------------
        // When the alert is dismissed by the player. The chronometer will continue the game clock.
        public void OnDismiss(IDialogInterface dialog)
        {
            chronometer.Base = SystemClock.ElapsedRealtime() + pausedAt;

            // Continue the chronometer.
            chronometer.Start();
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines, and returns the correct title for an alert about to be executed.
        private string getAlertTitle(int iMsg)
        {
            string title = "";
            switch (iMsg)
            {
                case 0:
                    title = "Incorrect move.";
                    break;
                case 1:
                    title = "You've won!!";
                    break;
            }
            return title;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines, and returns the correct message for an alert about to be executed.
        private string getAlertMessage(int iMsg)
        {
            string message = "";
            switch (iMsg)
            {
                case 0:
                    message = "You cannot place larger disks on top of smaller disks."
                            + "\n\nPress outside of the box to continue.";
                    break;
                case 1:
                    message = "Moves: " + numberOfMoves + ".\nTime: " + chronometer.Text
                            + "\n\nPress outside of the box.";
                    break;
            }
            return message;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines which @param lin is referring to. 
        // This method is vital for determining if the move is allowed or not. 
        private int findLinearLayoutIndex(LinearLayout lin)
        {
            int index = 0;
            foreach (LinearLayout l in linearLayout)
            {
                if (l == lin)
                {
                    break;
                }
                else
                {
                    index++;
                }
            }
            return index;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Adds the disk (view) into the new LinearLayout (dropzone). 
        // @param view is the LinearLayout;
        private void addToNewDropzone(View view)
        {
            // View v is the LinearLayout of the dropzone. 
            LinearLayout dropzone = (view as LinearLayout);

            // As the orientation of the LinearLayout is defined as vertical,
            // the top of the LinearLayout is indexed at 0. 
            dropzone.AddView(disk, 0);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Will make every top disk (if the LinearLayout has any views) clickable, with drag, and drop capability.
        // Essentially if the LinearLayout has 2 or more disks, the top will be clickable, and the next one down won't be.
        // Notice that there is not loop going through the all LinearLayout's, and setting each ImageViews property.
        // Why? Look at the function createDisks(). The loop in createDisks() does this for us when the ImageView as added, 
        // we need only check the values of the top and the next ImageView (if any), and set their respective properties.  
        private void rollDice()
        {
            if (numOfGoodShakeCount != 0)
            {
                //not enough shakes
                numOfGoodShakeCount--;
            }
            else
            {
                numOfGoodShakeCount = 5;
                foreach (LinearLayout lin in linearLayout)
                {
                    OnPause();
                    System.Diagnostics.Debug.Write("Rolling dice");
                }
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event handler: Triggered when the user pressed the replay button;
        protected void btnReplayOnClick(Object sender, EventArgs args)
        {
            determineResponse(true);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event handler: Triggered when the user pressed the quit button;
        protected void btnQuitOnClick(Object sender, EventArgs args)
        {
            determineResponse(false);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Determines the appropriate response if a particular button has been pressed. 
        private void determineResponse(bool isReplay)
        {
            if (chronometer != null)
            {
                chronometer.Stop();
                chronometer = null;
            }
            if (logic != null)
            {
                logic.deleteBoard();
                logic = null;
            }
            if (_sensorManager != null)
            {
                _sensorManager.UnregisterListener(this);
            }
            if (isReplay)
            {
                BeginActivity(typeof(DiceRolls), GlobalApp.getVariableDifficultyName(), Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1));
            }
            else
            {
                BeginActivity(typeof(GameMenuActivity), "", 0);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Begins the Activity specified by @param type.
        private void BeginActivity(Type type, string variableName, int value)
        {
            try
            {
                Intent intent = new Intent(this, type);
                if (type != typeof(MainActivity))
                {
                    intent.PutExtra(variableName, value);
                }
                StartActivity(intent);
            }
            catch
            {
                // because an error has happend at the Application level
                // We delegate the responsibility to the GlobalApp class.
                GlobalApp.Alert(this, 2);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Begins the Activity specified by @param type.
        private void BeginActivity(Type type, string variableName, string value)
        {
            try
            {
                Intent intent = new Intent(this, type);
                if (type != typeof(MainActivity))
                {
                    intent.PutExtra(variableName, value);
                }
                StartActivity(intent);
            }
            catch
            {
                // because an error has happend at the Application level
                // We delegate the responsibility to the GlobalApp class.
                GlobalApp.Alert(this, 2);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Continuously update the displayed time.
        protected void chronometerOnTick(Object sender, EventArgs arg)
        {
            elapsedTime.Text = String.Format("{0}", "Time: " + chronometer.Text);
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy) { }

        public void OnSensorChanged(SensorEvent e)
        {
            if (buffCount != 0)
            {
                buffCount--;
                lock (_syncLock)
                {
                    x = e.Values[0];
                    y = e.Values[1];
                    z = e.Values[2];
                }
            }
            else
            {
                buffCount = 5;
                lock (_syncLock)
                {
                    float num = 3;
                    float negNum = -3;

                    System.Diagnostics.Debug.Write("X "+ (x - e.Values[0])+" Y "+(y - e.Values[1]) +" Z "+(z - e.Values[2]));
                    if (x - e.Values[0] < negNum || num < x - e.Values[0] ||
                    y - e.Values[1] < negNum || num < y - e.Values[1] ||
                    z - e.Values[2] < negNum || num < z - e.Values[2] )
                    {
                        rollDice();
                    }
                }
            }
                
               
           
        }
        protected override void OnPause()
        {
            base.OnPause();
            _sensorManager.UnregisterListener(this);
        }
        protected override void OnResume()
        {
            base.OnResume();
            _sensorManager.RegisterListener(this,
                                            _sensorManager.GetDefaultSensor(SensorType.Accelerometer),
                                            SensorDelay.Ui);
        }
    }

}