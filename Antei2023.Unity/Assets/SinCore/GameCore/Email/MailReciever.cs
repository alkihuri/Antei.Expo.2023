using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using extOSC;

public class MailReciever : MonoBehaviour
{
    /*
    private OSCReceiver _receiver;

    private const string _oscAddress = "/mail";

    public Mail.Mail MailManager;

    public AppartmentSet[] Apparments;

    string OriginMail;

    private static string rusString = @"абвгдеёжзийклмнопрстуфхцчшщъыьэюя";
    private static string engString = @"abcdefghijklmnopqrstuvwxyz!;%_?*(";


    private static string ConvertToRu(string sourceString)
    {
        string result = sourceString;

        for (int i = 0; i < rusString.Length; i++)
        {
            result = result.Replace(engString[i], rusString[i]);
        }

        return result.ToUpper();
    }
    // Start is called before the first frame update
    void Start()
    {
        _receiver = gameObject.AddComponent<OSCReceiver>();

        // Set local port.
        _receiver.LocalPort = 10000;

        // Bind "MessageReceived" method to special address.
        _receiver.Bind(_oscAddress, MessageReceived);

        OriginMail = File.ReadAllText(Application.streamingAssetsPath + "/" + "Email.html");
    }

    protected void MessageReceived(OSCMessage message)
    {
        // Debug.Log(message);
        OSCValue[] values = message.GetValues(OSCValueType.String, OSCValueType.Int);
        if (values.Length < 1) return;
        string MailName = values[0].StringValue.Trim();
        string MailAdress = values[1].StringValue.Trim();
        int AppartNum = values[2].IntValue - 1;
        int RoomsNum = values[3].IntValue - 1;
        int RennovationNum = values[4].IntValue - 1;
        int PlanNum = values[5].IntValue - 1;

        MailName = ConvertToRu(MailName);

        Debug.Log("Пришел запрос на имя " + MailName + ", с адресом " + MailAdress + ", с параметрами: " + Apparments[AppartNum].name + ", количество комнат: " + (RoomsNum + 1) + ", " + (RennovationNum == 0 ? "с ремонтом, " : "без ремонта, ") + "с планировкой №" + (PlanNum + 1));

        string MailMessage = OriginMail;
        MailMessage = MailMessage.Replace("$0$", Apparments[AppartNum].HeaderUrl);
        if (RoomsNum >= Apparments[AppartNum].RoomsUrl.Length)
            RoomsNum=0;
        MailMessage = MailMessage.Replace("$1$", Apparments[AppartNum].RoomsUrl[RoomsNum]);
        MailMessage = MailMessage.Replace("$2$", Apparments[AppartNum].ThirdCardUrl);
        MailMessage = MailMessage.Replace("$3$", Apparments[AppartNum].FourthCardUrl);
        MailMessage = MailMessage.Replace("$4$", Apparments[AppartNum].FifthCardUrl);
        MailMessage = MailMessage.Replace("$5$", Apparments[AppartNum].RenovationUrl[RennovationNum]);
        MailMessage = MailMessage.Replace("$6$", Apparments[AppartNum].PlansUrl[PlanNum]);
        MailMessage = MailMessage.Replace("$7$", Apparments[AppartNum].BottomUrl);

        MailManager.SetMessageBody(MailMessage);
        MailManager.Send(MailAdress, null);

        

        string path = Application.dataPath;
        path = Path.GetFullPath(Path.Combine(path, @"..\"));

        if (!Directory.Exists(path + "Mails/" + Apparments[AppartNum].name + "/"))
            Directory.CreateDirectory(path + "Mails/" + Apparments[AppartNum].name + "/");
        System.IO.File.WriteAllText(path + "Mails/" + Apparments[AppartNum].name + "/" + MailAdress + "_"+ MailName + ".html", MailMessage);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
