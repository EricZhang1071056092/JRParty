using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class Stat_Activity
    {
        [DataMember]
        public string[] districtName
        {
            get;
            set;
        }
        [DataMember]
        public string[] expired
        {
            get;
            set;
        }
        [DataMember]
        public string[] complete
        {
            get;
            set;
        }
        [DataMember]
        public string[] Incomplete
        {
            get;
            set;
        }
    }
}
