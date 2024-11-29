using UnityEngine;

namespace ShareefSoftware
{
    public interface IGameObjectFactory
    {
        GameObject CreateAt(Vector3 position, string name = null);
    }
}
