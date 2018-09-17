using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public interface IVirtualListItemData
{
    int CurIndex { get; set; }
    object Content { get; set; }
}

