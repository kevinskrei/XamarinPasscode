using System;
using Android.App;
using Passcode.Google;
using Android.Runtime;
using Android.Widget;

namespace Passcode.Android
{
	[Application]
	public class App : Application
	{
		public App(IntPtr handle, JniHandleOwnership transfer)
			: base(handle, transfer)
		{
		}

		public PasscodeManager PasscodeManager;

		public override void OnCreate()
		{
			base.OnCreate();

			PasscodeManager = new Passcode.Google.PasscodeManager();
			PasscodeManager.PasscodeLength = 6;
			PasscodeManager.ShowCancelButton = true;

			PasscodeManager.PasscodeEntered += (object sender, Passcode.Google.PasscodeEnteredEventArgs e) => {
				if(e.GetPasscodeAsString() == "111111")
				{
					PasscodeManager.Dismiss();
				}
				else
				{
					PasscodeManager.WrongPasscode();
				}
			};
			PasscodeManager.PasscodeCancelled += (sender, e) => {
				PasscodeManager.Dismiss();
				Toast.MakeText(this, "You must sign in", ToastLength.Long).Show();
			};
		}
	}
}

