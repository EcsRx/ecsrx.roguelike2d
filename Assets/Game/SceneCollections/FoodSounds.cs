using System.Collections.Generic;
using UnityEngine;

namespace Game.SceneCollections
{
    public class FoodSounds
    {
        public IEnumerable<AudioClip> AvailableClips { get; private set; }

        public FoodSounds()
        {
            AvailableClips = new[]
            {
                Resources.Load<AudioClip>("Audio/scavengers_fruit1"),
                Resources.Load<AudioClip>("Audio/scavengers_fruit2")
            };
        }
    }
}