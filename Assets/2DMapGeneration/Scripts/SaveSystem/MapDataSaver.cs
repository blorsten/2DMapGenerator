using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace MapGeneration.SaveSystem
{
    /// <summary>
    /// Purpose: Used on maps to save its childs persistent data.
    /// Creator: MP
    /// </summary>
    public class MapDataSaver
    {
        //List containing all the found types and their variable data.
        private List<VariableInfo> _savedTypes;

        //List containing all the DataIdentities saved data.
        private readonly List<VariableData<FieldInfo>> _savedFieldInfos;

        //Same thing goes for properties.
        private readonly List<VariableData<PropertyInfo>> _savedPropertyInfos;

        //List used to mark the IDs of DataIdentities that has been modified.
        private readonly List<Guid> _dirtyIds;

        //Reference to the map that this data saver belongs to.
        public Map Map { get; set; }

        //ID of the map that this saver belongs to.
        public Guid MapId { get; set; }

        public int MapSeed { get; set; }
        public MapBlueprint MapBlueprint { get; set; }

        //Checks and see if theres any data saved.
        public bool HasSavedData { get { return _savedFieldInfos.Any() || _savedPropertyInfos.Any(); } }

        public MapDataSaver(Map map)
        {
            Map = map;

            MapId = map.ID;
            MapSeed = map.Seed;
            MapBlueprint = map.MapBlueprint;

            _savedFieldInfos = new List<VariableData<FieldInfo>>();
            _savedPropertyInfos = new List<VariableData<PropertyInfo>>();
            _dirtyIds = new List<Guid>();
        }

        /// <summary>
        /// Used to initialize all the collections and load all the types and default values
        /// of monobehaviours that are instantiated inside of the map loaded.
        /// </summary>
        private void Initialize()
        {
            _savedTypes = new List<VariableInfo>();

            //Lets grab all the monobehvaiours we can find.
            var components = Map.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour monoBehaviour in components)
            {
                //Grab their type.
                Type monoType = monoBehaviour.GetType();

                //If we havent saved that kind of monobehaviour before do so.
                if (_savedTypes.All(info => info.Type != monoType))
                {
                    VariableInfo newVariableInfo = new VariableInfo(monoType);
                    newVariableInfo.Load(monoBehaviour);
                    _savedTypes.Add(newVariableInfo);
                }
            }
        }

        /// <summary>
        /// Loads all the data that has been previously saved inside the map data saver.
        /// </summary>
        public void LoadPersistentData()
        {
            //If we havent initialized before, do so.
            if (_savedTypes == null)
                Initialize();

            //Find all data identity objects in the map.
            foreach (DataIdentity child in Map.GetComponentsInChildren<DataIdentity>())
            {
                //If the object doesn't have an ID yet, initialize the data identity.
                if (child.Id == Guid.Empty)
                {
                    child.Initialize(Map.Random);

                    if (_dirtyIds.Contains(child.Id))
                        child.IsDirty = true;
                }

                //Try and load som saved data into it.
                LoadPersistentData(child);
            }
        }

        /// <summary>
        /// Saved all data that need saving on data identity objects.
        /// </summary>
        public void SavePersistentData()
        {
            //Lets clear all the old data.
            _savedPropertyInfos.Clear();
            _savedFieldInfos.Clear();
            _dirtyIds.Clear();

            foreach (DataIdentity child in Map.GetComponentsInChildren<DataIdentity>())
                SavePersistentData(child);
        }

        /// <summary>
        /// Saves all modified data on a specific data identity object.
        /// </summary>
        /// <param name="identity"></param>
        private void SavePersistentData(DataIdentity identity)
        {
            //Goes through all the saved types we did in the initialize phase.
            foreach (VariableInfo persistentDataClass in _savedTypes)
            {
                //Try and grab the speicified component from the di obj.
                Component comp = identity.GetComponent(persistentDataClass.Type);

                //If the component has been deleted carry on.
                if (!comp)
                    continue;

                foreach (KeyValuePair<FieldInfo, object> fieldInfo in persistentDataClass.FieldInfos)
                {
                    //Pick out the data from the obj on the di.
                    var data = fieldInfo.Key.GetValue(comp);

                    //If the data is valid and isn't like the default value, save it.
                    if (data != null && !data.Equals(fieldInfo.Value))
                    {
                        _savedFieldInfos.Add(
                            new VariableData<FieldInfo>(persistentDataClass.Type, fieldInfo.Key, identity.Id, data));

                        _dirtyIds.Add(identity.Id);
                        identity.IsDirty = true;
                    }
                }

                //Same stuff goes for all the properties.
                foreach (KeyValuePair<PropertyInfo, object> propertyInfo in persistentDataClass.PropertiesInfos)
                {
                    var data = propertyInfo.Key.GetValue(comp, null);

                    if (data != null && !data.Equals(propertyInfo.Value))
                    {
                        _savedPropertyInfos.Add(
                            new VariableData<PropertyInfo>(persistentDataClass.Type, propertyInfo.Key, identity.Id, data));

                        _dirtyIds.Add(identity.Id);
                        identity.IsDirty = true;
                    }
                }
            }
        }

        /// <summary>
        /// Loads all the data onto a data identity.
        /// </summary>
        /// <param name="identity">The data identity we're trying to load data into.</param>
        private void LoadPersistentData(DataIdentity identity)
        {
            //If the di is not dirty dont even bother.
            if (!identity.IsDirty)
                return;

            //Go through all the saved data and load it in.
            foreach (VariableData<FieldInfo> fieldCabinet in _savedFieldInfos.Where(cabinet => cabinet.Id == identity.Id))
            {
                fieldCabinet.Info.SetValue(identity.GetComponent(fieldCabinet.Type), fieldCabinet.Data);
            }

            foreach (VariableData<PropertyInfo> propertyCabinet in _savedPropertyInfos.Where(cabinet => cabinet.Id == identity.Id))
            {
                propertyCabinet.Info.SetValue(identity.GetComponent(propertyCabinet.Type), propertyCabinet.Data, null);
            }

            identity.IsDirty = false;
        }
    }
}