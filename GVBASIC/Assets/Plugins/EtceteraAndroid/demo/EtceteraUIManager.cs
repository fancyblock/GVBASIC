using UnityEngine;
using System.Collections.Generic;
using Prime31;
using System.Collections;



namespace Prime31
{
	public class EtceteraUIManager : MonoBehaviourGUI
	{
		public GameObject testPlane;

#if UNITY_ANDROID

		void Start()
		{
			// kick off the TTS system so it is ready for use later
			EtceteraAndroid.initTTS();
			EtceteraAndroid.setAlertDialogTheme( 3 );
		}


		void OnEnable()
		{
			// Listen to the texture loaded methods so we can load up the image on our plane
			EtceteraAndroidManager.albumChooserSucceededEvent += imageLoaded;
			EtceteraAndroidManager.photoChooserSucceededEvent += imageLoaded;
		}


		void OnDisable()
		{
			EtceteraAndroidManager.albumChooserSucceededEvent -= imageLoaded;
			EtceteraAndroidManager.photoChooserSucceededEvent -= imageLoaded;
		}


		// Saves a screenshot to the SD card and then calls completionHandler with the path to the image
		private IEnumerator saveScreenshotToSDCard( System.Action<string> completionHandler )
		{
			yield return new WaitForEndOfFrame();

			var tex = new Texture2D( Screen.width, Screen.height, TextureFormat.RGB24, false );
			tex.ReadPixels( new Rect( 0, 0, Screen.width, Screen.height ), 0, 0, false );

			var bytes = tex.EncodeToPNG();
			Destroy( tex );

			var path = System.IO.Path.Combine( Application.persistentDataPath, "myImage.png" );
			System.IO.File.WriteAllBytes( path, bytes );

			completionHandler( path );
		}


		void OnGUI()
		{
			beginColumn();

			if( GUILayout.Button( "Show Toast" ) )
			{
				EtceteraAndroid.showToast( "Hi. Something just happened in the game and I want to tell you but not interrupt you", true );
			}


			if( GUILayout.Button( "Play Video" ) )
			{
				Debug.Log( "persistance: " + Application.persistentDataPath );
				Debug.Log( "caches: " + Application.temporaryCachePath );
				// closeOnTouch has no effect if you are showing controls
				EtceteraAndroid.playMovie( "http://techslides.com/demos/sample-videos/small.3gp", 0xFF0000, false, EtceteraAndroid.ScalingMode.AspectFit, true );
			}


			if( GUILayout.Button( "Show Alert" ) )
			{
				EtceteraAndroid.showAlert( "Alert Title Here", "Something just happened.  Do you want to have a snack?", "Yes", "Not Now" );
			}


			if( GUILayout.Button( "Single Field Prompt" ) )
			{
				EtceteraAndroid.showAlertPrompt( "Enter Digits", "I'll call you if you give me your number", "phone number", "867-5309", "Send", "Not a Chance" );
			}


			if( GUILayout.Button( "Two Field Prompt" ) )
			{
				EtceteraAndroid.showAlertPromptWithTwoFields( "Need Info", "Enter your credentials:", "username", "harry_potter", "password", string.Empty, "OK", "Cancel" );
			}


			if( GUILayout.Button( "Show Progress Dialog" ) )
			{
				EtceteraAndroid.showProgressDialog( "Progress is happening", "it will be over in just a second..." );
				Invoke( "hideProgress", 1 );
			}


			if( GUILayout.Button( "Text to Speech Speak" ) )
			{
				EtceteraAndroid.setPitch( Random.Range( 0, 5 ) );
				EtceteraAndroid.setSpeechRate( Random.Range( 0.5f, 1.5f ) );
				EtceteraAndroid.speak( "Howdy. Im a robot voice" );
			}


			if( GUILayout.Button( "Prompt for Video" ) )
			{
				EtceteraAndroid.promptToTakeVideo( "fancyVideo" );
			}



			endColumn( true );


			if( GUILayout.Button( "Show Web View" ) )
			{
				EtceteraAndroid.showWebView( "http://prime31.com" );
			}


			if( GUILayout.Button( "Email Composer" ) )
			{
				// grab an attachment for this email, in this case a screenshot
				StartCoroutine( saveScreenshotToSDCard( path =>
				{
					EtceteraAndroid.showEmailComposer( "noone@nothing.com", "Message subject", "click <a href='http://somelink.com'>here</a> for a present", true, path );
				} ) );
			}


			if( GUILayout.Button( "SMS Composer" ) )
			{
				EtceteraAndroid.showSMSComposer( "I did something really cool in this game!" );
			}


			if( GUILayout.Button( "Share Image Natively" ) )
			{
				StartCoroutine( saveScreenshotToSDCard( path =>
				                                       {
					EtceteraAndroid.shareImageWithNativeShareIntent( path, "Sharing a screenshot..." );
				} ) );
			}


			if( GUILayout.Button( "Share Text and Image Natively" ) )
			{
				StartCoroutine( saveScreenshotToSDCard( path =>
				                                       {
					EtceteraAndroid.shareWithNativeShareIntent( "Check this out!", "Some Subject", "Sharing a screenshot and text...", path );
				} ) );
			}


			if( GUILayout.Button( "Prompt to Take Photo" ) )
			{
				EtceteraAndroid.promptToTakePhoto( "photo.jpg" );
			}


			if( GUILayout.Button( "Prompt for Album Image" ) )
			{
				EtceteraAndroid.promptForPictureFromAlbum( "albumImage.jpg" );
			}


			if( GUILayout.Button( "Save Image to Gallery" ) )
			{
				StartCoroutine( saveScreenshotToSDCard( path =>
				{
					var didSave = EtceteraAndroid.saveImageToGallery( path, "My image from Unity" );
					Debug.Log( "did save to gallery: " + didSave );
				} ) );
			}


			if( GUILayout.Button( "Ask For Review" ) )
			{
				// reset just in case you accidentally press dont ask again while playing with the demo scene
				EtceteraAndroid.resetAskForReview();
				EtceteraAndroid.askForReviewNow( "Please rate my app!", "It will really make me happy if you do..." );
			}


			endColumn();


			if( bottomRightButton( "Next Scene" ) )
			{
				Application.LoadLevel( "EtceteraTestSceneTwo" );
			}
		}


		private void hideProgress()
		{
			EtceteraAndroid.hideProgressDialog();
		}


		// Texture loading delegates
		public void imageLoaded( string imagePath )
		{
			// scale the image down to a reasonable size before loading
			EtceteraAndroid.scaleImageAtPath( imagePath, 0.1f );
			testPlane.GetComponent<Renderer>().material.mainTexture = EtceteraAndroid.textureFromFileAtPath( imagePath );
		}
#endif
	}

}
