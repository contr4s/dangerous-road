using System.Collections;
using UnityEngine;


namespace Zindea.Sounds
{
    [RequireComponent(typeof(SoundManager))]
    public class SoundOnDistance : MonoBehaviour
    {
        public SoundSet AssignedSoundSet;
        public float Distance = 10;
        public bool PlayOnce = true;
        public float Timeout = 999;

        [Header("Trigger Settigns")]
        public bool UseTag = false;
        public Transform TriggerTransform;
        public string TriggerTag = "Player";

        private SoundManager m_Sound;

        private void Start()
        {
            InitSounds();
            StartCoroutine(DistanceCheck());
        }

        private void InitSounds()
        {
            m_Sound = GetComponent<SoundManager>();

            if (m_Sound == null)
                m_Sound = gameObject.AddComponent<SoundManager>();

        }

        private IEnumerator DistanceCheck()
        {
            bool done = false;
            while (!done)
            {
                Transform triggerObject = null;
                while (triggerObject == null)
                {
                    if (UseTag)
                    {
                        triggerObject = GameObject.FindWithTag(TriggerTag).transform;
                    }
                    else
                    {
                        triggerObject = TriggerTransform;
                    }
                    yield return null;
                }
                yield return new WaitUntil(() => Vector3.Distance(triggerObject.position, transform.position) < Distance);

                PlaySounds();

                if (PlayOnce)
                    done = true;
                else
                    yield return new WaitForSeconds(Timeout);

                yield return null;
            }
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