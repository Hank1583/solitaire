using UnityEngine;
using UnityEngine.Events;
//using System.Collections.Generic;
using Dance;

public class DanceIniter : MonoBehaviour
{
	[SerializeField]
	private DanceManager manager;
	private DancerData dancerData;


	public void Show (RectTransform slot1, RectTransform slot2, RectTransform slot3, RectTransform slot4,
        RectTransform slot5, RectTransform slot6, RectTransform slot7, RectTransform slot8, UnityAction callback)
	{
		RectTransform[] rt = new RectTransform[4];
		rt [0] = slot1;
		rt [1] = slot2;
		rt [2] = slot3;
		rt [3] = slot4;
       
        DancerData dancerData = GetNextDanceTrack ();
		if (dancerData.Init ())
		{
			manager.Init (rt, dancerData);
			manager.Run (callback);
		}
	}
	public void Hide ()
	{
		manager.DanceStop ();
	}
	private DancerData GetNextDanceTrack()
	{
		dancerData = new DancerData ();
     

        switch (GameSettings.Instance.danceCurrentCounter) 
		{
		case 0:
			//Star
			dancerData.Add (AnimationSettings.Instance.Star, 52, 5f);
          
                break;
		case 1:
			//Eye
			dancerData.Add (AnimationSettings.Instance.LeftArrow, 13, 2f);
			dancerData.Add (AnimationSettings.Instance.RightArrow, 13, 2f);
			dancerData.Add (AnimationSettings.Instance.CenterCircl, 26, 4f);
			break;
		case 2:
			//Perimetre
			dancerData.Add (AnimationSettings.Instance.Perimetre, 52, 12F);
			break;
		case 3:
			//Magendovid
			dancerData.Add (AnimationSettings.Instance.TriangleUp, 26, 7f);
			dancerData.Add (AnimationSettings.Instance.TriangleDown, 26, 12f);
			break;
		case 4:
			//Star-Per
			dancerData.Add (AnimationSettings.Instance.Star, 30, 5f);
			dancerData.Add (AnimationSettings.Instance.Perimetre, 22, 12F);
			break;
		case 5:
			//SquareRotation
			dancerData.Add (AnimationSettings.Instance.Perimetre, 19, 6F);
			dancerData.Add (AnimationSettings.Instance.Perimetre2, 14, 10F);
			dancerData.Add (AnimationSettings.Instance.Perimetre3, 11, 8F);
			dancerData.Add (AnimationSettings.Instance.Perimetre4, 8, 12F);
			break;
		case 6:
			//TwoTraingles
			dancerData.Add (AnimationSettings.Instance.PerTraingles1, 26, 6F);
			dancerData.Add (AnimationSettings.Instance.PerTraingles2, 26, 6F);
			break;
		case 7:
			//FourTraingles
			dancerData.Add (AnimationSettings.Instance.PerTraingles1, 13, 6F);
			dancerData.Add (AnimationSettings.Instance.PerTraingles2, 13, 6F);
			dancerData.Add (AnimationSettings.Instance.PerTraingles3, 13, 6F);
			dancerData.Add (AnimationSettings.Instance.PerTraingles4, 13, 6F);
			break;
		case 8:
			//ScheduleTraingles
			dancerData.Add (AnimationSettings.Instance.ScheduleTraingles1, 26, 12F);
			dancerData.Add (AnimationSettings.Instance.ScheduleTraingles2, 26, 12F);
			break;
		case 9:
			//FourScheduleTraingles
			dancerData.Add (AnimationSettings.Instance.ScheduleTraingles1, 13, 12F);
			dancerData.Add (AnimationSettings.Instance.ScheduleTraingles2, 13, 12F);
			dancerData.Add (AnimationSettings.Instance.ScheduleTraingles3, 13, 12F);
			dancerData.Add (AnimationSettings.Instance.ScheduleTraingles4, 13, 12F);
			break;
		case 10:
			//ScheduleUpDown
			dancerData.Add (AnimationSettings.Instance.ScheduleUp, 26, 9F);
			dancerData.Add (AnimationSettings.Instance.ScheduleDown, 26, 9F);
			break;
		case 11:
			//OECD
			dancerData.Add (AnimationSettings.Instance.OECDCircl, 26, 12F);
			dancerData.Add (AnimationSettings.Instance.OECDArrow1, 13, 8F);
			dancerData.Add (AnimationSettings.Instance.OECDArrow2, 13, 8F);
			break;
		case 12:
			//EngFlag
			dancerData.Add (AnimationSettings.Instance.VerLine, 26, 8F);
			dancerData.Add (AnimationSettings.Instance.HorLine, 26, 8F);
			break;
		case 13:
			//Mountains
			dancerData.Add (AnimationSettings.Instance.MountUp, 26, 10F);
			dancerData.Add (AnimationSettings.Instance.MountDown, 26, 10F);
			break;
		case 14:
			//Fountain
			dancerData.Add (AnimationSettings.Instance.Fountain, 52, 8F);
			break;
		case 15:
			//MalteseCross
			dancerData.Add (AnimationSettings.Instance.MalteseCross1, 13, 6F);
			dancerData.Add (AnimationSettings.Instance.MalteseCross2, 13, 6F);
			dancerData.Add (AnimationSettings.Instance.MalteseCross3, 13, 6F);
			dancerData.Add (AnimationSettings.Instance.MalteseCross4, 13, 6F);
			break;
		case 16:
			//BigMalCross
			dancerData.Add (AnimationSettings.Instance.BigMalCross1, 13, 4F);
			dancerData.Add (AnimationSettings.Instance.BigMalCross2, 13, 4F);
			dancerData.Add (AnimationSettings.Instance.BigMalCross3, 13, 4F);
			dancerData.Add (AnimationSettings.Instance.BigMalCross4, 13, 4F);
			break;
		case 17:
			//CirclVerHor
			dancerData.Add (AnimationSettings.Instance.CirclVerHor1, 26, 6F);
			dancerData.Add (AnimationSettings.Instance.CirclVer2, 13, 12F);
			dancerData.Add (AnimationSettings.Instance.CirclHor3, 13, 12);
			break;
		case 18:
			//Liliya
			dancerData.Add (AnimationSettings.Instance.LiliyaLeft1, 13, 6F);
			dancerData.Add (AnimationSettings.Instance.LiliyaRight2, 13, 6F);
			dancerData.Add (AnimationSettings.Instance.LiliyaCentr3, 16, 6F);
			dancerData.Add (AnimationSettings.Instance.LiliyaHor4, 10, 4F);
			break;
		case 19:
			//CirclLines
			dancerData.Add (AnimationSettings.Instance.CirclLines1, 12, 8F);
			dancerData.Add (AnimationSettings.Instance.CirclLines2, 5, 4F);
			dancerData.Add (AnimationSettings.Instance.CirclLines3, 5, 4F);
			dancerData.Add (AnimationSettings.Instance.CirclLines4, 5, 4F);
			dancerData.Add (AnimationSettings.Instance.CirclLines5, 5, 4F);
			dancerData.Add (AnimationSettings.Instance.CirclLines6, 5, 4F);
			dancerData.Add (AnimationSettings.Instance.CirclLines7, 5, 4F);
			dancerData.Add (AnimationSettings.Instance.CirclLines8, 5, 4F);
			dancerData.Add (AnimationSettings.Instance.CirclLines9, 5, 4F);
			break;
		case 20:
			//Atom
			dancerData.Add (AnimationSettings.Instance.Atom1, 13, 8F);
			dancerData.Add (AnimationSettings.Instance.Atom2, 13, 8F);
			dancerData.Add (AnimationSettings.Instance.Atom3, 13, 8F);
			dancerData.Add (AnimationSettings.Instance.Atom4, 13, 8F);
			break;
		case 21:
			//Triquctrum
			dancerData.Add (AnimationSettings.Instance.Triquctrum1, 25, 9F);
			dancerData.Add (AnimationSettings.Instance.Triquctrum2, 9, 10F);
			dancerData.Add (AnimationSettings.Instance.Triquctrum3, 9, 10F);
			dancerData.Add (AnimationSettings.Instance.Triquctrum4, 9, 10F);
			break;
		case 22:
			//Circulation
			dancerData.Add (AnimationSettings.Instance.Circulation, 52, 26F);
			break;
		case 23:
			//FourSquare
			dancerData.Add (AnimationSettings.Instance.FourSquare1, 13, 8F);
			dancerData.Add (AnimationSettings.Instance.FourSquare2, 13, 8F);
			dancerData.Add (AnimationSettings.Instance.FourSquare3, 13, 8F);
			dancerData.Add (AnimationSettings.Instance.FourSquare4, 13, 8F);
			break;
		case 24:
			//QuadroStar
			dancerData.Add (AnimationSettings.Instance.QuadroStar1, 26, 10F);
			dancerData.Add (AnimationSettings.Instance.QuadroStar2, 13, 10F);
			dancerData.Add (AnimationSettings.Instance.QuadroStar3, 13, 10F);
			break;
		case 25:
			//Labirint
			dancerData.Add (AnimationSettings.Instance.Labirint1, 26, 13F);
			dancerData.Add (AnimationSettings.Instance.Labirint2, 26, 13F);
			break;
		case 26:
			//Magendovid2
			dancerData.Add (AnimationSettings.Instance.Magendovid1, 26, 9F);
			dancerData.Add (AnimationSettings.Instance.Magendovid2, 26, 9F);
			break;
		case 27:
			//Hurt
			dancerData.Add (AnimationSettings.Instance.Hurt1, 26, 13F);
			dancerData.Add (AnimationSettings.Instance.Hurt2, 26, 13F);
			break;
		case 28:
			//Peak
			dancerData.Add (AnimationSettings.Instance.Peak1, 39, 6F);
			dancerData.Add (AnimationSettings.Instance.Peak2, 13, 10F);
			break;
		case 29:
			//Cross
			dancerData.Add (AnimationSettings.Instance.Cross1, 13, 8F);
			dancerData.Add (AnimationSettings.Instance.Cross2, 13, 6F);
			dancerData.Add (AnimationSettings.Instance.Cross3, 13, 6F);
			dancerData.Add (AnimationSettings.Instance.Cross4, 13, 6F);
			break;
		case 30:
			//ThreeCross
			dancerData.Add (AnimationSettings.Instance.ThreeCross, 52, 12F);
			break;
		default:
			break;
		}
		GameSettings.Instance.danceCurrentCounter++;
		if (GameSettings.Instance.danceCurrentCounter.Equals (GameSettings.Instance.danceTotalCounter))
			GameSettings.Instance.danceCurrentCounter = 0;
		return dancerData;
	}

}