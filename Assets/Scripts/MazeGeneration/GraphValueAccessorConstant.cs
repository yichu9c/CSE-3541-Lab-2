namespace ShareefSoftware
{
    public class GraphValueAccessorConstant<T> : IGraphValueAccessor<T>
    {
        public static readonly GraphValueAccessorConstant<T> DefaultConstant = new GraphValueAccessorConstant<T>(default);
        private readonly T nodeValue;

        public GraphValueAccessorConstant(T nodeValue)
        {
            this.nodeValue = nodeValue;
        }

        public T GetNodeValue(int row, int column)
        {
            return nodeValue;
        }

        public void SetNodeValue(int row, int column, T nodeValue)
        {
        }
    }
}