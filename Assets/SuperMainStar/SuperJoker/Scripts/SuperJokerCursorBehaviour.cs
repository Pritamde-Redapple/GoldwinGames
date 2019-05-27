using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SuperJoker
{
    public class SuperJokerCursorBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        Texture2D cursor;

        void Start()
        {
            cursor = LevelManager.inst.mouseCursor;
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
}
