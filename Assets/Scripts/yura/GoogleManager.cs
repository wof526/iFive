using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Firebase.Extensions;
using Firebase;
using static UnityEngine.UIElements.UxmlAttributeDescription;
using UnityEngine.SceneManagement;


public class GoogleManager : MonoBehaviour
{
    public TMP_Text googleLog;
    public TMP_Text firebaseLog;
    public GameObject loadingBar;
    public GameObject startButton;

    FirebaseAuth fbauth;

    public bool IsFirebaseReady { get; private set; } //현재 유니티 환경이 파이어베이스와 연결 가능한 상황인가 bool로 리턴.
    public bool IsSignInOnProgress { get; private set; } //중복 로그인 방지 boolean


    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public Button signButton;

    public static FirebaseApp firebaseApp;
    public static FirebaseUser User;

    // Start is called before the first frame update
    void Start()
    {
        // Play BGM
        AudioManager.instance.PlayBgm(true);


        emailField.gameObject.SetActive(false);
        passwordField.gameObject.SetActive(false);
        signButton.gameObject.SetActive(false);

        loadingBar.SetActive(false);
        startButton.SetActive(false);

        /*PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder()
            .RequestIdToken()
            .RequestEmail()
            .Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        */
        fbauth = FirebaseAuth.DefaultInstance;

        //TryGoogleLogin();

        loadingBar.SetActive(true); //새로 추가. 구글로그인시 시작되면 없애기
        EmailLogin();

        
    }

    public void TryGoogleLogin()
    {
        

       loadingBar.SetActive(true);

        /*PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptAlways, (success) =>
        {
            if (success == SignInStatus.Success)
            {
                googleLog.text = "Google Success";
                StartCoroutine(TryFirebaseLogin());
                
            }
            else
            {
                googleLog.text = "google Failure";
                EmailLogin();
               
                
            }
        });*/
    }

    /*IEnumerator TryFirebaseLogin()
    {
        while (string.IsNullOrEmpty(((PlayGamesLocalUser)Social.localUser).GetIdToken()))
        {
            yield return null;
        }

        string idToken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();

        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);

        fbauth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            if (task.IsCanceled)
                firebaseLog.text = "Firebase Cancles";
            else if (task.IsFaulted)
                firebaseLog.text = "firebase Faulted";
            else
            {
                firebaseLog.text = "Firebase success";
                User = task.Result; //check later..
                string userstring = User.ToString();
                firebaseLog.text = (userstring);
            }
        });
        

        loadingBar.SetActive(false);
        startButton.SetActive(true);
    }*/

    public void GameStartButton()
    {
        SceneManager.LoadScene("Lobby");
    }


    public void TryGoggleLogout()
    {
        /*if (Social.localUser.authenticated)
        {
            PlayGamesPlatform.Instance.SignOut();
            fbauth.SignOut();
        }*/
    }

    void EmailLogin() // 이메일 방법으로 로그인 기반 작업
    {
        //signButton.interactable = false;
        loadingBar.SetActive(false);
        emailField.gameObject.SetActive(true);
        passwordField.gameObject.SetActive(true);
        signButton.gameObject.SetActive(true);

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var result = task.Result;

            if (result != DependencyStatus.Available)
            {
                Debug.LogError(message: result.ToString());
                IsFirebaseReady = false;
            }
            else
            {
                IsFirebaseReady = true;
                firebaseApp = FirebaseApp.DefaultInstance;
                fbauth = FirebaseAuth.DefaultInstance;
            }

            signButton.interactable = IsFirebaseReady;
        }
        );
    }

    public void EmailSignin() // sign in 버튼 눌렀을 떄 이메일 방법으로 로그인 시도
    {
        // Play SFX
        AudioManager.instance.PlaySfx(AudioManager.Sfx.StartBtn);


        if (!IsFirebaseReady || IsSignInOnProgress || User != null)
        {
            return;
        }

        IsSignInOnProgress = true;
        signButton.interactable = false;

        fbauth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(task =>
        {
            Debug.Log(message: $"sign in status:{task.Status}");

            IsSignInOnProgress = false;
            signButton.interactable = true;

            if (task.IsFaulted)
            {
                Debug.LogError(task.Exception);
            }
            else if (task.IsCanceled)
            {
                Debug.LogError(message: "Sign-in canceled");
            }
            else
            {
                User = task.Result.User;
                Debug.Log("로그인 성공");
                Debug.Log(User.Email);
                SceneManager.LoadScene("Lobby");
                

            }
        });
    } 
}
