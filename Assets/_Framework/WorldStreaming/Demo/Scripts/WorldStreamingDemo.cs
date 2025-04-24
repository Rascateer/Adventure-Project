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
        _streamer.SetWorld(m_world);
    }
    
    private void Update()
    {
        _streamer.SetReferences(m_streamingObjects);
        _streamer.Update();
    }

    [ContextMenu("SetupWorld")]
    public void SetupWorld()
    {
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position + m_world.GameWorldSize / 2f, m_world.GameWorldSize);
    }
}