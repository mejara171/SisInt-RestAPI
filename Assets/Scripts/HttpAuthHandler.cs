using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HttpAuthHandler : MonoBehaviour
{
    [SerializeField]
    private string ServerApiUrl { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Registration()
    {
        User user = new User();

        user.username = GameObject.Find("InputFieldUsername").GetComponent<InputField>().text;
        user.password = GameObject.Find("InputFieldPassword").GetComponent<InputField>().text;

        string PostData = JsonUtility.ToJson(user);
        StartCoroutine(Register(PostData));
    }

    IEnumerator Register(string postData)
    {
        UnityWebRequest www = UnityWebRequest.Put(ServerApiUrl + "/api/usuarios", postData);

        www.method = "POST";
        www.SetRequestHeader("Content Type", "application/json");

        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR:" + www.error);
        }
        else
        {
            if (www.responseCode == 200)
            {
                AuthJsonData jsonData = JsonUtility.FromJson<AuthJsonData>(www.downloadHandler.text);

                Debug.Log(jsonData._user.username + " se registro con id " + jsonData._user._id);
            }
            else
            {
                string message = "Status:" + www.responseCode;
                message += "\ncontent-type:" + www.GetResponseHeader("content-type");
                message += "\nError:" + www.error;
                Debug.Log(message);
            }

            byte[] results = www.downloadHandler.data;
        }
    }
}

[System.Serializable]
public class User
{
    public string _id;
    public string username;
    public string password;
    public int score;
}

public class AuthJsonData
{
    public User _user;
    public string token;
}