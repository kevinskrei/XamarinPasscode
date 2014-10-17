using System;
using Android.Content;

namespace Passcode.Google
{
	public class PasscodeEnteredEventArgs : EventArgs
	{
		public int[] Code {get; private set;}

		public PasscodeEnteredEventArgs(int[] code)
		{
			Code = code;
		}

		public string GetPasscodeAsString()
		{
			return string.Join(string.Empty, Code);
		}
	}

	internal class IntentEventArgs : EventArgs
	{
		public Intent Value {get; private set;}

		internal IntentEventArgs(Intent value)
		{
			Value = value;
		}
	}
}

