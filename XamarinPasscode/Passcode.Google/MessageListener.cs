using System;
using Android.Content;
using Android.App;

namespace Passcode.Google
{
	[BroadcastReceiver]
	internal class MessageListener : BroadcastReceiver
	{
		public event EventHandler<Intent> MessageReceived;
		public MessageListener() {}
		public MessageListener(params string[] keys)
		{
			Register(keys);
		}

		private void Register(string[] keys)
		{
			IntentFilter filter = new IntentFilter();
			foreach(var key in keys)
			{
				filter.AddAction(key);
			}
			Application.Context.RegisterReceiver(this, filter);
		}

		public void Unregister()
		{
			Application.Context.UnregisterReceiver(this);
		}

		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
			if(disposing)
			{
				Unregister();
			}
		}

		#region implemented abstract members of BroadcastReceiver

		public override void OnReceive(Context context, Intent intent)
		{
			if(intent != null && MessageReceived != null)
			{
				if(MessageReceived != null)
					MessageReceived.Invoke(this, new Intent(intent));
			}
		}

		#endregion
	}
}

