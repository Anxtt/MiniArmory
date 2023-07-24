﻿namespace MiniArmory.Core.Models.Spell
{
    public class SpellListViewModel
    {
        public int PageNo { get; set; }
        
        public int PageSize { get; set; }
        
        public int TotalRecords { get; set; }

        public bool HasPreviousPage => PageNo > 1;

        public bool HasNextPage => PageNo < TotalRecords;

        public List<SpellViewModel> Spells { get; set; } = new List<SpellViewModel>();
    }
}
