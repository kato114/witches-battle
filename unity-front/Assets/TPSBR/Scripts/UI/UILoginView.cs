using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;

namespace TPSBR.UI
{
    public class Message
    {
        public string type;
        public string data;
    }

    public class UILoginView : MonoBehaviour
    {
        [SerializeField] private TMP_InputField username;
        [SerializeField] private TMP_InputField password;
        [SerializeField] private GameObject LoginPanel;
        [SerializeField] private GameObject MainMenuPanel;
        [SerializeField] private GameObject RegisterPanel;
        [SerializeField] private TextMeshProUGUI txtInfo;

        WebSocket w;

        IEnumerator Start()
        {
            Debug.Log("Start");
            w = new WebSocket(new Uri("ws://localhost:8000"));
            yield return StartCoroutine(w.Connect());
            Debug.Log("CONNECTING TO WEBSOCKETS");

            while (true)
            {
                // read message
                string message = w.RecvString();
                // check if message is not empty
                if (message != null)
                {
                    Debug.Log("Recv message : " + message);
                    Message replyObject = JsonUtility.FromJson<Message>(message);
                    if(replyObject.type == "success")
                    {
                        PlayerInfo userInfo = JsonConvert.DeserializeObject<PlayerInfo>(replyObject.data);
                        Debug.Log("userInfo : " + userInfo.coin);
                        SetPlayerInfo(userInfo);

                        LoginPanel.SetActive(false);
                        MainMenuPanel.SetActive(true);
                    }
                    else if(replyObject.type == "failed")
                    {
                        Debug.Log(replyObject.data);
                        StartCoroutine(ShowInfo(replyObject.data));
                        username.text = string.Empty;
                        password.text = string.Empty;
                    }           
                }
                /*
                if (Global.PlayerService.PlayerInfo != null && Global.PlayerService.PlayerInfo.status)
                {
                    Debug.Log("ddd");
                    LoginPanel.SetActive(false);
                    MainMenuPanel.SetActive(true);
                    SaveInfo(Global.PlayerService.PlayerInfo.name, Global.PlayerService.PlayerInfo.coin);
                    //ChangeNameButton.SetActive(true);
                }*/

                if (w.error != null)
                {
                    Debug.LogError("Error: " + w.error);
                    break;
                }

                yield return 0;
            }
            Debug.Log("Close WebSocket");
            w.Close();
        }
            
        public void  OnLogin()
        {
            string name = username.text;
            string pass = password.text;

            Message message = new Message();
            message.type = "login";
            message.data = name + "\t" + pass;
            w.SendString(JsonUtility.ToJson(message));
        }

        public void SetPlayerInfo(PlayerInfo info)
        {
            Global.PlayerService.PlayerInfo.name = info.name;
            Global.PlayerService.PlayerInfo.coin = info.coin;
        }

        public void SaveInfo(string name, int coin)
        {
            Message message = new Message();
            message.type = "save";
            message.data = name + "\t" + coin;
            w.SendString(JsonUtility.ToJson(message));
        }

        public void OnRegister()
        {
            Debug.Log("OnRegister");
            LoginPanel.SetActive(false);
            RegisterPanel.SetActive(true);
        }

        IEnumerator ShowInfo(string msg)
        {
            txtInfo.text = msg;
            yield return new WaitForSeconds(3);
            txtInfo.text = "";
        }
    }
}

