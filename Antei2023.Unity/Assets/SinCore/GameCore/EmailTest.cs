using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using System.Net.Mail;
using TMPro;
using UnityEngine.UI;
public class EmailTest : MonoBehaviour
{
    [SerializeField]
    private PhotoMaker photoMaker;

    public void Send(string email)
    {
        SendEmailAsync(email).GetAwaiter();
        Console.Read();
    }
    private async Task SendEmailAsync(string email)
    {
        MailAddress from = new MailAddress("littlejoesbros@yandex.ru", "Дмитрий Каракозов");
        MailAddress to = new MailAddress(email);
        MailMessage m = new MailMessage(from, to);
        m.Subject = "Тест";
        m.Body = "Письмо-тест 2 работы smtp-клиента";
        m.Attachments.Clear();
        m.Attachments.Add(new Attachment(photoMaker.PhotoPath[photoMaker.PhotoPath.Count - 1]));
        m.Attachments.Add(new Attachment(photoMaker.PhotoPath[photoMaker.PhotoPath.Count - 2]));
        m.Attachments.Add(new Attachment(photoMaker.PhotoPath[photoMaker.PhotoPath.Count - 3]));
        
        SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 25);
        smtp.Credentials = new NetworkCredential("littlejoesbros@yandex.ru", "yzeqzetpdemjxkio");
        smtp.EnableSsl = true;
        await smtp.SendMailAsync(m);
        Console.WriteLine("Письмо отправлено");
    }
}
