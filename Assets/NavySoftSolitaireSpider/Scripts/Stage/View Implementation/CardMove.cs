using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Assertions;

public class CardMove : MonoBehaviour
{
    [HideInInspector]
    public Transform followContTransform;
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

    private MovingObj mov;
    private bool isUpdate = false;

    private Transform followTransform;
    private float followSpeed = 0.05f;
    private Vector3 followVelocity = Vector3.zero;
    private Vector3 startPosition;
 
    public void Move(int id, Transform selectedCard, Transform destinationCard, Vector2 offset, bool moveCurve, UnityAction onFight)
    {
        mov = new MovingObj();
        mov.id = id;
        mov.moveCurve = moveCurve;
        mov.checkCardUndo = false;
        mov.target = selectedCard;
        mov.originPosition = mov.target.transform.position;
        mov.destination = destinationCard;
        mov.offset = offset;
        mov.onFinish = onFight;
      

  

    
        isUpdate = true;
    }

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

    private void Update()
    {
        UpdateFollow();
        if (!isUpdate) return;
            UpdateMove();
        
    }



    private void UpdateMove()
    {




        if (mov.moveCurve)
        {
            Vector3 p0 = mov.originPosition;

            Vector3 p2 = mov.destination.transform.position;

            Vector3 midpointAtoB = p0 + (p2 - p0) / 2;

            Vector3 p1 = new Vector3(midpointAtoB.x, -Mathf.Abs(midpointAtoB.y) / 4);

            mov.timer += Time.deltaTime * 2f;
            bool distanse_near_destination = Vector3.Distance(mov.target.transform.position, p2) < SmoothMovementManager.instance.GetDistanceMove;
            if (mov.timer >= 1 || distanse_near_destination || !ContinueModeGame.instance.LoadSuccess)
            {
                mov.target.transform.position = p2;
                mov.moveCurve = false;
                TriggerFinish(mov);
                return;
            }

            Vector3 position = (1.0f - mov.timer) * (1.0f - mov.timer) * p0
              + 2.0f * (1.0f - mov.timer) * mov.timer * p1 + mov.timer * mov.timer * p2;




            mov.target.transform.position = position;

        }
        else
        {

            Vector3 destinationPosition = mov.destination.transform.TransformPoint(mov.offset);

            bool distanse_near_destination = Vector3.Distance(mov.target.transform.position, destinationPosition) < SmoothMovementManager.instance.GetDistanceMove;
            if (!ContinueModeGame.instance.LoadSuccess)
            {

                mov.target.transform.position = destinationPosition;

            }
            else
            {


                mov.target.transform.position = Vector3.SmoothDamp(mov.target.transform.position, destinationPosition, ref mov.velocity, SmoothMovementManager.instance.GetSmoothSpeed);
            }
            if (distanse_near_destination)
            {

                mov.target.transform.position = destinationPosition;


                TriggerFinish(mov);
            }

        }

  

    }
    private void TriggerFinish(MovingObj mo)
    {
        if (mo.onFinish != null)
            mo.onFinish();

        isUpdate = false;

    }
}
