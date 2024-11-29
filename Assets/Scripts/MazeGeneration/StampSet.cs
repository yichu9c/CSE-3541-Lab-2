using System;
using System.Collections.Generic;

namespace ShareefSoftware
{
    public class StampSet<TName,T>
    {
        private readonly Dictionary<TName,IGridGraph<T>> _stamps = new Dictionary<TName, IGridGraph<T>>();
        private readonly IGridGraph<T> defaultStamp;
        public int Width { get; }
        public int Height { get; }

        public StampSet(int width, int height)
        {
            this.Width = width;
            this.Height = height;
            defaultStamp = new GridGraph<T>(width, height, GraphValueAccessorConstant<T>.DefaultConstant);
        }

        public void RegisterStamp(TName name, IGridGraph<T> stamp)
        {
            if (stamp.NumberOfColumns != Width && stamp.NumberOfRows != Height)
            {
                throw new ArgumentException(string.Format("Trying to register a stamp that is the wrong size: Width: {0}, Height: {1}", stamp.NumberOfColumns, stamp.NumberOfRows));
            }
            _stamps.Add(name, stamp);
        }

        public IGridGraph<T> GetStamp(TName name)
        {
            if (_stamps.TryGetValue(name, out IGridGraph<T> stamp))
            {
                return stamp;
            }
            return defaultStamp;
        }
    }
}