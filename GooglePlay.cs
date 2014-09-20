#if UZU_GOOGLEPLAY
using UnityEngine;
using System.Collections;

namespace Uzu
{
	// ***************************
	// TODO: Cleanup!!
	//	- namespace by platform?
	//  - folders by platform?
	//  - other
	// ***************************

	//GooglePlay service -> use this to armonise the interface with the social API
	//TODO:
	// Does not need to be a gameobject, but I look at the plugin code and I'm kind of worry of the timing where everything is call
	// Prefere use a game object and let everything get initialise on the proper timing for now.
	/*Memo how to add Achievement
		To run the game, you need to configure the application ID as a resource in your Android project.
		You will also need to add games metadata in the AndroidManifest.xml.
		1. Open Assets/Plugins/Android/res/values/ids.xml and replace the placeholder IDs.
			a. Specify your application ID in the app_id resource.
			b. Specify each achievement ID that you created earlier in the
				corresponding achievement_* resource.
			c. Specify each leaderboard ID that you created earlier in the corresponding
				leaderboard_* resource.
		2. Open AndroidManifest.xml and enter your package name in the package
		attribute of the <manifest> element.
	*/
		public class GooglePlay : SA_Singleton<GooglePlay> {
			
		static System.Action<bool> _onAuthentication;
		
		public void Authenticate (System.Action<bool> onAuthentication)
		{
			_onAuthentication = onAuthentication;
		}

		void Start() {

			//listen for GooglePlayConnection events
			GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
			GooglePlayConnection.instance.addEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);
			

			if(GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED) {
				//checking if player already connected
				OnPlayerConnected ();
			} 

			//GooglePlayConnection.instance.start (GooglePlayConnection.CLIENT_GAMES | GooglePlayConnection.CLIENT_APPSTATE );
		
			GooglePlayConnection.instance.connect ();
		}
		public  bool IsUserAuthenticated (){
			return (GooglePlayConnection.state == GPConnectionState.STATE_CONNECTED);
		}


		#region Show GameService
		public void ShowAchievementUI ()
		{
			if (IsUserAuthenticated ()) {
				GooglePlayManager.instance.showAchievementsUI ();
			}
		}
		
		public void ShowLeaderboardUI (string leaderboardId)
		{
			if (IsUserAuthenticated ()) {
				GooglePlayManager.instance.showLeaderBoard (leaderboardId);
			}
		}
		#endregion
		
		#region Score and achievement report
		public void ReportScore (long score, string leaderboardID)
		{
			if (IsUserAuthenticated ()) {
				GooglePlayManager.instance.submitScore (leaderboardID, (int)score);
			}
		}
		
		public void ReportAchievementProgress (string achievementID, float progress)
		{
			if (IsUserAuthenticated ()) {
				if(progress>=100.0f){
					GooglePlayManager.instance.reportAchievement(achievementID);
				}else{
					GooglePlayManager.instance.incrementAchievement (achievementID, (int)progress);
				}
			}
		}
		#endregion
		
		//--------------------------------------
		// EVENTS
		//--------------------------------------
		private void OnPlayerDisconnected() {

		}
		
		private void OnPlayerConnected() {
			//GooglePlayManager.instance.loadPlayer ();
			_onAuthentication(true);
		}
		
		
		protected override void  OnDestroy() {
			if(!GooglePlayConnection.IsDestroyed) {
				GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_CONNECTED, OnPlayerConnected);
				GooglePlayConnection.instance.removeEventListener (GooglePlayConnection.PLAYER_DISCONNECTED, OnPlayerDisconnected);

			}
			base.OnDestroy();
		}
	}
}
#endif // UZU_GOOGLEPLAY