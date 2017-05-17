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
        System.Timers.Timer timer;
        int delay;
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

                delay = 3;      // Arbitrary number (3 seconds). 
                chronometer = FindViewById<Chronometer>(Resource.Id.cTimer);
                Button btnReplay = FindViewById<Button>(Resource.Id.btnReplay);
                Button btnQuit = FindViewById<Button>(Resource.Id.btnQuit);
                txtOptimalNoOfMoves = FindViewById<TextView>(Resource.Id.txtViewOptNoOfMoves);
                elapsedTime = FindViewById<TextView>(Resource.Id.txtVElapsedTime);
                txtVScore = FindViewById<TextView>(Resource.Id.txtVScore);
                gameDisplay = FindViewById<LinearLayout>(Resource.Id.linLayGameDisplay);


                // Build the game display that the user will interact with;
                switch (Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1))
                {
                    case 2:
                        maxComponents = 5;
                        break;
                    case 3:
                        maxComponents = 4;
                        break;
                    case 4:
                        maxComponents = 3;
                        break;
                    case 5:
                        maxComponents = 2;
                        break;
                    default:
                        maxComponents = 1;
                        break;
                }

                 Game();

                logic = new GameLogic.DiceRollsLogic(maxComponents);
                txtVScore.Text = "Score: " + 0;
                txtOptimalNoOfMoves.Text = "no. of Rolls: " + numberOfRolls;
                chronometer.Visibility = ViewStates.Invisible;

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
        /*
            INTERNAL dice and Layout FOR Dice Rolls.        
        */
        // ----------------------------------------------------------------------------------------------------------------

        // ----------------------------------------------------------------------------------------------------------------
        // builds the game that the user will interact with at runtime. 
        private void Game()
        {
            createFrameLayouts();   
            createImageViews();    
            createLinearLayouts(); 
            createDice(false);      
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Initialize Layout manager (FrameLayout) as so to group an ImageView, and LinearLayout;
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
        // Creates the LinearLayouts that will hold the ImageViews (dice).
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
        // Builds all the dice, and adds then into the first LinearLayout and can make a roll if called after first call;
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

                // Add the view (imgView) into the linearlayout
                linearLayout[i].AddView(imgView, 0);
                
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Returns an imageView with a desired width;
        private ImageView getResizedImage(int num)
        {
            
            // So we simply form an imageView to the shape of dice. 
            ImageView img = new ImageView(this);
            Bitmap bMapDisk = BitmapFactory.DecodeResource(Resources, Resource.Drawable.gameBase);

            // Scale the Bitmap image to the desired specs;
            Bitmap bMapDiskScaled = Bitmap.CreateScaledBitmap(bMapDisk, 400, 400, true);

            //Add the numbers to each Bitmap 
            bMapDiskScaled = addNumbersToBitMap(bMapDiskScaled, num);

            img.SetImageBitmap(bMapDiskScaled);
            img.SetScaleType(ImageView.ScaleType.Center);


            return img;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Adds numbers to each of the dice
        private Bitmap addNumbersToBitMap(Bitmap bMapDiskScaled, int count)
        {

            // The top left hand corner of the image of the number is specified by the (x,y) 
            // Different values were tested to find the best size.
            float x = (float)(bMapDiskScaled.Width - bMapDiskScaled.Width * .78);
            float y = (float)(bMapDiskScaled.Height - bMapDiskScaled.Height / 6);

            // The bitmap must be immutable otherwise it will throw an exception, if changes are permitted. 
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
        /*
            INTERNAL rules FOR Dice Rolls.        
        */
        // ----------------------------------------------------------------------------------------------------------------

        // ----------------------------------------------------------------------------------------------------------------
        // if phone has been shaken well enough it will alow the dice to roll else more shakes needed 
        private void rollDice()
        {
            numOfGoodShakeCount--;

            if (numOfGoodShakeCount != 0)
            {
                //not enough shakes
                if (!sensorOn)
                {
                    sensorSwitch(true);
                }
            }
            else if (numOfGoodShakeCount == 0)
            {
                createDice(true);
                allowableMove();
                sensorSwitch(true);
                numOfGoodShakeCount = 5;

            }
        }

        // ----------------------------------------------------------------------------------------------------------------
        // Determines if the move is allowable and sees if there are no dupes
        private void allowableMove()
        {

            if (logic != null)
            {
                numberOfRolls++;
                txtVScore.Text = "Score: " + score;
                txtOptimalNoOfMoves.Text = "no. of Rolls: " + numberOfRolls;
                if (logic.ifWon())
                {
                    CountDown();
                    timer.Enabled = false;
                    end();
                }
                else
                {
                    Alert(0, 0);
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
                GlobalApp.BeginActivity(this, typeof(UserInputActivity), GlobalApp.getPlayersScoreVariable(), playersScore,
                    GlobalApp.getVariableChoiceName(), Intent.GetIntExtra(GlobalApp.getVariableChoiceName(), 0));
            }
            catch
            {
                GlobalApp.Alert(this, 0);
            }

        }
        // ----------------------------------------------------------------------------------------------------------------
        /*
            INTERNAL Timer FOR Dice Rolls.        
        */
        // ----------------------------------------------------------------------------------------------------------------

        private void CountDown()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
        }
        // Event handler
        private void OnTimedEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            delay--;
            if (delay == 0)
            {
                timer.Dispose();
                GlobalApp.BeginActivity(this, typeof(MainActivity), "", 0);
            }
        }

        
        // ----------------------------------------------------------------------------------------------------------------
        /*
            INTERNAL buttons and th FOR Dice Rolls.        
        */
        //----------------------------------------------------------------------------------------------------------------

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
                sensorSwitch(false);
            }
            if (isReplay)
            {
                GlobalApp.BeginActivity(this, typeof(DiceRolls), GlobalApp.getVariableDifficultyName(), Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1));
            }
            else
            {
                GlobalApp.BeginActivity(this, typeof(GameMenuActivity), GlobalApp.getVariableChoiceName(), Intent.GetIntExtra(GlobalApp.getVariableChoiceName(), 0));
            }
        }
        protected override void OnPause()
        {
            base.OnPause();
            sensorSwitch(false);
        }
        protected override void OnResume()
        {
            base.OnResume();
            sensorSwitch(true);
        }
        public override void OnBackPressed()
        {
            determineResponse(false);
        }
        // ----------------------------------------------------------------------------------------------------------------
        /*
            INTERNAL Sensors FOR Dice Rolls.        
        */
        // ----------------------------------------------------------------------------------------------------------------
        // Continuously update the displayed time.
        protected void chronometerOnTick(Object sender, EventArgs arg)
        {
            elapsedTime.Text = String.Format("{0}", "Time: " + chronometer.Text);
        }

        public void sensorSwitch(bool turnOn)
        {
            if (turnOn)
            {
                _sensorManager.RegisterListener(this,
                                            _sensorManager.GetDefaultSensor(SensorType.Accelerometer),
                                            SensorDelay.Ui);
                sensorOn = true;
            }
            else
            {

                _sensorManager.UnregisterListener(this);
                sensorOn = false;
            }
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy) { }
        //checks if phone has been moved in any direction to that directions previous point
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

                            sensorSwitch(false);
                            rollDice();
                        }
                    }
                }

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
                    title = "Roll again";
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
                    message = "You have rolled two or more ove the same dice."
                            + "\n\nYou get to roll again.";
                    break;
                case 1:
                    message = "Moves: " + numberOfRolls + ".\nTime: " + chronometer.Text
                            + "\n\nPress outside of the box.";
                    break;
            }
            return message;
        }
    }

}