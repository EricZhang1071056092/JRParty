using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class PAR_SubActivityList
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
        public string SubdistrictName
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
        public string record
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
        public int subDistrictNum
        {
            get;
            set;
        }
        [DataMember]
        public string flag0
        {
            get;
            set;
        }
        [DataMember]
        public string flag1
        {
            get;
            set;
        }
    }
}
