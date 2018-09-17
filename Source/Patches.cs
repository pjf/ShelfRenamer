using Harmony;
using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace ShelfRenamer
{
	// Add a "Rename" Gizmo to storage buildings.
	[HarmonyPatch(typeof(Building_Storage))]
	[HarmonyPatch("GetGizmos")]
    public static class Patch_Building_Storage_GetGizmos
    {
        public static void Postfix(Building_Storage __instance, ref IEnumerable<Gizmo> __result)
        {

            // If it has a user-accessible storage tab, then allow renaming.
            if (__instance.StorageTabVisible)
            {

                // TODO: Don't add a rename button if one already exists.

                __result = __result.Add(new Command_Action
                {
                    // TODO: Bundle own icon so we don't depend on RimFridge.
                    icon = ContentFinder<Texture2D>.Get("UI/Icons/Rename", true),
                    defaultDesc = "Rename".Translate(),
                    defaultLabel = "Rename".Translate(),
                    activateSound = SoundDef.Named("Click"),
                    action = delegate { Find.WindowStack.Add(new Dialog_Rename(__instance)); },               
                });
            }
        }
    }

    // Label checks if we've got a renamed shelf. If not, runs the original code.
    // Building_Storage inherits Label from Thing, so that's what we need to patch.
	[HarmonyPatch(typeof(Building_Storage))]
	[HarmonyPatch("Label", PropertyMethod.Getter)]
    public static class Patch_Building_Storage_Label
    {
        public static bool Prefix(Thing __instance, ref string __result)
        {
            if (ShelfRenamer.Instance.IsRenamed(__instance))
            {
                __result = ShelfRenamer.Instance.NameOf(__instance);
                return false; // Don't run original method.
            }

            // Shelf isn't renamed, run original Label method.
            return true;
        }
    }

	[HarmonyPatch(typeof(Building_Storage))]
	[HarmonyPatch("DeSpawn")]
    public static class Patch_Building_Storage_DeSpawn
	{
		public static void Prefix(Thing __instance)
		{
			ShelfRenamer.Instance.ClearName(__instance);
		}
	}

    // TODO: Clear entries in our data store for items which are destroyed.
 }