using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class userRegist
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
        [DataMember]
        public string id
        {
            get;
            set;
        }

        [DataMember]
        public string portrait
        {
            get;
            set;
        }
    }
}
