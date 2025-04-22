using UnityEngine;

namespace Rascateer.TopDownGame
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private CharacterStats m_stats;
        [SerializeField] private PlayerMovement m_movement;
        [SerializeField] private PlayerAbilities m_abilities;
    }
}