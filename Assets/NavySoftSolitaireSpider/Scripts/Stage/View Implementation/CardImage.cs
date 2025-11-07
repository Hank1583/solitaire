using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardImage : MonoBehaviour
{
    /*
     * 0:Diamonds
     * 1:Hearts    
     * 2:Clubs
     * 3 :Spaders    
    
    */

    [SerializeField]
    private Image suitName;
    [SerializeField]
    private TextMeshProUGUI numberRank;
    [SerializeField]
    private Image blankCard;
    [SerializeField]
    private Image iconSuit;
    [SerializeField]
    private Image suitMain;

    [System.Serializable]
    public class CardJQK
    {
        public string name;
        public List<GameObject> cardsObject = new List<GameObject>();
    }
    [SerializeField]
    private List<CardJQK> cardJQKs = new List<CardJQK>();






    private string[] namesSuit = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    [SerializeField]
    private bool prevCardItem = false;
    private void Start()
    {
        if (prevCardItem) return;
        CardItem cardItem = GetComponent<CardItem>();

        SetCard(cardItem.Suit, cardItem.Rank - 1);

    }

    public void VisibleJQK(Color color)
    {
        for (int i = 0; i < cardJQKs.Count; i++)
        {
            for (int j = 0; j < cardJQKs[i].cardsObject.Count; j++)
            {
                cardJQKs[i].cardsObject[j].GetComponent<Image>().color = color;
            }
        }
    }

    public void SetCard(int suit, int rank)
    {
        if (rank < 0) return;
    
        for (int i = 0; i < cardJQKs.Count; i++)
        {
            for (int j = 0; j < cardJQKs[i].cardsObject.Count; j++)
            {
                cardJQKs[i].cardsObject[j].SetActive(false);
            }

        }
        suitMain.GetComponent<RectTransform>().offsetMax = Vector2.zero;
        suitMain.GetComponent<RectTransform>().offsetMin = Vector2.zero;
       
    

        int visualCardFace = GameSettings.Instance.visualCardFacesSet;
        blankCard.sprite = ImageSettings.Instance.cardFaceGroups[visualCardFace].blankCard;
        suitMain.gameObject.SetActive(true);
        iconSuit.sprite = ImageSettings.Instance.cardFaceGroups[visualCardFace].suitLarge[suit];
        suitMain.sprite = ImageSettings.Instance.cardFaceGroups[visualCardFace].suitLarge[suit];

        for (int i = 0; i < ImageSettings.Instance.cardFaceGroups[visualCardFace].cardsRankHasIcon.Length; i++)
        {
            int rankHasIcon = ImageSettings.Instance.cardFaceGroups[visualCardFace].cardsRankHasIcon[i];

            if (rank == rankHasIcon)
            {
              
                // Debug.Log(string.Format("Rank Icon {0}-{1}-{2}", rankHasIcon, rank,visualCardFace));
                cardJQKs[visualCardFace].cardsObject[rank - 10].SetActive(true);
                suitMain.gameObject.SetActive(false);
                if (ImageSettings.Instance.cardFaceGroups[visualCardFace].useIconColor)
                {
                    if (suit == 0 || suit == 1)
                    {
                        cardJQKs[visualCardFace].cardsObject[rank - 10].GetComponent<Image>().color = ImageSettings.Instance.cardFaceGroups[visualCardFace].colorRed;
                    }
                    else
                    {
                        cardJQKs[visualCardFace].cardsObject[rank - 10].GetComponent<Image>().color = ImageSettings.Instance.cardFaceGroups[visualCardFace].colorBlack;
                    }
                }
            }
        }
        RectTransform rectSuit = suitMain.GetComponent<RectTransform>();
 
       
        rectSuit.offsetMax = new Vector2(-ImageSettings.Instance.cardFaceGroups[visualCardFace].offsetSuitLarge[suit].y, -ImageSettings.Instance.cardFaceGroups[visualCardFace].offsetTopBottomSuitLarge[suit].x);
        rectSuit.offsetMin = new Vector2(ImageSettings.Instance.cardFaceGroups[visualCardFace].offsetSuitLarge[suit].x, ImageSettings.Instance.cardFaceGroups[visualCardFace].offsetTopBottomSuitLarge[suit].y);
 
        // Debug.Log(string.Format("Visual Icon {0}-{1}", visualCardFace, rank));
          
      
            if (ImageSettings.Instance.cardFaceGroups[visualCardFace].useImageTextNumber)
        {
            suitName.color = Color.white;
            suitName.enabled = true;
            numberRank.enabled = false;
            if (suit == 0 || suit == 1)
            {

                suitName.sprite = ImageSettings.Instance.cardFaceGroups[visualCardFace].numbersRed[rank];

            }
            else
            {

                suitName.sprite = ImageSettings.Instance.cardFaceGroups[visualCardFace].numbersBlack[rank];

            }
        }
        else {
          //  suitName.sprite = ImageSettings.Instance.cardFaceGroups[visualCardFace].numbers[rank];
            numberRank.font  = ImageSettings.Instance.cardFaceGroups[visualCardFace].font;
 
            numberRank.text = (rank+1).ToString();
            numberRank.enabled = true;
           
            suitName.enabled = false;
            if (rank == 0)
            {
                numberRank.text = "A";
            }
            else if (rank==10)
            {
                numberRank.text = "J";
            }
            else if (rank == 11)
            {
                numberRank.text = "Q";
            }
            else  if (rank == 12)
            {
                numberRank.text = "K";
            }
            if (suit == 0 || suit == 1)
            {
                numberRank.color = ImageSettings.Instance.cardFaceGroups[visualCardFace].colorRed;
           //     suitName.color = ImageSettings.Instance.cardFaceGroups[visualCardFace].colorRed;

            }
            else
            {
                numberRank.color = ImageSettings.Instance.cardFaceGroups[visualCardFace].colorBlack;
              //  suitName.color = ImageSettings.Instance.cardFaceGroups[visualCardFace].colorBlack;

            }
        }

    }
}
