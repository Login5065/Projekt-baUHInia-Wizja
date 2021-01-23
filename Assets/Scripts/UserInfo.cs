using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo : MonoBehaviour
{
    public static UserInfo Instance;
    public string login = "";
    public bool isAdmin = false;
    public bool isOnline = false;
    public static string accessToken;
    public static string refreshToken;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else
        {
            Destroy(this.gameObject);
        }

    }


    public void SetUserInfo(string _login, bool _isAdmin, string _accToken, string _refToken, bool _isOnline)
    {
        login = _login;
        isAdmin = _isAdmin;
        accessToken = _accToken;
        refreshToken = _refToken;
        isOnline = _isOnline;
        DontDestroyOnLoad(this.gameObject);
    }

    public void ClearUserInfo()
    {
        login = "";
        isAdmin = false;
        accessToken = "";
        refreshToken = "";
    }


}
