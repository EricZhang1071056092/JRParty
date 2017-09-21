using JRPartyService.DataContracts;
using System.Runtime.Serialization;

namespace JRPartyService
{
    [DataContract]
    public class CommonOutPutT_MT<T>
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
        public E_AcIdAndNa[] type
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
