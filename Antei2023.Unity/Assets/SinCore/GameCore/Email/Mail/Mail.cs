using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.IO;
using System.Net.NetworkInformation;
using System.Text.RegularExpressions;
using Settings;
using UnityEngine;
using Ping = System.Net.NetworkInformation.Ping;

namespace Mail
{
    public class Mail : MonoBehaviour
    {
        #region Public Vars

        public string Host = "mail.host";
        public string Port = "mail.port";
        public string Login = "mail.user.login";
        public string Password = "mail.user.password";
        public string Ssl = "mail.ssl";
        public string FromMail = "mail.message.frommail";
        public string FromTitle = "mail.message.title";
        public string MessageSubject = "mail.message.subject";
        public string MessageBody = "mail.message.body";
        public string MessageAttachmentName = "mail.message.attachment";

        #endregion

        #region Protected Vars

        #endregion

        #region Private Vars

        private string _mailHost = "smtp.yandex.ru";
        private bool _mailSsl = true;
        private int _mailPort = 465;
        private string _mailLogin = "littlejoesbros@yandex.ru";
        private string _mailPassword = "rohubpdrqoxynlia";

        private string _fromMail = "littlejoesbros@yandex.ru";
        private string _fromTitle = "Антей груп";

        private string _messageBody = "Спасибо, что посетили нас!";
        private string _messageSubject = "какой-то текст";
        private string _messageAttachmentName = null;

        private bool _isProcess;

        private Thread _thread;

        private readonly List<MailTask> _tasks = new List<MailTask>();

        #endregion

        #region Unity Methods

        void Awake()
        {
            
            _mailHost = SettingsManager.Get(Host, Host);
            _mailSsl = SettingsManager.GetBool(Ssl, Ssl);
            _mailLogin = SettingsManager.Get(Login, Login);
            _mailPort = SettingsManager.GetInt(Port, Port);
            _mailPassword = SettingsManager.Get(Password, Password);

            _fromMail = SettingsManager.Get(FromMail, FromMail);
            _fromTitle = SettingsManager.Get(FromTitle, FromTitle);

            _messageBody = SettingsManager.Get(MessageBody, MessageBody);
            _messageSubject = SettingsManager.Get(MessageSubject, MessageSubject);
            _messageAttachmentName = SettingsManager.Get(MessageAttachmentName, MessageAttachmentName);
            
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }
        public void SetMessageBody(string body)
        {
            _messageBody = body;
        }
        void Start()
        {
            _isProcess = true;

            _thread = new Thread(ProcessMail);
            _thread.Start();

            //Send("littlejoesbros@gmail.com", null);
        }

        void OnDestroy()
        {
            _isProcess = false;
        }

        #endregion

        #region Public Methods

        public void Send(string mailAddress, List<string> attachments)
        {
                var task = new MailTask {Mail = mailAddress};

                if (attachments != null)
                {
                    task.Files = attachments.ToArray();
                }

            //string path = Application.dataPath;
            //path = Path.GetFullPath(Path.Combine(path, @"..\"));

            //string Category = Settings.SettingsManager.Get("info.name");

            /*if (!Directory.Exists(path +"Mails/"+ Category + "/"))
                Directory.CreateDirectory(path + "Mails/" + Category + "/");
            System.IO.File.WriteAllText(path + "Mails/" + Category + "/" +mailAddress+".txt", _messageBody);*/
                _tasks.Add(task);
        }

        #endregion

        #region Protected Methods

        #endregion

        #region Private Methods

