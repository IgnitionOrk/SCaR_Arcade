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

namespace SCaR_Arcade
{
    static class ScarConnectionManager
    {
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