using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shooter
{
    [CreateAssetMenu(fileName = "AudioClips", menuName = "ScriptableObject/AudioClips", order = 3)]
    public class AudioClipsSO:ScriptableObject
    {
        [field: SerializeField] public AudioClip[] ButtonSound { get; private set; }
        [field: SerializeField] public AudioClip[] ShootSound { get; private set; }
        [field: SerializeField] public AudioClip[] BulletImpactSound { get; private set; }
        [field: SerializeField] public AudioClip[] ExplosionSound { get; private set; }
        [field: SerializeField] public AudioClip WalkSound { get; private set; }
        [field: SerializeField] public AudioClip JumpSound { get; private set; }

        [field: SerializeField] public AudioClip[] PickupObjectSound { get; private set; }

        [field: SerializeField] public AudioClip[] TakeDamageSound { get; private set; }

    }
}
