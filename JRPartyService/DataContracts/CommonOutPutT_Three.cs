using System.Runtime.Serialization;

namespace JRPartyService
{
    [DataContract]
    public class CommonOutPutT_Three<T>
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
        public int completeNum
        {
            get;
            set;
        }
        [DataMember]
        public int IncompleteNum
        {
            get;
            set;
        }
        [DataMember]
        public int expireNum
        {
            get;
            set;
        }
        [DataMember]
        public T rows
        {
            get;
            set;
        }
    }
}
