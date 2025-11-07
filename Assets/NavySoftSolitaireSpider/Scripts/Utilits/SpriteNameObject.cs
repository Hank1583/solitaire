using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteNameObject : MonoHandler
{

    public string nameSprite   ;


    public void GetName()
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            nameSprite = GetComponent<SpriteRenderer>().sprite.ToString();
            nameSprite = nameSprite.Replace(" (UnityEngine.Sprite)", "");
        }
        else
        {

            nameSprite = GetComponent<Image>().sprite.ToString();
            nameSprite = nameSprite.Replace(" (UnityEngine.Sprite)", "");
        }
    }


    public void SetSprite(Sprite sprite)
    {
        if (GetComponent<SpriteRenderer>() != null)
        {
            GetComponent<SpriteRenderer>().sprite = sprite;
           
        }
        else
        {

            GetComponent<Image>().sprite = sprite;

        }
    }
    public override void GUIEditor()
    {

        if(GUILayout.Button("Get Name Sprite"))
        {
            GetName();
          

        }
        base.GUIEditor();
    }
}
