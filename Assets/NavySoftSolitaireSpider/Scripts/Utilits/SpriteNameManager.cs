using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SpriteNameManager : MonoHandler
{

    public SpriteNameObject[] nameManager;

    public Sprite[] sprites;

    public override void GUIEditor()
    {
        if(GUILayout.Button("Get Name"))
        {
            for (int i = 0; i < nameManager.Length; i++)
            {
                if (nameManager[i] == null) continue;
                nameManager[i].GetName();
            }
        }

        if (GUILayout.Button("Set Sprite To Image"))
        {
            for (int i = 0; i < nameManager.Length; i++)
            {
if(nameManager[i] == null) continue;
                for (int j = 0; j < sprites.Length; j++)
                {

                   
                    string nameSprite = sprites[j].ToString();
                     nameSprite = nameSprite.Replace(" (UnityEngine.Sprite)", "").Trim();
                    if (nameManager[i].nameSprite.Trim().ToUpper().Equals(nameSprite.ToUpper()))
                    {
                      
                        nameManager[i].SetSprite(sprites[j]);
                        break;
                    }
                }
            }
        }



        if (GUILayout.Button("Change Name Arrow White"))
        {
            for (int i = 0; i < nameManager.Length; i++)
            {
                if (nameManager[i] == null) continue;
                if (nameManager[i].nameSprite.Equals("ArrowWhite"))
                {
                    nameManager[i].nameSprite = "back_button";
                }
            }
        }

  if (GUILayout.Button("Change Name Panel bg"))
        {
            for (int i = 0; i < nameManager.Length; i++)
            {
                if (nameManager[i] == null) continue;
                if (nameManager[i].nameSprite.Equals("Panel bg"))
                {
                    nameManager[i].nameSprite = "Calendar bg";
                }
            }
        }
        if (GUILayout.Button("Change Name   button_temp"))
        {
            for (int i = 0; i < nameManager.Length; i++)
            {
                if (nameManager[i] == null) continue;
                if (nameManager[i].nameSprite.Equals("button_temp"))
                {
                    nameManager[i].nameSprite = "Calendar bg";
                }
            }
        }

      
        base.GUIEditor();
    }
}
