using UnityEngine;
using System.Collections.Generic;
using Prime31;



namespace Prime31
{
	public class EtceteraUIManagerTwo : MonoBehaviourGUI
	{
#if UNITY_ANDROID
		private int _fiveSecondNotificationId;
		private int _tenSecondNotificationId;


		void OnGUI()
		{
			beginColumn();


			if( GUILayout.Button( "Show Inline Web View" ) )
			{
				EtceteraAndroid.inlineWebViewShow( "http://prime31.com/", 160, 430, Screen.width - 160, Screen.height - 100 );
			}


			if( GUILayout.Button( "Close Inline Web View" ) )
			{
				EtceteraAndroid.inlineWebViewClose();
			}


			if( GUILayout.Button( "Set Url of Inline Web View" ) )
			{
				EtceteraAndroid.inlineWebViewSetUrl( "http://google.com" );
			}


			if( GUILayout.Button( "Set Frame of Inline Web View" ) )
			{
				EtceteraAndroid.inlineWebViewSetFrame( 80, 50, 300, 400 );
			}


			if( GUILayout.Button( "Get First 25 Contacts" ) )
			{
				EtceteraAndroid.loadContacts( 0, 25 );
			}


			endColumn( true );


			if( GUILayout.Button( "Schedule Notification in 5s" ) )
			{
				var noteConfig = new AndroidNotificationConfiguration( 5, "Notification Title - 5 Seconds", "The subtitle of the notification", "Ticker text gets ticked" )
				{
					extraData = "five-second-note",
					groupKey = "my-note-group"
				};
				_fiveSecondNotificationId = EtceteraAndroid.scheduleNotification( noteConfig );
				Debug.Log( "notificationId: " + _fiveSecondNotificationId );
			}


			if( GUILayout.Button( "Schedule Notification in 10s" ) )
			{
				var noteConfig = new AndroidNotificationConfiguration( 10, "Notification Title - 10 Seconds", "The subtitle of the notification", "Ticker text gets ticked" )
				{
					extraData = "ten-second-note",
					groupKey = "my-note-group"
				};
				_tenSecondNotificationId = EtceteraAndroid.scheduleNotification( noteConfig );
				Debug.Log( "notificationId: " + _tenSecondNotificationId );
			}


			if( GUILayout.Button( "Schedule Group Summary Notification in 5s" ) )
			{
				var noteConfig = new AndroidNotificationConfiguration( 5, "Group Summary Title", "Group Summary Subtitle - Stuff Happened", "Ticker text" )
				{
					extraData = "group-summary-note",
					groupKey = "my-note-group",
					isGroupSummary = true
				};
				EtceteraAndroid.scheduleNotification( noteConfig );
			}


			if( GUILayout.Button( "Cancel 5s Notification" ) )
			{
				EtceteraAndroid.cancelNotification( _fiveSecondNotificationId );
			}


			if( GUILayout.Button( "Cancel 10s Notification" ) )
			{
				EtceteraAndroid.cancelNotification( _tenSecondNotificationId );
			}


			if( GUILayout.Button( "Check for Notifications" ) )
			{
				EtceteraAndroid.checkForNotifications();
			}


			if( GUILayout.Button( "Cancel All Notifications" ) )
			{
				EtceteraAndroid.cancelAllNotifications();
			}


			if( GUILayout.Button( "Quit App" ) )
			{
				Application.Quit();
			}


			endColumn();


			if( bottomRightButton( "Previous Scene" ) )
			{
				Application.LoadLevel( "EtceteraTestScene" );
			}
		}

#endif
	}

}
