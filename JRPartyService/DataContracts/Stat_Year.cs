using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class Stat_Year
    {
        [DataMember]
        public string year
        {
            get;
            set;
        }
        [DataMember]
        public int total
        {
            get;
            set;
        }
        [DataMember]
        public int completeNum
        {
            get;
            set;
        }
        [DataMember]
        public int IncompleteNum
        {
            get;
            set;
        }
    }
}
