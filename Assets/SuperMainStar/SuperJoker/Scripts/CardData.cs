using System.Collections;
using System.Collections.Generic;
using System;

[Serializable]
public class UserData
{
     
    public CardData[]  cardData;

    public UserData(CardData[] cardDatas)
    {
        this.cardData = cardDatas;       
    }

    public CardData[] GetCardData()
    {
        return cardData;
    }
}

[Serializable]
public class FinalCardData
{
    public CardData[] cardData;

    public FinalCardData(CardData[] cardData)
    {
        this.cardData = cardData;
    }

    public CardData[] GetCardData()
    {
        return cardData;
    }
}

//[Serializable]
//public struct SuiteData
//{
//    public int cardID;
//    public int suiteID;
//    public int suiteBetValue;

//    public SuiteData(int cardID, int suiteID, int suiteBetValue)
//    {
//        this.cardID = cardID;
//        this.suiteID = suiteID;
//        this.suiteBetValue = suiteBetValue;
//    }
//}

[Serializable]
public struct CardData
{
    public int cardID;
    public int cardSuite;
    public int cardBetValue;

    public CardData(int cardID, int cardSuite, int cardBetValue)
    {
        this.cardID = cardID;
        this.cardSuite = cardSuite;
        this.cardBetValue = cardBetValue;
    }
}

[Serializable]
public struct PostData
{
    public string gamesession;
    public string device_type;
    public CardData[] cardData;

    public PostData(string gamesession, string device_type, CardData[] cd)
    {
        this.gamesession = gamesession;
        this.device_type = device_type;
        cardData = cd;
    }
}

[Serializable]
public class PlayerHistory
{
    public string sessionID;
    public double playAmount;
    public float winAmount;
    public int multiplier;
    public bool isSuperBumper;
    public PlayerHistory(string sessionID, double playAmount, float winAmount, int multi = 0)
    {
        this.sessionID = sessionID;
        this.playAmount = playAmount;
        this.winAmount = winAmount;
        this.multiplier = multi;
    }

    public PlayerHistory(string sessionID, double playAmount, float winAmount, int multiplier, bool isSuperBumper) : this(sessionID, playAmount, winAmount, multiplier)
    {
        this.isSuperBumper = isSuperBumper;
    }
}
