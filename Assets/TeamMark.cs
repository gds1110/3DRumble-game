using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeamMark : MonoBehaviour
{
    [SerializeField]
    Image markImage;


   public void SetMarkColor(Define.WorldObject owner)
    {
        if(markImage)
        {

            switch (owner)
            {
                case Define.WorldObject.Unknown:
                    markImage.color = Color.white;
                    break;
                case Define.WorldObject.Player:
                    markImage.color = Color.blue;
                    break;
                case Define.WorldObject.Monster:
                    markImage.color = Color.red;
                    break;
                case Define.WorldObject.None:
                    markImage.color = Color.white;
                    break;
            }

        }
    }

}
