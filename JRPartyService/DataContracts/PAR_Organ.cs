using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class PAR_Organ<T>
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
        public T SubOrgan
        {
            get;
            set;
        }
    }
}
