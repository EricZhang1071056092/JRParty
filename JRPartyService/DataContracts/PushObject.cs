using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class PushObject
    {
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
        public PushObject[] subAuthority
        {
            get;
            set;
        }
    }
}
