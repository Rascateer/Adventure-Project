using System.Runtime.CompilerServices;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.LowLevel;

namespace Rascateer.Game
{
    public static class Game
    {
        public static PlayerManager Players { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; private set; }
        public static EnemyManager Enemies { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; private set; }
        //public static ProjectileManager Projectiles { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; private set; }
        public static TimeManager Time { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; private set; }
        public static CommandSystem Commands { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; private set; }
        public static Settings Configuration { [MethodImpl(MethodImplOptions.AggressiveInlining)] get; private set; }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            Players = new PlayerManager();
            Enemies = new EnemyManager();
            //Projectiles = new ProjectileManager();
            Time = new TimeManager();
            Commands = new CommandSystem();
            Configuration = new Settings();
            
            InitializePlayerLoop();
        }

        private static void InitializePlayerLoop()
        {
            if (!PlayerLoopHelper.IsInjectedUniTaskPlayerLoop())
            {
                var loop = PlayerLoop.GetCurrentPlayerLoop();
                PlayerLoopHelper.Initialize(ref loop);
            }
        }

        private static void OnTimeUpdate()
        {
        }

        private static void OnManagerUpdate()
        {
        }

        private static void OnUpdate()
        {
            
        }
    }
}