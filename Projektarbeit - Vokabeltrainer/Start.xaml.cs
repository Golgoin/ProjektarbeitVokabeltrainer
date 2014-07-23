using ProjektarbeitVokabeltrainer.ProjektarbeitVokabeltrainerServer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
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
    /// Interaktionslogik für Start.xaml
    /// </summary>
    public partial class Start : UserControl
    {
        string uri = "http://localhost:52243/ProjektarbeitVokabeltrainerServer.svc/";

        public Start()
        {
            InitializeComponent();
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "benutzer/");
            webrequest.Method = "GET";
            HttpWebResponse webresponse = null;
            try
            {
                webresponse = (HttpWebResponse)webrequest.GetResponse();
                HttpStatusCode rc = webresponse.StatusCode;
                DataContractSerializer serl = new DataContractSerializer(typeof(List<Benutzer>));
                BenutzerNamen.ItemsSource = (List<Benutzer>)serl.ReadObject(webresponse.GetResponseStream());
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

        private void EinloggenClick(object sender, RoutedEventArgs e)
        {
            if (BenutzerNamen.SelectedItem is Benutzer)
            {
                Benutzer benutzer = BenutzerNamen.SelectedItem as Benutzer;
                benutzer.Passwort = Security.HashSHA1(pbPasswort.Password);
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "benutzer/" + benutzer.ID + "/");
                webrequest.Method = "POST";
                webrequest.ContentType = "application/xml";
                HttpWebResponse webresponse = null;
                try
                {
                    DataContractSerializer serl = new DataContractSerializer(typeof(Benutzer));
                    using (Stream requestStream = webrequest.GetRequestStream())
                        serl.WriteObject(requestStream, benutzer);
                    webresponse = (HttpWebResponse)webrequest.GetResponse();
                    HttpStatusCode rc = webresponse.StatusCode;
                    benutzer = (Benutzer)serl.ReadObject(webresponse.GetResponseStream());
                    mainGrid.Children.Clear();
                    mainGrid.Children.Add(new Eingelogged(benutzer));
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
        }

        private void BeendenClick(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }

        private void RegistrierenClick(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(new Registrieren());
        }
    }
}
