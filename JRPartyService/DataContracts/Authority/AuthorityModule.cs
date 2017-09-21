using System.Runtime.Serialization;

namespace JRPartyService.DataContracts.Authority
{
    [DataContract]
    public class AuthorityModule
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
        public string moduleName
        {
            get;
            set;
        }

        [DataMember]
        public AuthoritySub[] subAuthority
        {
            get;
            set;
        }
    }
}
