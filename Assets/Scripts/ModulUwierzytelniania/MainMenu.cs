﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    static bool isAdmin = false;
    static string accessToken;
    static string refreshToken;
    public GameObject Login1IF;
    public GameObject Login2IF;
    public GameObject Registration1IF;
    public GameObject Registration2IF;
    public GameObject Registration3IF;
    public GameObject Registration4IF;
    public GameObject ErrorText;
    public GameObject Toggle;
    public Button ZalogujSieBtn;
    public Button ZarejestrujSieBtn;
    public Button ExitBtn;
    public Button WylogujSieBtn;
    public Button StartBtn;
    private bool zalogowany;



    // Start is called before the first frame update
    void Start()
    {
        Toggle.SetActive(isAdmin);
        zalogowany = false;
        ButtonsForLogged();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartGame()
    {
        //SceneManager.LoadScene();
    }
    public void Login()
    {
        StartCoroutine(LoginCoroutine());
    }

    IEnumerator LoginCoroutine()
    {
        TurnOffButtons();
        int requestID;
        LoginRequest loginRequest = new LoginRequest();
        loginRequest.username = Login1IF.GetComponent<InputField>().text.ToString();
        loginRequest.password = Login2IF.GetComponent<InputField>().text.ToString();

        string json = JsonUtility.ToJson(loginRequest);

        requestID = RequestMan.SendRequest(json, Request.RequestType.REQ_LOGIN);


        /*while (!RequestMan.CheckRequest(requestID))
        {
            Debug.Log(";(");
            yield return new WaitForSeconds(0.5f);
        }*/


        //Response response = RequestMan.GetRequest(requestID);

        Response response = null;
        while (response == null)
        {
            response = RequestMan.GetRequest(requestID);
            yield return new WaitForSeconds(0.5f);
        }

        OtherError(response);

        if (response.status == Response.ResponseType.RES_ERROR_SAVE_FAILED)
        {
            ErrorText.GetComponent<Text>().text = "RES_ERROR_SAVE_FAILED";
        }
        if (response.status == Response.ResponseType.RES_ERROR_USER_NOT_FOUND)
        {
            ErrorText.GetComponent<Text>().text = "Nie znaleziona takiego użytkownika!";
        }
        if (response.status == Response.ResponseType.RES_ERROR_PASSWORD_INVALID)
        {
            ErrorText.GetComponent<Text>().text = "Błędne hasło!";
        }
        if (response.status == Response.ResponseType.RES_SUCCESS)
        {
            LoginReply rep = JsonUtility.FromJson<LoginReply>(response.jsonResponse);
            accessToken = rep.accToken;
            refreshToken = rep.refToken;
            Authorize();
            zalogowany = true;
            ButtonsForLogged();
            ErrorText.GetComponent<Text>().text = "Zalogowano!";
        }
        TurnOnButtons();
    }

    public void Registration()
    {
        StartCoroutine(RegistrationCoroutine());
    }

    IEnumerator RegistrationCoroutine()
    {
        TurnOffButtons();
        bool specialAdmin = false;
        if (Registration1IF.GetComponent<InputField>().text.Equals("Admin"))
        {
            if (Registration2IF.GetComponent<InputField>().text.Equals("Admin"))
            {
                if (Registration3IF.GetComponent<InputField>().text.Equals("Admin"))
                {
                    if (Registration4IF.GetComponent<InputField>().text.Equals("Admin"))
                    {
                        specialAdmin = true;
                    }
                }
            }
        }
        if (CheckRegistrationPassword() || specialAdmin) //sprawdzenie warunkow hasla
        {
            int requestID;
            RegisterRequest registerRequest = new RegisterRequest();
            registerRequest.username = Registration1IF.GetComponent<InputField>().text.ToString();
            registerRequest.password = Registration2IF.GetComponent<InputField>().text.ToString();
            registerRequest.email = Registration4IF.GetComponent<InputField>().text.ToString();
            if (specialAdmin)
            {
                registerRequest.admin = true;
            }
            else
            {
                registerRequest.admin = Toggle.GetComponent<Toggle>().isOn;
            }


            string json = JsonUtility.ToJson(registerRequest);

            requestID = RequestMan.SendRequest(json, Request.RequestType.REQ_REGISTER);


            /*while (!RequestMan.CheckRequest(requestID))
            {
                Debug.Log(";(");
                yield return new WaitForSeconds(0.5f);
            }

            Response response = RequestMan.GetRequest(requestID);*/

            Response response = null;
            while (response == null)
            {
                response = RequestMan.GetRequest(requestID);
                yield return new WaitForSeconds(0.5f);
            }

            OtherError(response);

            if (response.status == Response.ResponseType.RES_ERROR_USER_NOT_FOUND)
            {
                ErrorText.GetComponent<Text>().text = "RES_ERROR_USER_NOT_FOUND???";
            }
            if (response.status == Response.ResponseType.RES_ERROR_PASSWORD_INVALID)
            {
                ErrorText.GetComponent<Text>().text = "RES_ERROR_PASSWORD_INVALID???";
            }
            if (response.status == Response.ResponseType.RES_ERROR_SAVE_FAILED)
            {
                ErrorText.GetComponent<Text>().text = "Użytkownik już istnieje!";
            }
            if (response.status == Response.ResponseType.RES_SUCCESS)
            {
                ErrorText.GetComponent<Text>().text = "Zarejestrowano!";
            }
        }
        TurnOnButtons();
    }

    public void Authorize()
    {
        StartCoroutine(AuthorizeCoroutine());
    }

    IEnumerator AuthorizeCoroutine()
    {
        TurnOffButtons();
        int requestID;
        VerifyRequest verifyRequest = new VerifyRequest();
        verifyRequest.accToken = accessToken;
        string json = JsonUtility.ToJson(verifyRequest);

        requestID = RequestMan.SendRequest(json, Request.RequestType.REQ_ACCESS_USER);

        /*while (!RequestMan.CheckRequest(requestID))
        {
            Debug.Log(";(");
            yield return new WaitForSeconds(0.5f);
        }

        Response response = RequestMan.GetRequest(requestID);*/

        Response response = null;
        while (response == null)
        {
            response = RequestMan.GetRequest(requestID);
            yield return new WaitForSeconds(0.5f);
        }

        VerifyReply rep = JsonUtility.FromJson<VerifyReply>(response.jsonResponse);

        OtherError(response);

        if (response.status == Response.ResponseType.RES_SUCCESS)
        {
            if (rep.privilege.Equals("true"))
            {
                isAdmin = true;

            }
            else
            {
                isAdmin = false;
            }
            Toggle.SetActive(isAdmin);
        }
        TurnOnButtons();
    }

    public void Exit()
    {
        if (zalogowany)
        {
            Logout();
        }
        Application.Quit();
    }

    public void RefreshToken()
    {
        StartCoroutine(RefreshTokenCoroutine());
    }

    IEnumerator RefreshTokenCoroutine()
    {
        TurnOffButtons();
        int requestID;
        RefreshRequest refreshRequest = new RefreshRequest();
        refreshRequest.refToken = refreshToken;
        string json = JsonUtility.ToJson(refreshRequest);

        requestID = RequestMan.SendRequest(json, Request.RequestType.REQ_REFRESH_USER);

        /*while (!RequestMan.CheckRequest(requestID))
        {
            Debug.Log(";(");
            yield return new WaitForSeconds(0.5f);
        }

        Response response = RequestMan.GetRequest(requestID);*/

        Response response = null;
        while (response == null)
        {
            response = RequestMan.GetRequest(requestID);
            yield return new WaitForSeconds(0.5f);
        }

        OtherError(response);

        if (response.status == Response.ResponseType.RES_SUCCESS)
        {
            LoginReply rep = JsonUtility.FromJson<LoginReply>(response.jsonResponse);
            accessToken = rep.accToken;
            refreshToken = rep.refToken;
        }
        TurnOnButtons();
    }

    public void Logout()
    {
        StartCoroutine(LogoutCoroutine());
    }

    IEnumerator LogoutCoroutine()
    {
        TurnOffButtons();
        int requestID;
        LogoutRequest logoutRequest = new LogoutRequest();
        logoutRequest.refToken = refreshToken;
        string json = JsonUtility.ToJson(logoutRequest);
        requestID = RequestMan.SendRequest(json, Request.RequestType.REQ_LOGOUT);

        /*while (!RequestMan.CheckRequest(requestID))
        {
            Debug.Log(";(");
            yield return new WaitForSeconds(0.5f);
        }

        Response response = RequestMan.GetRequest(requestID);*/

        Response response = null;
        while (response == null)
        {
            response = RequestMan.GetRequest(requestID);
            yield return new WaitForSeconds(0.5f);
        }

        OtherError(response);

        if (response.status == Response.ResponseType.RES_SUCCESS)
        {
            isAdmin = false;
            Toggle.SetActive(isAdmin);
            zalogowany = false;
            ButtonsForLogged();
            ErrorText.GetComponent<Text>().text = "Wylogowano!";
        }
        TurnOnButtons();
    }


    /// <summary>
    /// ////////////////////////////////////////////////////////////////////////////////////////
    /// </summary>
    bool CheckRegistrationPassword()
    {
        bool good = true;
        if (Registration2IF.GetComponent<InputField>().text.Equals(Registration3IF.GetComponent<InputField>().text))
        {
            good=CheckRegistrationPasswordStrength();
        }
        else
        {
            ErrorText.GetComponent<Text>().text = "Hasła nie są takie same";
            good = false;
        }
        return good;
    }

    bool CheckRegistrationPasswordStrength()
    {
        System.String tmp = Registration2IF.GetComponent<InputField>().text;
        bool good = true;
        bool upper = false;
        bool length = false;
        bool specialCharacter = false;
        for (int i = 0; i < tmp.Length; ++i)
        {
            if (char.IsUpper(tmp[i]))
            {
                upper = true;
            }
            if (!char.IsLetter(tmp[i]))
            {
                specialCharacter = true;
            }
        }
        if (tmp.Length >= 8)
        {
            length = true;
        }

        System.String result = "";
        if (!upper)
        {
            result += "Brak dużej litery. ";
            good = false;
        }
        if (!length)
        {
            result += "Za krótkie hasło. ";
            good = false;
        }
        if (!specialCharacter)
        {
            result += "Brak znaku specjalnego. ";
            good = false;
        }
        ErrorText.GetComponent<Text>().text = result;
        return good;
    }

    void OtherError(Response response)
    {
        if(response.status == Response.ResponseType.RES_ERROR_GENERAL)
        {
            ErrorText.GetComponent<Text>().text = "RES_ERROR_GENERAL";
        }
        if(response.status == Response.ResponseType.RES_ERROR_HTTP)
        {
            ErrorText.GetComponent<Text>().text = "RES_ERROR_HTTP";
        }
        if(response.status == Response.ResponseType.RES_ERROR_NETWORK)
        {
            ErrorText.GetComponent<Text>().text = "RES_ERROR_NETWORK";
        }
        if(response.status == Response.ResponseType.RES_ERROR_BAD_TOKEN)
        {
            ErrorText.GetComponent<Text>().text = "RES_ERROR_BAD_TOKEN";
        }
        if(response.status == Response.ResponseType.RES_ERROR_INSUFFICIENT)
        {
            ErrorText.GetComponent<Text>().text = "Nie podano wszystkich danych!";
        }
        if(response.status == Response.ResponseType.RES_ERROR_NO_TOKEN)
        {
            ErrorText.GetComponent<Text>().text = "RES_ERROR_NO_TOKEN";
        }  
    }

    void TurnOffButtons()
    {
        ZalogujSieBtn.GetComponent<Button>().interactable = false;
        ZarejestrujSieBtn.GetComponent<Button>().interactable = false;
        ExitBtn.GetComponent<Button>().interactable = false;
        ButtonsForLogged();
    }

    void TurnOnButtons()
    {
        ZalogujSieBtn.GetComponent<Button>().interactable = true;
        ZarejestrujSieBtn.GetComponent<Button>().interactable = true;
        ExitBtn.GetComponent<Button>().interactable = true;
        ButtonsForLogged();
    }

    void ButtonsForLogged()
    {
        if (zalogowany)
        {
            StartBtn.GetComponent<Button>().interactable = true;
            WylogujSieBtn.GetComponent<Button>().interactable = true;
            ZalogujSieBtn.GetComponent<Button>().interactable = false;
        }
        else
        {
            StartBtn.GetComponent<Button>().interactable = false;
            WylogujSieBtn.GetComponent<Button>().interactable = false;
            ZalogujSieBtn.GetComponent<Button>().interactable = true;
        }
    }
}