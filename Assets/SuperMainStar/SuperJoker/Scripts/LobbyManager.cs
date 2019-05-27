using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperJoker
{
    public class LobbyManager : MonoBehaviour
    {

        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
        private void Update()
        {
            UIController uIController = FindObjectOfType<UIController>();
            if(uIController)
            {
                uIController.TransitionTo(UIPage.PageType.LOBBY);
                Destroy(this.gameObject);
            }
        }
    }
}