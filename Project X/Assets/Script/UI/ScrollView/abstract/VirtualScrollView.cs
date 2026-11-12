using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class VirtualScrollView<TData, TView> : MonoBehaviour where TView : Component, IVirtualItemView<TData>   
{
    [Header("Scroll References")]
    [SerializeField] protected ScrollRect scrollRect;
    [SerializeField] protected RectTransform content;
    [SerializeField] protected GameObject itemPrefab;
    [Header("Grid / List Settings")]
    [SerializeField] protected int columns = 1; // Mặc định 1 cột (danh sách dọc như Text), >1 là lưới (Grid như túi đồ)
    [SerializeField] protected float itemWidth = 100f;
    [SerializeField] protected float itemHeight = 50f;
    [SerializeField] protected float spacingX = 10f;
    [SerializeField] protected float spacingY = 10f;
    [SerializeField] protected int visibleRows = 8; // Số hàng nhìn thấy + hàng đệm
    protected List<TData> dataSource = new List<TData>();
    private List<TView> pooledViews = new List<TView>();
    private int previousTopRow = -1;
    private bool isInitialized = false;
    // Gọi hàm này bất cứ khi nào bạn muốn nạp danh sách dữ liệu mới (Items, Text, Quests...)
    public virtual void SetData(List<TData> newDataSource)
    {
        this.dataSource = newDataSource ?? new List<TData>();

        if (!isInitialized)
        {
            InitPool();
            scrollRect.onValueChanged.AddListener(OnScrollChanged);
            isInitialized = true;
        }
        UpdateContentHeight();
        previousTopRow = -1; // Reset để vẽ lại
        RefreshVisibleItems(0);
    }
    private void InitPool()
    {
        int poolCount = visibleRows * columns;
        for (int i = 0; i < poolCount; i++)
        {
            GameObject obj = Instantiate(itemPrefab, content);
            TView view = obj.GetComponent<TView>();
            pooledViews.Add(view);
        }
    }
    private void UpdateContentHeight()
    {
        int totalRows = Mathf.CeilToInt((float)dataSource.Count / columns);
        float totalHeight = totalRows * (itemHeight + spacingY);
        content.sizeDelta = new Vector2(content.sizeDelta.x, totalHeight);
    }
    private void OnScrollChanged(Vector2 scrollPos)
    {
        float contentY = content.anchoredPosition.y;
        float rowHeight = itemHeight + spacingY;
        int currentTopRow = Mathf.FloorToInt(contentY / rowHeight);
        int maxRows = Mathf.Max(0, Mathf.CeilToInt((float)dataSource.Count / columns) - visibleRows);
        currentTopRow = Mathf.Clamp(currentTopRow, 0, maxRows);
        if (currentTopRow != previousTopRow)
        {
            previousTopRow = currentTopRow;
            RefreshVisibleItems(currentTopRow);
        }
    }
    private void RefreshVisibleItems(int startRow)
    {
        for (int r = 0; r < visibleRows; r++)
        {
            int currentRow = startRow + r;
            for (int c = 0; c < columns; c++)
            {
                int poolIndex = (r * columns) + c;
                int dataIndex = (currentRow * columns) + c;
                if (poolIndex >= pooledViews.Count) continue;
                var view = pooledViews[poolIndex];
                if (dataIndex < dataSource.Count)
                {
                    view.gameObject.SetActive(true);
                    // Định vị toạ độ trên Content
                    float posX = c * (itemWidth + spacingX);
                    float posY = -(currentRow * (itemHeight + spacingY));
                    ((RectTransform)view.transform).anchoredPosition = new Vector2(posX, posY);
                    // BIND DATA ĐA NĂNG
                    view.UpdateData(dataSource[dataIndex], dataIndex);
                }
                else
                {
                    view.gameObject.SetActive(false);
                }
            }
        }
    }
}
