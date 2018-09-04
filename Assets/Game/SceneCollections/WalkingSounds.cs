using System.Collections.Generic;
using UnityEngine;

namespace Game.SceneCollections
{
    public class WalkingSounds
    {
        public IEnumerable<AudioClip> AvailableClips { get; private set; }

        public WalkingSounds()
        {
            AvailableClips = new[]
            {
                Resources.Load<AudioClip>("Audio/scavengers_footstep1"),
                Resources.Load<AudioClip>("Audio/scavengers_footstep2")
            };
        }
    }
}