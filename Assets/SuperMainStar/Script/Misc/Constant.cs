using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Constant
{

    #region Server Constant

    public static string BaseURL = "http://35.154.209.176/admin/api/";// "http://18.219.52.107/supermainlott/admin/api/"; //"http://35.154.209.176/admin/api/";//"http://18.219.52.107/supermainlott/admin/api/";  
    public static string LoginURL = BaseURL + "users/login";
    public static string LogOutURL = BaseURL + "users/logout";
    public static string ForgotPasswordURL = BaseURL + "users/forgot-password";
    public static string ChangePasswordURL = BaseURL + "users/change-password";
    public static string GameDetailsURL = BaseURL + "game-details";
    public static string AddGameDataURL = BaseURL + "users/add-game-data";
    public static string GameResultURL = BaseURL + "users/game-result";
    //superJoker
    public static string AddSJGameDataURL = BaseURL + "users/add-joker-game-data";
    public static string SJGameResultURL  = BaseURL + "users/super-joker-result"; 
    public static string SJGameDetailsURL = BaseURL + "users/super-joker-game-details";


    public static string Username = "username";
    public static string Password = "password";
    public static string Login_Device = "login_device";
    public static string ConfirmPassword = "conf_password";
    public static string Current_Password = "current_pass";
    public static string GameSession = "game_session";
    public static string AccessToken = "access-token";
    public static string DeviceType = "device_type";
    public static string GameData = "gamedata";
    #endregion

    #region Login Data
    public static string LoginUserName = "LoginUserName";
    public static string LoginPassword = "LoginPassword";
    #endregion
    public static string Name = "";
    public static double PointBalance = 0;
    public static string CurrentGameSession = "";
    public static string CurrentAccessToken = "";
    public static string CurrentDeviceType = "";
    public static string GameStatus = "";
    public static float TimerForGame = 0;


    public static string ResultNumber = "";
    public static string WinAmount = "";

    public static string NoInternet = "ERROR : NO ACTIVE CONNECTION";
    public static string ResolveDestinationHost = "Cannot resolve destination host";
	public static string ConnectDestinationHost = "Cannot connect to destination host";



    public static string ScreenRatio = "";


    public static float GetWiningRatio(string _number)
    {
        float _winValue = 9.1f;
        if (_number.Distinct().Count() == 1 && _number.Length != 1)
            _winValue = 15f;
        //else if (_number.Distinct().Count() == 2 && _number.Length == 3)
        else if(_number.Length == 3 && HasRepeatingChars(_number))
            _winValue = 11f;
        return _winValue;
    }

    public static bool HasRepeatingChars(string str)
    {
        for (int i = 0; i < str.Length - 1; i++)
            if (str[i] == str[i + 1])
                return true;
        return false;
    }


    public static float GetWinValue(Block.BlockType _blockType, string _number, int _iAmount)
    {
        float _winValue = GetWiningRatio(_number) * _iAmount;

        if (_blockType == Block.BlockType.SINGLE)
        {
            _winValue *= 1;
        }
        else if (_blockType == Block.BlockType.DOUBLE)
        {
            _winValue *= 10;
        }
        else
        {
            _winValue *= 100;
        }

        return _winValue;

    }

    public static int GetLimitedBetAmount(Block.BlockType _blockType, int _iCurrentAmount)
    {
        int _fAmount = 0;
        switch (_blockType)
        {
            case Block.BlockType.SINGLE:
                _fAmount = 10000;
                break;
            case Block.BlockType.DOUBLE:
                _fAmount = 1000;
                break;
            case Block.BlockType.TRIPLE:
                _fAmount = 100;
                break;
        }
        _fAmount = Mathf.Clamp(_iCurrentAmount, 2, _fAmount);
        return _fAmount;
    }

    public static int GetMaximumBetAmount(Block.BlockType _blockType)
    {
        int _fAmount = 0;
        switch (_blockType)
        {
            case Block.BlockType.SINGLE:
                _fAmount = 10000;
                break;
            case Block.BlockType.DOUBLE:
                _fAmount = 1000;
                break;
            case Block.BlockType.TRIPLE:
                _fAmount = 100;
                break;
        }
        return _fAmount;
    }

    public static bool IsEligibleForDoubleUp(Block.BlockType _blockType, int _iCurrentAmount)
    {
        int _fAmount = GetMaximumBetAmount(_blockType);
        
        if ((_iCurrentAmount * 2) <= _fAmount)
            return true;
        else
            return false;
    }
}
