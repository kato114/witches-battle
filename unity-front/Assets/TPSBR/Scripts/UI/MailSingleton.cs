using ESN;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class MailSingleton : MonoBehaviour {

    string SMTPAddress = "smtp.gmail.com";
    int SMTPPort = 587;
    string From = "codemaster0208@gmail.com"; // Enter your gmail id here
    // Enter the app password that you are getting from https://security.google.com/settings/security/apppasswords
    // Generate one with custom name 
    string Password = "tpaasnvgwbkldxxj"; 
    bool EnableSSL = true;
    bool UseDefaultCredentials = false;

    public TextMeshProUGUI txtInfo;
    public static MailSingleton Instance;

    MailSender mailSender;
    bool b_SendSuccess = false;
    
    void OnEnable () {
        if (MailSingleton.Instance != null && MailSingleton.Instance != gameObject.GetComponent<MailSingleton>())
        {
            DestroyImmediate(gameObject);
        } else
        {
            DontDestroyOnLoad(gameObject);
            MailSingleton.Instance = gameObject.GetComponent<MailSingleton>();
        }

        mailSender = new MailSender(From, Password, SMTPAddress, SMTPPort, EnableSSL, UseDefaultCredentials);
    }

    public void SendMailWithAttachment(string from, string fromName, string to, string subject, string body, string attachment)
    {
        mailSender.SendMail(from, fromName, to, subject, body, onAsyncComplete, attachment);
    }

    public void SendPlainMail(string from, string fromName, string to, string subject, string body)
    {
        try
        {
            mailSender.SendMail(from, fromName, to, subject, body, onAsyncComplete);
        }
        catch(FormatException ex)
        {
            //Debug.Log("The specified string is not in the form required for an e-mail address");
            Debug.Log("Error : " + ex.ToString());
            StartCoroutine(ShowInfo("The specified string is not in the form required for an e-mail address"));
        }
        
    }

    void onAsyncComplete(object sender, AsyncCompletedEventArgs completedEventArgs)
    {

        if (completedEventArgs.Error != null)
        {
            StartCoroutine(ShowInfo("Mail configuration error"));
            Debug.LogError(completedEventArgs.Error.Message);
            Debug.LogWarning("If you are using gmail please setup an application password to use with this application");
            return;
        }

        if (completedEventArgs.Cancelled)
        {
            StartCoroutine(ShowInfo("Sending cancelled"));
            Debug.LogWarning("Sending cancelled");
            return;
        }

        StartCoroutine(ShowInfo("Mail sent successfully"));
        Debug.Log("Mail sent successfully");

        b_SendSuccess = true;
        SmtpClient sndr = (SmtpClient)sender;
        sndr.SendCompleted -= onAsyncComplete;
    }

    IEnumerator ShowInfo(string msg){
        txtInfo.text = msg;
        yield return new WaitForSeconds(3);
        txtInfo.text = "";
    }

    public bool GetSendState() { return b_SendSuccess; }
}
