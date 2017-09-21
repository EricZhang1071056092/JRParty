using System.Runtime.Serialization;

namespace JRPartyService.DataContracts.Authority
{
    [DataContract]
    public class AuthorityModel
    {
        [DataMember]
        public AuthorityModule[] module
        {
            get;
            set;
        }

        [DataMember]
        public bool haveRole
        {
            get;
            set;
        }

        [DataMember]
        public string roleID
        {
            get;
            set;
        }
    }
}
