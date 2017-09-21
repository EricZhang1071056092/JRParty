using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class ActivityPicture
    {
        [DataMember]
        public string id
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
        public string content
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
        [DataMember]
        public string[] URL
        {
            get;
            set;
        }
    }
}
