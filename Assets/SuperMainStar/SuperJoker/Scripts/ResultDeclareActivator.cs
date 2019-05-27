using UnityEngine;
using System.Collections.Generic;
namespace SuperJoker
{
    public class ResultDeclareActivator : MonoBehaviour
    {
        List<CardHistoryData> historyDeck;
        int outerTarget;
        int innerTarget;
        int cardIdSlot;
        int cardSuiteSlot;

        public void HistoryDeckData(List<CardHistoryData> hD)
        {
            historyDeck = new List<CardHistoryData>(hD);           
        }

        public void SetWinningResults(int outer, int inner, int idSlot, int suiteSlot)
        {
            outerTarget = outer;
            innerTarget = inner;
            cardIdSlot = idSlot;
            cardSuiteSlot = suiteSlot;
        }
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }        

        // Update is called once per frame
        void Update()
        {
            if (LevelManager.inst != null && historyDeck != null && UIManager.inst != null && ButtonManager.instance  != null)
            {
                Debug.Log("ResultDeclareActivator");
                ButtonManager.instance.ToggleClearDoubleRepeat(false);
                LevelManager.inst.SetInputBlocker(true);
                LevelManager.inst.SetText(false);
                LevelManager.inst.SetCardHistoryDeck(historyDeck);
                LevelManager.inst.SetWheelResults(outerTarget, innerTarget);
                UIManager.inst.ShowWaitForNextSession(true);
                LevelManager.inst.centerWheelCardAndSuite.SetCardImage(outerTarget, innerTarget);
                LevelManager.inst.SetMultiplierText((int)historyDeck[0].mulitiplier);
                LevelManager.inst.CheckMultiplierStateOnStop();
                LevelManager.inst.SetSelectionWheelLights(false);
                LevelManager.inst.SetCenterWheelTexts(true);
                LevelManager.inst.SetSlotResults(cardIdSlot, cardSuiteSlot);
                Destroy(this.gameObject);
            }
        }
    }
}
