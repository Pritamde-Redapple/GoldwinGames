using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    Texture2D cursor;

    void Start()
    {
        cursor = UIController.instance.cursor;
    }

   

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(cursor, new Vector2(21, 0), CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
