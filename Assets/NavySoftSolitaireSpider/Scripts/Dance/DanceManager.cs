namespace Dance
{
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.Events;
    using System.Collections.Generic;
    //using System.Collections.Generic;

    public class DanceManager : MonoHandler
    {
        private const float autoStopTime = 20f;
        private const float WAKE_UP_DECK_TIME = 2f;
        private const float START_FLY_TIME = .6f;
        private UnityAction callback;
        [SerializeField]
        private GameObject cardDancePrefab;
        [SerializeField]
        private List<GameObject> cardObj;
        [SerializeField]
        private List<RectTransform> cardRT;

        [SerializeField]
        private List<CardImage> cardsImage;
        public RectTransform dancePlace;

        private DancerData dancerData;

        private bool isAnimate = false;
        private float startDeckCurrentTime;
        private bool isWakingDeck;
        private float workAnimationTime;
        private bool isCircleAnimate;
        private DancerCard[] danceCard = new DancerCard[52];

        private bool inited = false;

        private bool isOutsideFlying = false;
        private Vector2[] startOutsizeCardPos;
        private Vector2[] finalOutsizeCardPos;
        private int countCard = 0;
        public void Init(RectTransform[] rt, DancerData dancerData)
        {


            for (int suitIndex = 0; suitIndex < rt.Length; suitIndex++)
            {
                int suit = suitIndex;
                if (suitIndex > 3) suit = suitIndex - 4;
                InitCards(suit, rt[suitIndex]);
            }




            countCard = 0;




            for (int index = 0; index < 52; index++) danceCard[index] = new DancerCard();
            this.dancerData = dancerData;
            inited = true;

            int cardCount = cardRT.Count;
            startOutsizeCardPos = new Vector2[cardCount];
            finalOutsizeCardPos = new Vector2[cardCount];
            for (int index = 0; index < cardCount; index++)
            {
                startOutsizeCardPos[index] = Vector2.zero;
                finalOutsizeCardPos[index] = Vector2.zero;
            }
        }
        private void InitCards(int suit, RectTransform rt)
        {

            for (int rankIndex = 0; rankIndex < 13; rankIndex++)
            {

                cardRT[countCard].position = rt.position;
                cardsImage[countCard].SetCard(suit, rankIndex);
                countCard++;
            }
        }
        private void SetActiveCards(bool value)
        {

            foreach (GameObject element in cardObj)
                element.SetActive(value);
        }
        private void MoveCards(Vector2 pos)
        {
            foreach (RectTransform element in cardRT)
                element.position = pos;
        }
        public void Run(UnityAction callback)
        {

            this.callback = callback;
#if UNITY_EDITOR
            if (!inited)
                throw new UnityException("Dance animation isn't inited!");
#endif
            InitValue(true);
        }
        public void DanceStop()
        {
            CreateFinalCardPosition();
            workAnimationTime = 0f;
            isCircleAnimate = false;
            isOutsideFlying = true;
        }
        private void CreateFinalCardPosition()
        {

            float width = dancePlace.rect.width;
            float height = dancePlace.rect.height;

            int cardCount = cardRT.Count;
            for (int index = 0; index < cardCount; index++)
            {
                startOutsizeCardPos[index] = cardRT[index].localPosition;

                bool isXForce = (cardRT[index].localPosition.x > (width / 2));
                bool isYForce = (cardRT[index].localPosition.y > (height / 2));

                float x = 0f;
                float y = 0f;
                do
                {
                    x = (isXForce) ? Random.Range(0f, width * 2) : Random.Range(-1f * width, width);
                    y = (isYForce) ? Random.Range(0f, height * 2) : Random.Range(-1f * height, height);
                }
                while ((x > 0f) && (x < width) && (y > 0f) && (y < height));
                finalOutsizeCardPos[index] = new Vector2(x, y);
            }
        }

        public void DirectStop()
        {
            InitValue(false);
            MoveCards(Vector2.zero);
            callback();
        }

        private void InitValue(bool isActive)
        {

            SetActiveCards(isActive);
            startDeckCurrentTime = 0f;
            workAnimationTime = 0;
            isAnimate = isActive;
            isWakingDeck = isActive;
            isCircleAnimate = false;
            isOutsideFlying = false;
        }

        private void FixedUpdate()
        {
            if (isAnimate)
            {
                float time = Time.deltaTime;
                workAnimationTime += time;
                if (isWakingDeck)
                {
                    WaikingFrame(time);
                    if (workAnimationTime > (WAKE_UP_DECK_TIME + START_FLY_TIME))
                    {
                        isWakingDeck = false;
                        isCircleAnimate = true;
                        workAnimationTime = 0f;
                    }
                }
                if (isCircleAnimate)
                {
                    ShowAllCircleCards(time);
                    if (workAnimationTime > autoStopTime)
                        DanceStop();
                }
                if (isOutsideFlying)
                {
                    ShowAllOutsideFlyingCards(workAnimationTime);
                }
            }
        }
        private void ShowAllOutsideFlyingCards(float time)
        {

            const float speed = 2f;
            float flyValue = time * speed;
            if (flyValue > 1f)
                flyValue = 1f;
            int cardCount = cardRT.Count;
            for (int index = 0; index < cardCount; index++)
            {

                cardRT[index].localPosition = Vector2.Lerp(startOutsizeCardPos[index], finalOutsizeCardPos[index], flyValue);
            }
            if (flyValue.Equals(1f))
                DirectStop();
        }

        private void ShowAllCircleCards(float time)
        {

            int countTrecks = dancerData.Count;
            int shiftCard = 0;
            for (int trackIndex = 0; trackIndex < countTrecks; trackIndex++)
            {
                dancerData.AddTimeToCircle(trackIndex, time);

                int countCardsInSnake = dancerData.CardCount(trackIndex);
                ShowTrackCircleCards(shiftCard, shiftCard + countCardsInSnake, trackIndex, dancerData.NormalizeCircleTime(trackIndex));
                shiftCard += countCardsInSnake;
            }
        }
        private void ShowTrackCircleCards(int from, int to, int index, float normalizeTime)
        {

            if (!dancerData.IsLoop[index]) to = LastWaikedCardInSnake(from, to, index, normalizeTime);

            Vector2[] pivots = dancerData.GetTrackPivots(index, normalizeTime);
            int pivotIndex = 0;
            for (int cardIndex = from; cardIndex < to; cardIndex++)
            {

                cardRT[cardIndex].localPosition = PositionInDancePlace(pivots[pivotIndex]);
                pivotIndex++;
            }
        }

        private int LastWaikedCardInSnake(int from, int to, int index, float normalizeTime)
        {

            int totalCard = to - from;
            int awakedCard = (int)(totalCard * normalizeTime);
            return from + awakedCard;
        }

        private void WaikingFrame(float time)
        {

            startDeckCurrentTime += time;
            if (startDeckCurrentTime > WAKE_UP_DECK_TIME)
                startDeckCurrentTime = WAKE_UP_DECK_TIME;
            float normal = startDeckCurrentTime / WAKE_UP_DECK_TIME;
            int countTrecks = dancerData.Count;
            int shiftCard = 0;


            for (int trackIndex = 0; trackIndex < countTrecks; trackIndex++)
            {
                int countCardsInSnake = dancerData.CardCount(trackIndex);
                int toCardInSnake = (int)(countCardsInSnake * normal);
                WaikeCards(shiftCard, shiftCard + toCardInSnake, trackIndex);
                MoveCards(shiftCard, shiftCard + toCardInSnake, time);

                shiftCard += countCardsInSnake;
            }

        }
        private void WaikeCards(int from, int to, int index)
        {

            for (int cardIndex = from; cardIndex < to; cardIndex++)
            {
                if (danceCard[cardIndex].IsSleep)
                {
                    Vector2 source = cardRT[cardIndex].localPosition;
                    Vector2 destanation = PositionInDancePlace(dancerData.Track[index][0]);
                    danceCard[cardIndex].SetFlyToStart(source, destanation, START_FLY_TIME);
                }
            }
        }
        private Vector2 PositionInDancePlace(Vector2 normalize)
        {

            return Vector2.Scale(dancePlace.rect.size, normalize);
        }
        private void MoveCards(int from, int to, float time)
        {

            for (int cardIndex = from; cardIndex < to; cardIndex++)
            {
                if (danceCard[cardIndex].IsFlyingToStart)
                {
                    Vector2 newPosition = danceCard[cardIndex].NextFrameToStart(time);

                    cardRT[cardIndex].localPosition = newPosition;
                }
            }
        }



#if UNITY_EDITOR
        public override void GUIEditor()
        {
        if(GUILayout.Button("Create Card Dance"))
            {
                for (int i = 0; i < cardObj.Count; i++)
                {
                    DestroyImmediate(cardObj[i]);
                }
                cardObj.Clear();
                cardRT.Clear();
                cardsImage.Clear();

                for (int i = 0; i < 52; i++)
                {
                    GameObject card = Instantiate(cardDancePrefab) as GameObject;
                    card.transform.SetParent(cardDancePrefab.transform.parent, false);
                    card.transform.localScale = Vector3.one;
                    cardObj.Add(card);
                    cardRT.Add(card.GetComponent<RectTransform>());
                    cardsImage.Add(card.GetComponent<CardImage>());
                }
            }
            base.GUIEditor();
        }
#endif
    }
}
