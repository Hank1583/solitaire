using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class SoundButton : MonoBehaviour,IPointerDownHandler
{
    [System.Serializable]
    public enum ButtonSoundType
    {
        None,
        Click,
        Hint,
        Undo
    }

    [SerializeField]
    private ButtonSoundType buttonSoundType;

    public void OnPointerDown(PointerEventData eventData)
    {

        switch (buttonSoundType)
        {
            case ButtonSoundType.Click:
                Sound.Instance.ButtonClick();
                break;
            case ButtonSoundType.Undo:
              Sound.Instance.UndoCard();
                break;
            case ButtonSoundType.Hint:
            Sound.Instance.HintCard();
                break;
        }
      
    }
}
