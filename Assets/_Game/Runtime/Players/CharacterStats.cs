using UnityEngine;

namespace Rascateer.TopDownGame
{
    [System.Serializable]
    public class CharacterStats
    {
        [Header("Movement")]
        public Stat MovementSpeed;
        
        [Header("Defense")]
        public Stat Health;
        public Stat Armor;
        public Stat Shield;

        [Header("Stats")] 
        public Stat Power;
        public Stat Dissipation;
        public Stat Capacity;
    }
}