using System.Runtime.Serialization;

namespace JRPartyService
{
    [DataContract]
    public class TV_BoxActivity<T1,T2,T3>
    {

        [DataMember]
        public int total
        {
            get;
            set;
        }
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
        public T1 complete
        {
            get;
            set;
        }
        [DataMember]
        public T2 Incomplete
        {
            get;
            set;
        }
        [DataMember]
        public T3 expired
        {
            get;
            set;
        }
    }
}
