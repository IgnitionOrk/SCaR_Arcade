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
        private GameLogic.DiceRollsLogic logic;
        private Chronometer chronometer;
        private TextView elapsedTime;
        private TextView txtVScore;
        private TextView txtOptimalNoOfMoves;
        private LinearLayout gameDisplay;
        private FrameLayout[] frameLayout;
        private LinearLayout[] linearLayout;
        private ImageView[] diceSlots;
        private int maxComponents = 1;
        private int numberOfRolls = 0;
        private int score = 0;
        private int buffCount = 5;
        private int numOfGoodShakeCount = 5;
        private float x;
        private float y;
        private float z;
        private bool sensorOn = false;
        // Used when a drag and drop event has occured to store data. 
        private long pausedAt = 0;
        static readonly object _syncLock = new object();
        SensorManager _sensorManager;
        Random rnd = new Random();

        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.TowersOfHanoi);

                _sensorManager = (SensorManager)GetSystemService(SensorService);
                sensorOn = true;

                chronometer = FindViewById<Chronometer>(Resource.Id.cTimer);
                Button btnReplay = FindViewById<Button>(Resource.Id.btnReplay);
                Button btnQuit = FindViewById<Button>(Resource.Id.btnQuit);
                txtOptimalNoOfMoves = FindViewById<TextView>(Resource.Id.txtViewOptNoOfMoves);
                elapsedTime = FindViewById<TextView>(Resource.Id.txtVElapsedTime);
                txtVScore = FindViewById<TextView>(Resource.Id.txtVScore);
                gameDisplay = FindViewById<LinearLayout>(Resource.Id.linLayGameDisplay);


                // Build the game display that the user will interact with;
                maxComponents = Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1);

                Game();

                logic = new GameLogic.DiceRollsLogic(maxComponents);
                txtVScore.Text = "Score: " + 0;
                txtOptimalNoOfMoves.Text = "no. of Rolls: " + numberOfRolls;
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
            createDice(false);          //  Movable disks that the user interacts with;
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
        private void createDice(bool roll)
        {
            int dieWorth = 0;
            for (int i = 0; i < maxComponents; i++)
            {
                if (roll)
                {
                    linearLayout[i].RemoveAllViews();
                    dieWorth = rnd.Next(1, 7);
                    score = score + dieWorth;
                    logic.finalizeMove(dieWorth, i);
                }
                else
                {
                    dieWorth = maxComponents;
                }

                ImageView imgView = getResizedImage(dieWorth);

                // Add the view (imgView) into the linearlayout, with the added effect of the disks appearing in ascending order.
                linearLayout[i].AddView(imgView, 0);

                //Only the top disk is allowed to be clickable.
                //rollDice();
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns an imageView with a desired width;
        private ImageView getResizedImage(int num)
        {
            // Why use Disk.png to determine the width, and height?
            // Disk.png is used because it has been calibrated to a desired shape (width, height).
            // So we simply form an imageView to the shape of Disk.png. 
            ImageView img = new ImageView(this);
            Bitmap bMapDisk = BitmapFactory.DecodeResource(Resources, Resource.Drawable.gameBase);

            // Scale the Bitmap image to the desired specs;
            Bitmap bMapDiskScaled = Bitmap.CreateScaledBitmap(bMapDisk, 400, 400, true);

            //Add the numbers to each Bitmap so the player can differentiate between disks, particularly if there are a large number of them.
            bMapDiskScaled = addNumbersToBitMap(bMapDiskScaled, num);

            img.SetImageBitmap(bMapDiskScaled);
            img.SetScaleType(ImageView.ScaleType.Center);


            return img;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Adds numbers to each of the disks, as some players may find it hard to differentiate between disks.
        // Particularly if there are alot of them.
        private Bitmap addNumbersToBitMap(Bitmap bMapDiskScaled, int count)
        {

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
            canvas.DrawText(String.Format("{0}", count), x, y, paint);
            return bMapDiskScaled;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Will make every top disk (if the LinearLayout has any views) clickable, with drag, and drop capability.
        // Essentially if the LinearLayout has 2 or more disks, the top will be clickable, and the next one down won't be.
        // Notice that there is not loop going through the all LinearLayout's, and setting each ImageViews property.
        // Why? Look at the function createDisks(). The loop in createDisks() does this for us when the ImageView as added, 
        // we need only check the values of the top and the next ImageView (if any), and set their respective properties.  
        private void rollDice()
        {
            numOfGoodShakeCount--;

            if (numOfGoodShakeCount != 0)
            {
                //not enough shakes
                if (!sensorOn)
                {
                    _sensorManager.RegisterListener(this,
                                        _sensorManager.GetDefaultSensor(SensorType.Accelerometer),
                                        SensorDelay.Ui);
                    sensorOn = true;
                }
            }
            else if (numOfGoodShakeCount == 0)
            {

                createDice(true);
                allowableMove();


            }
        }

        // ----------------------------------------------------------------------------------------------------------------
        // Determines if the move is allowable
        // If the move is the disk is dropped into the desired dropzone.
        // Otherwise return the disk back from whence it came (removedFromLinearLayout).  
        private void allowableMove()
        {

            if (logic != null)
            {
                numberOfRolls++;
                txtVScore.Text = "Score: " + score;
                txtOptimalNoOfMoves.Text = "no. of Rolls: " + numberOfRolls;
                if (logic.ifWon())
                {
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
                string playersScore = LeaderBoardInterface.formatLeaderBoardScore("", score.ToString(), Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1), chronometer.Text);
                BeginActivity(typeof(UserInputActivity), GlobalApp.getPlayersScoreVariable(), playersScore);
            }
            catch
            {
                GlobalApp.Alert(this, 0);
            }

        }

        // ----------------------------------------------------------------------------------------------------------------
        /*
            INTERNAL ALERTS FOR Dice Rolls.        
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
                    message = "Moves: " + numberOfRolls + ".\nTime: " + chronometer.Text
                            + "\n\nPress outside of the box.";
                    break;
            }
            return message;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // ----------------------------------------------------------------------------------------------------------------
        //buttons and game responses
        // ----------------------------------------------------------------------------------------------------------------

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
            if (sensorOn)
            {
                _sensorManager.UnregisterListener(this);
                sensorOn = false;
            }
            if (isReplay)
            {
                BeginActivity(typeof(DiceRolls), GlobalApp.getVariableDifficultyName(), Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1));
            }
            else
            {
                BeginActivity(typeof(GameMenuActivity), GlobalApp.getVariableChoiceName(), Intent.GetIntExtra(GlobalApp.getVariableChoiceName(), 0));
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
            lock (_syncLock)
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

                        if (x - e.Values[0] < negNum || num < x - e.Values[0] ||
                        y - e.Values[1] < negNum || num < y - e.Values[1] ||
                        z - e.Values[2] < negNum || num < z - e.Values[2])
                        {

                            _sensorManager.UnregisterListener(this);
                            sensorOn = false;
                            rollDice();
                        }
                    }
                }

            }

        }
        protected override void OnPause()
        {
            base.OnPause();
            _sensorManager.UnregisterListener(this);
            sensorOn = false;

        }
        protected override void OnResume()
        {
            base.OnResume();
            _sensorManager.RegisterListener(this,
                                            _sensorManager.GetDefaultSensor(SensorType.Accelerometer),
                                            SensorDelay.Ui);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event Handler: Will direct the player to the Game menu.
        public override void OnBackPressed()
        {
            determineResponse(false);
        }
    }

}