using System.Runtime.Serialization;

namespace JRPartyService.DataContracts.Authority
{
    [DataContract]
    public class AuthoritySub
    {
        [DataMember]
        public bool isControl
        {
            get;
            set;
        }

        [DataMember]
        public string authorityID
        {
            get;
            set;
        }

        [DataMember]
        public string authorityName
        {
            get;
            set;
        }
    }
}
