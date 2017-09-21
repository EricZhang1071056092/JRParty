using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class TissueList2
    {
        [DataMember]
        public string name
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
        public TissueList2[] children
        {
            get;
            set;
        }
    }
}
