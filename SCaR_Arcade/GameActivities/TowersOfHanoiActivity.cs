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
/// <summary>
/// Creator: Ryan Cunneen
/// Student number: 3179234
/// Date created: 25-Mar-17
/// Date modified: 10-Apr-17
/// </summary>
namespace SCaR_Arcade.GameActivities
{
    [Activity(
        Label = "TowersOfHanoiActivity",
        MainLauncher = false,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
        Theme = "@android:style/Theme.NoTitleBar"
    )]
    public class TowersOfHanoiActivity :  Activity, View.IOnLongClickListener, View.IOnDragListener
    {
        private GameLogic.TowersOfHanoiLogic logic;
        private Chronometer chronometer;
        private TextView elapsedTime;
        private TextView txtVScore;
        private LinearLayout gameDisplay;
        private FrameLayout[] frameLayout;
        private LinearLayout[] linearLayout;
        private ImageView[] poles;
        private const int MAXCOMPONENTS = 3;
        private int numberOfMoves = 0;
        // Used when a drag and drop event has occured to store data. 
        private View disk;
        private LinearLayout removedFromLinearLayout;

        protected override void OnCreate(Bundle bundle)
        {
            try
            {
                base.OnCreate(bundle);
                SetContentView(Resource.Layout.TowersOfHanoi);

                Button btnReplay = FindViewById<Button>(Resource.Id.btnReplay);
                Button btnQuit = FindViewById<Button>(Resource.Id.btnQuit);
                TextView txtOptimalNoOfMoves = FindViewById<TextView>(Resource.Id.txtViewOptNoOfMoves);
                chronometer = FindViewById<Chronometer>(Resource.Id.cTimer);
                elapsedTime = FindViewById<TextView>(Resource.Id.txtVElapsedTime);
                txtVScore = FindViewById<TextView>(Resource.Id.txtVScore);
                gameDisplay = FindViewById<LinearLayout>(Resource.Id.linLayGameDisplay);
                // Build the game display that the user will interact with;
                Game();

                // Initializing data for the game.
                logic = new GameLogic.TowersOfHanoiLogic(MAXCOMPONENTS, Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1));
                txtOptimalNoOfMoves.Text = string.Format("{0}", "Optimal no. of moves: " + logic.calOptimalNoOfMoves(Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1)));
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
            frameLayout = new FrameLayout[MAXCOMPONENTS];

