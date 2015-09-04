using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;



namespace Prime31
{
	public class EtceteraAndroidEventListener : MonoBehaviour
	{
#if UNITY_ANDROID
		void OnEnable()
		{
			// Listen to all events for illustration purposes
			EtceteraAndroidManager.alertButtonClickedEvent += alertButtonClickedEvent;
			EtceteraAndroidManager.alertCancelledEvent += alertCancelledEvent;
			EtceteraAndroidManager.promptFinishedWithTextEvent += promptFinishedWithTextEvent;
			EtceteraAndroidManager.promptCancelledEvent += promptCancelledEvent;		
			EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent += twoFieldPromptFinishedWithTextEvent;
			EtceteraAndroidManager.twoFieldPromptCancelledEvent += twoFieldPromptCancelledEvent;
			EtceteraAndroidManager.webViewCancelledEvent += webViewCancelledEvent;
			EtceteraAndroidManager.inlineWebViewJSCallbackEvent += inlineWebViewJSCallbackEvent;
			
			EtceteraAndroidManager.albumChooserCancelledEvent += albumChooserCancelledEvent;
			EtceteraAndroidManager.albumChooserSucceededEvent += albumChooserSucceededEvent;
			EtceteraAndroidManager.photoChooserCancelledEvent += photoChooserCancelledEvent;
			EtceteraAndroidManager.photoChooserSucceededEvent += photoChooserSucceededEvent;
			EtceteraAndroidManager.videoRecordingCancelledEvent += videoRecordingCancelledEvent;
			EtceteraAndroidManager.videoRecordingSucceededEvent += videoRecordingSucceededEvent;
			
			EtceteraAndroidManager.ttsInitializedEvent += ttsInitializedEvent;
			EtceteraAndroidManager.ttsFailedToInitializeEvent += ttsFailedToInitializeEvent;
			EtceteraAndroidManager.askForReviewDontAskAgainEvent += askForReviewDontAskAgainEvent;
			EtceteraAndroidManager.askForReviewRemindMeLaterEvent += askForReviewRemindMeLaterEvent;
			EtceteraAndroidManager.askForReviewWillOpenMarketEvent += askForReviewWillOpenMarketEvent;
			EtceteraAndroidManager.notificationReceivedEvent += notificationReceivedEvent;
			EtceteraAndroidManager.contactsLoadedEvent += contactsLoadedEvent;
		}
	
	
		void OnDisable()
		{
			// Remove all event handlers
			EtceteraAndroidManager.alertButtonClickedEvent -= alertButtonClickedEvent;
			EtceteraAndroidManager.alertCancelledEvent -= alertCancelledEvent;
			EtceteraAndroidManager.promptFinishedWithTextEvent -= promptFinishedWithTextEvent;
			EtceteraAndroidManager.promptCancelledEvent -= promptCancelledEvent;
			EtceteraAndroidManager.twoFieldPromptFinishedWithTextEvent -= twoFieldPromptFinishedWithTextEvent;
			EtceteraAndroidManager.twoFieldPromptCancelledEvent -= twoFieldPromptCancelledEvent;
			EtceteraAndroidManager.webViewCancelledEvent -= webViewCancelledEvent;
			EtceteraAndroidManager.inlineWebViewJSCallbackEvent -= inlineWebViewJSCallbackEvent;
			
			EtceteraAndroidManager.albumChooserCancelledEvent -= albumChooserCancelledEvent;
			EtceteraAndroidManager.albumChooserSucceededEvent -= albumChooserSucceededEvent;
			EtceteraAndroidManager.photoChooserCancelledEvent -= photoChooserCancelledEvent;
			EtceteraAndroidManager.photoChooserSucceededEvent -= photoChooserSucceededEvent;
			EtceteraAndroidManager.videoRecordingCancelledEvent -= videoRecordingCancelledEvent;
			EtceteraAndroidManager.videoRecordingSucceededEvent -= videoRecordingSucceededEvent;
			
			EtceteraAndroidManager.ttsInitializedEvent -= ttsInitializedEvent;
			EtceteraAndroidManager.ttsFailedToInitializeEvent -= ttsFailedToInitializeEvent;
			EtceteraAndroidManager.askForReviewDontAskAgainEvent -= askForReviewDontAskAgainEvent;
			EtceteraAndroidManager.askForReviewRemindMeLaterEvent -= askForReviewRemindMeLaterEvent;
			EtceteraAndroidManager.askForReviewWillOpenMarketEvent -= askForReviewWillOpenMarketEvent;
			EtceteraAndroidManager.notificationReceivedEvent -= notificationReceivedEvent;
			EtceteraAndroidManager.contactsLoadedEvent -= contactsLoadedEvent;
		}
	
	
	
