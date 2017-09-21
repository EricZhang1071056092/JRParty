using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class List_Tissue
    {
        [DataMember]
        public string id
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
        public string tissueName
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
        public string type
        {
            get;
            set;
        }

        [DataMember]
        public string populationNum
        {
            get;
            set;
        }
        [DataMember]
        public string leaderName
        {
            get;
            set;
        }
    }
}
