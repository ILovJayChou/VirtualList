using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VirtualListItem : MonoBehaviour
{
    public VirtualList CurList;
    public float SizeY { get; private set; }

    public float SizeX { get; private set; }

    public int RowIndex { get; private set; }

    public int ColIndex { get; private set; }

    public int TotalIndex { get { return Data.CurIndex; } }

    [SerializeField]
    private RectTransform curRectTransform;

    public Text IndexText;

    public IVirtualListItemData Data;

    public void Init(VirtualList list)
    {
        CurList = list;
        SizeY = curRectTransform.rect.height;
        SizeX = curRectTransform.rect.width;
    }

    public void SetIndex(int rowIndex, int colIndex)
    {
        RowIndex = rowIndex;
        ColIndex = colIndex;
        Debug.Log(CurList.Content.rect.y);
        curRectTransform.localPosition = new Vector2(0, -4*CurList.Content.rect.y/ CurList.CanViewNum - (rowIndex * SizeY));

    }

    public void SetContent(string content)
    {
        IndexText.text = content;
    }

    public void BindData(IVirtualListItemData data)
    {
        this.Data = data;
        IndexText.text = data.Content.ToString();
    }
}