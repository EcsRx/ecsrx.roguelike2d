using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.SceneCollections
{
    public class DeathSounds
    {
        public IEnumerable<AudioClip> AvailableClips { get; private set; }

        public DeathSounds()
        {
            AvailableClips = new[]
            {
                Resources.Load<AudioClip>("Audio/scavengers_die")
            };
        }
    }
}