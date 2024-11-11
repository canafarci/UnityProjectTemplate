using System;
using System.Collections.Generic;
using System.Reflection;
using ProjectTemplate.Runtime.Infrastructure.ApplicationState;
using UnityEngine;
using UnityEngine.Pool;

namespace ProjectTemplate.Runtime.Infrastructure.MemoryPool
{
    public abstract class PoolBase<T> where T : class, IPoolable
    {
        private readonly int _initialSize;
        private readonly AppStateID _lifetimeSceneID;
        private readonly List<T> _initialObjectList = new List<T>();

        protected readonly bool _managePoolOnSceneChange;
        protected readonly List<T> _pooledObjectsList;

        protected ObjectPool<T> _pool { get; private set; }

        public PoolBase(PoolParams poolParams)
        {
            _initialSize = poolParams.InitialSize;
            _lifetimeSceneID = poolParams.LifetimeSceneID;
            _managePoolOnSceneChange = poolParams.ManagePoolOnSceneChange;

            _pool = new ObjectPool<T>(CreateObject,
                GetFromPool,
                ReturnToPool,
                DestroyObject,
                true,
                poolParams.DefaultCapacity,
                poolParams.MaximumSize);

            _pooledObjectsList = GetInternalList();
            
            if (_managePoolOnSceneChange)
            {
                PoolManager.OnAppStateChanged += OnAppStateChangedHandler;
            }
        }

        private void OnAppStateChangedHandler(AppStateID newState, AppStateID oldState)
        {
            if (oldState == _lifetimeSceneID)
            {
                DestroyAll();
            }
            else if (newState == _lifetimeSceneID)
            {
                InstantiateDefaultObjects();
            }
        }
        
        protected void InstantiateDefaultObjects()
        {
            for (int i = 0; i < _initialSize; i++)
            {
                T obj = _pool.Get();
                _initialObjectList.Add(obj);
            }
            
            _initialObjectList.ForEach(Release);
            _initialObjectList.Clear();
        }

        public T Get() => _pool.Get();

        public void Release(T obj) => _pool.Release(obj);

        private List<T> GetInternalList()
        {
            FieldInfo listField =
                typeof(ObjectPool<T>).GetField("m_List", BindingFlags.NonPublic | BindingFlags.Instance);
            if (listField != null)
            {
                return (List<T>)listField.GetValue(_pool);
            }

            throw new InvalidOperationException("Field 'm_List' not found.");
        }

        private void DestroyAll()
        {
            _pool.Clear();
        }

        // Abstract methods to be implemented by derived classes for specific object creation and lifecycle management
        protected abstract T CreateObject();
        protected abstract void GetFromPool(T obj);
        protected abstract void ReturnToPool(T obj);
        protected abstract void DestroyObject(T obj);

        ~PoolBase()
        {
            if (_managePoolOnSceneChange)
            {
                PoolManager.OnAppStateChanged -= OnAppStateChangedHandler;
            }
            else
            {
                DestroyAll();
            }
        }
    }
}
    


