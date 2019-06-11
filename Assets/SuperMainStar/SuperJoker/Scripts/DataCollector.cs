using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using GWebUtility;
using DG.Tweening;
using System.Linq;
namespace SuperJoker {

    public class DataCollector : MonoBehaviour {

        public static DataCollector instance;

        Card[] card;
        Web web;
        CardSelectingButton[] cardSelectingButtons;

        UserData userData;
        bool canSendData;

        public GameHistory gameHistory;
        
        private void Awake()
        {
            instance = this;
            userData = new UserData(new CardData[0]);

        }

        private void Start()
        {
            TimerController.inst.TimeUp += FetchAllCardsAndSuites;
            canSendData = true;
        }

        public void SetCards(Card[] cards)
        {
            card = cards;            
        }

        public void SetCardCollectingButtons(CardSelectingButton[] selectingButtons)
        {
            cardSelectingButtons = selectingButtons;
        }

        public void FetchAllCardsAndSuites()
        {
            if(!canSendData)
            {
                canSendData = true;
                TimerController.inst.TimeUp -= FetchAllCardsAndSuites;
                return;
            }
            Invoke("CanSendData", 5);
            canSendData = false;
            TimerController.inst.TimeUp -= FetchAllCardsAndSuites;
            List<CardData>  cardDatas   = new List<CardData>();            
            

            foreach (Card item in card)
            {
                CardData newCardData = item.GetCardData();
                if(newCardData.cardBetValue != 0)
                    cardDatas.Add(item.GetCardData());
            }
            
            userData  = new UserData( cardDatas.ToArray());
        
            SendData(userData);            
        }


        public UserData GetLastUserData()
        {
            return userData;
        }
        
        void CanSendData()
        {
            canSendData = true;
        }

        //FinalCardData AddSuiteDataToCards(UserData newData)
        //{
        //    SuiteData[] suiteDatas = newData.suiteData;
        //    CardData[] cardDatas = newData.cardData;

        //    for (int i = 0; i < suiteDatas.Length; i++)
        //    {
        //        for (int j = 0; j < cardDatas.Length; j++)
        //        {
        //            if(cardDatas[j].cardID == suiteDatas[i].cardID )
        //            {
        //                cardDatas[j].cardBetValue += (suiteDatas[i].suiteBetValue / 4);                      
        //            }

        //            else if(cardDatas[j].cardSuite == suiteDatas[i].suiteID)
        //            {
        //                cardDatas[j].cardBetValue += (suiteDatas[i].suiteBetValue / 3);
        //            }
        //        }
        //    }

        //    List<CardData> newCardList = new List<CardData>();

        //    for (int i = 0; i < cardDatas.Length; i++)
        //    {
        //        if(cardDatas[i].cardBetValue  != 0)
        //        {
        //            newCardList.Add(cardDatas[i]);
        //        }
        //    }
        //    FinalCardData newFinaCardData = new FinalCardData(newCardList.ToArray());

        //    return newFinaCardData;
        //}

        bool HasThisSuiteCard(CardData[] data, int targetSuite)
        {
            foreach (CardData item in data)
            {
                if(item.cardSuite == targetSuite)
                {
                    return true;
                }
            }

            return false; 
        }

        bool HasThisCard(CardData[] data, int targetCard)
        {
            foreach (CardData item in data)
            {
                if(item.cardID == targetCard)
                {
                    return true;
                }
            }

            return false;
        }
        //"gamesession":"1544424572787",
        //"device_type":"pc",
        //"cardData":[{"cardID":1,"cardSuite":1,"cardBetValue":5}]
        void SendData(UserData data)
        {

            
            PostData postData = new PostData(Constant.CurrentGameSession, Constant.CurrentDeviceType, data.GetCardData());
           // Debug.Log("PostData: "+ JsonUtility.ToJson(postData));
            Debug.Log(System.DateTime.Now + "Sending data : " + JsonUtility.ToJson(postData));
            Web.Create()
           .SetUrl(Constant.AddSJGameDataURL, Web.RequestType.POST, Web.ResponseType.TEXT)
             .AddPostData(JsonUtility.ToJson(postData))
             .AddHeader("Content-Type", "application/json")
             .AddHeader(Constant.AccessToken, Constant.CurrentAccessToken)
                    .SetOnSuccessDelegate((Web _web, Response _response) =>
                    {
                        Debug.Log("Success sending data " + _response.GetText());

                        JSONNode _jsonNode = JSON.Parse(_response.GetText());
                        if (_jsonNode["status"].Value == "1")
                        {
                            
                        }
                        else
                        {
                            UIManager.inst.ShowNoInternetPopup();
                        }

                        _web.Close();
                    })
                    .SetOnFailureDelegate((Web _web, Response _response) =>
                    {
                        Debug.Log("Found Error " + _response.GetError());
                        UIManager.inst.ShowNoInternetPopup();
                        _web.Close();
                    })
                    .Connect();
        }