            LinearLayout.LayoutParams frameParameters = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.WrapContent,
                LinearLayout.LayoutParams.MatchParent,
                1
            );
            for (int i = 0; i < MAXCOMPONENTS; i++)
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
            poles = new ImageView[MAXCOMPONENTS];
            for (int i = 0; i < MAXCOMPONENTS; i++)
            {
                poles[i] = new ImageView(this);
                poles[i].SetScaleType(ImageView.ScaleType.FitCenter);
                poles[i].Enabled = false;
                frameLayout[i].AddView(poles[i], imageViewParameters);
                if (i == MAXCOMPONENTS - 1)
                {
                    // Make the target pole a different colour.
                    // As so the player can differentiate which is the target pole. 
                    poles[i].SetImageResource(Resource.Drawable.TargetPole);
                }
                else
                {
                    poles[i].SetImageResource(Resource.Drawable.Pole);
                }
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Creates the LinearLayouts (vertical) that will hold the ImageViews (disks).
        private void createLinearLayouts()
        {
            int paddingHeight = Resources.DisplayMetrics.HeightPixels / 6;

            linearLayout = new LinearLayout[MAXCOMPONENTS];
            LinearLayout.LayoutParams linearParameters = new LinearLayout.LayoutParams(
                LinearLayout.LayoutParams.MatchParent,
                LinearLayout.LayoutParams.MatchParent
            );
            for (int i = 0; i < MAXCOMPONENTS; i++)
            {
                linearLayout[i] = new LinearLayout(this);
                linearLayout[i].SetMinimumWidth(25);
                linearLayout[i].SetMinimumHeight(25);
                linearLayout[i].Orientation = Orientation.Vertical;
                linearLayout[i].SetGravity(Android.Views.GravityFlags.Bottom);
                linearLayout[i].SetHorizontalGravity(Android.Views.GravityFlags.Center);
                linearLayout[i].SetOnDragListener(this);
                linearLayout[i].SetPadding(0, 0, 0, paddingHeight);
                frameLayout[i].AddView(linearLayout[i], linearParameters);
            }
        }
    // ----------------------------------------------------------------------------------------------------------------
    // Builds all the disks, and adds then into the first LinearLayout;
    private void createDisks()
        {
            int numberOfDisks = Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(),1);
            for (int i = 0; i < numberOfDisks; i++)
            {
                ImageView imgView = getResizedImage(i);

                // Add the view (imgView) into the linearlayout, with the added effect of the disks appearing in ascending order.
                linearLayout[0].AddView(imgView, 0);

                //Only the top disk is allowed to be clickable.
                topDiskIsOnlyClickable();
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
            Bitmap bMapDisk = BitmapFactory.DecodeResource(Resources, Resource.Drawable.Disk);

            // Determine the width of the new Bitmap image;
            int newWidth = determineNewWidth(bMapDisk.Width, count);

            // Scale the Bitmap image to the desired specs;
            Bitmap bMapDiskScaled = Bitmap.CreateScaledBitmap(bMapDisk, newWidth, bMapDisk.Height, true);
            
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
            int number = Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(),1) - count;
            // The top left hand corner of the image of the number is specified by the (x,y)
            // the number will not be placed exactly in the middle, instead it will be slightly off centre. 
            // The 0.15 (15%), and 0.10 (10%) have been determined by testing different values
            // To find the optimal (x,y) values so the image looks to be in the middle of the Bitmap. 
            float x = (float)((bMapDiskScaled.Width / 2) - (bMapDiskScaled.Height * 0.15));
            float y = (float)(bMapDiskScaled.Height - (bMapDiskScaled.Height * 0.10));

            // The bitmap must be immutable otherwise it will through an exception, if changes are permitted. 
            bMapDiskScaled = bMapDiskScaled.Copy(Bitmap.Config.Argb8888, true);
            Canvas canvas = new Canvas(bMapDiskScaled);
            Paint paint = new Paint();
            paint.Color = Color.Black;

            // Again different values were tested to find the best text size. 
            paint.TextSize = (int)(bMapDiskScaled.Height - (bMapDiskScaled.Height * 0.05));

            // Now draw the number at the specified x, and y coordinates. 
            canvas.DrawText(String.Format("{0}", number), x, y, paint);
            return bMapDiskScaled;
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Calculate the new width for a Bitmap image;
        // the image will be 5% shorter, than its predecessor;
        private int determineNewWidth(int currentWidth, int count)
        {
            int fivePercentWidth = (int)(currentWidth * 0.10);
            // Continously remove 5% from the current width;
            for (int i = 0; i < count; i++)
            {
                currentWidth = (int)(currentWidth - fivePercentWidth);
            }
            return currentWidth;
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
        // Executes after the user initiates a Drag event.
        // Whilst the drag is still in progress
        // Reference: https://forums.xamarin.com/discussion/63590/drag-and-drop-in-android-c
        public bool OnDrag(View v, DragEvent args)
        {          
            switch (args.Action)
            {
                case DragAction.Entered:
                    return true;
                case DragAction.Exited:
                    return true;
                case DragAction.Ended:
                    // Essentially if the Player moves the disk out of the game screen
                    // the disk will disappear;
                    returnDiskToPlacement(args);
                    return true;
                case DragAction.Started:
                    return true;
                case DragAction.Drop:
                    // Parameter v is of type LinearLayout and is defined as the dropzone
                    // the new disk will be added to. 
                        allowableMove(v);
                    return true;
                default:
                    return false;
            }
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
                    topDiskIsOnlyClickable();
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
            chronometer.Stop();
            Alert(1, 1);
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Event Handler: Will direct the player to the Game menu.
        public override void OnBackPressed()
        {
            BeginActivity(typeof(GameMenuActivity),"", 0);
        }
        // ----------------------------------------------------------------------------------------------------------------
        /*
            INTERNAL ALERTS FOR TOWERS OF HANOI.        
        */
        // Display an error message for the invalid move.
        private void Alert(int iTitle, int iMessage)
        {
            string title = this.getAlertTitle(iTitle);
            string message = this.getAlertMessage(iMessage);
            // Show an alert.
            AlertDialog.Builder adb = new AlertDialog.Builder(this);
            adb.SetMessage(message);
            adb.SetTitle(title);
            adb.Show();
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
        private void topDiskIsOnlyClickable()
        {
            foreach (LinearLayout lin in linearLayout)
            {
                if (lin.ChildCount > 0)     // Only one ImageView in the LinearLayout.
                {
                    (lin.GetChildAt(0) as ImageView).Clickable = true;
                    (lin.GetChildAt(0) as ImageView).SetOnLongClickListener(this);
                    if (lin.ChildCount > 1) // two or more ImageViews in the LinearLayout. 
                    {
                        (lin.GetChildAt(1) as ImageView).Clickable = false;
                        (lin.GetChildAt(1) as ImageView).SetOnLongClickListener(null);
                    }
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
            if (isReplay)
            {
                BeginActivity(typeof(TowersOfHanoiActivity), GlobalApp.getVariableDifficultyName(), Intent.GetIntExtra(GlobalApp.getVariableDifficultyName(), 1));
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
        // Continuously update the displayed time.
        protected void chronometerOnTick(Object sender, EventArgs arg)
        {
            elapsedTime.Text = String.Format("{0}", "Time: " + chronometer.Text);
        }
    }
}