using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiniArmory.Core.Models;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IFactionService
    {
        void AddFaction(FactionFormModel model);
    }
}
