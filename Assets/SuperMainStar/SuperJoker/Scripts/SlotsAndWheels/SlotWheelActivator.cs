using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperJoker
{
    public class SlotWheelActivator : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        List<CardHistoryData> historyDeck;

        public void HistoryDeckData(List<CardHistoryData> hD)
        {
            historyDeck = new List<CardHistoryData>(hD);
          //  historyDeck = new List<Vector2>(hD);
        }

        private void Update()
        {
            if(LevelManager.inst != null && historyDeck != null)
            {
                LevelManager.inst.StartSpinning();
                LevelManager.inst.SetInputBlocker(true);
                LevelManager.inst.SetText(false);
                LevelManager.inst.SetCardHistoryDeck(historyDeck);
                Destroy(this.gameObject);
            }
        }
    }
}
