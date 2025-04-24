using UnityEngine;

namespace Rascateer.Framework.WorldStreaming
{
    [System.Serializable]
    public class World
    {
        public Vector2Int WorldSizeInRegions;
        public Vector3 GameWorldSize => new Vector3(WorldSizeInRegions.x * 500f, 10f, WorldSizeInRegions.y * 500f);
    }
}