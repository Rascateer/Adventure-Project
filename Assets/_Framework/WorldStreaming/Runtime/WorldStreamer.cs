using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rascateer.Framework.WorldStreaming
{
    public class WorldStreamer
    {
        // World Settings
        private World _world;
        private const int RegionSize = 500;
        private const int RegionsToLoad = 4;

        // Region Data
        private readonly Dictionary<Vector2Int, Region> _regions = new();
        private readonly HashSet<string> _loadedScenes = new();
        private List<Transform> _references;
        private Dictionary<string, int> _currentFrameUsage = new();

        // Timer for checking regions
        private float _checkupInterval = 0.5f;
        private float _timer;

        public void SetWorld(World world)
        {
            _world = world;
        }

        public void SetReferences(List<Transform> references)
        {
            _references = references;
        }

        public void Update()
        {
            if (_references == null || _references.Count == 0 || _world == null)
                return;

            UpdateTimerAndLoadRegions();
        }

        private void UpdateTimerAndLoadRegions()
        {
            _timer += Time.deltaTime;
            if (_timer < 0.5f)
                return;

            _timer = 0f;

            LoadNearbyRegions();
        }

        private void LoadNearbyRegions()
        {
            _currentFrameUsage = new();

            foreach (var tr in _references)
            {
                // Get the closest region to the reference transform
                Vector2 referencePos = new Vector2(tr.position.x, tr.position.z);
                var closestRegions = _regions.Values
                    .OrderBy(r => Vector2.Distance(referencePos, r.Coords))
                    .Take(RegionsToLoad)
                    .ToList();

                foreach (var region in closestRegions)
                {
                    if (!_currentFrameUsage.ContainsKey(region.SceneName))
                        _currentFrameUsage[region.SceneName] = 0;
                    _currentFrameUsage[region.SceneName]++;
                }
            }

            // Load scenes in the currently used regions list
            foreach (var kvp in _currentFrameUsage)
            {
                if (!_loadedScenes.Contains(kvp.Key))
                {
                    if (Application.CanStreamedLevelBeLoaded(kvp.Key))
                    {
                        SceneManager.LoadSceneAsync(kvp.Key, LoadSceneMode.Additive);
                        _loadedScenes.Add(kvp.Key);
                    }
                    else
                    {
                        Debug.LogWarning($"Scene '{kvp.Key}' not found in Build Settings. Skipping load.");
                    }
                }
            }

            // Unload scenes that are not in the current frame usage
            var toUnload = _loadedScenes.Except(_currentFrameUsage.Keys).ToList();
            foreach (var scene in toUnload)
            {
                SceneManager.UnloadSceneAsync(scene);
                _loadedScenes.Remove(scene);
            }
        }

        public void SetupWorld()
        {
            _regions.Clear();
            for (int y = 0; y < _world.WorldSizeInRegions.y; y++)
            {
                for (int x = 0; x < _world.WorldSizeInRegions.x; x++)
                {
                    Vector2Int gridPos = new(x, y);
                    Region region = new Region
                    {
                        SceneName = $"Region_{x}_{y}",
                        Coords = new Vector2Int((int)(x * RegionSize + RegionSize / 2f), (int)(y * RegionSize + RegionSize / 2f))
                    };

                    _regions[gridPos] = region;
                }
            }
        }

#if UNITY_EDITOR
        public void GenerateEmptyRegionScenes()
        {
            string baseFolder = "Assets";
            string[] folders = new[] { "_Framework", "WorldStreaming", "Demo", "Regions", "EmptyRegions" };
            string currentPath = baseFolder;

            foreach (var folder in folders)
            {
                string newPath = $"{currentPath}/{folder}";
                if (!AssetDatabase.IsValidFolder(newPath))
                {
                    AssetDatabase.CreateFolder(currentPath, folder);
                }
                currentPath = newPath;
            }

            string targetFolder = currentPath;

            List<EditorBuildSettingsScene> newScenes = new(EditorBuildSettings.scenes);

            for (int y = 0; y < _world.WorldSizeInRegions.y; y++)
            {
                for (int x = 0; x < _world.WorldSizeInRegions.x; x++)
                {
                    string name = $"Region_{x}_{y}";
                    string path = $"{targetFolder}/{name}.unity";
                    if (!System.IO.File.Exists(path))
                    {
                        var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

                        // Create a plane for the scene for easier displaying
                        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
                        plane.transform.position = new Vector3(x * RegionSize + RegionSize / 2f, 0, y * RegionSize + RegionSize / 2f);
                        plane.transform.localScale = new Vector3(50, 1, 50);

                        EditorSceneManager.SaveScene(newScene, path);
                        Debug.Log($"Created {path}");
                    }

                    if (!newScenes.Any(s => s.path == path))
                    {
                        newScenes.Add(new EditorBuildSettingsScene(path, true));
                    }
                }
            }

            EditorBuildSettings.scenes = newScenes.ToArray();
            AssetDatabase.Refresh();
        }
#endif
    }
}