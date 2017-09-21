using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class App_InformationList
    {
        [DataMember]
        public string title
        {
            get;
            set;
        }
        [DataMember]
        public string picture
        {
            get;
            set;
        }
        [DataMember]
        public string Detail
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
        public string department
        {
            get;
            set;
        }
    }
}
