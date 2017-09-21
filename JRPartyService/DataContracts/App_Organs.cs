using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class App_Organs
    {
        [DataMember]
        public string id
        {
            get;
            set;
        }
        [DataMember]
        public string name
        {
            get;
            set;
        }
    }
}