		void alertButtonClickedEvent( string positiveButton )
		{
			Debug.Log( "alertButtonClickedEvent: " + positiveButton );
		}
	
	
		void alertCancelledEvent()
		{
			Debug.Log( "alertCancelledEvent" );
		}
	
	
		void promptFinishedWithTextEvent( string param )
		{
			Debug.Log( "promptFinishedWithTextEvent: " + param );
		}
	
	
		void promptCancelledEvent()
		{
			Debug.Log( "promptCancelledEvent" );
		}
		
		
		void twoFieldPromptFinishedWithTextEvent( string text1, string text2 )
		{
			Debug.Log( "twoFieldPromptFinishedWithTextEvent: " + text1 + ", " + text2 );
		}
	
	
		void twoFieldPromptCancelledEvent()
		{
			Debug.Log( "twoFieldPromptCancelledEvent" );
		}
	
	
		void webViewCancelledEvent()
		{
			Debug.Log( "webViewCancelledEvent" );
		}
		
		
		void inlineWebViewJSCallbackEvent( string message )
		{
			Debug.Log( "inlineWebViewJSCallbackEvent: " + message );
		}
	
	
		void albumChooserCancelledEvent()
		{
			Debug.Log( "albumChooserCancelledEvent" );
		}
	
	
		void albumChooserSucceededEvent( string imagePath )
		{
			Debug.Log( "albumChooserSucceededEvent: " + imagePath );
			Debug.Log( "image size: " + EtceteraAndroid.getImageSizeAtPath( imagePath ) );
		}
	
	
		void photoChooserCancelledEvent()
		{
			Debug.Log( "photoChooserCancelledEvent" );
		}
	
	
		void photoChooserSucceededEvent( string imagePath )
		{
			Debug.Log( "photoChooserSucceededEvent: " + imagePath );
			Debug.Log( "image size: " + EtceteraAndroid.getImageSizeAtPath( imagePath ) );
		}
		
		
		void videoRecordingCancelledEvent()
		{
			Debug.Log( "videoRecordingCancelledEvent" );
		}
		
		
		void videoRecordingSucceededEvent( string path )
		{
			Debug.Log( "videoRecordingSucceededEvent: " + path );
		}
		
		
		void ttsInitializedEvent()
		{
			Debug.Log( "ttsInitializedEvent" );
		}
		
		
		void ttsFailedToInitializeEvent()
		{
			Debug.Log( "ttsFailedToInitializeEvent" );
		}
		
		
		void askForReviewDontAskAgainEvent()
		{
			Debug.Log( "askForReviewDontAskAgainEvent" );
		}
		
		
		void askForReviewRemindMeLaterEvent()
		{
			Debug.Log( "askForReviewRemindMeLaterEvent" );
		}
		
		
		void askForReviewWillOpenMarketEvent()
		{
			Debug.Log( "askForReviewWillOpenMarketEvent" );
		}
		
		
		void notificationReceivedEvent( string extraData )
		{
			Debug.Log( "notificationReceivedEvent: " + extraData );
		}
		
		
		void contactsLoadedEvent( List<EtceteraAndroid.Contact> contacts )
		{
			Debug.Log( "contactsLoadedEvent" );
			Prime31.Utils.logObject( contacts );
		}
		
#endif
	}

}
	
	
