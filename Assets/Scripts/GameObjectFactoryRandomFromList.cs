using System.Collections.Generic;
using UnityEngine;

namespace ShareefSoftware
{
    class GameObjectFactoryRandomFromList : IGameObjectFactory
    {
        private readonly List<GameObject> prefabList;
        private readonly System.Random random;

        public Transform Parent { get; set; } = null;

        public GameObjectFactoryRandomFromList(System.Random random = null) : this(new List<GameObject>(), random)
        {
        }
        public GameObjectFactoryRandomFromList(List<GameObject> prefabList, System.Random random = null)
        {
            this.prefabList = prefabList;
            this.random = random ?? new System.Random();
        }

        public void AddPrefab(GameObject prefab)
        {
            prefabList.Add(prefab);
        }

        public GameObject CreateAt(Vector3 position, string name = null)
        {
            int index = random.Next(0, prefabList.Count);
            GameObject selectedPrefab = prefabList[index];
            var prefab = GameObject.Instantiate<GameObject>(selectedPrefab, position, Quaternion.identity, Parent);
            prefab.name = name ?? selectedPrefab.name;
            return prefab;
        }
    }
}