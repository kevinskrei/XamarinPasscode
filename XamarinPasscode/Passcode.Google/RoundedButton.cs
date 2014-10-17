
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace Passcode.Google
{
	internal class RoundedButton : Button
	{
		public RoundedButton(Context context)
			: base(context)
		{
			Initialize();
		}

		public RoundedButton(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
			Initialize();
		}

		public RoundedButton(Context context, IAttributeSet attrs, int defStyle)
			: base(context, attrs, defStyle)
		{
			Initialize();
		}

		void Initialize()
		{
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			int size = widthMeasureSpec > heightMeasureSpec ? heightMeasureSpec : widthMeasureSpec;
			base.OnMeasure(size, size);
		}
	}
}

