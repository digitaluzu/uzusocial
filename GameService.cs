using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;

#if UZU_GAMESTICK
// TODO: commenting out since it's not working and I don't want to add the dependency to the project if we're not using it.
//using PlayJamUnity;
#endif

namespace Uzu
{
	/// <summary>
	/// Interface for interacting with various game services (GameCenter, Google Play, etc).
	/// </summary>
	public static class GameService
	{
		#region Authentication
		public static void Authenticate (System.Action<bool> onAuthentication)
		{
#if UNITY_EDITOR

#elif UNITY_IPHONE
			if (!IsUserAuthenticated ()) {
				Social.localUser.Authenticate (onAuthentication);	
			}
#elif UZU_GOOGLEPLAY
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
#if UNITY_EDITOR
			return false;
#elif UNITY_IPHONE
			return Social.localUser.authenticated;
#elif UZU_GOOGLEPLAY 
			return GooglePlay.instance.IsUserAuthenticated();
#elif UZU_GAMESTICK
			return true;
#else
			#error Unhandled platform.
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
#if UNITY_IPHONE
				Social.ShowAchievementsUI ();
#elif UZU_GOOGLEPLAY
				GooglePlay.instance.ShowAchievementUI();
#elif UZU_GAMESTICK
				//TODO ... I should call a pannel change to show to custome UI but I do it direcly now ("Because the panel is game related")
				//Maybe I should set a callback?
#else
				#error Unhandled platform.
#endif
			}
		}

		public static void ShowLeaderboardUI (string leaderboardId, TimeScope timeScope)
		{
			if (IsUserAuthenticated ()) {
#if UNITY_IPHONE
				GameCenterPlatform.ShowLeaderboardUI (leaderboardId, timeScope);
#elif UZU_GOOGLEPLAY
				GooglePlay.instance.ShowLeaderboardUI (leaderboardId);
#elif UZU_GAMESTICK
				//TODO ... I should call a pannel change to show to custome UI but I do it direcly now ("Because the panel is game related")
				//Maybe I should set a callback?
#else
				#error Unhandled platform.
#endif
			}
		}

		public static void ReportScore (long score, string leaderboardID)
		{
			if (IsUserAuthenticated ()) {
#if UNITY_IPHONE
				Social.ReportScore (score, leaderboardID, result => { /*TODO*/});
#elif UZU_GOOGLEPLAY
				GooglePlay.instance.ReportScore (score, leaderboardID);
#elif UZU_GAMESTICK
				// TODO: commenting out since it's not working and I don't want to add the dependency to the project if we're not using it.
				//TODO the leaderBoardID need to be formated now it won't work
				//PlayJamServices.LeaderBoard_SaveScore ((int)score, 0);
#else
				#error Unhandled platform.
#endif
			}
		}

		public static void ReportAchievementProgress (string achievementID, float progress)
		{
			if (IsUserAuthenticated ()) {
#if UNITY_IPHONE
				Social.ReportProgress (achievementID, progress, result => {/*TODO*/});
#elif UZU_GOOGLEPLAY
				GooglePlay.instance.ReportAchievementProgress (achievementID, progress);
#elif UZU_GAMESTICK
				// TODO: commenting out since it's not working and I don't want to add the dependency to the project if we're not using it.
				//TODO the leaderBoardID need to be formated now it won't work
				//PlayJamServices.Achievement_SetAchievementComplete (achievementID);
#else
				#error Unhandled platform.
#endif
			}
		}
	}
}