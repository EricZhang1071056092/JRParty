using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class App_Login
    {
        [DataMember]
        public string userID
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
        public string department
        {
            get;
            set;
        }
        
        [DataMember]
        public string userImage
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
        public string flag
        {
            get;
            set;
        }

    }
}
