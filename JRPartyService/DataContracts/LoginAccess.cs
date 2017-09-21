using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class LoginAccess<T>
    {
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
        public string userID
        {
            get;
            set;
        }
        [DataMember]
        public string districtID
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
        public T rows
        {
            get;
            set;
        }
    }
}
