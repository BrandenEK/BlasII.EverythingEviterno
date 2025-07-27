using BlasII.ModdingAPI;
using Il2CppSystem.Threading;
using Il2CppTGK.Game.Managers;
using Il2CppTGK.Game;
using Il2CppTGK.Game.Components;
using System.Collections;
using System.Linq;
using UnityEngine;
using Il2CppTGK.Game.Systems;
using MelonLoader;

namespace BlasII.EverythingEviterno;

/// <summary>
/// Replaces every enemy with Eviterno
/// </summary>
public class EverythingEviterno : BlasIIMod
{
    internal EverythingEviterno() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    internal GameObjectAssetReference EviternoReference { get; private set; }

    /// <summary>
    /// Load the eviterno reference
    /// </summary>
    protected override void OnSceneLoaded(string sceneName)
    {
        if (sceneName != "MainMenu")
            return;

        if (EviternoReference != null)
            return;

        MelonCoroutines.Start(LoadEviternoReference());
    }

    private IEnumerator LoadEviternoReference()
    {
        ModLog.Info("Starting eviterno process");
        RoomScene scene = FindEviternoScene();

        string path = "Enemies/BS10-logic/BS10 - EviternoSword - SpawnPoint";
        string[] parts = path.Split('/');

        // Load temp scene
        scene.Load(CancellationToken.None);

        while (scene.InOperation)
            yield return null;
        ModLog.Warn("Finished loading");

        // Find object through path
        Transform parent = scene.GetRootGameObjects().First(x => x.name == parts[0]).transform;
        for (int i = 1; i < parts.Length; i++)
        {
            parent = parent?.Find(parts[i]);
        }

        // Store the spawnpoint
        if (parent == null)
        {
            ModLog.Error("Failed to find Eviterno spawn");
        }
        else
        {
            ModLog.Info($"Found eviterno spawn: {parent.name}");
            EnemySpawnPoint spawnpoint = parent.GetComponent<EnemySpawnPoint>();
            EviternoReference = spawnpoint.prefabReference;
        }

        // Unload temp scene
        scene.Unload();

        while (scene.InOperation)
            yield return null;
        ModLog.Warn("Finished unloading");
    }

    private RoomScene FindEviternoScene()
    {
        //string room = "Z1808";
        //string prefix = "LOGIC";

        foreach (var room in CoreCache.Room.rooms.Values)
        {
            if (room.Name == "Z1808")
            {
                foreach (var kvp in room.Scenes)
                {
                    if (kvp.Key == "LOGIC")
                        return kvp.Value;
                }

                throw new System.Exception("Failed to find Eviterno logic room");
            }
        }

        throw new System.Exception("Failed to find Eviterno room");
    }
}
