/// <summary>
/// Image settings.
/// Singleton image data holder
/// </summary>
using UnityEngine;
using TMPro;
public class ImageSettings : ScriptableObject
{
    private static ImageSettings _instance = null;
    public static ImageSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = (ImageSettings)Resources.Load("ImageSettings");
                if (_instance == null)
                {
                    throw new UnityException("Asset can't found");
                }
            }
            return _instance;
        }
    }

    [System.Serializable]
    public class CardFaceGroup
    {
        public string nameGroup;
        public Sprite[] numbers;
        public Sprite blankCard;
        public Sprite[] suitLarge;
        public float[] rotationZSuit;
        public Vector2[] offsetSuitLarge;
        public Vector2[] offsetTopBottomSuitLarge;

        public TMP_FontAsset font;
        public TMP_ColorGradient colorGradient; 
        [Header("Image Text Number")]
        public bool useImageTextNumber = false;
        public Sprite[] numbersBlack;
        public Sprite[] numbersRed;


        [Header("Color Text Number")]
        public Color colorRed = Color.white;
        public Color colorBlack = Color.white;

        [Header("Card Has Icon")]
        public bool useIconColor = false;
        public int[] cardsRankHasIcon; //J Q K


    }


    public CardFaceGroup[] cardFaceGroups;
 
 
    public Sprite[] background;
    public Sprite[] backgroundPortrait;
    public Sprite[] cardbackHiResolurion;
    public Sprite[] cardFacesIcon;
    public Sprite[] medal;
}
