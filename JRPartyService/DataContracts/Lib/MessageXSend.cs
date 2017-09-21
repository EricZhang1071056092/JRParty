﻿using JRPartyService.DataContracts.AppConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRPartyService.DataContracts.Lib
{
    public class MessageXSend : SendBase
    {
        public const string ADDRESSBOOK = "addressbook";
        public const string TO = "to";
        public const string PROJECT = "project";
        public const string VARS = "vars";
	    public const string LINKS = "links";

        public MessageXSend(IAppConfig appConfig) : base(appConfig)
        {
        }

        protected override ISender GetSender()
        {
            return new Message(_appConfig);
        }

        public void AddTo(string address)
        {
            this.AddWithComma(TO, address);
        }

        public void AddAddressBook(string addressbook)
        {
            this.AddWithComma(ADDRESSBOOK, addressbook);
        }

        public void SetProject(string project)
        {
            this._dataPair.Add(PROJECT, project);
        }

        public void AddVar(string key, string val)
        {
            this.AddWithJson(VARS, key, val);
        }

        public bool XSend(out string returnMessage)
        {
           return this.GetSender().XSend(_dataPair, out returnMessage);
        }
    }
}
