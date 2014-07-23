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
    /// Interaktionslogik für VokabelBearbeitenDetails.xaml
    /// </summary>
    public partial class VokabelBearbeitenDetails : UserControl
    {
        Benutzer benutzer;
        Vokabel vokabel;
        string uri = "http://localhost:52243/ProjektarbeitVokabeltrainerServer.svc/";

        public VokabelBearbeitenDetails(Benutzer benutzer, Vokabel vokabel)
        {
            InitializeComponent();
            this.benutzer = benutzer;
            this.vokabel = vokabel;
            this.DataContext = vokabel;
        }

        private void ZurückClick(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(new VokabelBearbeiten(benutzer));
        }

        private void BearbeitenClick(object sender, RoutedEventArgs e)
        {
            if (txtDeutsch.Text != "" && txtEnglisch.Text != "")
            {
                vokabel.Deutsch = txtDeutsch.Text;
                vokabel.Deutsch2 = txtDeutsch2.Text;
                vokabel.Englisch = txtEnglisch.Text;
                vokabel.Englisch2 = txtEnglisch2.Text;

                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "vokabel/" + vokabel.ID + "/");
                webrequest.Method = "PUT";
                webrequest.ContentType = "application/xml";
                HttpWebResponse webresponse = null;
                try
                {
                    DataContractSerializer serl = new DataContractSerializer(typeof(Vokabel));
                    using (Stream requestStream = webrequest.GetRequestStream())
                        serl.WriteObject(requestStream, vokabel);
                    webresponse = (HttpWebResponse)webrequest.GetResponse();
                    MessageBox.Show("Vokabel bearbeitet", "Erfolg");
                    mainGrid.Children.Clear();
                    mainGrid.Children.Add(new VokabelBearbeiten(benutzer));
                }
                catch (Exception)
                {
                    MessageBox.Show("Server nicht erreichbar!", "Fehler");
                }
                finally
                {
                    if (webresponse != null)
                        webresponse.Close();
                }
            }
            else
            {
                MessageBox.Show("Bitte eine deutsche und englische Bedeutung angeben!", "Fehler");
            }
        }
    }
}
