using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class RegimeList
    {
        [DataMember]
        public string id
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
        public string name
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
        public string[] imageURL
        {
            get;
            set;
        }
    }
}
