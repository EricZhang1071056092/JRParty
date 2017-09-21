using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class SYS_SendToUser
    {
        [DataMember]
        public string name
        {
            get;
            set;
        }
        [DataMember]
        public string phone
        {
            get;
            set;
        }
        [DataMember]
        public string userName
        {
            get;
            set;
        }
    }
}
