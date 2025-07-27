using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppTeam17.Platform.UserService;
using Il2CppTGK.Game.Components;
using Il2CppTGK.Game.Components.Attack;
using Il2CppTGK.Game.Components.Enemies;
using Il2CppTGK.Game.Managers;
using UnityEngine;
using System.Text;
using Il2CppTGK.Game.Components.Collisions;
using Il2CppTGK.Game.Components.StatsSystem;
using Il2CppTGK.Game.Components.StatsSystem.Data;
using BlasII.ModdingAPI.Assets;
using System.IO;
using System.Linq;

namespace BlasII.EverythingEviterno;

/// <summary>
/// Replace asset reference when spawning an enemy
/// </summary>
[HarmonyPatch(typeof(EnemySpawnManager), nameof(EnemySpawnManager.CreateEnemyAsyncRequest))]
class EnemySpawnManager_CreateEnemyAsyncRequest_Patch
{
    public static void Prefix(EnemySpawnPoint spawnPoint)
    {
        if (Main.EverythingEviterno.EviternoReference == null)
            return;

        if (spawnPoint.name.StartsWith("BS"))
            return;

        ModLog.Warn($"Replacing async reference for {spawnPoint.name}");
        spawnPoint.prefabReference = Main.EverythingEviterno.EviternoReference;
    }
}

//[HarmonyPatch(typeof(EnemySpawnPoint), nameof(EnemySpawnPoint.OnEnemySpawned))]
//class EnemySpawnPoint_OnEnemySpawned_Patch
//{
//    public static void Postfix(EnemySpawnPoint __instance, GameObject spawnedObject)
//    {
//        ModLog.Info($"Spawned enemy: {spawnedObject.name}");
//        var component = spawnedObject.GetComponentInChildren<StatsComponent>();

//        if (component == null)
//        {
//            ModLog.Error("null comp");
//            return;
//        }

//        RangeStatID health = AssetStorage.RangeStats["Health"];
//        ModLog.Warn("Old: " + component.GetMaxValue(health));
//        component.AddBonus(health, "EASY", 0, -50);
//        ModLog.Warn("New: " + component.GetMaxValue(health));

//        ModifiableStatID pattack = AssetStorage.ModifiableStats["BasePhysicalattack"];
//        ModLog.Warn("Old: " + component.GetCurrentUpgrades(pattack));
//        component.SetCurrentUpgrades(pattack, 2);
//        //component.AddBonus(pattack, "EASY", 0, -50);
//        ModLog.Warn("New: " + component.GetCurrentUpgrades(pattack));

//        ModifiableStatID eattack = AssetStorage.ModifiableStats["BaseElementalattack"];
//        ModLog.Warn("Old: " + component.GetCurrentUpgrades(eattack));
//        component.SetCurrentUpgrades(eattack, 2);
//        //component.AddBonus(eattack, "EASY", 0, -50);
//        ModLog.Warn("New: " + component.GetCurrentUpgrades(eattack));
//    }
//}

//public static class t
//{
//    // Recursive method that returns the entire hierarchy of an object
//    public static string DisplayHierarchy(this Transform transform, int maxLevel, bool includeComponents)
//    {
//        return transform.DisplayHierarchy_INTERNAL(new StringBuilder(), 0, maxLevel, includeComponents).ToString();
//    }

//    private static StringBuilder DisplayHierarchy_INTERNAL(this Transform transform, StringBuilder currentHierarchy, int currentLevel, int maxLevel, bool includeComponents)
//    {
//        // Indent
//        for (int i = 0; i < currentLevel; i++)
//            currentHierarchy.Append('\t');

//        // Add this object
//        currentHierarchy.Append(transform.name);

//        // Add components
//        if (includeComponents)
//        {
//            currentHierarchy.Append(" - ");
//            foreach (Component c in transform.GetComponents<Component>())
//                currentHierarchy.Append(c.GetIl2CppType().FullName + ", ");
//        }
//        currentHierarchy.AppendLine();

//        // Add children
//        if (currentLevel < maxLevel)
//        {
//            for (int i = 0; i < transform.childCount; i++)
//                currentHierarchy = transform.GetChild(i).DisplayHierarchy_INTERNAL(currentHierarchy, currentLevel + 1, maxLevel, includeComponents);
//        }

//        // Return output
//        return currentHierarchy;
//    }
//}