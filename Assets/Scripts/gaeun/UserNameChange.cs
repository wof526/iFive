using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UserNameChange : MonoBehaviour
{
    public TextMeshProUGUI NameText;


    // Start is called before the first frame update
    void Start()
    {
        NameText.text = GoogleManager.User.Email;
    }

}
