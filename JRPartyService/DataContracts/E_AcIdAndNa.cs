using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class E_AcIdAndNa
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
