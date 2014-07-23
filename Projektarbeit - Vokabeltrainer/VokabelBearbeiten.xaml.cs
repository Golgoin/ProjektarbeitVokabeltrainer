using ProjektarbeitVokabeltrainer.ProjektarbeitVokabeltrainerServer;
using System;
using System.Collections.Generic;
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
    /// Interaktionslogik für VokabelBearbeiten.xaml
    /// </summary>
    public partial class VokabelBearbeiten : UserControl
    {
        Benutzer benutzer;
        string uri = "http://localhost:52243/ProjektarbeitVokabeltrainerServer.svc/";

        public VokabelBearbeiten(Benutzer benutzer)
        {
            InitializeComponent();
            this.benutzer = benutzer;
            HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "benutzer/" + benutzer.ID + "/vokabel/");
            webrequest.Method = "GET";
            HttpWebResponse webresponse = null;
            try
            {
                webresponse = (HttpWebResponse)webrequest.GetResponse();
                DataContractSerializer serl = new DataContractSerializer(typeof(List<Vokabel>));
                dataGrid.ItemsSource = (List<Vokabel>)serl.ReadObject(webresponse.GetResponseStream());
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                if (webresponse != null)
                    webresponse.Close();
            }
        }

        private void ZurückClick(object sender, RoutedEventArgs e)
        {
            mainGrid.Children.Clear();
            mainGrid.Children.Add(new Eingelogged(benutzer));
        }

        private void BearbeitenClick(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is Vokabel)
            {
                mainGrid.Children.Clear();
                mainGrid.Children.Add(new VokabelBearbeitenDetails(benutzer, dataGrid.SelectedItem as Vokabel));
            }
        }

        private void LöschenClick(object sender, RoutedEventArgs e)
        {
            if (dataGrid.SelectedItem is Vokabel)
            {
                HttpWebRequest webrequest = (HttpWebRequest)WebRequest.Create(uri + "vokabel/" + ((Vokabel)dataGrid.SelectedItem).ID  + "/");
                webrequest.Method = "DELETE";
                HttpWebResponse webresponse = null;
                try
                {
                    webresponse = (HttpWebResponse)webrequest.GetResponse();
                    MessageBox.Show("Vokabel gelöscht!", "Erfolg");
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
        }
    }
}
