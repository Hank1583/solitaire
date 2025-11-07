namespace Calendar
{
    using System;
   
    using System.Collections.Generic;
    using UnityEngine;
    using SolitaireEngine;
    using SolitaireEngine.Model;

    public class CalendarMonthData
    {
        private int year;
        private int month;
        private GameSettings.MedalType medal;
        private int earn;
        private int total;
        public int Year {get{return year;}}
        public int Month {get{return month;}}
        public GameSettings.MedalType Medal {get{return medal;}}
        public int Earn {get{return earn;}}
        public int Total {get{return total;}}
        private CalendarMonthData(){}
        public CalendarMonthData (int year, int month, GameSettings.MedalType medal, int earn, int total)
        {
            this.year = year;
            this.month = month;
            this.medal = medal;
            this.earn = earn;
            this.total = total;
        }

    }
    public class CalendarInfoData
    {
        private GameSettings.MedalType medal;
        private int needToNext;
        private int earn;
        private int total;
        public GameSettings.MedalType Medal {get{return medal;}}
        public int NeedToNext {get{return needToNext;}}
        public int Earn {get{return earn;}}
        public int Total {get{return total;}}
        private CalendarInfoData(){}
        public CalendarInfoData (GameSettings.MedalType medal, int needToNext, int earn, int total)
        {
            this.medal = medal;
            this.needToNext = needToNext;
            this.earn = earn;
            this.total = total;
        }
    }
    public class CalendarIOManager
    {
        public string[] monthName = {"January", "Fabruary","March","April","May","June","July","August","September","October","November","December"};
        private string CreateFileName (int month, int year)
        {
            return monthName [month-1] + " " + year.ToString ()+".txt";
        }
        #region Tournament
        public List<CalendarMonthData> InitTournament()
        {
            List<CalendarMonthData> tournamentData = new List<CalendarMonthData> ();

            int currentYear = 2019; // ref 2017 publishing
            int currentMonth = 10;
            while (!HasMonthRanking (currentMonth, currentYear))
            {
                currentMonth++;
                if (currentMonth.Equals (13))
                {
                    currentMonth = 1;
                    currentYear++;
                }
            }
            while (HasMonthRanking (currentMonth, currentYear))
            {
                CalendarMonthData data = CreateTournamentData (currentMonth, currentYear);
                tournamentData.Add (data);
                currentMonth++;
                if (currentMonth.Equals (13))
                {
                    currentMonth = 1;
                    currentYear++;
                }
            }
            return tournamentData;
        }

        public CalendarMonthData CreateTournamentData (int month, int year)
        {
            int daysInMonth = DateTime.DaysInMonth (year, month);
            int[] medalData = LoadMonthRanking (month, year);
            int earnMedal = CountMedals (medalData);

            GameSettings.MedalType medalType = MedalType (earnMedal,1);
            return new CalendarMonthData (year, month, medalType, earnMedal, daysInMonth);
        }
        private int CountMedals (int[] medal)
        {
            int total = 0;
            for (int index = 0; index < medal.Length; index++)
                total += medal [index];
            return total;
        }
        private GameSettings.MedalType MedalType (int earn, int monthCount)
        {
            int totalMedalType = GameSettings.Instance.calendarRankLevelUp.Length;
            int index = 0;
            while (index < totalMedalType && earn >= (GameSettings.Instance.calendarRankLevelUp [index] * monthCount)) index++;
            return (GameSettings.MedalType) index;
        }
        public int MedalToNextType (int earn, int monthCount)
        {
            int totalMedalType = GameSettings.Instance.calendarRankLevelUp.Length;
            int index = 0;
            while (index < totalMedalType && earn >= (GameSettings.Instance.calendarRankLevelUp [index] * monthCount)) index++;
            return (index.Equals(totalMedalType)) ? 0 : (GameSettings.Instance.calendarRankLevelUp [index] * monthCount) - earn;
        }
        public CalendarInfoData GetMonthRankingData  (int month, int year, List<CalendarMonthData> tournamentData)
        {
            CalendarMonthData findData = new CalendarMonthData(-1,-1,GameSettings.MedalType.None,-1,-1);
            foreach (CalendarMonthData element in tournamentData)
                if (element.Year.Equals (year) && element.Month.Equals (month))
                    findData = element;
            int needToNext = MedalToNextType (findData.Earn, 1);

           
            return new CalendarInfoData (findData.Medal, needToNext, findData.Earn, findData.Total);
        }
        public CalendarInfoData GetGlobalRankingData (List<CalendarMonthData> tournamentData)
        {
            int earn = 0;
            int total = 0;
            foreach (CalendarMonthData element in tournamentData)
            {
                earn += element.Earn;
                total += element.Total;
             
            }
           
            int monthCount = tournamentData.Count;
            GameSettings.MedalType medalType = MedalType (earn, monthCount);
            int needToNext = MedalToNextType (earn, monthCount);
            return new CalendarInfoData (medalType, needToNext, earn, total);
        }
        #endregion
        #region Ranking
        public bool HasMonthRanking(int month, int year)
        {



            int dayIndex = 0;
            string fileName = string.Format("{0}{1}{2}",  (dayIndex<9)?"0"+dayIndex:dayIndex.ToString(), (month < 9) ? "0" + month : month.ToString(), year);



            return PlayerPrefs.HasKey(fileName);
        }
        public void SaveMonthRanking (int month, int year, int[] monthRanking)
        {
            int daysInMonth = DateTime.DaysInMonth (year, month);
        //    Debug.Log("Save Month");
            for (int dayIndex = 0; dayIndex < daysInMonth; dayIndex++)
            {
                string fileName = string.Format("{0}{1}{2}", (dayIndex < 9) ? "0" + dayIndex : dayIndex.ToString(), (month < 9) ? "0" + month : month.ToString(), year);
                PlayerPrefs.SetInt(fileName, PlayerPrefs.GetInt(fileName,0));
             
            }
        }
        public int[] LoadMonthRanking (int month, int year)
        {
            int[] loadedRanking = new int[31];
        
        
 
            int daysInMonth = DateTime.DaysInMonth (year, month);
            for (int dayIndex = 0; dayIndex < daysInMonth; dayIndex++)
            {
                string fileName = string.Format("{0}{1}{2}", (dayIndex < 9) ? "0" + dayIndex : dayIndex.ToString(), (month < 9) ? "0" + month : month.ToString(), year);
 
                loadedRanking [dayIndex] = PlayerPrefs.GetInt(fileName, 0);

            //    Debug.Log("  loadedRanking [dayIndex]  " + loadedRanking[dayIndex] );
            }
 
            return loadedRanking;
        }
        public int GetDayMedal (int day, int month, int year)
        {
            if (!HasMonthRanking(month, year))
                return 0;
            int[] ranking = LoadMonthRanking(month, year);
            if (day >= ranking.Length) day = ranking.Length;


            return ranking[day - 1];
        }
        #endregion
        #region MonthSolitaireChallenge

        public bool HasMonthChallenge (int month, int year)
        {
         
            return HasMonthRanking(month,year);
        }

        public string GetDayChallenge(int day, int month, int year)
        {
            string solitaireData = string.Empty;
            int position = 0;
            string fileName = string.Format("Match{0}{1}{2}", day < 9 ? "0" + day : day.ToString(), month < 9 ? "0" + month : month.ToString(), year);

            int active = PlayerPrefs.GetInt(fileName);
       
            if (active == 0)
            {
                
                position = UnityEngine.Random.Range(0, SolitaireStageViewHelperClass.instance.GetOneCardWining.Count - 1);
                solitaireData = SolitaireStageViewHelperClass.instance.GetOneCardWining[position];
                PlayerPrefs.SetInt(fileName, position);

            }
            else
            {
                position = active;
                solitaireData = SolitaireStageViewHelperClass.instance.GetOneCardWining[position];

                if (GameSettings.Instance.numberOfSuit == 2)
                {
                    solitaireData = SolitaireStageViewHelperClass.instance.GetTwoCardWining[position];
                }
                else if (GameSettings.Instance.numberOfSuit == 4)
                {
                    solitaireData = SolitaireStageViewHelperClass.instance.GetFourCardWining[position];
                }


              

            }


            return solitaireData;
        }

       
        #endregion
        public bool IsToday (int compareDay, int compareMonth, int compareYear)
        {
            DateTime today = DateTime.Today;
            if (compareYear.Equals (today.Year) && compareMonth.Equals (today.Month) && compareDay.Equals (today.Day)) return true;
            return false;
        }
    }
        
    public class SolutionAsset
    {
        string[] txtLines;
        public int Count { get {return (txtLines.Length-1)/2;}}
        public SolutionAsset (bool isOneCard)
        {
            TextAsset textAsset = (isOneCard) ? GameSettings.Instance.textAssetOneCard : GameSettings.Instance.textAssetThreeCards;
            txtLines= textAsset.text.Split('\n');
        }
        public string[] Get ()
        {
            int index = UnityEngine.Random.Range (0, Count);
            string[] solitaireData = new string[2];
            solitaireData[0] = txtLines[index*2].Substring(0, txtLines[index*2].Length-1);
            solitaireData[1] = txtLines[index*2+1].Substring(0,txtLines[index*2+1].Length-1);
            return solitaireData;
        }
    }
}
