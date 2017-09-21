using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class PAR_CheckActivityList
    {
        [DataMember]
        public string id
        {
            get;
            set;
        }
        [DataMember]
        public string title
        {
            get;
            set;
        }
        [DataMember]
        public string type
        {
            get;
            set;
        }
        [DataMember]
        public string content
        {
            get;
            set;
        }
        [DataMember]
        public string month
        {
            get;
            set;
        }

        [DataMember]
        public string percentage
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

        [DataMember]
        public string TVPicture
        {
            get;
            set;
        }
        [DataMember]
        public string PhonePicture
        {
            get;
            set;
        }
        [DataMember]
        public string picture
        {
            get;
            set;
        }

        [DataMember]
        public string isZhen
        {
            get;
            set;
        }
    }
}
