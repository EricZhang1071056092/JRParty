using System.Runtime.Serialization;

namespace JRPartyService
{
    [DataContract]
    public class TV_TopBox
    {
        [DataMember]
        public string IP
        {
            get;
            set;
        }
        [DataMember]
        public string port
        {
            get;
            set;
        }
        [DataMember]
        public string ChannelID
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
        [DataMember]
        public string password
        {
            get;
            set;
        }
        [DataMember]
        public string time
        {
            get;
            set;
        }
    }
}
