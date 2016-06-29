using System.Collections.Generic;
using UnityEngine;

namespace Assets.Game.SceneCollections
{
    public class DrinkSounds
    {
        public IEnumerable<AudioClip> AvailableClips { get; private set; }

        public DrinkSounds()
        {
            AvailableClips = new[]
            {
                Resources.Load<AudioClip>("Audio/scavengers_soda1"),
                Resources.Load<AudioClip>("Audio/scavengers_soda2")
            };
        }
    }
}