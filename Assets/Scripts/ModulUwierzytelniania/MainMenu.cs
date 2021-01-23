using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static bool isAdmin;
    public static string accessToken;
    public static string refreshToken;
    public static bool czyTrybOnline;   //true - komunikujemy sie z serwerem, false - offline
                                        //public static string login;
                                        //Dictionary<login,  Tuple<email,  haslo,  isAdmin>>
    private static Dictionary<string, Tuple<string, string, bool>> users = new Dictionary<string, Tuple<string, string, bool>>();
    public GameObject Login1IF;
    public GameObject Login2IF;
    public GameObject Registration1IF;
    public GameObject Registration2IF;
    public GameObject Registration3IF;
    public GameObject Registration4IF;
    public GameObject ErrorText;
    public GameObject Toggle;
    public GameObject ToggleTryb;
    public Button ZalogujSieBtn;
    public Button ZarejestrujSieBtn;
    public Button ExitBtn;
    public Button WylogujSieBtn;
    public Button StartBtn;
    private bool zalogowany;




    // Start is called before the first frame update
    void Start()
    {
        ToggleTryb.GetComponent<Toggle>().isOn = czyTrybOnline;
        Toggle.GetComponent<Toggle>().isOn = false;
        Toggle.SetActive(false);
        zalogowany = false;
        isAdmin = false;
        ButtonsForLogged();
    }

    // Update is called once per frame
    void Update()
    {
        if (czyTrybOnline != ToggleTryb.GetComponent<Toggle>().isOn)
        {
            if (zalogowany)
            {
                Logout();
            }
        }
        czyTrybOnline = ToggleTryb.GetComponent<Toggle>().isOn;
    }

    public void StartGame() //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    { //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        //SceneManager.LoadScene(); //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        if (zalogowany)
        {
            //UserInfo.Instance.SetUserInfo(Login1IF.GetComponent<InputField>().text, isAdmin, accessToken, refreshToken);
            SceneManager.LoadScene(1);
        }
    } //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    public void Login()
    {
        if (czyTrybOnline)
        {
            StartCoroutine(LoginCoroutine());
        }
        else
        {
            TurnOffButtons();
            if (users.TryGetValue(Login1IF.GetComponent<InputField>().text, out Tuple<string, string, bool> tuple))
            {
                if (Login2IF.GetComponent<InputField>().text.Equals(tuple.Item2))
                {
                    zalogowany = true;
                    isAdmin = tuple.Item3;
                    Toggle.SetActive(isAdmin);
                    //login = Login1IF.GetComponent<InputField>().text;
                    ErrorText.GetComponent<Text>().text = "Zalogowano!";
                }
                else
                {
                    ErrorText.GetComponent<Text>().text = "Błędne hasło!";
                }
            }
            else
            {
                ErrorText.GetComponent<Text>().text = "Nie znaleziona takiego użytkownika!";
            }
            TurnOnButtons();
        }

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
            ErrorText.GetComponent<Text>().text = "Zalogowano!";
        }
        else
        {
            TurnOnButtons();
        }
    }

    public void Registration()
    {

        if (czyTrybOnline)
        {
            StartCoroutine(RegistrationCoroutine());
        }
        else
        {
            TurnOffButtons();
            string login;
            string haslo;
            string email;
            bool czyAdmin;

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
            if ((CheckRegistrationPassword() || specialAdmin) && !users.ContainsKey(Registration1IF.GetComponent<InputField>().text)) //sprawdzenie warunkow hasla
            {
                if (specialAdmin)
                {
                    czyAdmin = true;
                }
                else
                {
                    czyAdmin = Toggle.GetComponent<Toggle>().isOn;
                }
                login = Registration1IF.GetComponent<InputField>().text;
                haslo = Registration2IF.GetComponent<InputField>().text;
                email = Registration4IF.GetComponent<InputField>().text;
                users.Add(login, new Tuple<string, string, bool>(email, haslo, czyAdmin));
                ErrorText.GetComponent<Text>().text = "Zarejestrowano!";
            }
            else if (users.ContainsKey(Registration1IF.GetComponent<InputField>().text))
            {
                ErrorText.GetComponent<Text>().text = "Użytkownik już istnieje!";
            }
            TurnOnButtons();
        }
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




            Response response = null;
            while (response == null)
            {
                response = RequestMan.GetRequest(requestID);
                yield return new WaitForSeconds(0.5f);
            }

            OtherError(response);

            if (response.status == Response.ResponseType.RES_ERROR_USER_NOT_FOUND)
            {
                ErrorText.GetComponent<Text>().text = "RES_ERROR_USER_NOT_FOUND";
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

        if (czyTrybOnline)
        {
            StartCoroutine(LogoutCoroutine());
        }
        else
        {
            TurnOffButtons();
            Toggle.GetComponent<Toggle>().isOn = false;
            Toggle.SetActive(false);
            isAdmin = false;
            zalogowany = false;
            ErrorText.GetComponent<Text>().text = "Wylogowano!";
            TurnOnButtons();
        }
    }

    IEnumerator LogoutCoroutine()
    {
        TurnOffButtons();
        Toggle.GetComponent<Toggle>().isOn = false;
        int requestID;
        LogoutRequest logoutRequest = new LogoutRequest();
        logoutRequest.refToken = refreshToken;
        string json = JsonUtility.ToJson(logoutRequest);
        requestID = RequestMan.SendRequest(json, Request.RequestType.REQ_LOGOUT);


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
            good = CheckRegistrationPasswordStrength();
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
        if (response.status == Response.ResponseType.RES_ERROR_GENERAL)
        {
            ErrorText.GetComponent<Text>().text = "RES_ERROR_GENERAL";
        }
        if (response.status == Response.ResponseType.RES_ERROR_HTTP)
        {
            ErrorText.GetComponent<Text>().text = "RES_ERROR_HTTP";
        }
        if (response.status == Response.ResponseType.RES_ERROR_NETWORK)
        {
            ErrorText.GetComponent<Text>().text = "RES_ERROR_NETWORK";
        }
        if (response.status == Response.ResponseType.RES_ERROR_BAD_TOKEN)
        {
            ErrorText.GetComponent<Text>().text = "RES_ERROR_BAD_TOKEN";
        }
        if (response.status == Response.ResponseType.RES_ERROR_INSUFFICIENT)
        {
            ErrorText.GetComponent<Text>().text = "Nie podano wszystkich danych!";
        }
        if (response.status == Response.ResponseType.RES_ERROR_NO_TOKEN)
        {
            ErrorText.GetComponent<Text>().text = "RES_ERROR_NO_TOKEN";
        }
    }

    void TurnOffButtons()
    {
        ZalogujSieBtn.GetComponent<Button>().interactable = false;
        ZarejestrujSieBtn.GetComponent<Button>().interactable = false;
        ExitBtn.GetComponent<Button>().interactable = false;
        StartBtn.GetComponent<Button>().interactable = false;
        WylogujSieBtn.GetComponent<Button>().interactable = false;
        ToggleTryb.GetComponent<Toggle>().interactable = false;
    }

    void TurnOnButtons()
    {
        ZarejestrujSieBtn.GetComponent<Button>().interactable = true;
        ExitBtn.GetComponent<Button>().interactable = true;
        ToggleTryb.GetComponent<Toggle>().interactable = true;
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
