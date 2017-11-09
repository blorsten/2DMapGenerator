using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MapGeneration.Extensions;
using UnityEngine;

namespace MapGeneration.SaveSystem
{
    /// <summary>
    /// Purpose: Used on maps to save its childs persistent data.
    /// Creator: MP
    /// </summary>
    [RequireComponent(typeof(Map))]
    public class MapDataSaver : MonoBehaviour
    {
        private Map _map;

        public List<VariableData<FieldInfo>> SavedFieldInfos;
        public List<VariableData<PropertyInfo>> SavedPropertyInfos;
        public List<Guid> DirtyGuids;

        private List<VariableInfo> _prefabClasses;

        void Start()
        {
            _map = GetComponent<Map>();

            _prefabClasses = new List<VariableInfo>();

            var components = GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour monoBehaviour in components)
            {
                Type monoType = monoBehaviour.GetType();

                if (_prefabClasses.All(info => info.Type != monoType))
                {
                    VariableInfo newVariableInfo = new VariableInfo(monoType);    
                    newVariableInfo.Load(monoBehaviour);
                    _prefabClasses.Add(newVariableInfo);
                }
            }
        }

        public void LoadPersistentData()
        {
            foreach (DataIdentity child in GetComponentsInChildren<DataIdentity>())
            {
                LoadPersistentData(child);
            }
        }

        public void SavePersistentData()
        {
            foreach (DataIdentity child in GetComponentsInChildren<DataIdentity>())
            {
                SavePersistentData(child);
            }
        }

        private void SavePersistentData(DataIdentity go)
        {
            foreach (VariableInfo persistentDataClass in _prefabClasses)
            {
                Component comp = go.GetComponent(persistentDataClass.Type);

                foreach (KeyValuePair<FieldInfo, object> fieldInfo in persistentDataClass.FieldInfos)
                {
                    var data = fieldInfo.Key.GetValue(comp);

                    if (data != null && !data.Equals(persistentDataClass))
                    {
                        SavedFieldInfos.Add(
                            new VariableData<FieldInfo>(persistentDataClass.Type, fieldInfo.Key, go.Id, data));

                        DirtyGuids.Add(go.Id);
                        go.IsDirty = true;
                    }
                }

                foreach (KeyValuePair<PropertyInfo, object> propertyInfo in persistentDataClass.PropertiesInfos)
                {
                    var data = propertyInfo.Key.GetValue(comp, null);

                    if (data != null && !data.Equals(persistentDataClass))
                    {
                        SavedPropertyInfos.Add(
                            new VariableData<PropertyInfo>(persistentDataClass.Type, propertyInfo.Key, go.Id, data));

                        DirtyGuids.Add(go.Id);
                        go.IsDirty = true;
                    }
                }
            }
        }

        private void LoadPersistentData(DataIdentity character)
        {
            if (!character.IsDirty)
                return;

            foreach (VariableData<FieldInfo> fieldCabinet in SavedFieldInfos.Where(cabinet => cabinet.Id == character.Id))
            {
                fieldCabinet.Info.SetValue(character.GetComponent(fieldCabinet.Type), fieldCabinet.Data);
            }

            foreach (VariableData<PropertyInfo> propertyCabinet in SavedPropertyInfos.Where(cabinet => cabinet.Id == character.Id))
            {
                propertyCabinet.Info.SetValue(character.GetComponent(propertyCabinet.Type), propertyCabinet.Data, null);
            }

            character.IsDirty = false;
        }
    }
}