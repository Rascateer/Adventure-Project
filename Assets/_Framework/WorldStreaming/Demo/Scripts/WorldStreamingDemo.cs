using System;
using System.Collections.Generic;
using Rascateer.Framework.WorldStreaming;
using UnityEngine;

public class WorldStreamingDemo : MonoBehaviour
{
    [SerializeField] private World m_world;
    [SerializeField] private List<Transform> m_streamingObjects;

    private WorldStreamer _streamer = new();
    
    
    private void Start()
    {
        SetupWorld();
    }
    
    private void Update()
    {
        _streamer.SetReferences(m_streamingObjects);
        _streamer.Update();
    }

#if UNITY_EDITOR
    [ContextMenu("Auto-Generate Region Scenes")]
    public void AutoGenerateRegionScenes()
    {
        _streamer.SetWorld(m_world);
        _streamer.GenerateEmptyRegionScenes();
    }
#endif

    [ContextMenu("SetupWorld")]
    public void SetupWorld()
    {
        _streamer.SetWorld(m_world);
        _streamer.SetupWorld();
    }

    private void OnDrawGizmos()
    {
        if (m_world != null)
        {
            Gizmos.DrawWireCube(transform.position + m_world.GameWorldSize / 2f, m_world.GameWorldSize);
        }
    }
}