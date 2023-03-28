using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
using static UnityEngine.Random;


namespace TPSBR.UI
{
   
    public class UIRegisterView : UIView
    {
        [SerializeField] private TMP_InputField username;
        [SerializeField] private TMP_InputField password;
        [SerializeField] private TMP_InputField passwordConfirm;
        [SerializeField] private TMP_InputField emailAddress;
        [SerializeField] private TMP_InputField inOtpCode;
        [SerializeField] private TextMeshProUGUI otpRequestTime;
        [SerializeField] private UIButton _otpRequestBtn;
        [SerializeField] private UIButton _signUpBtn;
        [SerializeField] private UIButton _signInBtn;
        [SerializeField] private GameObject registerPanel;
        [SerializeField] private GameObject loginPanel;

        //[SerializeField] private GameObject ChangeNameButton;
        private string strCheckOtp;

        protected override void OnInitialize()
        {
            _otpRequestBtn.onClick.AddListener(OnRequestOTP);
            _signUpBtn.onClick.AddListener(OnRegister);
            _signInBtn.onClick.AddListener(OnSignin);
        }

        protected override void OnDeinitialize()
        {
            _otpRequestBtn.onClick.RemoveListener(OnRequestOTP);
            _signUpBtn.onClick.RemoveListener(OnRegister);
            _signInBtn.onClick.AddListener(OnSignin);
        }

        WebSocket w;
        IEnumerator Start()
        {
            // connect to server
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
                    if (replyObject.type == "success")
                    {
                        var dialog = Open<UIYesNoDialogView>();

                        dialog.Title.text = "REGISTER";
                        dialog.Description.text = "You register successfully. Please login to enjoy game.";

                        dialog.HasClosed += (result) =>
                        {
                            if (result == true)
                            {
                                registerPanel.SetActive(false);
                                loginPanel.SetActive(true);
                            }
                        };
                    }
                    else
                    {
                        var dialog = Open<UIYesNoDialogView>();

                        dialog.Title.text = "REGISTER";
                        dialog.Description.text = "You register failed. Please enter again.";

                        dialog.HasClosed += (result) =>
                        {
                            if (result == true)
                            {
                                username.text = string.Empty;
                                password.text = string.Empty;
                                passwordConfirm.text = string.Empty;
                                emailAddress.text = string.Empty;
                                inOtpCode.text = string.Empty;
                            }
                        };
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


        private void OnRegister()
        {
            //PlayerInfo player = new PlayerInfo();
            string name = username.text;
            string pass = password.text;
            string passConf = passwordConfirm.text;
            string email = emailAddress.text;
            string otpCode = inOtpCode.text;
                        

            if (name == string.Empty || pass == string.Empty
                || passConf == string.Empty || email == string.Empty
                    || otpCode == string.Empty)
            {
                var errDialog = Open<UIErrorDialogView>();
                errDialog.Title.text = "Error";
                errDialog.Description.text = "Please insert user information correctly.";
                return;
            }

            if (pass != passConf)
            {
                var errDialog = Open<UIErrorDialogView>();
                errDialog.Title.text = "Error";
                errDialog.Description.text = "Your password confirm is not same with password. please input again.";

                password.text = string.Empty;
                passwordConfirm.text = string.Empty;
                return;
            }

            if (string.IsNullOrEmpty(strCheckOtp) && otpCode != strCheckOtp)
            {
                var errDialog = Open<UIErrorDialogView>();
                errDialog.Title.text = "Error";
                errDialog.Description.text = "please input correct otp code.";
                inOtpCode.text = string.Empty;
                otpRequestTime.text = "OTP";
                return;
            }


            // send user information
            Message message = new Message();
            message.type = "register";
            message.data = name + "\t" + pass + "\t" + email + "\t" + "";// "" is wallet address;
            w.SendString(JsonUtility.ToJson(message));
        }
        Coroutine co;
        private void OnRequestOTP()
        {
            if(co != null)
                StopCoroutine(co);
            
            string strEmail = emailAddress.text;
            string verifyCode = inOtpCode.text;

            if(string.IsNullOrEmpty(strEmail))
            {
                var errDialog = Open<UIErrorDialogView>();
                errDialog.Title.text = "Error";
                errDialog.Description.text = "Please input email address.";
                return; 
            }

            Message message = new Message();
            message.type = "verify";
            message.data = verifyCode;
            w.SendString(JsonUtility.ToJson(message));


            //if (MailSingleton.Instance.GetSendState())
            co = StartCoroutine(ModeTimer());
        }

        private void OnSignin()
        {
            registerPanel.SetActive(false);
            loginPanel.SetActive(true);
        }

        private IEnumerator ModeTimer()
        {
            int timerTimeLeft = 60;
            WaitForSeconds oneSecond = new WaitForSeconds(1f);
            yield return new WaitForSeconds(0.1f);

            while (timerTimeLeft > 0)
            {
                otpRequestTime.text = timerTimeLeft.ToString() + " s";
                yield return oneSecond;
                timerTimeLeft--;
            }

            Debug.Log("Time is up");
            otpRequestTime.text = "OTP";
            strCheckOtp = string.Empty;
        }
    }
}

