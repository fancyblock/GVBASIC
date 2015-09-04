using UnityEngine;
using System.Collections;
using System.Collections.Generic;


#if UNITY_ANDROID
namespace Prime31
{
	public class AndroidNotificationConfiguration
	{
		public long secondsFromNow;
		public string title = string.Empty;
		public string subtitle = string.Empty;
		public string tickerText = string.Empty;
		public string extraData = string.Empty;
		public string smallIcon = string.Empty;
		public string largeIcon = string.Empty;
		public int requestCode = -1;
		public string groupKey = string.Empty;
		public int color = -1;
		public bool isGroupSummary;
		public int cancelsNotificationId = -1;


		public AndroidNotificationConfiguration( long secondsFromNow, string title, string subtitle, string tickerText )
		{
			this.secondsFromNow = secondsFromNow;
			this.title = title;
			this.subtitle = subtitle;
			this.tickerText = tickerText;
		}


		public AndroidNotificationConfiguration build()
		{
			if( requestCode == -1 )
				requestCode = Random.Range( 0, int.MaxValue );

			return this;
		}

	}


	public enum TTSQueueMode
	{
		Flush = 0,
		Add = 1
	}


	public class EtceteraAndroid
	{
		private static AndroidJavaObject _plugin;

		public enum ScalingMode
		{
			None,
			AspectFit,
			Fill
		}


		static EtceteraAndroid()
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			// find the plugin instance
			using( var pluginClass = new AndroidJavaClass( "com.prime31.EtceteraPlugin" ) )
				_plugin = pluginClass.CallStatic<AndroidJavaObject>( "instance" );
		}


		// Loads up a Texture2D with the image at the given path
		public static Texture2D textureFromFileAtPath( string filePath )
		{
			var bytes = System.IO.File.ReadAllBytes( filePath );
			var tex = new Texture2D( 1, 1 );
			tex.LoadImage( bytes );
			tex.Apply();

			Debug.Log( "texture size: " + tex.width + "x" + tex.height );

			return tex;
		}


