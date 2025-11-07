using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StatusBar : MonoBehaviour
{

    const int MAX_MOVES = 999;


    [SerializeField]
    private GameObject moveTextContainer;

    [SerializeField]
    private GameObject timeTextContainer;

    [SerializeField]
    private GameObject scoreTextContainer;

    [SerializeField]
    private TextMeshProUGUI moveText;

    [SerializeField]
    private TextMeshProUGUI timeText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    // Set default values
    //	void Start () {
    //		reset ();
    //	}

    public void SetVisibility(bool isMove, bool isTime, bool isScore)
    {
        moveTextContainer.SetActive(isMove);
        timeTextContainer.SetActive(isTime);
        scoreTextContainer.SetActive(isScore);
    }


    /// <summary>
    /// Set all values as default
    /// </summary>
    public void reset()
    {
        move = 0;
        time = 0;
        score = 0;
    }
    // **************************
    // *******  GETTERS  ********
    // ****** / SETTERS  ********
    // **************************
    int _move = 0;
    public int move
    {
        get
        {
            return _move;
        }
        set
        {
#if UNITY_EDITOR
            if (!isMoveValueValid(value))
            {
                throw new UnityException("Invalid move value!");
            }
#endif
            _move = value;


            moveText.text = string.Format("Moves:{0}", parseScoreText(_move));
        }
    }
    /// <summary>
    /// Sets the time in a seconds
    /// </summary>
    int _time = 0;
    public int time
    {
        get
        {
            return _time;
        }
        set
        {
#if UNITY_EDITOR
            if (!isTimeValueValid(value))
            {
                throw new UnityException("Invalid time value!");
            }
#endif
            _time = value;
            timeText.text = string.Format("Time:{0}", parseTimeText(_time));
            ContinueModeGame.instance.SaveTimeAndMove(_move, (int)_time, _score);
        }
    }
    int _score = 0;
    public int score
    {
        get
        {
            return _score;
        }
        set
        {
#if UNITY_EDITOR
            if (!isScoreValueValid(value))
            {
                throw new UnityException("Invalid score value!");
            }
#endif
            _score = value;
          
            scoreText.text =string.Format("Score:{0}", parseScoreText(_score)) ;
        }
    }



    // **************************
    // *****  VALIDATORS  *******
    // **************************

    bool isMoveValueValid(int val)
    {
        if (val < 0 || val > 99999)
            return false;

        return true;
    }

    bool isTimeValueValid(int val)
    {
        int dayInSeconds = 86400;
        if (val < 0 || val > dayInSeconds)
            return false;

        return true;
    }

    bool isScoreValueValid(int val)
    {
        if (val > 99999)
            return false;
        return true;
    }




    // **************************
    // *******  PARSERS  ********
    // **************************

    string parseMoveText(int text)
    {
        if (text > MAX_MOVES)
        {
            // make text such as 999+ format
            return String.Format("{0}+", MAX_MOVES);
        }
        return text.ToString();
    }

    string parseTimeText(int seconds)
    {
 

      
        int min = seconds / 60  ;
        int sec = seconds % 60;
        return String.Format("{0:D2}:{1:D2}", min, sec);
    }

    string parseScoreText(int text)
    {
        return text.ToString();
    }

}
