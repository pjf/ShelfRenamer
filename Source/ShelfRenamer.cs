using System.Collections.Generic;
using RimWorld;
using Verse;
using Harmony;
using UnityEngine;
using HugsLib;
using HugsLib.Utils;

namespace ShelfRenamer
{
	// HugsLib config
	public class ShelfRenamer : ModBase
	{

		// Having a state variable means our patches can find us and our data store.
		internal static ShelfRenamer Instance { get; private set; }
		private DataStore _dataStore;

		public ShelfRenamer()
		{
			Instance = this;
		}
        
		public override string ModIdentifier
		{
			get { return "ShelfRenamer"; }
		}

		public override void WorldLoaded()
		{
			base.WorldLoaded();
			_dataStore = UtilityWorldObjectManager.GetUtilityWorldObject<DataStore>();

		}
        
        // Set the name of a thing.
		public void SetName(Thing thing, string name)
		{
			if (name.Length == 0)
			{
				ClearName(thing);
			}
			else
			{
				_dataStore.shelfNames[thing.ThingID] = name;
			}
		}

		public void Log(string str)
		{
			this.Logger.Message(str);
		}

		public void ClearName(Thing thing)
		{
			_dataStore.shelfNames.Remove(thing.ThingID);
		}
        
		public bool IsRenamed(Thing thing)
		{
			if (thing == null)
			{
				return false;
			}
			return _dataStore.shelfNames.ContainsKey(thing.ThingID);
		}

		public string NameOf(Thing thing)
		{
			return _dataStore.shelfNames[thing.ThingID];
		}

	}

    // Data store where we keep our shelf names.
	public class DataStore : UtilityWorldObject
	{
		public Dictionary<string, string> shelfNames = new Dictionary<string, string>();

        // Expose our data store to the serialiser.
		public override void ExposeData()
		{
			base.ExposeData();

			Scribe_Collections.Look(ref shelfNames, "shelfNames");
		}
	}    
}
