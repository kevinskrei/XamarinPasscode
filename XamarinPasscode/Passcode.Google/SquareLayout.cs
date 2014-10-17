
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
using Android.Content.Res;

namespace Passcode.Google
{
	internal class SquareLayout : LinearLayout
	{
		private int _rows;
		private int _cols;
		public SquareLayout(Context context, int rows, int cols)
			: base(context)
		{
			_rows = rows;
			_cols = cols;
		}

		public SquareLayout(Context context, IAttributeSet attrs)
			: base(context, attrs)
		{
			Initialize(context, attrs);
		}

		public SquareLayout(Context context, IAttributeSet attrs, int defStyle)
			: base(context, attrs, defStyle)
		{
			Initialize(context, attrs);
		}

		void Initialize(Context context, IAttributeSet attrs)
		{
			TypedArray a = context.Theme.ObtainStyledAttributes(
				attrs,
				Resource.Styleable.squarelayout,
				0, 0);

			try {
				_rows = a.GetInt(Resource.Styleable.squarelayout_rows, 0);
				_cols = a.GetInt(Resource.Styleable.squarelayout_cols, 0);
			} finally {
				a.Recycle();
			}
		}

		protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure(widthMeasureSpec, heightMeasureSpec);

			int width = MeasuredWidth;
			int desiredHeight = Calc(width);
			while(desiredHeight > MeasuredHeight)
			{
				width -= 10;
				desiredHeight = Calc(width);
			}

			SetMeasuredDimension(width, desiredHeight);

			for(int i=0; i< ChildCount;i++) {

				int specWidth = MeasureSpec.MakeMeasureSpec(width, MeasureSpecMode.Exactly);
				int specHeight = MeasureSpec.MakeMeasureSpec( (int)((float)desiredHeight / (float)_rows), MeasureSpecMode.Exactly);
				View v = GetChildAt(i);
				v.Measure(specWidth, specHeight);
			}
		}

		private int Calc(int width)
		{
			return (int)((float)width * ((float) _rows / (float) _cols));
		}
	}
}

