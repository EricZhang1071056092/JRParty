using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JRPartyService.DataContracts.Lib
{
    public interface ISenderFactory
    {
        ISender GetSender();
    }
}
