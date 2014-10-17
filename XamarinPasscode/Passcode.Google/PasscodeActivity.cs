
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
using Android.Util;
using Android.Views.Animations;
using Android.Graphics.Drawables;

namespace Passcode.Google
{
	[Activity]			
	internal class PasscodeActivity : Activity
	{
		public const string ACTION_SHAKE = "com.passcode.google.shake";
		public const string ACTION_DISMISS = "com.passcode.google.dismiss";

		public const string EXTRA_LENGTH = "com.passcode.passcodelength";
		public const string EXTRA_SHOW_CANCEL = "com.passcode.showcancel";
		public const string EXTRA_IN_ANIMATION = "com.passcode.inanimation";
		public const string EXTRA_OUT_ANIMATION = "com.passcode.outanimation";

		private const string KEY_PASSCODE_ENTERED = "com.passcode.passcodeentered";
		private const string KEY_PASSCODE_COUNT = "com.passcode.count";

		private LinearLayout _selectedLayout;
		private MessageListener _listener;
		private int _passcodeLength;
		private int _numbersEntered;
		private int[] _passcodeEntered;
		private Animation _shakeAnimation;
		private int _animationIn = 0;
		private int _animationOut = 0;

		private RoundedButton[] _numberButtons;
		private Button _clearButton;
		private Button _cancelButton;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			RequestWindowFeature(WindowFeatures.NoTitle);
			SetContentView(Resource.Layout.passcode_main);

			_listener = new MessageListener(ACTION_SHAKE, ACTION_DISMISS);
			_listener.MessageReceived += HandleMessageReceived;
			_numbersEntered = 0;

			InitButtons();
			_selectedLayout = FindViewById<LinearLayout>(Resource.Id.passcode_toplayout);

			if(Intent != null)
			{
				bool showCancel = Intent.GetBooleanExtra(EXTRA_SHOW_CANCEL, false);
				_cancelButton.Visibility = showCancel ? ViewStates.Visible : ViewStates.Invisible;

				_passcodeLength = Intent.GetIntExtra(EXTRA_LENGTH, 4);
				_animationIn = Intent.GetIntExtra(EXTRA_IN_ANIMATION, 0);
				_animationOut = Intent.GetIntExtra(EXTRA_OUT_ANIMATION, 0);
			}

			HideUnusedCircles();

			_passcodeEntered = new int[_passcodeLength];
			_shakeAnimation = AnimationUtils.LoadAnimation(this, Resource.Animation.passcode_shake);
		}

		private void InitButtons()
		{
			_numberButtons = new RoundedButton[10];
			_numberButtons[0] = FindViewById<RoundedButton>(Resource.Id.passcode_button0);
			_numberButtons[1] = FindViewById<RoundedButton>(Resource.Id.passcode_button1);
			_numberButtons[2] = FindViewById<RoundedButton>(Resource.Id.passcode_button2);
			_numberButtons[3] = FindViewById<RoundedButton>(Resource.Id.passcode_button3);
			_numberButtons[4] = FindViewById<RoundedButton>(Resource.Id.passcode_button4);
			_numberButtons[5] = FindViewById<RoundedButton>(Resource.Id.passcode_button5);
			_numberButtons[6] = FindViewById<RoundedButton>(Resource.Id.passcode_button6);
			_numberButtons[7] = FindViewById<RoundedButton>(Resource.Id.passcode_button7);
			_numberButtons[8] = FindViewById<RoundedButton>(Resource.Id.passcode_button8);
			_numberButtons[9] = FindViewById<RoundedButton>(Resource.Id.passcode_button9);

			foreach(var button in _numberButtons)
			{
				button.Click += HandleNumberClick;
			}

			_clearButton = FindViewById<Button>(Resource.Id.passcode_clear);
			_cancelButton = FindViewById<Button>(Resource.Id.passcode_cancel);
			_clearButton.Click += HandleClearClick;
			_cancelButton.Click += HandleCancelClick;
		}

		void HandleNumberClick (object sender, EventArgs e)
		{
			_passcodeEntered[_numbersEntered] = Array.FindIndex(_numberButtons, x => x == sender);
			_numbersEntered++;
			UpdateFilledCircles();
			CheckIfDone();
		}

		void HandleCancelClick (object sender, EventArgs e)
		{
			SendCancelledBroadcast();
		}

		void HandleClearClick (object sender, EventArgs e)
		{
			Reset();
		}

		private void Shake()
		{
			_selectedLayout.StartAnimation(_shakeAnimation);
			Reset();
		}

		private void UpdateFilledCircles()
		{
			for(int i = 0; i < _numbersEntered; i++)
			{
				_selectedLayout.GetChildAt(i).Activated = true;
			}
		}

		private void ResetFilledCircles()
		{
			for(int i = 0; i < _selectedLayout.ChildCount; i++)
			{
				_selectedLayout.GetChildAt(i).Activated = false;
			}
		}

		private void HideUnusedCircles()
		{
			for(int i = _selectedLayout.ChildCount - 1; i >= _passcodeLength; i--)
			{
				_selectedLayout.GetChildAt(i).Visibility = ViewStates.Gone;
			}
		}

		private void CheckIfDone()
		{
			if(_numbersEntered == _passcodeLength)
			{
				//Done
				SendCompletedBroadcast();
				SetButtonState(false);
			}
		}

		private void SetButtonState(bool isEnabled)
		{
			for(int i = 0; i < _selectedLayout.ChildCount; i++)
			{
				_selectedLayout.GetChildAt(i).Enabled = isEnabled;
			}
		}

		private void Reset()
		{
			_numbersEntered = 0;
			Array.Clear(_passcodeEntered, 0, _passcodeEntered.Length);
			ResetFilledCircles();
			SetButtonState(true);
		}

		private void SendCompletedBroadcast()
		{
			var i = new Intent(PasscodeManager.ACTION_COMPLETED);
			i.PutExtra(PasscodeManager.EXTRA_CODE, _passcodeEntered);
			SendBroadcast(i);
		}

		private void SendCancelledBroadcast()
		{
			SendBroadcast(new Intent(PasscodeManager.ACTION_CANCELLED));
		}

		void HandleMessageReceived (object sender, Intent e)
		{
			if(e.Action != null)
			{
				if(e.Action == ACTION_DISMISS)
				{
					Done();
				}
				else if(e.Action == ACTION_SHAKE)
				{
					Shake();
				}
			}
		}

		protected override void OnSaveInstanceState(Bundle outState)
		{
			outState.PutIntArray(KEY_PASSCODE_ENTERED, _passcodeEntered);
			outState.PutInt(KEY_PASSCODE_COUNT, _numbersEntered);
			base.OnSaveInstanceState(outState);
		}

		protected override void OnRestoreInstanceState(Bundle savedInstanceState)
		{
			base.OnRestoreInstanceState(savedInstanceState);
			if(savedInstanceState != null)
			{
				_passcodeEntered = savedInstanceState.GetIntArray(KEY_PASSCODE_ENTERED);
				_numbersEntered = savedInstanceState.GetInt(KEY_PASSCODE_COUNT);
				UpdateFilledCircles();
			}
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			DestroyButtons();
		}

		private void DestroyButtons()
		{
			_clearButton.Click -= HandleClearClick;
			_cancelButton.Click -= HandleCancelClick;
			foreach(var button in _numberButtons)
			{
				if(button != null)
					button.Click -= HandleNumberClick;
			}
		}

		private void Done()
		{
			Finish();
			OverridePendingTransition(_animationIn, _animationOut);
		}

		public override void OnBackPressed()
		{
			//Do nothing
		}
	}
}

