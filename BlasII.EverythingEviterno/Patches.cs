using BlasII.ModdingAPI;
using HarmonyLib;
using Il2CppTGK.Game.Components;
using Il2CppTGK.Game.Managers;

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

        ModLog.Info($"Replacing enemy reference for {spawnPoint.name}");
        spawnPoint.prefabReference = Main.EverythingEviterno.EviternoReference;
    }
}
