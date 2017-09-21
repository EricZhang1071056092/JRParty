using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class App_Information
    {
        [DataMember]
        public string title
        {
            get;
            set;
        }
        [DataMember]
        public string picture
        {
            get;
            set;
        }
        [DataMember]
        public string Detail
        {
            get;
            set;
        }
    }
}
