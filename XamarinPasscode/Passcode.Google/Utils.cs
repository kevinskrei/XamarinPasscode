using System;
using Android.Util;
using Android.App;

namespace Passcode.Google
{
	internal static class Utils
	{
		internal static int GetDp(int px)
		{
			return (int)TypedValue.ApplyDimension(ComplexUnitType.Dip, px, Application.Context.Resources.DisplayMetrics);
		}
	}
}

