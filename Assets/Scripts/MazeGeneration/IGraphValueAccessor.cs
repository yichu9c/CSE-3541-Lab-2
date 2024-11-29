namespace ShareefSoftware
{
    public interface IGraphValueAccessor<T>
    {
        T GetNodeValue(int row, int column);
        void SetNodeValue(int row, int column, T nodeValue);
    }
}