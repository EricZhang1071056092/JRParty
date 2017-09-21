using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class Stat_Activityone1
    {

        [DataMember]
        public bool success
        {
            get;
            set;
        }
        [DataMember]
        public string message
        {
            get;
            set;
        }
        [DataMember]
        public string[] incompleteName
        {
            get;
            set;
        }
        [DataMember]
        public string[] completeName
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
        public int incomplete
        {
            get;
            set;
        }
    }
}
