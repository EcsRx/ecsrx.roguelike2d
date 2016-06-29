using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.SceneCollections
{
    public class PlayerAttackSounds
    {
        public IEnumerable<AudioClip> AvailableClips { get; private set; }

        public PlayerAttackSounds()
        {
            AvailableClips = new[]
            {
                Resources.Load<AudioClip>("Audio/scavengers_chop1"),
                Resources.Load<AudioClip>("Audio/scavengers_chop2")
            };
        }
    }
}