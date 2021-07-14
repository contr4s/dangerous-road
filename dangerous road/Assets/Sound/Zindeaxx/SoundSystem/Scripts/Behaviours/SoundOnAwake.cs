using UnityEngine;


namespace Zindea.Sounds
{
    [RequireComponent(typeof(SoundManager))]
    public class SoundOnAwake : MonoBehaviour
    {
        public SoundSet AssignedSoundSet;

        private SoundManager m_Sound;

        private void Start()
        {
            InitSounds();
            PlaySounds();
        }

        private void InitSounds()
        {
            m_Sound = GetComponent<SoundManager>();

            if (m_Sound == null)
                m_Sound = gameObject.AddComponent<SoundManager>();

        }

        private void PlaySounds()
        {
            if (AssignedSoundSet != null && AssignedSoundSet.RandomSound != null)
            {
                if (m_Sound == null)
                    InitSounds();

                m_Sound.PlaySound(AssignedSoundSet);
            }
            else
            {
                Debug.LogError(gameObject.name + " has no assigned SoundSet to play!");
            }
        }

    }
}