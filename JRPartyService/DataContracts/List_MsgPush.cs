using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class List_MsgPush
    {
        [DataMember]
        public string id
        {
            get;
            set;
        }
        [DataMember]
        public string name
        {
            get;
            set;
        }
        [DataMember]
        public string phone 
        {
            get;
            set;
        }
        [DataMember]
        public string districtName
        {
            get;
            set;
        }

        [DataMember]
        public string IsPush
        {
            get;
            set;
        }
    }
}
