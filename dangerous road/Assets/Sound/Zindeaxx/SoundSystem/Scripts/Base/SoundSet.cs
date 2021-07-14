using UnityEngine;

namespace Zindea.Sounds
{
    [CreateAssetMenu(fileName = "New Soundset", menuName = "Zindea/Sound/Soundset", order = 1)]
    public class SoundSet : ScriptableObject
    {
        [Header("Sound Settings")]
        [Range(0, 1)] [Tooltip("Defines the loudness of the sound. 1 is loud, 0 is silent.")]
        public float VolumeAmount = 1;

        [Tooltip("This defines the category of the sound which will be used for adjusting the volume based on the settings. Examples: Ambient, Environment, Voices")]
        public string VolumeID = "None";

        [Tooltip("Is this a collection of looped sounds?")]
        public bool LoopedSounds = false;

        [Tooltip("The minimum distance this sound can travel")]
        public float MinDistance = 1;

        [Tooltip("The maximum distance this sound can travel")]
        public float MaxDistance = 500;

        [Range(-3, 3)]
        public float Pitch = 1;

        [Tooltip("This is the same settings as the usual unity setting!")]
        public int Priority = 128;

        [Tooltip("Sets how much the sound is affected by 3D spatialisation calculations (attenuation, doppler etc). 0.0 makes the sound full 2D, 1.0 makes it full 3D.")]
        [Range(0, 1)] public float SpatialBlend = 1;

        /// <summary>
        /// All the clips of this soundset.
        /// </summary>
        [SerializeField]
        private AudioClip[] m_Clips;

        public AudioClip[] Clips => m_Clips;

        [HideInInspector]public AudioClip curClip;
        /// <summary>
        /// Returns a random sound from this set
        /// </summary>
        public AudioClip RandomSound
        {
            get
            {
                if (Clips.Length != 0)
                {
                    curClip = Clips[ZindeaLibrary.RandomRange(0, Clips.Length)];
                    return curClip;
                }
                return default(AudioClip);
            }
        }

        /// <summary>
        /// Returns a given sound from this set
        /// </summary>
        public AudioClip GetSound (int index)
        {
            return Clips[index];
        }
    }
}