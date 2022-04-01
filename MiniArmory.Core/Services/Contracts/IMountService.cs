﻿using MiniArmory.Core.Models;
using MiniArmory.Core.Models.Mount;

namespace MiniArmory.Core.Services.Contracts
{
    public interface IMountService
    {
        Task Add(MountFormModel model);

        Task<IEnumerable<MountViewModel>> AllMounts();

        Task<IEnumerable<JsonFormModel>> GetFactions();
    }
}
