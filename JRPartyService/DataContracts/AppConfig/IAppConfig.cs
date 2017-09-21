using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JRPartyService.DataContracts.AppConfig
{
    public interface IAppConfig
    {
        string AppId { get; set; }

        string AppKey { get; set; }

        SignType SignType { get; set; }
    }

    public enum SignType
    {
        normal,
        md5,
        sha1
    }
}
