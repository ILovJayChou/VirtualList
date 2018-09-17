using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class VirtualListItemData : IVirtualListItemData
{
    public object Content { get; set; }

    public int CurIndex { get; set; }

    public VirtualListItemData(object content,int index)
    {
        Content = content;
        CurIndex = index;
    }
}