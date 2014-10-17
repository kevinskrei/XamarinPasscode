using System;
using Android.App;
using Android.Runtime;
using Passcode.Google;
using Android.Widget;

namespace Passcode.Android.Customized
{
	[Application]
	public class App : Application
	{
		public App(IntPtr handle, JniHandleOwnership transfer)
			: base(handle, transfer)
		{
		}

		public PasscodeManager PasscodeManager {get; private set;}

		public override void OnCreate()
		{
			base.OnCreate();

			PasscodeManager = new Passcode.Google.PasscodeManager();
			PasscodeManager.PasscodeLength = 8;
			PasscodeManager.AnimationEnterInResource = Resource.Animation.fadein;
			PasscodeManager.AnimationEnterOutResource = Resource.Animation.fadeout;
			PasscodeManager.AnimationExitInResource = Resource.Animation.fadein;
			PasscodeManager.AnimationExitOutResource = Resource.Animation.fadeout;

			PasscodeManager.PasscodeEntered += (object sender, Passcode.Google.PasscodeEnteredEventArgs e) => {
				if(e.GetPasscodeAsString() == "11111111")
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

