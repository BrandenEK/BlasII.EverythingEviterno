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
        ModLog.Info("Loading eviterno reference...");
        RoomScene scene = FindEviternoScene();

        string root = "Enemies";
        string path = "BS10-logic/BS10 - EviternoSword - SpawnPoint";

        // Load temp scene
        scene.Load(CancellationToken.None);
        while (scene.InOperation)
            yield return null;

        // Find object through path
        Transform parent = scene.GetRootGameObjects().First(x => x.name == root).transform;
        parent = parent.Find(path);

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
    }

    private RoomScene FindEviternoScene()
    {
        if (!CoreCache.Room.rooms.TryGetValue(-1699097293, out Room room))
            throw new System.Exception("Failed to find Eviterno room");

        if (!room.Scenes.TryGetValue("LOGIC", out RoomScene scene))
            throw new System.Exception("Failed to find Eviterno logic scene");

        return scene;
    }
}
