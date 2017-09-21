using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class Stat_Activityone
    {
        [DataMember]
        public string districtName
        {
            get;
            set;
        }
        [DataMember]
        public int expired
        {
            get;
            set;
        }
        [DataMember]
        public int complete
        {
            get;
            set;
        }
        [DataMember]
        public int Incomplete
        {
            get;
            set;
        }
    }
}
