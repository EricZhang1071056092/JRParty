using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class App_Code
    {
        [DataMember]
        public string phone
        {
            get;
            set;
        }
        [DataMember]
        public int code
        {
            get;
            set;
        }
    }
}
