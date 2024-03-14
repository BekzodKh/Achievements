using UnityEngine.Rendering;

namespace Core.Utils.Sorting
{
    public class GroupSortable : ISortable
    {
        private readonly SortingGroup _sortingGroup;

        public GroupSortable(SortingGroup sortingGroup)
        {
            _sortingGroup = sortingGroup;
        }

        public int GetOrder()
        {
            return _sortingGroup.sortingOrder;
        }

        public void SetOrder(int order)
        {
            _sortingGroup.sortingOrder = order;
        }

        public int GetLayer()
        {
            return _sortingGroup.sortingLayerID;
        }

        public void SetLayer(int layer)
        {
            _sortingGroup.sortingLayerID = layer;
        }
    }
}