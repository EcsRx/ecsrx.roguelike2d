using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.SceneCollections
{
    public class EnemyAttackSounds
    {
        public IEnumerable<AudioClip> AvailableClips { get; private set; }

        public EnemyAttackSounds()
        {
            AvailableClips = new[]
            {
                Resources.Load<AudioClip>("Audio/scavengers_enemy1"),
                Resources.Load<AudioClip>("Audio/scavengers_enemy2")
            };
        }
    }
}