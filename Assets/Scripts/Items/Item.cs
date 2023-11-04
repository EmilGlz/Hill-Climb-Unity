using System;
using UnityEngine;
namespace Scripts.Items
{
    public class Item : IDisposable
    {
        public GameObject Instance;
        public RectTransform Rect;
        public ScriptableObject Data {  get; protected set; }
        protected readonly Transform Parent;
        protected virtual string PrefabName => "";
        private bool _isSelected;
        public Item(ScriptableObject data, Transform parent)
        {
            Data = data;
            Parent = parent;
            Load();
        }

        protected virtual void Load()
        {
            if (Data == null)
                return;
            if(Instance == null)
                Instance = ResourceHelper.InstantiatePrefab(PrefabName, Parent);
            Instance.transform.localScale = Vector3.one;
            Rect = Instance.GetComponent<RectTransform>();
        }
        public virtual void Dispose()
        {
            if (Instance != null)
            {
                UnityEngine.Object.Destroy(Instance);
                Instance = null;
            }
        }

        protected virtual void OnClick()
        {
        }

        public virtual bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
            }
        }

    }
}
