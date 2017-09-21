using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class PAR_WarningPara
    {
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
        public string time
        {
            get;
            set;
        }
    }
}
