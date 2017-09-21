using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class CommonOutputApp
    {
        [DataMember]
        public int IsOk
        {
            get;
            set;
        }
        [DataMember]
        public string Msg
        {
            get;
            set;
        }
    }
}
