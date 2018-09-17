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
        
		public void SetName(Thing thing, string name)
		{
			_dataStore.shelfNames.Add(thing.ThingID, name);
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

			Scribe_Collections.Look(
				ref shelfNames, "shelfNames",
				LookMode.Value, LookMode.Deep
			);

            /*
			if (Scribe.mode == LoadSaveMode.LoadingVars && shelfNames == null)
			{
				shelfNames = new Dictionary<string, string>();
			}
			*/
		}
	}
    
}
