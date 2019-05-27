using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputDetection : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            LeftClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Middle)
        {
            MiddleClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightClick();
        }
    }


    public virtual void LeftClick()
    {
       // Debug.Log("Left click");
    }

    public virtual void MiddleClick()
    {
        //Debug.Log("Middle click");
    }

    public virtual void RightClick()
    {
        //Debug.Log("Right click");
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
       
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        
    }


}
