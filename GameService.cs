using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

#if UZU_GAMESTICK
using PlayJamUnity;
#endif
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
			if (!IsUserAuthenticated ()) {
				Social.localUser.Authenticate (onAuthentication);	
			}
#elif UNITY_ANDROID && !UNITY_EDITOR && !UZU_GAMESTICK
			if((!IsUserAuthenticated ())){
				GooglePlay.instance.Authenticate(onAuthentication);
			}
#elif UZU_GAMESTICK
			{
				//Do nothing?					
			}
#endif
		}

		public static bool IsUserAuthenticated ()
		{
#if UZU_GAMESTICK
			return true;							
#elif UNITY_EDITOR
			return false;   
#elif UNITY_ANDROID 
			return GooglePlay.instance.IsUserAuthenticated();
#else
			return Social.localUser.authenticated;
#endif
		}

		#endregion

		/// <summary>
		/// Show the default iOS banner when achievements are completed.
		/// </summary>
		public static void ShowDefaultAchievementCompletionBanner (bool doesShow = true)
		{
#if UNITY_IPHONE && !UNITY_EDITOR
			// Show the default iOS banner when achievements are completed.
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(doesShow);
#endif
		}

		public static void ShowAchievementUI ()
		{
			if (IsUserAuthenticated ()) {
#if UZU_GAMESTICK
				//TODO ... I should call a pannel change to show to custome UI but I do it direcly now ("Because the panel is game related")
				//Maybe I should set a callback?
#elif UNITY_ANDROID 
				GooglePlay.instance.ShowAchievementUI();
#else
				Social.ShowAchievementsUI ();
#endif
			}
		}

		public static void ShowLeaderboardUI (string leaderboardId, TimeScope timeScope)
		{
			if (IsUserAuthenticated ()) {
#if UZU_GAMESTICK
				//TODO ... I should call a pannel change to show to custome UI but I do it direcly now ("Because the panel is game related")
				//Maybe I should set a callback?
#elif UNITY_ANDROID 
				GooglePlay.instance.ShowLeaderboardUI (leaderboardId);
#else
				GameCenterPlatform.ShowLeaderboardUI (leaderboardId, timeScope);
#endif
			}
		}

		public static void ReportScore (long score, string leaderboardID)
		{
			if (IsUserAuthenticated ()) {
#if UZU_GAMESTICK
				//TODO the leaderBoardID need to be formated now it won't work
				PlayJamServices.LeaderBoard_SaveScore ((int)score, 0);
#elif UNITY_ANDROID 
				GooglePlay.instance.ReportScore (score, leaderboardID);
#else
				Social.ReportScore (score, leaderboardID, result => { /*TODO*/});
#endif
			}
		}

		public static void ReportAchievementProgress (string achievementID, float progress)
		{
			if (IsUserAuthenticated ()) {
#if UZU_GAMESTICK
				//TODO the leaderBoardID need to be formated now it won't work
				PlayJamServices.Achievement_SetAchievementComplete (achievementID);
#elif UNITY_ANDROID  
				GooglePlay.instance.ReportAchievementProgress (achievementID, progress);
#else
				Social.ReportProgress (achievementID, progress, result => {/*TODO*/});
#endif
			}
		}
	}
}