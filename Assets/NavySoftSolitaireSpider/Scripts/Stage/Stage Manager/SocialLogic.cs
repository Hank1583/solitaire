using UnityEngine;

public class SocialLogic : MonoBehaviour
{
	private const string achievement_money_cards = "CgkI6ord7qEbEAIQAA";
	private const string achievement_fast_cards = "CgkI6ord7qEbEAIQAw";
	private const string achievement_one_card = "CgkI6ord7qEbEAIQBA";
	private const string achievement_helping_hand = "CgkI6ord7qEbEAIQBQ";
	private const string achievement_50_cards = "CgkI6ord7qEbEAIQBg";
	private const string achievement_100_cards = "CgkI6ord7qEbEAIQBw";
	private const string achievement_500_cards = "CgkI6ord7qEbEAIQCA";
	private const string achievement_enjoyner = "CgkI6ord7qEbEAIQCQ";
	private const string achievement_intermediate = "CgkI6ord7qEbEAIQCg";
	private const string achievement_expert = "CgkI6ord7qEbEAIQCw";

	private const string leaderboard_best_score_standard_draw_1 = "CgkI6ord7qEbEAIQAQ";
	private const string leaderboard_best_score_standard_draw_3 = "CgkI6ord7qEbEAIQDA";
	private const string leaderboard_best_score_vegas_draw_1 = "CgkI6ord7qEbEAIQDQ";
	private const string leaderboard_best_score_vegas_draw_3 = "CgkI6ord7qEbEAIQDg";
	private const string leaderboard_best_score_cumulative_draw_1 = "CgkI6ord7qEbEAIQDw";
	private const string leaderboard_best_score_cumulative_draw_3 = "CgkI6ord7qEbEAIQEA";

	private const string leaderboard_best_time_draw_1 = "CgkI6ord7qEbEAIQEQ";
	private const string leaderboard_best_time_draw_3 = "CgkI6ord7qEbEAIQEg";

	private string[] leaderboard_best_score =
	{	
		leaderboard_best_score_standard_draw_1, leaderboard_best_score_standard_draw_3,
		leaderboard_best_score_vegas_draw_1, leaderboard_best_score_vegas_draw_3,
		leaderboard_best_score_cumulative_draw_1, leaderboard_best_score_cumulative_draw_3
	};
	#region Action
	public void Set()
	{
		if (GameSettings.Instance.isSocial)
		{
			ReportLeaderboard ();
			ReportAchievement ();
		}
	}
	public void Hint()
	{	
	 
	}
	#endregion
	private void ReportLeaderboard()
	{/*
		ManagerLogic managerLogic = new ManagerLogic ();
		int solitaireType = managerLogic.GetSolitaireType ();

		SocialAPI socialAPI = new SocialAPI ();

		int score = StatsSettings.Instance.topScore [solitaireType];
		string leaderboard_Score_ID = leaderboard_best_score [solitaireType];
		socialAPI.ReportScore(score, leaderboard_Score_ID, ReportScoreCallback);

		int time = StatsSettings.Instance.shortestTime [solitaireType];
		string leaderboard_Time_ID = (GameSettings.Instance.isOneCardSet) ? leaderboard_best_time_draw_1 : leaderboard_best_time_draw_3;
		socialAPI.ReportScore(time, leaderboard_Time_ID, ReportTimeCallback);
        */
	}

	private void ReportAchievement ()
	{
        /*
		ManagerLogic managerLogic = new ManagerLogic ();
		int solitaireType = managerLogic.GetSolitaireType ();

		SocialAPI socialAPI = new SocialAPI ();

		double scale;
		bool isVegasGame = !GameSettings.Instance.isStandardSet;
		if (isVegasGame)
		{
			const int vegasLow = 2;
			const int vegasHigh = 6;
			int totalWinVegasGame = 0;
			for (int index = vegasLow; index < vegasHigh; index++)
				totalWinVegasGame += StatsSettings.Instance.gamesWon [index];
			if (totalWinVegasGame >= 1)
				socialAPI.ReportAchivement (achievement_money_cards, 100.0f, ReportAcievementCallback);

			const int enjoner = 5;
			const int intermediate = 10;
			const int expert = 30;

			scale = totalWinVegasGame / enjoner;
			if (scale >= 1f)
				scale = 100.0f;
			socialAPI.ReportAchivement (achievement_enjoyner, scale, ReportAcievementCallback);

			scale = totalWinVegasGame / intermediate;
			if (scale >= 1f)
				scale = 100.0f;
			socialAPI.ReportAchivement (achievement_intermediate, scale, ReportAcievementCallback);

			scale = totalWinVegasGame / expert;
			if (scale >= 1f)
				scale = 100.0f;
			socialAPI.ReportAchivement (achievement_expert, scale, ReportAcievementCallback);

		}

		int totalWinGames = 0;
		foreach (int element in StatsSettings.Instance.gamesWon)
			totalWinGames += element;

		if (totalWinGames >= 1)
			socialAPI.ReportAchivement (achievement_one_card, 100.0f, ReportAcievementCallback);

		const int win_50 = 50;
		const int win_100 = 100;
		const int win_500 = 500;

		scale = totalWinGames / win_50;
		if (scale >= 1f)
			scale = 100.0f;
		socialAPI.ReportAchivement (achievement_50_cards, scale, ReportAcievementCallback);

		scale = totalWinGames / win_100;
		if (scale >= 1f)
			scale = 100.0f;
		socialAPI.ReportAchivement (achievement_100_cards, scale, ReportAcievementCallback);

		scale = totalWinGames / win_500;
		if (scale >= 1f)
			scale = 100.0f;
		socialAPI.ReportAchivement (achievement_500_cards, scale, ReportAcievementCallback);

		const int two_minutes = 120;
		if (StatsSettings.Instance.shortestTime [solitaireType] <= two_minutes) 
			socialAPI.ReportAchivement (achievement_fast_cards, 100.0f, ReportAcievementCallback);
            */
	}
	#region Callback
	private void ReportScoreCallback(bool isDone)
	{
		
	}
	private void ReportTimeCallback(bool isDone)
	{

	}
	private void ReportAcievementCallback(bool isDone)
	{

	}
	#endregion
}