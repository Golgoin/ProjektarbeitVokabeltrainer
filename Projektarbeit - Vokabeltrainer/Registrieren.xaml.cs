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
        string uri = "http://localhost:52243/ProjektarbeitVokabeltrainerServer.svc/";

        public Registrieren()
        {
            InitializeComponent();
        }

        private void RegistrierenClick(object sender, RoutedEventArgs e)
        {

            if (txtName.Text != "")
            {
                if (pbPasswort.Password != "")
                {
                    if (IsEqualTo(pbPasswort.SecurePassword, pbPasswortBestätigen.SecurePassword))
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
                            webresponse = (HttpWebResponse)we.Response;
                            MessageBox.Show(webresponse.StatusDescription + "!", "Fehler");
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
