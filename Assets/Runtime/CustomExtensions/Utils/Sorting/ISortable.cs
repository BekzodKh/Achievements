namespace Core.Utils.Sorting
{
    public interface ISortable
    {
        int GetOrder();

        void SetOrder(int order);

        int GetLayer();

        void SetLayer(int layer);
    }
}