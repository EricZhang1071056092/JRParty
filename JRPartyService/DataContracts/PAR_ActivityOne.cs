using JRPartyData;
using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class PAR_ActivityOne
    {
        [DataMember]
        public string id
        {
            get;
            set;
        }
        [DataMember]
        public string month
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
        public string type
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
        public string distritID
        {
            get;
            set;
        }
        [DataMember]
        public PAR_ActivityReleaseFile[] file
        {
            get;
            set;
        }
    }
}
