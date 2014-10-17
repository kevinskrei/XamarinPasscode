using System;
using Android.App;
using Android.Content;

namespace Passcode.Google
{
	public class PasscodeManager
	{
		public event EventHandler<PasscodeEnteredEventArgs> PasscodeEntered;
		public event EventHandler PasscodeCancelled;

		public int PasscodeLength { get; set; }
		public bool ShowCancelButton {get;set;}
		public int AnimationEnterInResource {get;set;}
		public int AnimationEnterOutResource {get;set;}
		public int AnimationExitInResource {get;set;}
		public int AnimationExitOutResource {get;set;}

		internal static readonly string ACTION_COMPLETED = "com.skrei.passcode.completed";
		internal static readonly string ACTION_CANCELLED = "com.skrei.passcode.cancelled";
		internal static readonly string EXTRA_CODE = "com.skrei.passcode.code";

		private MessageListener _messageListener;
		public PasscodeManager()
		{
			SetDefaults();
			_messageListener = new MessageListener(ACTION_CANCELLED, ACTION_COMPLETED);
			_messageListener.MessageReceived += HandleMessageReceived;
		}

		private void SetDefaults()
		{
			ShowCancelButton = false;
			AnimationEnterInResource = 0;
			AnimationEnterOutResource = 0;
			AnimationExitInResource = 0;
			AnimationExitOutResource = 0;
		}

		void HandleMessageReceived (object sender, Android.Content.Intent e)
		{
			if(e.Action == ACTION_CANCELLED)
			{
				if(PasscodeCancelled != null)
				{
					PasscodeCancelled.Invoke(this, EventArgs.Empty);
				}
			}
			else if(e.Action == ACTION_COMPLETED)
			{
				if(PasscodeEntered != null)
				{
					PasscodeEntered.Invoke(this, new PasscodeEnteredEventArgs(e.GetIntArrayExtra(EXTRA_CODE)));
				}
			}
		}

		public void Show(Activity activity)
		{
			Validate();

			var i = FillIntent();
			activity.StartActivity(i);
			activity.OverridePendingTransition(AnimationEnterInResource, AnimationEnterOutResource);
		}

		private void Validate()
		{
			if(PasscodeLength < 2 || PasscodeLength > 8)
			{
				throw new ArgumentOutOfRangeException("PasscodeLength", "Passcode length must be between 2 and 8 inclusive");
			}
		}

		private Intent FillIntent()
		{
			Intent i = new Intent(Application.Context, typeof(PasscodeActivity));
			i.PutExtra(PasscodeActivity.EXTRA_LENGTH, PasscodeLength);
			i.PutExtra(PasscodeActivity.EXTRA_SHOW_CANCEL, ShowCancelButton);
			i.PutExtra(PasscodeActivity.EXTRA_IN_ANIMATION, AnimationExitInResource);
			i.PutExtra(PasscodeActivity.EXTRA_OUT_ANIMATION, AnimationExitOutResource);
			return i;
		}

		public void Dismiss()
		{
			Application.Context.SendBroadcast(new Intent(PasscodeActivity.ACTION_DISMISS));
		}

		public void WrongPasscode()
		{
			Application.Context.SendBroadcast(new Intent(PasscodeActivity.ACTION_SHAKE));
		}

	}
}

