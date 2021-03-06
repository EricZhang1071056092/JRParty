﻿using System.Runtime.Serialization;

namespace JRPartyService
{
    [DataContract]
    public class CommonOutPutT_M<T>
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
        public T rows
        {
            get;
            set;
        }
    }
}
