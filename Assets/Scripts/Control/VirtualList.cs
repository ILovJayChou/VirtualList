using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 虚拟列表
/// </summary>
public class VirtualList : MonoBehaviour, IScrollHandler
{
    /// <summary>
    /// 元件尺寸
    /// </summary>
    public Vector2 ItemSize;

    /// <summary>
    /// 放置内容
    /// </summary>
    public RectTransform Content;

    /// <summary>
    /// 列表项缓存
    /// </summary>
    private LinkedList<VirtualListItem> virtualListItems;

    public List<IVirtualListItemData> DataList = new List<IVirtualListItemData>();

    /// <summary>
    /// 列表单元预制体
    /// </summary>
    [SerializeField]
    private GameObject ListItemPrefab;

    /// <summary>
    /// 当前的中心二维坐标
    /// </summary>
    private Vector2 curCenterPos;

    /// <summary>
    /// 初始的中心二维坐标
    /// </summary>
    private Vector2 originCenterPos;

    /// <summary>
    /// 行数
    /// </summary>
    private uint VirtualRow;

    private Vector2 TopPos;

    private Vector2 LastPos;

    public uint CanViewNum;

    /// <summary>
    /// 每次的偏移量
    /// </summary>
    private Vector2 deltaPos;

    public bool HasEdge = true;

    public void Start()
    {
        curCenterPos.y += Content.rect.height / 2;
        LastPos.y = -200;
        Debug.Log(LastPos.y);
        originCenterPos.y = Content.rect.height / 2;
        VirtualRow = CanViewNum+1;
        TopPos.y =200;
        virtualListItems = new LinkedList<VirtualListItem>();
        InitData();
        for (int i = 0; i < VirtualRow; i++)
        {
            VirtualListItem temp = MonoBehaviour.Instantiate(ListItemPrefab, Content).GetComponent<VirtualListItem>();
            virtualListItems.AddLast(temp);
            temp.Init(this);
            temp.SetIndex(i, 1);
            temp.BindData(DataList[i]);
        }

    }

    public void InitData()
    {
        for(int i = 0; i < 1001; i++)
        {
            DataList.Add(new VirtualListItemData(i.ToString(),i));
        }
    }

    private void CalculatePosistion(VirtualListItem item)
    {
        float tempPosY = deltaPos.y + item.transform.localPosition.y;


        Vector3 tempPos = item.transform.localPosition;
        item.transform.localPosition = new Vector3(item.transform.localPosition.x, tempPosY,item.transform.localPosition.z);
        if (tempPosY < LastPos.y*1.5)
        {
            if (HasEdge&& virtualListItems.First.Value.Data.CurIndex - 1 < 0)
            {
                return;
            }
            int index = virtualListItems.First.Value.Data.CurIndex - 1;
            if (index < 0)
                index += DataList.Count;
            LinkedListNode<VirtualListItem> tempNode = virtualListItems.Last;
            tempNode.Value.transform.localPosition = new Vector3(virtualListItems.First.Value.transform.localPosition.x,
                virtualListItems.First.Value.transform.localPosition.y + item.SizeY,
                virtualListItems.First.Value.transform.localPosition.z);
            virtualListItems.RemoveLast();
            tempNode.Value.BindData(DataList[index]);
            virtualListItems.AddFirst(tempNode);
        }

        if (tempPosY > TopPos.y * 1.5)
        {
            if (HasEdge && virtualListItems.Last.Value.Data.CurIndex +1> DataList.Count - 1)
            {
                return;
            }
            int index = virtualListItems.Last.Value.Data.CurIndex + 1;
            if (index > DataList.Count - 1)
                index = index-(DataList.Count);

            LinkedListNode<VirtualListItem> tempNode = virtualListItems.First;
            tempNode.Value.transform.position = new Vector3(virtualListItems.Last.Value.transform.position.x,
                virtualListItems.Last.Value.transform.position.y - item.SizeY,
                virtualListItems.Last.Value.transform.position.z);
            virtualListItems.RemoveFirst();
            tempNode.Value.BindData(DataList[index]);
            virtualListItems.AddLast(tempNode);
        }

    }

    public void OnScroll(PointerEventData eventData)
    {
        deltaPos.y = eventData.scrollDelta.y*30;
        Debug.Log(deltaPos.y);
        LinkedListNode<VirtualListItem> node = virtualListItems.First;
        if (HasEdge)
        {
            if (virtualListItems.First.Value.TotalIndex == 0 && deltaPos.y < 0 && virtualListItems.First.Value.transform.localPosition.y < TopPos.y)
            {
                return;
            }
            if (virtualListItems.Last.Value.TotalIndex == DataList.Count - 1 && deltaPos.y > 0 && virtualListItems.Last.Value.transform.localPosition.y > LastPos.y - 30)
            {
                return;
            }
        }

        bool isForeachFinish = false;
        while (node != null && !isForeachFinish)
        {
            if (node == virtualListItems.Last)
            {
                isForeachFinish = true;
            }
            CalculatePosistion(node.Value);

            node = node.Next;
        }
    }
}
