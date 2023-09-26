using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Settings;
//using TalesFromTheRift;
using System.IO;
//using TSSystem;
//using Keyboard;

public class MailSendController : MonoBehaviour 
{
    //public SlideViewController SlideController;
   // public TSSCore core;

	public string SendedSlide = "mail.default";

	public string FilePath = "mail.message.filepath";

    //public CanvasKeyboard Keyboard;

	public Mail.Mail MailManager;


	void Start()
	{
		SendedSlide = SettingsManager.Get(SendedSlide,SendedSlide);
	}


	public void OnPress(bool isPress)
	{
		if (isPress) return;
		/*if (Mail.Mail.IsValidEmail(Keyboard.text))
		{

            string path = Application.streamingAssetsPath;

            string fullpath = path+"/"+ SettingsManager.Get(FilePath);

            // MailManager.Send(Keyboard.text, new List<string>() { FilePath });
            //MailManager.Send(Keyboard.text, null);
            core.SelectState("0");

           // Reset();
        }*/
	}

}
