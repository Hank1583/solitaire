using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatsGroup : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI GamesPlayed;
    [SerializeField]
    private TextMeshProUGUI GamesWon;
    [SerializeField]
    private TextMeshProUGUI Moves;
    [SerializeField]
    private TextMeshProUGUI WinRate;
    [SerializeField]
    private TextMeshProUGUI HighScores;
    [SerializeField]
    private TextMeshProUGUI ShortestTime;
    [SerializeField]
    private TextMeshProUGUI AVGGame;
 

    public void SetGamesPlayed(string gamesPlayed)
    {
        GamesPlayed.text = gamesPlayed;
    }
    public void SetGamesWon(string gamesWon)
    {
        GamesWon.text = gamesWon;
    }
    public void SetMoves(string moves)
    {
        Moves.text = moves;
    }
    public void SetWinRate(string winRate)
    {
        WinRate.text = winRate;
    }
    public void SetHighScore(string highScore)
    {
        HighScores.text = highScore;
    }

    public void SetShortestTime(string shortestTime)
    {
        ShortestTime.text = shortestTime;
    }
    public void SetAVGGame(string avgGame)
    {
        AVGGame.text = avgGame;
    }
  
}
