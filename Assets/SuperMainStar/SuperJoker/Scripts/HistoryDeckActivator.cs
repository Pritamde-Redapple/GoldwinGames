using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SuperJoker
{
    public class HistoryDeckActivator : MonoBehaviour
    {

        List<CardHistoryData> historyDeck;
        int outerTarget;
        int innerTarget;
        int cardID;
        int suiteID;
        public void HistoryDeckData(List<CardHistoryData> hD)
        {
            historyDeck = new List<CardHistoryData>(hD);
           // historyDeck = hD;
        }

        public void SetWinningResults(int o, int i, int id, int suite)
        {
            outerTarget = o;
            innerTarget = i;
            cardID = id;
            suiteID = suite;
        }
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }

        // Update is called once per frame
        void Update()
        {
            if (LevelManager.inst != null && historyDeck != null)
            {
                Debug.Log("History Deck Activator");
                LevelManager.inst.SetCardHistoryDeck(historyDeck);
                LevelManager.inst.SetWheelResults(outerTarget, innerTarget);
                LevelManager.inst.SetSlotResults(cardID, suiteID);
                LevelManager.inst.centerWheelCardAndSuite.SetCardImage(outerTarget, (innerTarget));
                LevelManager.inst.SetMultiplierText((int)historyDeck[0].mulitiplier);
                LevelManager.inst.CheckMultiplierStateOnStop();
                LevelManager.inst.SetSelectionWheelLights(false);
                LevelManager.inst.SetCenterWheelTexts(true);
                Destroy(this.gameObject);
            }
        }
    }
}
