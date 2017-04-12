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
/// <summary>
/// Creator: Ryan Cunneen
/// Student number: 3179234
/// Date created: 25-Mar-17
/// Date modified: 10-Apr-17
/// </summary>
namespace SCaR_Arcade
{
    [Activity(
        Label = "TowersOfHanoiActivity",
        MainLauncher = false,
        ScreenOrientation = Android.Content.PM.ScreenOrientation.Landscape,
        Theme = "@android:style/Theme.NoTitleBar"
    )]
    public class TowersOfHanoiActivity :  Activity, View.IOnLongClickListener, View.IOnDragListener
    {
        // ----------------------------------------------------------------------------------------------------------------
        // Instances of TowersOfHanoiActivity;
        private Player player;
        private GameLogic logic;
        private Chronometer chronometer;
        private TextView elapsedTime;
        private TextView txtVScore;
        private LinearLayout gameDisplay;
        private FrameLayout[] frameLayout;
        private LinearLayout[] linearLayout;
        private ImageView[] poles;
        private const int MAXCOMPONENTS = 3;

        private Color[] cPalette ={
            Color.AliceBlue,
            Color.Coral,
            Color.Gold,
            Color.Honeydew,
            Color.Pink,
            Color.Tan,
            Color.WhiteSmoke,
            Color.Blue
        };

        // Used when a drag and drop event has occured to store data. 
        private View disk;
        private LinearLayout removedFromLinearLayout;

        protected override void OnCreate(Bundle bundle)
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
            player = new Player();
            logic = new GameLogic(Convert.ToInt32(Intent.GetStringExtra("gameDifficulty")));
            txtOptimalNoOfMoves.Text = string.Format("{0}", "Optimal no. of moves: " + logic.calOptimalNoOfMoves(Convert.ToInt32(Intent.GetStringExtra("gameDifficulty"))));
            txtVScore.Text = "No. of moves: " + 0;
            chronometer.Visibility = Android.Views.ViewStates.Invisible;


            // Event handlers:
            btnReplay.Click += btnReplayOnClick;
            btnQuit.Click += btnQuitOnClick;
            chronometer.ChronometerTick += chronometerOnTick;

            // Begin the timer;
            chronometer.Start();

