using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class App_ActivityList
    {
        [DataMember]
        public string id
        {
            get;
            set;
        }
        [DataMember]
        public string title
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
        public string type
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
        public string department
        {
            get;
            set;
        }
        [DataMember]
        public string status
        {
            get;
            set;
        }
        [DataMember]
        public string address
        {
            get;
            set;
        }
    }
}
