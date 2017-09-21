using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class App_PlanList<T>
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
        public T activityList
        {
            get;
            set;
        }
    }
}
