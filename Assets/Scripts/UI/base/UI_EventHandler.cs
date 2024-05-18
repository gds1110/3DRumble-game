using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{

    public  Action<PointerEventData> OnBeginDragHandler = null;
    public  Action<PointerEventData> OnLeftClickHandler = null;
    public  Action<PointerEventData> OnRightClickHandler = null;
    public  Action<PointerEventData> OnDragHandler = null;



    public void OnPointerClick(PointerEventData eventData)
    {
        //if (OnClickHandler != null)
        //{
        //    OnClickHandler.Invoke(eventData);
        //}
        if(eventData.button==PointerEventData.InputButton.Left)
        {
            OnLeftClickHandler?.Invoke(eventData);
        }
        else if(eventData.button== PointerEventData.InputButton.Right)
        {
            OnRightClickHandler?.Invoke(eventData);
        }

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        if(OnBeginDragHandler!= null)
        {
            OnBeginDragHandler.Invoke(eventData);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {

        Debug.Log("OnDrag");
        if (OnDragHandler != null)
        {
            OnDragHandler.Invoke(eventData);
        }

    }


}