        private void ProcessMail()
        {

            while (_isProcess)
            {

                lock (_tasks)
                {
                    MailTask task = null;

                    try
                    {
                        task = _tasks.FirstOrDefault();
                    }
                    catch (Exception e)
                    {
                        Debug.Log("> " + e);
                    }
                    

                    if (task != null)
                    {
                        if (CheckForInternetConnection2())
                        {
                            Debug.Log("Internet Not Found :c");

                            Thread.Sleep(10*1000);

                            continue;
                        }

                        Debug.Log("Start sending message to: " + task.Mail);

                        using (var message = new MailMessage())
                        {
                            try
                            {
                                var credential = new NetworkCredential(_mailLogin, _mailPassword);
                                var fromMailAddress = new MailAddress(_fromMail, _fromTitle, Encoding.UTF8);
                                var toMailAddress = new MailAddress(task.Mail);

                                message.IsBodyHtml = true;

                                message.From = fromMailAddress;
                                message.To.Add(toMailAddress);

                                message.Body = _messageBody;
                                message.BodyEncoding = Encoding.UTF8;

                                message.Subject = _messageSubject;
                                message.SubjectEncoding = Encoding.UTF8;

                                if (task.Files != null)
                                {
                                    var counter = 0;

                                    foreach (var file in task.Files)
                                    {
                                        if (!File.Exists(file)) continue;

                                        var attachmentName = string.IsNullOrEmpty(_messageAttachmentName)
                                            ? Path.GetFileNameWithoutExtension(file)
                                            : _messageAttachmentName;

                                        if (task.Files.Length > 1)
                                        {
                                            attachmentName += String.Format("-{0}", counter);
                                        }

                                        attachmentName += Path.GetExtension(file);

                                        var attachment = new Attachment(file);
                                        attachment.Name = attachmentName;
                                        attachment.NameEncoding = Encoding.UTF8;

                                        message.Attachments.Add(attachment);

                                        Debug.Log("\tAdd File: " + file);
                                    }
                                }

                                _tasks.Remove(task);
                      

                                var client = new SmtpClient(_mailHost);
                                client.EnableSsl = _mailSsl;
                                client.Port = _mailPort;
                                client.Credentials = credential as ICredentialsByHost;
                                client.Timeout = 360*1000;

                                Debug.Log("Senting...");

                                message.Sender = new MailAddress(_fromMail, _fromTitle, Encoding.UTF8);
                                client.Send(message);

                                if (task.Files != null)
                                {
                                    Debug.Log("Message Sented To: " + task.Mail + ", With files: " +
                                              task.Files.Aggregate("", (current, file) => current + (file + ", ")));
                                }
                                else
                                {
                                    Debug.Log("Message Sented To: " + task.Mail);
                                }
                            }
                            catch (Exception exception)
                            {
                                Debug.LogError(exception.StackTrace);
                            }
                            finally
                            {
                                foreach (var attachment in message.Attachments)
                                {
                                    attachment.Dispose();
                                }

                                message.Attachments.Dispose();
                            }
                        }
                    }
                }

                Thread.Sleep(100);
            }
        }


        public static bool CheckForInternetConnection()
        {
            const int timeout = 1000;
            const string host = "8.8.8.8";


            var ping = new Ping();
            var buffer = new byte[32];
            var pingOptions = new PingOptions();

            try
            {
                var reply = ping.Send(host, timeout, buffer, pingOptions);
                return (reply != null && reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }

        }

        private bool CheckForInternetConnection2()
        {
            System.Net.WebClient client = null;
            System.IO.Stream stream = Stream.Null;

            try
            {
                client = new System.Net.WebClient();
                stream = client.OpenRead("https://api.visocial.ru/");

                var avaible = client.ResponseHeaders != null && client.ResponseHeaders.Count > 0;

                return avaible;
            }
            catch
            {
                return false;
            }
            finally
            {
                if (client != null) { client.Dispose(); }
                if (stream != null) { stream.Dispose(); }
            }
        }


        static bool invalid = false;

        public static bool IsValidEmail(string strIn)
        {
            invalid = false;
            if (String.IsNullOrEmpty(strIn))
                return false;

            // Use IdnMapping class to convert Unicode domain names.
            try
            {
                strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None);
            }
            catch (Exception)
            {
                return false;
            }

            if (invalid)
                return false;

            // Return true if strIn is in valid e-mail format.
            try
            {
                return Regex.IsMatch(strIn, @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
     + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
     + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
				[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
     + @"([a-zA-Z0-9]+[\w-]+\.)+[a-zA-Z]{1}[a-zA-Z0-9-]{1,23})$",
                      RegexOptions.IgnoreCase);
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static string DomainMapper(Match match)
        {
            var idn = new IdnMapping();

            var domainName = match.Groups[2].Value;
            try
            {
                domainName = idn.GetAscii(domainName);
            }
            catch (ArgumentException)
            {
                invalid = true;
            }
            return match.Groups[1].Value + domainName;
        }


        #endregion
    }
}