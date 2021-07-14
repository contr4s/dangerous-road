using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Zindea.Sounds
{
    public class SoundManager : MonoBehaviour
    {
        /// <summary>
        /// Current List of Volume Settings
        /// </summary>
        public static List<VolumeControl> VolumeControllers;

        /// <summary>
        /// The default volume of all sounds. If this gets set to 0 the game will be completely silent
        /// </summary>
        public static float DefaultVolume = 1;

        /// <summary>
        /// Current instances of sounds
        /// </summary>
        private List<AudioSource> m_Audio = new List<AudioSource>();

        /// <summary>
        /// This enables sounds to keep playing even if the object is already deleted 
        /// </summary>
        public static bool UseInstances = true;

        private void Update()
        {
            CheckAudioEnd();
        }

        private void Awake()
        {

        }

        /// <summary>
        /// This checks if any sound instances are running and terminates them if them are over
        /// </summary>
        private void CheckAudioEnd()
        {
            if (m_Audio != null)
            {
                for (int i = 0; i < m_Audio.Count; i++)
                {
                    if (m_Audio[i] != null)
                    {
                        if (!m_Audio[i].isPlaying)
                        {
                            if (UseInstances)
                                Destroy(m_Audio[i].gameObject);
                            else
                                Destroy(m_Audio[i]);
                        }
                    }
                }
            }
        }

        private AudioSource CreateChannel()
        {
            if (UseInstances)
            {
                if (m_Audio == null)
                    m_Audio = new List<AudioSource>();

                //Create a new object where we can attach the sound onto
                AudioSource source = new GameObject("SoundPlayer", typeof(AudioSource)).GetComponent<AudioSource>();

                //Set the position to the same as for the origin object
                source.transform.position = transform.position;

                //Add the sound to the running instances
                m_Audio.Add(source);

                return source;
            }
            else
            {
                //Adds the sound to this gameObject
                AudioSource output = gameObject.AddComponent<AudioSource>();
                    
                //Now refresh the channels since we added another sound
                RefreshChannels();
                return output;
            }
        }

        /// <summary>
        /// Reloads all sound instances
        /// </summary>
        private void RefreshChannels()
        {
            m_Audio = new List<AudioSource>(GetComponents<AudioSource>());
        }

        /// <summary>
        /// Base Method for playing sounds
        /// </summary>
        /// <param name="audio">The audio to be played</param>
        /// <param name="VolumeID">Settings identifier to adjust volume</param>
        /// <param name="Secondaryvolume">The volume of the sound itself</param>
        /// <param name="looped">Will this sound be looped</param>
        /// <param name="MinDistance">Minimum distance of this sound</param>
        /// <param name="MaxDistance">Maximum distance of this sound</param>
        /// <param name="pitch"></param>
        /// <param name="Priority"></param>
        /// <param name="SpatialBlend"></param>
        /// <returns></returns>
        public AudioClip PlaySound(AudioClip audio, string VolumeID, float Secondaryvolume, bool looped, float MinDistance = 1, float MaxDistance = 500, float pitch = 1, int Priority = 128, float SpatialBlend = 1)
        {
            //First we create a channel (AudioSource) which will be used to play the sound
            AudioSource SelectedChannel = CreateChannel();

            //Now we apply the soundset settings to the audiosource
            SelectedChannel.clip = audio;
            SelectedChannel.volume = Secondaryvolume * GetVolume(VolumeID.ToLower());
            SelectedChannel.loop = looped;
            SelectedChannel.minDistance = MinDistance;
            SelectedChannel.maxDistance = MaxDistance;
            SelectedChannel.rolloffMode = AudioRolloffMode.Linear;
            SelectedChannel.priority = Priority;
            SelectedChannel.spatialBlend = SpatialBlend;
            SelectedChannel.pitch = pitch;

            //Play the sound :)
            SelectedChannel.Play();
            return audio;
        }

        /// <summary>
        /// Plays a random sound from a given Soundset
        /// </summary>
        /// <param name="soundSet">The Sounds we want to use</param>
        /// <param name="looped">Are these sounds looped (This is an override)</param>
        /// <returns></returns>
        public AudioClip PlaySound(SoundSet soundSet, bool looped)
        {
            if (soundSet == null)
            {
                Debug.LogError("Tried to play a nulled SoundSet on: " + gameObject.name);
                return null;
            }
            else
            {
                return PlaySound(soundSet.RandomSound, soundSet.VolumeID, soundSet.VolumeAmount, soundSet.LoopedSounds, soundSet.MinDistance, soundSet.MaxDistance, soundSet.Pitch, soundSet.Priority, soundSet.SpatialBlend);
            }
        }

        /// <summary>
        /// Stops sounds from this soundset
        /// </summary>
        public void StopSound(SoundSet soundSet)
        {
            if (soundSet == null)
            {
                Debug.LogError("Tried to stop a NULL SoundSet at: " + gameObject.name);

            }
            else
            {
                foreach (AudioClip s in soundSet.Clips)
                    StopSound(s);
            }
        }

        /// <summary>
        /// Stops the given sound
        /// </summary>
        /// <param name="sound">The sound to stop</param>
        public void StopSound(AudioClip sound)
        {
            if (sound == null)
            {
                Debug.LogError("Tried to stop a NULL sound at: " + gameObject.name);
            }
            else
            {
                foreach (AudioSource s in m_Audio)
                {
                    if (s != null && s.clip == sound)
                        s.Stop();
                }
            }
        }

        public float GetSoundDuration(SoundSet soundSet)
        {
            return soundSet.curClip.length;
        }

        public void ChangeVolume(SoundSet soundSet, float amount)
        {
            foreach (AudioClip c in soundSet.Clips)
            {
                foreach (AudioSource s in m_Audio)
                {
                    if (s != null)
                    {
                        if (s.clip == c)
                        {
                            s.volume = amount;
                        }
                    }
                }
            }                         
        }

        public void SetPitch(SoundSet soundSet)
        {
            foreach (AudioClip c in soundSet.Clips)
            {
                
                foreach (AudioSource s in m_Audio)
                {
                    if (s != null)
                    {
                        if (s.clip == c)
                        {
                            s.pitch = Time.timeScale;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Plays sounds from this soundset
        /// </summary>
        public AudioClip PlaySound(SoundSet soundSet)
        {
            if (soundSet != null)
            {
                return PlaySound(soundSet, soundSet.LoopedSounds);
            }
            else
            {
                Debug.LogWarning("Tried to play NULL SoundSet on " + gameObject.name);
                return null;
            }
        }

        /// <summary>
        /// This can be used to play sounds defined in an Animator. (Use this with Animation Events)
        /// </summary>
        /// <param name="soundSet"></param>
        public void PlayAnimatorSound(SoundSet soundSet)
        {
            PlaySound(soundSet, soundSet.LoopedSounds);

        }

        public AudioSource[] AudioChannels => m_Audio.ToArray();

        /// <summary>
        /// Returns the current volume for the given identifier
        /// </summary>
        /// <param name="ID">The settings id of the volume to be checked. Example: Ambient, Environment, Voices, Music</param>
        /// <returns></returns>
        public float GetVolume(string ID)
        {
            if (VolumeControllers != null)
            {
                if (VolumeControllers.Any(x => x.ControlID.ToLower() == ID.ToLower()))
                {
                    //Debug.Log("SOUND: Returning Volume of: " + ID);
                    return VolumeControllers[VolumeControllers.FindIndex(x => x.ControlID.ToLower() == ID.ToLower())].ControlVolume * DefaultVolume;
                }
            }
            return DefaultVolume;
        }

        public static void SetVolumeControl(VolumeControl ToAdd)
        {
            SetVolumeControl(ToAdd.ControlID, ToAdd.ControlVolume);
        }

        public static void SetVolumeControl(string Id, float Volume)
        {
            if (VolumeControllers == null)
                VolumeControllers = new List<VolumeControl>();

            if (!VolumeControllers.Any(x => x.ControlID == Id))
            {
                VolumeControl newvol = new VolumeControl();
                newvol.ControlID = Id;
                newvol.ControlVolume = Volume;
                VolumeControllers.Add(newvol);
            }
            else
                VolumeControllers.Find(x => x.ControlID == Id).ControlVolume = Volume;
        }


    }

    public class VolumeControl
    {
        public string ControlID;
        public float ControlVolume;
    }
}