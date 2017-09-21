using JRPartyData;
using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class VOL_Detail
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
        public string IDCard
        {
            get;
            set;
        }
        [DataMember]
        public string sex
        {
            get;
            set;
        }
        [DataMember]
        public string nation
        {
            get;
            set;
        }
        [DataMember]
        public string birthDay
        {
            get;
            set;
        }
        [DataMember]
        public string JionTime
        {
            get;
            set;
        }
        [DataMember]
        public string workTime
        {
            get;
            set;
        }
        [DataMember]
        public string duty
        {
            get;
            set;
        }
        [DataMember]
        public string education
        {
            get;
            set;
        }
        [DataMember]
        public string TrainingTitle
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
        public string phone
        {
            get;
            set;
        }
        [DataMember]
        public string financialType
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
        public POP_Portrait[] imageURL
        {
            get;
            set;
        }
    }
}
