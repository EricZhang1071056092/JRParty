using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class List_Position
    {
        [DataMember]
        public string id
        {
            get;
            set;
        }
        [DataMember]
        public string districtID
        {
            get;
            set;
        }
        [DataMember]
        public string town
        {
            get;
            set;
        }
        [DataMember]
        public string districtName
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
        public string description
        {
            get;
            set;
        }
        [DataMember]
        public string area
        {
            get;
            set;
        }

        [DataMember]
        public string releaseTime
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
        public string[] imageURL
        {
            get;
            set;
        }
    }
}
