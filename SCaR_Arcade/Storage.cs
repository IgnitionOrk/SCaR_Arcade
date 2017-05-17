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
/// <summary>
/// Creator: Ryan Cunneen
/// Student number: 3179234
/// Date modified: 15-May-2017
/// /// Date created: 15-May-2017
/// </summary>
namespace SCaR_Arcade
{
    interface Storage
    {
        void addData(string gameFilePath, string score);
        void removeData(string path, int position);
        void assignGameFilePaths(Game g);
        string readDescription(string fileName);
        List<string> readData(string path);
        void updateData(string path, int atPosition);
        bool reachedLimit(string path, int limit);
    }
}