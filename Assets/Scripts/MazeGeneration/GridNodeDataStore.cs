namespace ShareefSoftware
{
    public class GridNodeDataStore<T> : IGraphValueAccessor<T>
    {
        private readonly int numberOfRows;
        private readonly int numberOfColumns;
        private readonly T[,] backingStore;

        public GridNodeDataStore(int numberOfRows, int numberOfColumns)
        {
            this.numberOfRows = numberOfRows;
            this.numberOfColumns = numberOfColumns;
            backingStore = new T[numberOfRows, numberOfColumns];
        }

        public T GetNodeValue(int row, int column)
        {
            return backingStore[row, column];
        }

        public void SetNodeValue(int row, int column, T nodeValue)
        {
            backingStore[row, column] = nodeValue;
        }
    }
}