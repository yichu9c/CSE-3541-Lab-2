using UnityEngine;

namespace ShareefSoftware
{
    class GameObjectFactoryPrefab : IGameObjectFactory
    {
        public GameObject Prefab { get; set; }
        public Transform Parent { get; set; }

        public GameObject CreateAt(Vector3 position, string name = null)
        {
            var prefab = GameObject.Instantiate<GameObject>(Prefab, position, Quaternion.identity, Parent);
            prefab.name = name ?? Prefab.name;

            // Add a collider if the prefab doesn't already have one
            if (prefab.GetComponent<Collider>() == null)
            {
                // Example: Add a BoxCollider; modify as needed
                BoxCollider collider = prefab.AddComponent<BoxCollider>();
                collider.isTrigger = true; // Set this based on your needs
            }

            return prefab;
        }
    }
}
