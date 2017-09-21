using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class CommonOutputAppT<T>
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
        public T data
        {
            get;
            set;
        }
    }
}
