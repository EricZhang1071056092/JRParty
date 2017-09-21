using JRPartyData;
using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class POS_Detail
    {
        [DataMember]
        public string id
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
        public string releaseTime
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
        public string districtName
        {
            get;
            set;
        }
        [DataMember]
        public PAR_ActivityReleaseFile[] imageURL
        {
            get;
            set;
        }
    }
}
