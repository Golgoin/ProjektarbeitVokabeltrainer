using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ProjektarbeitVokabeltrainerServer
{
    // HINWEIS: Mit dem Befehl "Umbenennen" im Menü "Umgestalten" können Sie den Schnittstellennamen "IProjektarbeitVokabeltrainerServer" sowohl im Code als auch in der Konfigurationsdatei ändern.
    [ServiceContract]
    public interface IProjektarbeitVokabeltrainerServer
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "benutzer/")]
        bool Registrieren(Benutzer benutzer);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "benutzer/{benutzerid}/")]
        Benutzer Anmelden(string benutzerid, Benutzer benutzer);

        [OperationContract]
        [WebGet(UriTemplate = "benutzer/{benutzerid}/")]
        Benutzer GetBenutzer(string benutzerid);

        [OperationContract]
        [WebGet(UriTemplate = "benutzer/")]
        List<Benutzer> GetAllBenutzer();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "vokabel/")]
        bool CreateVokabel(Vokabel vokabel);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "vokabel/{vokabelid}/")]
        bool UpdateVokabel(string vokabelid, Vokabel vokabel);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "vokabel/")]
        bool UpdateVokabelList(List<Vokabel> vokabel);

        [OperationContract]
        [WebInvoke(Method = "DELETE", UriTemplate = "vokabel/{vokabelid}/")]
        bool DeleteVokabel(string vokabelid);

        [OperationContract]
        [WebGet(UriTemplate = "vokabel/{vokabelid}/")]
        Vokabel GetVokabel(string vokabelid);

        [OperationContract]
        [WebGet(UriTemplate = "benutzer/{benutzerid}/vokabel/")]
        List<Vokabel> GetVokabelForBenutzer(string benutzerid);

        [OperationContract]
        [WebGet(UriTemplate = "benutzer/{benutzerid}/vokabel/{vokabelFach}/")]
        List<Vokabel> GetVokabelForBenutzerForFach(string benutzerid, string vokabelFach);

        //[OperationContract]
        //[WebGet(UriTemplate = "benutzer/{benutzerid}/Fach/")]
        //List<Fach> GetFächer(string benutzerid);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "benutzer/{benutzerid}/Fach/")]
        bool UpdateFach(string benutzerid, Fach fach);

        //[OperationContract]
        //[WebGet(UriTemplate = "benutzer/{benutzerid}/Statistik/")]
        //Teststatistik GetStatistik(string benutzerid);

        [OperationContract]
        [WebInvoke(Method = "PUT", UriTemplate = "benutzer/{benutzerid}/Statistik/")]
        bool UpdateStatistik(string benutzerid, Teststatistik statistik);
    }
}
