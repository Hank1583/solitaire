using UnityEngine;
using System;
using System.Collections.Generic;
public class PopUpAnimator : MonoBehaviour
{
	private const float SPEED = 2.5f;

	public enum Direction {None=0,FromLeft, FromRight, FromDown, FromUp, Central}

	private List<PopUpData> window;
	private int windowsCount;

	private bool isAnimating;
	private bool isForvard;

	private float animationTime;

	private void Awake ()
	{
		isAnimating = false;
		window = new List<PopUpData> ();
		windowsCount = 0;
	}
	public void Show (GameObject origin, Direction direction, Action func)
	{
		if (isAnimating)
		{
			DestroyLastWindow ();
			isAnimating = false;
		}

		Vector2 startPosition = StartPosition (direction);

		PopUpData popUpData = new PopUpData (origin, startPosition, func);
		popUpData.SetInitPositionTransform ();

		window.Add(popUpData);
		windowsCount++;

		if (direction.Equals (Direction.Central))
			return;

		animationTime = 0f;
		isAnimating = true;
		isForvard = true;
	}
	public bool IsPresentWindowInPopUp(GameObject origin)
	{
		bool isPresent = false;
		foreach (PopUpData element in window)
		{	
			if (element.Obj.name.Equals (origin.name))
				isPresent = true;
		}
		return isPresent;
	}
	private Vector2 StartPosition (Direction direction)
	{
		Vector2 startPosition = Vector2.zero;
		switch (direction)
		{
		case Direction.FromLeft:
			{
				startPosition = new Vector2 (-2000f, 0f);
				break;
			}
		case Direction.FromRight:
			{
				startPosition = new Vector2 (2000f, 0f);
				break;
			}
		case Direction.FromUp:
			{
				startPosition = new Vector2 (0f, 2000f);
				break;
			}
		case Direction.FromDown:
			{
				startPosition = new Vector2 (0f, -2000f);
				break;
			}
		case Direction.Central:
			{
				startPosition = Vector2.zero;
				break;
			}
		default: startPosition = Vector2.zero;
			break;
		}
		return startPosition;
	}
	public void CloseLastWindow()
	{
       
		if (windowsCount.Equals (0))		//throw new System.Exception ("Try close empty windows");
			return;

		if (isAnimating)
		{
			DestroyLastWindow ();
			isAnimating = false;
		}

		if (!isAnimating)
		{
			int lastIndex = windowsCount -1;
			if (lastIndex < 0)
				return;
			if (window[lastIndex].Position.Equals (Vector2.zero))
			{
				DestroyLastWindow ();
				return;
			}
			animationTime = 0f;
			isAnimating = true;
			isForvard = false;
		}

	}
	private void DestroyLastWindow()
	{

      
        if (windowsCount>0)
		{
			int lastIndex = windowsCount -1;
           
            Destroy (window[lastIndex].Obj);
           // Debug.Log("windowsCount " + window[lastIndex].Obj.name);
            Action callback = window [lastIndex].Callback;
			if (callback != null) callback();

			window.RemoveAt (lastIndex);
			windowsCount--;
		}
	}
	private void Update()
	{
		if (isAnimating) 
		{
			float time = Time.deltaTime;
			animationTime += time * SPEED;
			if (animationTime > 1f)
			{
				animationTime = 1f;
				isAnimating = false;
			}
			int lastIndex = windowsCount -1;
			if (isForvard)
				window[lastIndex].RT.localPosition = Vector2.Lerp (window[lastIndex].Position, Vector2.zero, animationTime);
			else
			{
				window[lastIndex].RT.localPosition = Vector2.Lerp (Vector2.zero, window[lastIndex].Position, animationTime);
				if (animationTime.Equals(1f)) DestroyLastWindow ();
			}
		}
	}
}