        public void GetResult()
        {
            float nowTime = Time.time;
            /*
             * {"status":1,
                "message":"success",
                "result":{
                "game_session":"1544424572787",
                "curenttime":"2018-12-10 13:04:46",
                "wining_number":{"card_id":"3","card_suite":"4"},"bumper_number":{"card_id":"1","card_suite":"2"},
                "multiplier":"1","remaining_balance":"-418.00",
                "win_amount":"0.00","previous_win_arr":[{"winning_number":{"card_id":"3","card_suite":"4"},
                "return_amount":"0.00","multiplier":"1"},{"winning_number":{"card_id":"4","card_suite":"1"},
                "return_amount":"0.00","multiplier":"1"}]}}          
             * /*/
            // Debug.Log("Getting data at: " + System.DateTime.Now);
            Web.Create()
          .SetUrl(Constant.SJGameResultURL, Web.RequestType.POST, Web.ResponseType.TEXT)
            .AddField(Constant.GameSession, Constant.CurrentGameSession)
            .AddHeader("Content-Type", "application/x-www-form-urlencoded")
            .AddHeader(Constant.AccessToken, Constant.CurrentAccessToken)
                   .SetOnSuccessDelegate((Web _web, Response _response) =>
                   {
                       Debug.LogError("Time Taken: "+ (Time.time - nowTime));
                   Debug.Log(System.DateTime.Now + "Success getting data " + _response.GetText());
                   JSONNode _jsonNode = JSON.Parse(_response.GetText());
                   if (_jsonNode["status"].Value == "1")
                   {
                       int cardID = _jsonNode["result"]["wining_number"]["card_id"].AsInt;
                       int cardSuite = _jsonNode["result"]["wining_number"]["card_suite"].AsInt;

                       int slotCard = _jsonNode["result"]["bumper_number"]["card_id"].AsInt;
                       int slotSuite = _jsonNode["result"]["bumper_number"]["card_suite"].AsInt;
                       float winAmount = _jsonNode["result"]["win_amount"].AsFloat;
                       LevelManager.inst.SetMultiplierText(_jsonNode["result"]["multiplier"].AsInt);
                       //player history is for seeing the hands played in previous rounds
                       PlayerHistory playerHistory = new PlayerHistory(Constant.CurrentGameSession, UIManager.inst.GetPlayAmount(), (float)winAmount, _jsonNode["result"]["multiplier"].AsInt, System.Convert.ToBoolean(_jsonNode["result"]["is_bumper"].AsInt));
                       gameHistory.AddToGameHistory(playerHistory);

                       LevelManager.inst.StopSlot(slotCard, slotSuite);                       
                       DOVirtual.DelayedCall(2, () => LevelManager.inst.StopWheel(cardID, cardSuite));
                           //LevelManager.inst.StopSlot(slotCard, slotSuite);
                          
                         
                       DOVirtual.DelayedCall(4, () => { CardDeck.instance.ShowWinningCard(cardID, cardSuite);
                           SuperJokerConstants.PointBalance = Constant.PointBalance = _jsonNode["result"]["remaining_balance"].AsDouble;
                           Debug.Log("Result: "+ Constant.PointBalance+ "  "+SuperJokerConstants.PointBalance);
                           UIManager.inst.SetWinAmount(winAmount);
                          // LevelManager.inst.CheckMultiplierStateOnStop();

                           //history cards are for the deck showing previous rounds cards
                           List<CardHistoryData> historyCards = new List<CardHistoryData>();
                           for (int i = 0; i < _jsonNode["result"]["previous_win_arr"].Count; i++)
                           {
                               historyCards.Add(new CardHistoryData(_jsonNode["result"]["previous_win_arr"][i]["winning_number"]["card_id"].AsInt, _jsonNode["result"]["previous_win_arr"][i]["winning_number"]["card_suite"].AsInt, _jsonNode["result"]["previous_win_arr"][i]["multiplier"].AsInt, System.Convert.ToBoolean(_jsonNode["result"]["previous_win_arr"][i]["is_bumper"].AsInt)));
                           }
                           LevelManager.inst.SetCardHistoryDeck(historyCards);
                       });
                       }

                       else if (_jsonNode["status"].Value == "7")
                       {
                           //ButtonManager.instance.ToggleClearDoubleRepeat(false);
                           //LevelManager.inst.SetInputBlocker(true);
                           //LevelManager.inst.SetText(false);
                           //UIManager.inst.ShowWaitForNextSession(true);
                       }
                       else
                       {
                           Debug.Log("Geting resilt........................");
                           UIManager.inst.ShowNoInternetPopup();
                       }

                       _web.Close();
                   })
                   .SetOnFailureDelegate((Web _web, Response _response) =>
                   {
                       Debug.Log("Found Error " + _response.GetError());
                       UIManager.inst.ShowNoInternetPopup();
                       _web.Close();
                   })
                   .Connect();
        }

        public bool CheckEnoughCash(int amountToConsume)
        {
           

            if(SuperJokerConstants.pointBalance-amountToConsume >= 0)
            {
                SuperJokerConstants.PointBalance = SuperJokerConstants.pointBalance - (double)amountToConsume;
                
               
                return true;
            }
            UIManager.inst.SetNotEnoughCash(true);
            return false;
        }

        public void AddCashBack(int amount)
        {
            SuperJokerConstants.PointBalance = SuperJokerConstants.pointBalance + amount;
        }
    }
}
