using UnityEngine;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TPSBR.UI
{
   public class UICreditsView : UIView
    {
        [SerializeField] private TMP_InputField _username;
        [SerializeField] private TMP_InputField _password;
        [SerializeField] private TMP_InputField _email;
        [SerializeField] private TMP_InputField _walletaddress;
        [SerializeField] private UIButton _confirmBtn;
        [SerializeField] private UIButton _cancelBtn;
        [SerializeField] private UIButton _closeBtn;
        [SerializeField] private GameObject creditPanel;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _confirmBtn.onClick.AddListener(OnRegister);
            _cancelBtn.onClick.AddListener(OnCancelButton);
            _closeBtn.onClick.AddListener(OnCancelButton);
            //StartCoroutine(Connect());
        }

        protected override void OnDeinitialize()
        {
            _confirmBtn.onClick.RemoveListener(OnRegister);
            _cancelBtn.onClick.RemoveListener(OnCancelButton);
            _closeBtn.onClick.RemoveListener(OnCancelButton);
        }

        WebSocket w;
        IEnumerator Start()
        {
            // connect to server
            w = new WebSocket(new Uri("ws://localhost:8000"));
            yield return StartCoroutine(w.Connect());
            Debug.Log("CONNECTING TO WEBSOCKETS");

            Message request = new Message();
            request.type = "getUserProfile";            
            w.SendString(JsonUtility.ToJson(request));

            while (true)
            {
                // read message
                string message = w.RecvString();
                // check if message is not empty
                if (message != null)
                {
                    Debug.Log("Recv message : " + message);
                    Message replyObject = JsonUtility.FromJson<Message>(message);
                    if(replyObject.type == "profile")
                    {
                        PlayerInfo userInfo = JsonConvert.DeserializeObject<PlayerInfo>(replyObject.data);
                        Debug.Log("email : " + userInfo.email);
                        ShowProfileInfo(userInfo);
                    }
                    if(replyObject.type == "success")
                    {
                        Debug.Log("username" + _username.text);
                        Global.PlayerService.PlayerInfo.name = _username.text;
                    }
                }

                if (w.error != null)
                {
                    Debug.LogError("Error: " + w.error);
                    break;
                }

                yield return 0;
            }

            w.Close();
        }

        private void OnCancelButton()
        {
            Close();
        }

        protected override void OnClose()
        {
            base.OnClose();
        }

        private void ShowProfileInfo(PlayerInfo userinfo)
        {
            _username.text = userinfo.name;
            _password.text = userinfo.password;
            _email.text = userinfo.email;
            _walletaddress.text = userinfo.walletaddress;
        }

        private void OnRegister()
        {
            string name = _username.text;
            string password = _password.text;
            string email = _email.text;
            string walletaddress = _walletaddress.text;

            Message message = new Message();
            message.type = "register";
            message.data = name + "\t" + password + "\t" + email + "\t" + walletaddress;
            w.SendString(JsonUtility.ToJson(message));

            Close();
        }
    }
}
