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
using Android.Net;

/// <summary>
/// Creator: Ryan Cunneen
/// Student number: 3179234
/// Date modified: 15-May-2017
/// /// Date created: 15-May-2017
/// </summary>
namespace SCaR_Arcade
{
    static class ScarConnectionManager
    {
        // ----------------------------------------------------------------------------------------------------------------
        // Determines if the device has an internet connection. 
        public static bool hasInternetConnection()
        {
            try
            {
                ConnectivityManager connectManager = (ConnectivityManager)Context.ConnectivityService;
                NetworkInfo networkInfo = connectManager.ActiveNetworkInfo;
                return networkInfo.IsConnected;
            }
            catch
            {
                return false;
            }
        }
    }
}