﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KbcKegs.Models.Api
{
    public class CollectionEventViewModel
    {
        public CollectionEventViewModel()
        {
            Assets = new List<AssetViewModel>();
        }

        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public IEnumerable<AssetViewModel> Assets { get; set; }
    }
}