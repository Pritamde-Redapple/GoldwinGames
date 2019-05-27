using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SuperJoker
{
    public class SuperJokerConstants 
    {
        public const float selectedChipSize = 1.2f;
        public const float normalChipSize   = 1.0f;
        public const string CARDNAMEPREFIX  = "C_";
        public const string SUITENAMEPREFIX = "S_";

        public enum ChipSelected { FIVE, TEN, TWENTY, FIFTY, HUNDREAD, FIVEHUNDREAD };
        public enum CardState { SELECTED, DESELECTED};
        public static double pointBalance;
        public const double MaxBetAmount = 10000;

        public static double PointBalance
        {
            get
            {
                return pointBalance;
            }

            set
            {
                pointBalance = value;              

                if (UIManager.inst)
                     UIManager.inst.SetPointBalance();
            }
        }

        public static bool IsSuperBumber(int outerwheel, int innerwheel, int slotA, int slotB)
        {
            if(outerwheel == slotA && innerwheel == slotB)
            {
                return true;
            }

            else

            {
                return false;
            }
        }

        public static bool IsSuperBumber(int sb)
        {
            if (sb == 0)
                return false;
            else
                return true;
        }
    }

    public class CardHistoryData
    {
        public int  cardValue;
        public int suiteValue;
        public int mulitiplier;
        public bool isSuperBumper;

        public CardHistoryData(int cardValue, int suiteValue, int mulitiplier, bool isSuperBumper)
        {
            this.cardValue = cardValue;
            this.suiteValue = suiteValue;
            this.mulitiplier = mulitiplier;
            this.isSuperBumper = isSuperBumper;
        }
    }
}