            System.Diagnostics.Debug.WriteLine("HAHAHAHAHAHAHAHAAHHERE " + Intent.GetStringExtra("gameDifficulty"));
        }
        // ----------------------------------------------------------------------------------------------------------------
        // builds the game that the user will interact with at runtime. 
        // Initially the game had multiple instance variables for each ImageViews (Poles), and LinearLayouts (vertical)
        // By removing them from the axml file, and bulding them at runtime the Activity file's simplicity, and readability has been enhanced.
        private void Game()
        {
            createFrameLayouts();   // Will Allow both ImageViews (Poles) and ImageView (Disks) to be placed on top of eachother.
            createImageViews();       // Images of the poles that are displayed. 
            createLinearLayouts();   // (Vertical) Stacks that will hold the disks. 
            createDisks();                 //  Movable disks that the user interacts with;
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
                poles[i].SetImageResource(Resource.Drawable.Pole);
                poles[i].SetScaleType(ImageView.ScaleType.FitCenter);
                poles[i].Enabled = false;
                frameLayout[i].AddView(poles[i], imageViewParameters);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Creates the LinearLayouts (vertical) that will hold the ImageViews (disks).
        private void createLinearLayouts()
        {
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
                linearLayout[i].SetPadding(0, 0, 0, 100);
                linearParameters.SetMargins(0, 0, 0, 50);
                linearLayout[i].LayoutParameters = linearParameters;
                frameLayout[i].AddView(linearLayout[i], linearParameters);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        // Builds all the disks, and adds then into the first LinearLayout;
        private void createDisks()
        {
            int numberOfDisks = Convert.ToInt32(Intent.GetStringExtra("gameDifficulty"));
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
            int number = Convert.ToInt32(Intent.GetStringExtra("gameDifficulty")) - count;
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
        private Bitmap changeBitmapColour(Bitmap bit, int index)
        {
            for (int xCoord = 0; xCoord < bit.Width; xCoord++)
            {
                for (int yCoord = 0; yCoord < bit.Height; yCoord++)
                {
                    bit.SetPixel(xCoord, yCoord, cPalette[index]);
                }
            }
            return bit;
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
        // Determines if the move is allowable
        // If the move is the disk is dropped into the desired dropzone.
        // Otherwise return the disk back from whence it came (removedFromLinearLayout).  
        private void allowableMove(View view)
        {
            int indexFrom = findLinearLayoutIndex(removedFromLinearLayout);
            int indexTo = findLinearLayoutIndex((view as LinearLayout));
            if (droppedOutsideGameScreen(view as LinearLayout))
            {
                // The player has accidentally dropped the disk outside of the game screen.
                // If they have the game will automatically remove the disk.
                // Therefore, we must save it back into the LinearLayout from whence it came.
                removedFromLinearLayout.AddView(disk, 0);
                // Show an alert.
                AlertDialog.Builder adb = new AlertDialog.Builder(this);
                adb.SetTitle("Dropzone not allowed!");
                adb.SetMessage("You have dropped the disk outside of the game screen.");
                adb.Show();
            }
            else if (logic.canDropDisk(indexFrom, indexTo))
            {
                //Essentially we now save the moves into the game logic object 'logic' for further use.
                logic.finalizeMove(indexFrom, indexTo);

                // Add the new disk into the @param view.
                addToNewDropzone(view);

                // Now we set the appropriate properties of the disks for each LinearLayout.
                topDiskIsOnlyClickable();

                // If all goes well when dropping the disk into the LinearLayout,
                // increment the number of moves the user has made;
                player.incrementNumberOfMoves();
                txtVScore.Text = "No. of moves: " + player.getNumberOfMoves();
            }
            else
            {
                // Show an alert.
                AlertDialog.Builder adb = new AlertDialog.Builder(this);
                adb.SetTitle("Move not allowed!");
                adb.SetMessage("You cannot place larger disks on top of smaller disks");
                adb.Show();
                // Adding the disk, that was just removed back into the linearlayout it came from at the top of the stack;
                removedFromLinearLayout.AddView(disk, 0);
            }

            if (logic.ifWon())
            {
                determineResponse(false);
            }
        }
        // ----------------------------------------------------------------------------------------------------------------
        private bool droppedOutsideGameScreen(LinearLayout view)
        {
            int iFind = 0;
            for (int i = 0; i < linearLayout.Length; i++)
            {
                if (linearLayout[i] == view)
                {
                    iFind++;
                }
            }
            return iFind == 0;
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
            chronometer.Stop();
            chronometer = null;
            logic.deleteBoard();
            logic = null;
            Intent intent = null;
            if (isReplay)
            {
                intent = new Intent(this, typeof(TowersOfHanoiActivity));
                intent.PutExtra("gameDifficulty", Intent.GetStringExtra("gameDifficulty"));
            }
            else
            {
                intent = new Intent(this, typeof(GameMenuActivity));
            }
            StartActivity(intent);
        }
        // ----------------------------------------------------------------------------------------------------------------
        protected void chronometerOnTick(Object sender, EventArgs arg)
        {
            elapsedTime.Text = String.Format("{0}", "Time: " + chronometer.Text);
        }
        /// <summary>
        /// Creator: Ryan Cunneen
        /// Student number: 3179234
        /// Date created: 25-Mar-17
        /// Date modified: 10-Apr-17
        /// </summary>
        private class GameLogic
        {
            // Jagged array.
            private int[][] gameBoard;
            private int height;
            // ----------------------------------------------------------------------------------------------------------------
            // Constructor:
            public GameLogic(int height)
            {
                this.height = height;
                gameBoard = new int[MAXCOMPONENTS][];
                initializeGameBoard();
            }
            // ----------------------------------------------------------------------------------------------------------------
            // Remove the components of the game;
            public void deleteBoard()
            {
                gameBoard = null;
            }
            // ----------------------------------------------------------------------------------------------------------------
            // Create the game but in array form (with populated integers as data).
            // The idea is index 0 is the furtherest pole (left), with MAXCOMPONENTS representing the last (right) pole.
            private void initializeGameBoard()
            {
                for (int x = 0; x < MAXCOMPONENTS; x++)
                {
                    if (x == 0)     // Furthest left pole. 
                    {
                        gameBoard[x] = new int[height];
                        for (int y = 0; y < height; y++)
                        {
                            gameBoard[x][y] = height - y;  // Add in values (descending) into the array index (ascending).
                        }
                    }
                    else
                    {
                        // Every other pole will not have any disks saved into them.
                        // Therefore, their length is 0.
                        gameBoard[x] = new int[0];
                    }
                }
            }
            // ----------------------------------------------------------------------------------------------------------------
            // Determines the optimal number of moves the player can beat the game in.
            public int calOptimalNoOfMoves(int number)
            {
                int optimalNoOfMoves = (int)Math.Pow(2, number);
                optimalNoOfMoves -= 1;
                return optimalNoOfMoves;
            }
            // ----------------------------------------------------------------------------------------------------------------
            // Determines if the game was been won.
            public bool ifWon()
            {
                return gameBoard[gameBoard.Length - 1].Length == height;
            }
            // ----------------------------------------------------------------------------------------------------------------
            // Determines whether the player is allowed to make their desired move. 
            public bool canDropDisk(int from, int to)
            {
                // Return the values from the top of there respective arrays. 
                int fromTopDisk = topIndexValue(from);
                int toTopDisk = topIndexValue(to);
                if (toTopDisk == 0)   // The array @param 'to' has length 0. Therefore, you can always be able to drop.
                {
                    return true;
                }
                else
                {
                    // Why is the boolean condition defined as <=, and no <?
                    // Because we might be dropping the disk back into the same LinearLayout from whence it came. 
                    return fromTopDisk <= toTopDisk;
                }
            }
            // ----------------------------------------------------------------------------------------------------------------
            // Determines the 'top' value of the array (the last value in the array).
            private int topIndexValue(int index)
            {
                int value = 0;
                if (gameBoard[index] != null && gameBoard[index].Length != 0)
                {
                    // The jagged array can be thought of a representation of values in the cartesian plane.
                    // gameBoard[index] can be thought of as the x component;
                    // With gameBoard[index].Length - 1 as the y component;
                    // Essentially we are finding the y value specified by the x component. 
                    value = gameBoard[index][gameBoard[index].Length - 1];
                }
                return value;
            }
            // ----------------------------------------------------------------------------------------------------------------
            // Removes, and adds the move made by the player. 
            public void finalizeMove(int from, int to)
            {
                // Remove the top value (last value in the array).
                int iMove = this.remove(from);

                // Re-add the value back into the gameBoard.
                add(to, iMove);
            }
            // ----------------------------------------------------------------------------------------------------------------
            // Adds the @param iMove into the array specified by the @param to index;
            private void add(int to, int iMove)
            {
                int[] temp = copy(gameBoard[to], true);

                // Save iMove into the temporary array;
                temp[temp.Length - 1] = iMove;

                //Save the temporary array back into the gameBoard;
                gameBoard[to] = temp;
            }

            // ----------------------------------------------------------------------------------------------------------------
            // Remove the integer data from the array specified by the @param from index.
            private int remove(int from)
            {
                // Getting the top value from the array specifed by the @param from.
                // Before we save the new data back into the gameBoard;
                int iDisk = gameBoard[from][gameBoard[from].Length - 1];

                // Now we save the new array of integers (by creating a temporary array) into the gameBoard.
                gameBoard[from] = copy(gameBoard[from], false);
                return iDisk;
            }
            // ----------------------------------------------------------------------------------------------------------------
            // Copies a temporary array of integers.
            // The @param ifAdd specifies if we need to increase or decrease by 1 the length of the @param array. 
            private int[] copy(int[] array, bool ifAdd)
            {
                int[] temp;
                if (ifAdd)
                {
                    temp = new int[array.Length + 1];
                    // Because we are adding in only the contents of @param array.
                    // So we can have array.length + 1;
                    // Otherwise an exception will be thrown;
                    for (int i = 0; i < array.Length; i++)
                    {
                        temp[i] = array[i];
                    }
                    // The last index of the temporary array has not been populate with data as of yet.
                }
                else
                {
                    temp = new int[array.Length - 1];
                    for (int i = 0; i < temp.Length; i++)
                    {
                        temp[i] = array[i];
                    }
                }
                return temp;
            }
        }
        /// <summary>
        /// Creator: Ryan Cunneen
        /// Student number: 3179234
        /// Date created: 25-Mar-17
        /// Date modified: 10-Apr-17
        /// </summary>
        private class Player
        {
            private int numberOfMoves;
            private string time;
            // ----------------------------------------------------------------------------------------------------------------
            // Constructor:
            public Player()
            {
                this.numberOfMoves = 0;
                this.time = "00:00";
            }
            // ----------------------------------------------------------------------------------------------------------------
            public int getNumberOfMoves()
            {
                return this.numberOfMoves;
            }
            // ----------------------------------------------------------------------------------------------------------------
            public void incrementNumberOfMoves()
            {
                this.numberOfMoves++;
            }
            // ----------------------------------------------------------------------------------------------------------------
            public void timed(string time)
            {
                this.time = time;
            }
        }
    }
}