﻿using System.Runtime.Serialization;

namespace JRPartyService.DataContracts
{
    [DataContract]
    public class List_information
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
        public string title
        {
            get;
            set;
        }
        [DataMember]
        public string description
        {
            get;
            set;
        }

        [DataMember]
        public string releaseTime
        {
            get;
            set;
        }
    }
}
