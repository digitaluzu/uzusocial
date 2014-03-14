using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

namespace Uzu
{
	/// <summary>
	/// Interface for interacting with various game services (GameCenter, Google Play, etc).
	/// </summary>
	public class GameService
	{
		#region Authentication 
		public static void Authenticate (System.Action<bool> onAuthentication)
		{
#if UNITY_IPHONE && !UNITY_EDITOR
			// Show the default iOS banner when achievements are completed.
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);

			if (!IsUserAuthenticated ()) {
				Social.localUser.Authenticate (onAuthentication);	
			}
#elif UNITY_ANDROID && !UNITY_EDITOR
			if((!IsUserAuthenticated ())){
				GooglePlay.instance.Authenticate(onAuthentication);
			}
#endif
		}

		public static bool IsUserAuthenticated ()
		{
#if UNITY_EDITOR
			return false;   
#elif UNITY_ANDROID 
			return GooglePlay.instance.IsUserAuthenticated();
#else
			return Social.localUser.authenticated;
#endif
		}
		#endregion

		public static void ShowAchievementUI ()
		{
			if (IsUserAuthenticated ()) {
#if UNITY_ANDROID 
				GooglePlay.instance.ShowAchievementUI();
#else
				Social.ShowAchievementsUI ();
#endif
			}
		}
	
		public static void ShowLeaderboardUI (string leaderboardId, TimeScope timeScope)
		{
			if (IsUserAuthenticated ()) {
#if UNITY_ANDROID 
				GooglePlay.instance.ShowLeaderboardUI (leaderboardId);
#else
				GameCenterPlatform.ShowLeaderboardUI (leaderboardId, timeScope);
#endif
			}
		}

		public static void ReportScore (long score, string leaderboardID)
		{
			if (IsUserAuthenticated ()) {
#if UNITY_ANDROID 
				GooglePlay.instance.ReportScore (score, leaderboardID);
#else
				Social.ReportScore (score, leaderboardID, result => { /*TODO*/});
#endif
			}
		}
		
		public static void ReportAchievementProgress (string achievementID, float progress)
		{
			if (IsUserAuthenticated ()) {
#if UNITY_ANDROID 
				GooglePlay.instance.ReportAchievementProgress (achievementID, progress);
#else
				Social.ReportProgress (achievementID, progress, result => {/*TODO*/});
#endif
			}
		}
	}
}