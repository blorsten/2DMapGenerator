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
    public class MapDataSaver
    {
        private List<VariableInfo> _prefabClasses;

        public Map Map { get; set; }
        public List<VariableData<FieldInfo>> SavedFieldInfos { get; set; }
        public List<VariableData<PropertyInfo>> SavedPropertyInfos { get; set; }
        public List<Guid> DirtyIds { get; set; }

        public Guid ID { get; set; }
        public int Seed { get; set; }
        public MapBlueprint MapBlueprint { get; set; }

        public MapDataSaver(Map map)
        {
            Map = map;

            ID = map.ID;
            Seed = map.Seed;
            MapBlueprint = map.MapBlueprint;

            SavedFieldInfos = new List<VariableData<FieldInfo>>();
            SavedPropertyInfos = new List<VariableData<PropertyInfo>>();
            DirtyIds = new List<Guid>();
        }

        public void Initialize()
        {
            _prefabClasses = new List<VariableInfo>();

            var components = Map.GetComponentsInChildren<MonoBehaviour>();
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
            if (_prefabClasses == null)
                Initialize();

            foreach (DataIdentity child in Map.GetComponentsInChildren<DataIdentity>())
            {
                if (child.Id == Guid.Empty)
                {
                    child.Initialize(Map.Random);

                    if (DirtyIds.Contains(child.Id))
                        child.IsDirty = true;
                }

                LoadPersistentData(child);
            }
        }

        public void SavePersistentData()
        {
            SavedPropertyInfos.Clear();
            SavedFieldInfos.Clear();
            DirtyIds.Clear();

            foreach (DataIdentity child in Map.GetComponentsInChildren<DataIdentity>())
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

                    if (data != null && !data.Equals(fieldInfo.Value))
                    {
                        SavedFieldInfos.Add(
                            new VariableData<FieldInfo>(persistentDataClass.Type, fieldInfo.Key, go.Id, data));

                        DirtyIds.Add(go.Id);
                        go.IsDirty = true;
                    }
                }

                foreach (KeyValuePair<PropertyInfo, object> propertyInfo in persistentDataClass.PropertiesInfos)
                {
                    var data = propertyInfo.Key.GetValue(comp, null);

                    if (data != null && !data.Equals(propertyInfo.Value))
                    {
                        SavedPropertyInfos.Add(
                            new VariableData<PropertyInfo>(persistentDataClass.Type, propertyInfo.Key, go.Id, data));

                        DirtyIds.Add(go.Id);
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