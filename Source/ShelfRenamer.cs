using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Harmony;
using UnityEngine;

namespace ShelfRenamer
{
	[HarmonyPatch(typeof(Building_Storage))]
	[HarmonyPatch("GetGizmos")]
	public static class ShelfRenamer
    {

		[HarmonyPostfix]
		public static void RenameShelves(Building_Storage __instance, ref IEnumerable<Gizmo> __result)
		{
			// If it has a user-accessible storage tab, then allow renaming.

			// TODO: Don't add a rename button if one already exists.

			if (__instance.StorageTabVisible)
			{
				__result.Add(new Command_Action
				{
					icon = ContentFinder<Texture2D>.Get("UI/Icons/Rename", true),
					defaultDesc = "Rename".Translate(),
					defaultLabel = "Rename".Translate(),
					activateSound = SoundDef.Named("Click"),
					action = delegate { Find.WindowStack.Add(new Dialog_Rename(__instance)); }
                    // groupKey ??
				});
			}
		}
    }
}
