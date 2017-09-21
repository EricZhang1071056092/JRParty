using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class SYS_UserList
    {
        [DataMember]
        public string id
        {
            get;
            set;
        }
        [DataMember]
        public string district
        {
            get;
            set;
        }
        [DataMember]
        public string enable
        {
            get;
            set;
        }
        [DataMember]
        public string lastTime
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
        public string password
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
        [DataMember]
        public string roleID
        {
            get;
            set;
        }
        [DataMember]
        public string userName
        {
            get;
            set;
        }

        [DataMember]
        public string department
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
        public string attachDistrict
        {
            get;
            set;
        }
    }
}
