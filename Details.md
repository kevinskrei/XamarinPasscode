This component can be used to lock your application. It simply displays a passcode lock screen to the user. Your application gets a callback with the code entered. **Styled for phones and tablets in both portrait and landscape!**

You can find the source code on github here: [GitHub](https://github.com/kevinskrei/XamarinPasscode)

## Simple Use ##
---

The default styles can all be overridden and this is explained in the next section as well as in the Customized example. By default this component uses a black and white color scheme.

You can keep the instance of PasscodeManager anywhere but I'll put in the Application class for simplicity.


    using Passcode.Google;
    ...
    //This is in your Application class
    public PasscodeManager PasscodeManager { get; private set; }
    public override void OnCreate(Bundle bundle)
    {
    	PasscodeManager = new PasscodeManager();
		PasscodeManager.PasscodeLength = 6; //Default is 4
		
		PasscodeManager.PasscodeEntered += (object sender, PasscodeEnteredEventArgs e) => {
			//Check if passcode matches the one you got from the user.
			if(e.GetPasscodeAsString() == "111111")
			{
			    //Success. User is authenticated.
				PasscodeManager.Dismiss();
			}
			else
			{
				//Shows a shaking animation
				PasscodeManager.WrongPasscode();
			}
		};
    }

Next, Put the `Show(Activity activity)` method in the (base) `Activity` where you want to show the passcode lock screen. 

    using Passcode.Google;
    ...
    //This is in your (base) activity
    public override void OnResume()
    {
    	if(!IsUserAuthenticated)
    		((App)Application.Context).PasscodeManager.Show(this);
    }

That's it! The defaults for the customizations are in a section below.

## Defaults ##
---
*The italic Properties should be Android Resource ID's (such as Resource.Anim.FadeIn) etc*

Property | Description | Default Value
------------ | ------------- | ------------
**PasscodeLength** | Length of passcode | `4`
**ShowCancelButton** | Shows/Hides the cancel button  | `false`
***AnimationEnterInResource*** | Animation for Lock Screen enter  | `0 (none)`
***AnimationEnterOutResource*** | Animation for your Activity exit | `0 (none)`
***AnimationExitInResource*** | Animation for Lock Screen exit | `0 (none)`
***AnimationExitOutResource*** | Animation for your Activity enter  | `0 (none)`


## Customizing the Lock Screen With Styles ##
---
<h4> See the sample project "Customized" to see how to override these styles. </h4>

The following styles are for default values/styles.xml. There is also sw600dp and sm720dp.

Style Name | Description | Relevant Style
------------ | ------------- | ------------
**PasscodeHeader** | TextView header style that text defaults to "Enter your passcode".  | None
**PasscodeFillableLayout** | Layout style for the container of fillable circles that become filled as you tap buttons.  | PasscodeSmallCircle
**PasscodeNumberButton** | Style for buttons numbered 0-9. You can supply your own drawables to customize the shape and color.  | None
**PasscodeSmallButton** | Style for the "Cancel" and "Clear" buttons.  | None
**PasscodeSmallCircle** | Style for the fillable circles (Sit inside "**PasscodeFillableLayout**" above). You can supply the same background drawable to match the **PasscodeNumberButton** style.  | PasscodeFillableLayout, PasscodeNumberButton
**PasscodeNumberLayout** | Container layout for the buttons 0-9, Clear, and Cancel buttons.  | None
**PasscodeContainer** | The entire page container. Set a background drawable on this style if you want to set the page background to a color/image.  | None

Size Description | Width | Example Style Name | Folder/ File Name 
------------ | ------------- | ------------
**Default** | width < sw600 dp  | PasscodeHeader | values/styles.xml
**Large** | width 600 - 720 dp  | PasscodeHeader.Large | values-sw600dp/styles.xml
**XLarge** | width > 720 dp | PasscodeHeader.XLarge | values-sw720dp/styles.xml

See the github repository source code for example on how to customize specific sizes. Simply append Large or XLarge to the original style.

<h4> Drawables </h4>

You can also set customized drawables for the buttons, fillable views, and button text colors. See the Customized sample `drawable/` folder.

**NOTE**: You must follow the pattern below if providing new drawables (ie. use the `pressed` and `activated` states). The fillable circles used `activated` to show the alternative color.

	<?xml version="1.0" encoding="utf-8"?>
	<selector xmlns:android="http://schemas.android.com/apk/res/android">
		<item android:drawable="@drawable/button_state_pressed" android:state_pressed="true"/>
    	<item android:drawable="@drawable/button_state_pressed" android:state_activated="true"/>
    	<item android:drawable="@drawable/button_state_normal"/>
	</selector>


---

screenshots taken with placeit.net
<div>Icon made by <a href="http://www.freepik.com" title="Freepik">Freepik</a> from <a href="http://www.flaticon.com" title="Flaticon">www.flaticon.com</a> is licensed under <a href="http://creativecommons.org/licenses/by/3.0/" title="Creative Commons BY 3.0">CC BY 3.0</a></div>

<h3> Release Notes </h3>
<h4> Version 1.0.2 </h4>
Fixed incompatibility with the AppCompat V7 library. Thank you @Alex Reyes for fixing the issue!
<h4> Version 1.0 </h4>
- Initial Release