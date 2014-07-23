using ProjektarbeitVokabeltrainer.ProjektarbeitVokabeltrainerServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProjektarbeitVokabeltrainer
{
    /// <summary>
    /// Interaktionslogik für Registrieren.xaml
    /// </summary>
    public partial class Registrieren : UserControl
    {
        private string uri = "http://localhost:52243/ProjektarbeitVokabeltrainerServer.svc/";

        public Registrieren()
        {
            InitializeComponent();
            this.Loaded += Registrieren_Loaded;
        }

        void Registrieren_Loaded(object sender, RoutedEventArgs e)
        {
            txtName.Focus();
        }

        //Registrierung wird durchgeführt
        private void RegistrierenClick(object sender, RoutedEventArgs e)
        {

            if (txtName.Text != "") //Benutzername eingegeben?
            {
                if (pbPasswort.Password != "") //Passwort eingegeben?
                {
                    if (IsEqualTo(pbPasswort.SecurePassword, pbPasswortBestätigen.SecurePassword)) //Passwörter stimmen überein?
                    {
                        HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "benutzer/");
                        webrequest.Method = "POST";
                        webrequest.ContentType = "application/xml";
                        HttpWebResponse webresponse = null;
                        try
                        {
                            DataContractSerializer serl = new DataContractSerializer(typeof(Benutzer));
                            Benutzer benutzer = new Benutzer();
                            benutzer.Username = txtName.Text;
                            benutzer.Passwort = Security.HashSHA1(pbPasswort.Password);
                            using (Stream requestStream = webrequest.GetRequestStream())
                                serl.WriteObject(requestStream, benutzer);
                            webresponse = (HttpWebResponse)webrequest.GetResponse();
                            HttpStatusCode rc = webresponse.StatusCode;
                            MessageBox.Show("Registrierung erfolgreich", "Registrierung");
                            mainGrid.Children.Clear();
                            mainGrid.Children.Add(new Start());
                        }
                        catch (WebException we)
                        {
                            if (we.Response != null)
                            {
                                webresponse = (HttpWebResponse)we.Response;
                                MessageBox.Show(webresponse.StatusDescription + "!", "Fehler");
                            }
                            else
                            {
                                MessageBox.Show("Server nicht erreichbar!", "Fehler");
                            }
                        }
                        finally
                        {
                            if (webresponse != null)
                                webresponse.Close();
                        }
                    }
                    else
                        MessageBox.Show("Die Passwörter stimmen nicht überein!", "Fehler");
                }
                else
                    MessageBox.Show("Bitte geben sie ein Passwort an!", "Fehler");
            }
            else
                MessageBox.Show("Bitte geben sie einen Benutzernamen an!", "Fehler");
        }


        //Überprüft die PasswortBoxen auf gleichen Inhalt
        public static bool IsEqualTo(SecureString ss1, SecureString ss2)
        {
            IntPtr bstr1 = IntPtr.Zero;
            IntPtr bstr2 = IntPtr.Zero;
            try
            {
                bstr1 = Marshal.SecureStringToBSTR(ss1);
                bstr2 = Marshal.SecureStringToBSTR(ss2);
                int length1 = Marshal.ReadInt32(bstr1, -4);
                int length2 = Marshal.ReadInt32(bstr2, -4);
                if (length1 == length2)
                {
                    for (int x = 0; x < length1; ++x)
                    {
                        byte b1 = Marshal.ReadByte(bstr1, x);
                        byte b2 = Marshal.ReadByte(bstr2, x);
                        if (b1 != b2) return false;
                    }
                }
                else return false;
                return true;
            }
            finally
            {
                if (bstr2 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr2);
                if (bstr1 != IntPtr.Zero) Marshal.ZeroFreeBSTR(bstr1);
            }
        }

        private void AbbrechenClick(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(new Start());
        }
    }
}