		// Toggles low profile mode for the standard decor view on Honeycomb+
		public static void setSystemUiVisibilityToLowProfile( bool useLowProfile )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "setSystemUiVisibilityToLowProfile", useLowProfile );
		}


		// Plays a video either locally (must be in the StreamingAssets folder or accessible via full path) or remotely.  The video format must be compatible with the current
		// device.  Many devices have different supported video formats so choose the most common (probably 3gp).
		// When playing a video from the StreamingAssets folder you only need to provide the filename via the pathOrUrl parameter.
		public static void playMovie( string pathOrUrl, uint bgColor, bool showControls, ScalingMode scalingMode, bool closeOnTouch )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "playMovie", pathOrUrl, (int)bgColor, showControls, (int)scalingMode, closeOnTouch );
		}


		// Sets the theme for any alerts displayed. See the Android documentation for available themes: http://developer.android.com/reference/android/app/AlertDialog.html#constants
		// Make sure you know which Android OS version your app is currently installed on and ensure that you only use themes supported by that version!
		public static void setAlertDialogTheme( int theme )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "setAlertDialogTheme", theme );
		}


		// Shows a Toast notification.  You can choose either short or long duration
		public static void showToast( string text, bool useShortDuration )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "showToast", text, useShortDuration );
		}


		// Shows a native alert with a single button
		public static void showAlert( string title, string message, string positiveButton )
		{
			showAlert( title, message, positiveButton, string.Empty );
		}


		// Shows a native alert with two buttons
		public static void showAlert( string title, string message, string positiveButton, string negativeButton )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "showAlert", title, message, positiveButton, negativeButton );
		}


		// Shows an alert with a text prompt embedded in it
		public static void showAlertPrompt( string title, string message, string promptHint, string promptText, string positiveButton, string negativeButton )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "showAlertPrompt", title, message, promptHint, promptText, positiveButton, negativeButton );
		}


		// Shows an alert with two text prompts embedded in it
		public static void showAlertPromptWithTwoFields( string title, string message, string promptHintOne, string promptTextOne, string promptHintTwo, string promptTextTwo, string positiveButton, string negativeButton )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "showAlertPromptWithTwoFields", title, message, promptHintOne, promptTextOne, promptHintTwo, promptTextTwo, positiveButton, negativeButton );
		}


		// Shows a native progress indicator.  It will not be dismissed until you call hideProgressDialog
		public static void showProgressDialog( string title, string message )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "showProgressDialog", title, message );
		}


		// Hides the progress dialog
		public static void hideProgressDialog()
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "hideProgressDialog" );
		}


		// Shows a web view with the given url. To display local files they must be in the StreamingAssets folder and can be referenced with a url like this: "file:///android_asset/some_file.html"
		public static void showWebView( string url )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "showWebView", url );
		}


		// Shows a web view without a title bar optionally disabling the back button. If you disable the back button the only way
		// a user can close the web view is if the web page they are on has a link with the protocol close://  It is highly recommended
		// to not disable the back button! Users are accustomed to it working as it is a default Android feature.
		public static void showCustomWebView( string url, bool disableTitle, bool disableBackButton )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "showCustomWebView", url, disableTitle, disableBackButton );
		}


		// Lets the user choose an email program (or uses the default one) to send an email prefilled with the arguments
		public static void showEmailComposer( string toAddress, string subject, string text, bool isHTML )
		{
			showEmailComposer( toAddress, subject, text, isHTML, string.Empty );
		}

		public static void showEmailComposer( string toAddress, string subject, string text, bool isHTML, string attachmentFilePath )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "showEmailComposer", toAddress, subject, text, isHTML, attachmentFilePath );
		}


		// Checks to see if the SMS composer is available on this device
		public static bool isSMSComposerAvailable()
		{
			if( Application.platform != RuntimePlatform.Android )
				return false;

			return _plugin.Call<bool>( "isSMSComposerAvailable" );
		}


		// Shows the SMS composer with the body string and optional recipients
		public static void showSMSComposer( string body )
		{
			showSMSComposer( body, null );
		}

		public static void showSMSComposer( string body, string[] recipients )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			// prep the recipients if we have any
			var recipientString = string.Empty;
			if( recipients != null && recipients.Length > 0 )
			{
				recipientString = "smsto:";
				foreach( var r in recipients )
					recipientString += r + ";";
			}

			_plugin.Call( "showSMSComposer", recipientString, body );
		}


		// Displays the Android native share intent for sharing just an image. Any apps installed that implement image sharing will show up
		// in the chooser.
		public static void shareImageWithNativeShareIntent( string pathToImage, string chooserText )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "shareImageWithNativeShareIntent", pathToImage, chooserText );
		}


		// Displays the Android native share intent for sharing text with an optional image. Any apps installed that implement image sharing will show up
		// in the chooser.
		public static void shareWithNativeShareIntent( string text, string subject, string chooserText, string pathToImage = null )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "shareWithNativeShareIntent", text, subject, chooserText, pathToImage );
		}


		// Prompts the user to take a photo. The photoChooserSucceededEvent/photoChooserCancelledEvent will fire with the result.
		public static void promptToTakePhoto( string name )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "promptToTakePhoto", name );
		}


		// Prompts the user to choose an image from the photo album
		public static void promptForPictureFromAlbum( string name )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "promptForPictureFromAlbum", name );
		}


		// Prompts the user to take a video and records it saving with the given name (no file extension is needed for the name)
		public static void promptToTakeVideo( string name )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "promptToTakeVideo", name );
		}


		// Saves an image to the photo gallery
		public static bool saveImageToGallery( string pathToPhoto, string title )
		{
			if( Application.platform != RuntimePlatform.Android )
				return false;

			return _plugin.Call<bool>( "saveImageToGallery", pathToPhoto, title );
		}


		// Scales the image. Scale should be 1 to not change the size and less than 1 for smaller images.
		public static void scaleImageAtPath( string pathToImage, float scale )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "scaleImageAtPath", pathToImage, scale );
		}


		// Gets the image size for the image at the given path
		public static Vector2 getImageSizeAtPath( string pathToImage )
		{
			if( Application.platform != RuntimePlatform.Android )
				return Vector2.zero;

			var sizeString = _plugin.Call<string>( "getImageSizeAtPath", pathToImage );
			var parts = sizeString.Split( new char[] { ',' } );
			return new Vector2( int.Parse( parts[0] ), int.Parse( parts[1] ) );
		}


		public static void enableImmersiveMode( bool shouldEnable )
		{
			if( Application.platform == RuntimePlatform.Android )
				_plugin.Call( "enableImmersiveMode", shouldEnable ? 1 : 0 );
		}


		public static void loadContacts( int startingIndex, int totalToRetrieve )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "loadContacts", startingIndex, totalToRetrieve );
		}


		#region TTS

		// Starts up the TTS system
		public static void initTTS()
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "initTTS" );
		}


		// Tears down and destroys the TTS system
		public static void teardownTTS()
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "teardownTTS" );
		}


		// Speaks the text passed in
		public static void speak( string text )
		{
			speak( text, TTSQueueMode.Add );
		}


		// Speaks the text passed in optionally queuing it or flushing the current queue
		public static void speak( string text, TTSQueueMode queueMode )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "speak", text, (int)queueMode );
		}


		// Stops the TTS system from speaking the current text
		public static void stop()
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "stop" );
		}


		// Plays silence for the specified duration in milliseconds
		public static void playSilence( long durationInMs, TTSQueueMode queueMode )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "playSilence", durationInMs, (int)queueMode );
		}


		// Speech pitch. 1.0 is the normal pitch, lower values lower the tone of the synthesized voice, greater values increase it.
		public static void setPitch( float pitch )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "setPitch", pitch );
		}


		// Speech rate. 1.0 is the normal speech rate, lower values slow down the speech (0.5 is half the
		// normal speech rate), greater values accelerate it (2.0 is twice the normal speech rate).
		public static void setSpeechRate( float rate )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "setSpeechRate", rate );
		}

	#endregion


		#region Ask For Review

		// Allows you to set the button titles when using any of the askForReview methods below
		public static void askForReviewSetButtonTitles( string remindMeLaterTitle, string dontAskAgainTitle, string rateItTitle )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "askForReviewSetButtonTitles", remindMeLaterTitle, dontAskAgainTitle, rateItTitle );
		}


		// Shows the ask for review prompt with constraints. launchesUntilPrompt will not allow the prompt to be shown until it is launched that many times.
		// hoursUntilFirstPrompt is the time since the first launch that needs to expire before the prompt is shown
		// hoursBetweenPrompts is the time that needs to expire since the last time the prompt was shown
		// NOTE: once a user reviews your app the prompt will never show again until you call resetAskForReview
		public static void askForReview( int launchesUntilPrompt, int hoursUntilFirstPrompt, int hoursBetweenPrompts, string title, string message, bool isAmazonAppStore = false )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			if( isAmazonAppStore )
				_plugin.Set<bool>( "isAmazonAppStore", true );

			_plugin.Call( "askForReview", launchesUntilPrompt, hoursUntilFirstPrompt, hoursBetweenPrompts, title, message );
		}


		// Shows the ask for review prompt immediately unless the user pressed the dont ask again button
		public static void askForReviewNow( string title, string message, bool isAmazonAppStore = false )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			if( isAmazonAppStore )
				_plugin.Set<bool>( "isAmazonAppStore", true );

			_plugin.Call( "askForReviewNow", title, message );
		}


		// Resets all stored values such as launch count, first launch date, etc. Use this if you release a new version and want that version to be reviewed
		public static void resetAskForReview()
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "resetAskForReview" );
		}


		// Opens the review page in the Play Store directly. This will do not checks. It is here to allow you to make your own in-game UI for your ask for review prompt.
		public static void openReviewPageInPlayStore( bool isAmazonAppStore = false )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			if( isAmazonAppStore )
				_plugin.Set<bool>( "isAmazonAppStore", true );

			_plugin.Call( "openReviewPageInPlayStore" );
		}

		#endregion


		#region Inline web view

		// Shows the inline web view. The values sent are multiplied by the screens dpi on the native side. Note that Unity's input handling will still occur so make sure
		// nothing is touchable that is behind the web view while it is displayed.
		public static void inlineWebViewShow( string url, int x, int y, int width, int height )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "inlineWebViewShow", url, x, y, width, height );
		}


		// Closes the inline web view
		public static void inlineWebViewClose()
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "inlineWebViewClose");
		}


		// Sets the current url for the inline web view
		public static void inlineWebViewSetUrl( string url )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "inlineWebViewSetUrl", url );
		}


		// Sets the current frame for the inline web view. The values sent are multiplied by the screens dpi on the native side.
		public static void inlineWebViewSetFrame( int x, int y, int width, int height )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "inlineWebViewSetFrame", x, y, width, height );
		}

		#endregion


		#region Notifications

		// Schedules a notification to fire in secondsFromNow. The extraData will be returned with the notificationReceivedEvent and the int returned (requestCode) can be used to cancel the notification.
		// Note that you can specify your own requestCode by just passing in an int of your choosing for the requestCode parameter.
		[System.Obsolete( "Use the scheduleNotification variant that accepts a AndroidNotificationConfiguration parameter" )]
		public static int scheduleNotification( long secondsFromNow, string title, string subtitle, string tickerText, string extraData, int requestCode = -1 )
		{
			return scheduleNotification( new AndroidNotificationConfiguration( secondsFromNow, title, subtitle, tickerText )
			{
				extraData = extraData,
				requestCode = requestCode
			});
		}


		// Schedules a notification to fire in secondsFromNow. The extraData will be returned with the notificationReceivedEvent and the int returned (requestCode) can be used to cancel the notification.
		// Note that you can specify your own requestCode by just passing in an int of your choosing for the requestCode parameter.
		[System.Obsolete( "Use the scheduleNotification variant that accepts a AndroidNotificationConfiguration parameter" )]
		public static int scheduleNotification( long secondsFromNow, string title, string subtitle, string tickerText, string extraData, string smallIcon, string largeIcon, int requestCode = -1 )
		{
			return scheduleNotification( new AndroidNotificationConfiguration( secondsFromNow, title, subtitle, tickerText )
			{
				extraData = extraData,
				smallIcon = smallIcon,
				largeIcon = largeIcon,
				requestCode = requestCode
			});
		}


		// Schedules a notification to fire. Use the fields in the AndroidNotificationConfiguration object to configure the notification. When dealing with grouped notifications
		// the groupKey must match for all notifications and you must set one notification to be the summary notification (AndroidNotificationConfiguration.isGroupSummary
		// must be true). The extraData will be returned with the notificationReceivedEvent and the int returned (requestCode) can be used to cancel the notification.
		// If AndroidNotificationConfiguration.cancelsNotificationId is set when the notification fires it will call cancelNotification with that notification Id.
		public static int scheduleNotification( AndroidNotificationConfiguration config )
		{
			if( Application.platform != RuntimePlatform.Android )
				return -1;

			config.build();
			//return _plugin.Call<int>( "scheduleNotification", config.secondsFromNow, config.title, config.subtitle, config.tickerText, config.extraData, config.smallIcon, config.largeIcon, config.requestCode, config.color, config.groupKey, config.isGroupSummary );
			return _plugin.Call<int>( "scheduleNotification", Json.encode( config ) );
		}


		// Cancels the notification with the given notificationId
		public static void cancelNotification( int notificationId )
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "cancelNotification", notificationId );
		}


		// Cancels all pending notifications
		public static void cancelAllNotifications()
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "cancelAllNotifications" );
		}


		// Checks to see if the app was launched from a notification. If it was the normal notificationReceivedEvent will fire.
		// Calling this at every launch is a good idea if you are using notifications and want the extraData. This methiod
		// will return the extraData from the last used intent everytime you call it.
		public static void checkForNotifications()
		{
			if( Application.platform != RuntimePlatform.Android )
				return;

			_plugin.Call( "checkForNotifications" );
		}

		#endregion


		public class Contact
		{
			public string name;
			public List<string> emails;
			public List<string> phoneNumbers;
		}

	}
}
#endif
