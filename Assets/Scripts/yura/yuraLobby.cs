using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class yuraLobby : MonoBehaviour
{
    public TextMeshProUGUI userIdText;
    string userId;
    // Start is called before the first frame update
    void Start()
    {
        if (GoogleManager.User.Email != null)
        {
            userIdText.text = GoogleManager.User.Email;
        }
        else
            userIdText.text = GoogleManager.User.DisplayName;
           
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
