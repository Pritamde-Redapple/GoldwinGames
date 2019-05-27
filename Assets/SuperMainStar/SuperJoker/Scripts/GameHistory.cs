using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
namespace SuperJoker
{
    public class GameHistory : MonoBehaviour
    {
       
        public GameObject historyStrip;
        public GameObject parentSpawnner;
        int slno=0;
        double totalPlay;
        float totalWin;

        public Text totalPlayText;
        public Text totalWinText;

       

        public void AddToGameHistory(PlayerHistory newPlayerHistory)
        {
            if (newPlayerHistory.playAmount <= 0)
                return;
            slno++;
            GameObject newStrip = Instantiate(historyStrip);
            newStrip.transform.SetParent(parentSpawnner.transform);
            newStrip.transform.localPosition = Vector3.zero;
            newStrip.transform.localScale = Vector3.one;
            HistoryStrip newHistoryStrip = newStrip.GetComponent<HistoryStrip>();
            newHistoryStrip.slno.text = ""+slno;
            newHistoryStrip.handId.text = newPlayerHistory.sessionID;
            newHistoryStrip.playAmount.text = ""+newPlayerHistory.playAmount;
            newHistoryStrip.winAmount.text = ""+newPlayerHistory.winAmount;
            newHistoryStrip.SetMultiplier(newPlayerHistory.multiplier, newPlayerHistory.isSuperBumper);
            totalPlay += newPlayerHistory.playAmount;
            totalWin += newPlayerHistory.winAmount;
            totalPlayText.text = "" + totalPlay;
            totalWinText.text = "" + totalWin;          
           
        }

                
    }
}
