using JRPartyData;
using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class BrandDetail
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
        public string districtName
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
        public string rate
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
