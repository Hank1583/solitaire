using System.Collections.Generic;

public interface IHUDActions
{
	// back and store buttons
	void OnBackToMenuPressed();
	void OnStorePressed();

	// "i" button
	void OnShowManualPressed();

	// bottom buttons
	void OnOptionsPressed();
	void OnSolutionPressed();
	// new pop up options 
		void OnNewGamePressed();
		void OnRestartPressed();
		void OnDailyChallengePressed();
		void OnStatisticPressed();
	void OnHintsPressed();
	void OnUndoPressed();
		void OnNoUsefulHintsPressed();

	// solution
	void OnExitSolution();


}