using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class InfPush
    {
        [DataMember]
        public string id
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
        public string town
        {
            get;
            set;
        }
        //[DataMember]
        //public string village
        //{
        //    get;
        //    set;
        //}
        [DataMember]
        public string title
        {
            get;
            set;
        }
        [DataMember]
        public string creatTime
        {
            get;
            set;
        }
        [DataMember]
        public string objects
        {
            get;
            set;
        }
    }
}
