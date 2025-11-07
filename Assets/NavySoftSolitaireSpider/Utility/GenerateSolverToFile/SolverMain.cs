using System.Collections;

namespace Test
{
	using UnityEngine;
	using UnityEngine.UI;
	using System;
	using System.IO;
	using System.Collections.Generic;
	using SolitaireEngine;
	using SolitaireEngine.Model;

	public class SolverMain : MonoBehaviour
	{
		[SerializeField]
		private int solutionCount;
		[SerializeField]
		private Image image;
		private float kof;

		public void OnOneCard()
		{
			StartCoroutine(CreateMonthChallenge ("OneCardData.txt", true));
		}
		public void OnThreeCards()
		{
			StartCoroutine(CreateMonthChallenge ("ThreeCardsData.txt", false));
		}

		private void Update()
		{
			image.fillAmount = kof;
		}

		private IEnumerator CreateMonthChallenge (string fileName, bool isOneCardSolver)
		{
			string path = Application.persistentDataPath+"/";
			StreamWriter sw = File.CreateText (path+fileName);

			for (int index = 0; index < solutionCount; index++)
			{
				kof = (float)index / (float)solutionCount;

				Solitaire solitaire;
				string cards;
				string solution;

				do
				{
					solitaire = new Solitaire (true, isOneCardSolver);
					cards = ConvertDeckToString (solitaire.GetStartingCards ());
					solution = ConvertSolutionToString (solitaire.GetSolution ());
				}
				while(solitaire.GetSolution ().Count < 180);
				// one: 180, stop-185
				// three : 195, slowly-200
				sw.WriteLine (cards);
				sw.WriteLine (solution);
				yield return null;
			}
			sw.Close ();
		}

		private string ConvertDeckToString(List<Card> deck)
		{
			string convertedDeck = "";
			foreach (Card element in deck)
			{
				string openCard = (element.IsOpened) ? "1" : "0";
				string separateNextCard = "*";
				convertedDeck = convertedDeck + separateNextCard + element.Id.ToString () + ":" + element.Rank.ToString () + ":" + element.Suit.ToString () + ":" + openCard;
			}
			convertedDeck = convertedDeck.Substring (1);
			return convertedDeck;
		}
		// Del it obsolate
		private string ConvertSolutionToString(List<ContractCommand> solution)
		{
			string convertedSolution = "";
			foreach (ContractCommand element in solution)
			{
				string separateNextCard = "*";
				convertedSolution = convertedSolution + separateNextCard + ((int)element.Action).ToString () + ":" + element.IdFrom.ToString () + ":" + element.IdTo.ToString ();
			}
			convertedSolution = convertedSolution.Substring (1);
			return convertedSolution;
		}
	}
}