namespace Core.Utils.Sorting
{
    public class SortingChanger
    {
        private readonly ISortable _sortable;
        private readonly int _initOrder;
        private readonly int _initLayer;

        public SortingChanger(ISortable sortable)
        {
            _sortable = sortable;

            _initOrder = sortable.GetOrder();
            _initLayer = sortable.GetLayer();
        }

        public void SetOrder(int order)
        {
            _sortable.SetOrder(order);
        }

        public void SetLayer(int order)
        {
            _sortable.SetLayer(order);
        }

        public void AppendOrder(int order)
        {
            _sortable.SetOrder(_sortable.GetOrder() + order);
        }

        public void Reset()
        {
            ResetOrder();
            ResetLayer();
        }

        public void ResetOrder()
        {
            _sortable.SetOrder(_initOrder);
        }

        public void ResetLayer()
        {
            _sortable.SetLayer(_initLayer);
        }
    }
}