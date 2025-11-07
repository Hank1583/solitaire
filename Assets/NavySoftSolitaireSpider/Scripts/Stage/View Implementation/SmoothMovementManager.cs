using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

public class SmoothMovementManager : MonoBehaviour
{

	public static SmoothMovementManager instance;

	public Transform followContTransform;
	[SerializeField]
	private float smoothTimeMove = 10f;
	[SerializeField]
	private float distMove = 5f;
	public float GetDistanceMove { get { return distMove; } }
	public float GetSmoothSpeed { get { return smoothTimeMove; } }
	[System.Serializable]
	class MovingObj
	{
		public int id;
		public bool checkCardUndo;
		public bool moveCurve;
		public float timer;
		public Vector3 otherPosition;
		public Vector3 originPosition;
		public Transform target;
		public Transform destination;
		public Vector3 velocity = Vector3.zero;
		public Vector3 offset = Vector3.zero;
		public UnityAction onFinish;
	}


	public enum Speed
	{
		Deal,
		Deck,
		Move,
		Undo,
		Hint,
		AutoComplete,
		Solution1x,
		Solution2x,
		Solution3x
	}

	[Space(20f)]
	[SerializeField]
	private Speed currentSpeedType;


	[Header("Speed Consts")]


	[SerializeField]
	private float speedDeal;
	[SerializeField]
	private float speedDeck;
	[SerializeField]
	private float speedMove;
	[SerializeField]
	private float speedUndo;
	[SerializeField]
	private float speedHint;
	[SerializeField]
	private float speedAutoComplete;
	[SerializeField]
	private float speedSolution1x;
	[SerializeField]
	private float speedSolution2x;
	[SerializeField]
	private float speedSolution3x;

	[Space(20f)]
	[SerializeField]
	private List<MovingObj> cardsOnMoving = new List<MovingObj>();

	private void Awake()
	{
		instance = this;
	}



	void Update()
	{
		UpdateFollow();
		UpdateMove();
	}




	public bool isMoving
	{
		get
		{
			return cardsOnMoving.Count > 0;
		}
	}





	// FOLLOW

	private Transform followTransform;
	private float followSpeed = 0.05f;
	private Vector3 followVelocity = Vector3.zero;

	public void Follow(Transform transform)
	{
		followTransform = transform;
	}

	private void UpdateFollow()
	{
		if (followTransform == null)
			return;
		followTransform.position = Vector3.SmoothDamp(followTransform.position, followContTransform.position, ref followVelocity, followSpeed);
	}



	// MOVES

	public void SetMovingSpeed(Speed speed)
	{
		currentSpeedType = speed;
		speed = currentSpeedType;
		switch (speed)
		{
			case Speed.Deal:
				smoothTimeMove = speedDeal;
				break;
			case Speed.Move:
				smoothTimeMove = speedMove;
				break;
			case Speed.Deck:
				smoothTimeMove = speedDeck;
				break;
			case Speed.Undo:
				smoothTimeMove = speedUndo;
				break;
			case Speed.Hint:
				smoothTimeMove = speedHint;
				break;
			case Speed.AutoComplete:
				smoothTimeMove = speedAutoComplete;
				break;
			case Speed.Solution1x:
				smoothTimeMove = speedSolution1x;
				break;
			case Speed.Solution2x:
				smoothTimeMove = speedSolution2x;
				break;
			case Speed.Solution3x:
				smoothTimeMove = speedSolution3x;
				break;
		}

	}


	public void Move(int id, Transform selectedCard, Transform destinationCard, Vector2 offset, bool moveCurve, UnityAction onFight)
	{

		foreach (var c in cardsOnMoving)
		{

			if (id.Equals(c.id))
			{

				return;
			}
		}



		MovingObj mov = new MovingObj();
		mov.id = id;
		mov.moveCurve = moveCurve;
		mov.checkCardUndo = false;
		mov.target = selectedCard;

		mov.originPosition = mov.target.transform.position;
		mov.destination = destinationCard;
		mov.offset = offset;
		mov.onFinish = onFight;
		cardsOnMoving.Add(mov);
		CardItem card = destinationCard.transform.GetComponent<CardItem>();
	}

	private void UpdateMove()
	{

		for (int i = cardsOnMoving.Count - 1; i >= 0; i--)
		{

			MovingObj mo = cardsOnMoving[i];

			if (mo.moveCurve)
			{


				Vector3 p0 = mo.originPosition;

				Vector3 p2 = mo.destination.transform.position;

				Vector3 midpointAtoB = p0 + (p2 - p0) / 2;

				Vector3 p1 = new Vector3(midpointAtoB.x, -Mathf.Abs(midpointAtoB.y) / 4);

				mo.timer += Time.deltaTime * 2f;
				bool distanse_near_destination = Vector3.Distance(mo.target.transform.position, p2) < distMove;
				if (mo.timer >= 1 || distanse_near_destination || !ContinueModeGame.instance.LoadSuccess)
				{
					mo.target.transform.position = p2;
					// Debug.Log("Move " + mo.target.transform.name);
					mo.moveCurve = false;
					TriggerFinish(mo);
					return;
				}

				Vector3 position = (1.0f - mo.timer) * (1.0f - mo.timer) * p0
				  + 2.0f * (1.0f - mo.timer) * mo.timer * p1 + mo.timer * mo.timer * p2;




				mo.target.transform.position = position;

			}
			else
			{

				Vector3 destinationPosition = mo.destination.transform.TransformPoint(mo.offset);

				bool distanse_near_destination = Vector3.Distance(mo.target.transform.position, destinationPosition) < distMove;
				if (!ContinueModeGame.instance.LoadSuccess)
				{

					mo.target.transform.position = destinationPosition;

				}
				else
				{


					mo.target.transform.position = Vector3.SmoothDamp(mo.target.transform.position, destinationPosition, ref mo.velocity, smoothTimeMove);
				}
				if (distanse_near_destination)
				{

					mo.target.transform.position = destinationPosition;


					TriggerFinish(mo);
				}

			}

		}

	}

	public bool replay = false;

	private void TriggerFinish(MovingObj mo)
	{
		mo.onFinish();
		cardsOnMoving.Remove(mo);
	}

	public void ClearMoveWhenUndo()
	{
		for (int i = cardsOnMoving.Count - 1; i >= 0; i--)
		{

			MovingObj mo = cardsOnMoving[i];

			if (mo.moveCurve)
			{
				mo.target.transform.position = mo.destination.transform.position;

			}
			else
			{
				Vector3 destinationPosition = mo.destination.transform.TransformPoint(mo.offset);
				mo.target.transform.position = destinationPosition;
				TriggerFinish(mo);
			}

		}

		cardsOnMoving.Clear();

	}

}
