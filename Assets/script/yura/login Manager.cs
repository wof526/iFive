using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using UnityEngine.UI;
using TMPro;

public class loginManager : MonoBehaviour
{
    public bool IsFirebaseReady { get; private set; } //현재 유니티 환경이 파이어베이스와 연결 가능한 상황인가 bool로 리턴.
    public bool IsSignInOnProgress { get; private set; } //중복 로그인 방지 boolean

    
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    public Button signButton;

    public static FirebaseApp firebaseApp; //파이어베이스 전체 app 관리 오브젝트
    public static FirebaseAuth firebaseAuth; //파이어베이스 auth 관리 오브젝트

    public static FirebaseUser User; // 입력된 이메일, pw 와 대응되는 유저 정보 저장되는 곳.
    void Start()
    {
        signButton.interactable = false;

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
                firebaseAuth = FirebaseAuth.DefaultInstance;
            }

            signButton.interactable = IsFirebaseReady;
        }
       );  //실행 하자마자 바로 넘어감 -> callback 검

    }

    // Update is called once per frame
    public void Signin()
    {
        if(!IsFirebaseReady || IsSignInOnProgress ||User != null)
        {
            return;
        }

        IsSignInOnProgress = true;
        signButton.interactable = false;

        firebaseAuth.SignInWithEmailAndPasswordAsync(emailField.text, passwordField.text).ContinueWithOnMainThread(task =>
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

            }
        });
    }
}
