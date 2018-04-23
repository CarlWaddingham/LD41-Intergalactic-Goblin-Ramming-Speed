using UnityEngine;
using System.Collections;

	public class SoundManager : MonoBehaviour
	{

        [Range(0, 1)]
        public float MasterVolume = 1f;

        [Header("Music")]
		[Range(0,1)]
		public float MusicVolume=0.3f;

		[Header("Sound Effects")]
		[Range(0,1)]
		public float SfxVolume=1f;

        public AudioClip UISound;

	    protected AudioSource _backgroundMusic;
        public static SoundManager instance;

        public void OnEnable()
        {
            instance = this;
            if (PlayerPrefs.HasKey("masterVolume"))
            {
                MasterVolume = PlayerPrefs.GetFloat("masterVolume");
            }
            if (PlayerPrefs.HasKey("musicVolume"))
            {
                MusicVolume = PlayerPrefs.GetFloat("musicVolume");
            }
            if (PlayerPrefs.HasKey("soundVolume"))
            {
                SfxVolume = PlayerPrefs.GetFloat("soundVolume");
            }
        }

        /// <summary>
        /// Plays a background music.
        /// Only one background music can be active at a time.
        /// </summary>
        /// <param name="Clip">Your audio clip.</param>
        public virtual void PlayBackgroundMusic(AudioSource Music)
		{
			// if we already had a background music playing, we stop it
			if (_backgroundMusic!=null)
				_backgroundMusic.Stop();
			// we set the background music clip
			_backgroundMusic=Music;
			// we set the music's volume
			_backgroundMusic.volume=MusicVolume * MasterVolume;
			// we set the loop setting to true, the music will loop forever
			_backgroundMusic.loop=true;
			// we start playing the background music
			_backgroundMusic.Play();		
		}

        /// <summary>
        /// Plays a sound
        /// </summary>
        /// <returns>An audiosource</returns>
        /// <param name="sfx">The sound clip you want to play.</param>
        /// <param name="location">The location of the sound.</param>
        /// <param name="loop">If set to true, the sound will loop.</param>
        public virtual AudioSource PlaySound(AudioClip sfx, Vector3 location, bool sound3D, GameObject audioOriginator = null, bool loop=false, Transform parent = null)
		{
        Debug.Log("Hello");
            if(sfx == null)
            {
                Debug.Log("No Audio Clip");
                return null;
            }
			// we create a temporary game object to host our audio source
			GameObject temporaryAudioHost = new GameObject("TempAudio");
			temporaryAudioHost.transform.position = location;
			// we add an audio source to that host
			AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource; 
			// we set that audio source clip to the one in paramaters
			audioSource.clip = sfx; 
			// we set the audio source volume to the one in parameters
			audioSource.volume = SfxVolume * MasterVolume;
            if (sound3D)
            {
                audioSource.spatialBlend = 1;
                audioSource.minDistance = .2f;
            }
			// we set our loop setting

            audioSource.loop = loop;

            if (parent != null)
            {
                audioSource.transform.SetParent(parent);
            }
			// we start playing the sound
			audioSource.Play(); 

			if (!loop)
			{
				// we destroy the host after the clip has played
				Destroy(temporaryAudioHost, sfx.length);
			}

			// we return the audiosource reference
			return audioSource;
		}

		/// <summary>
		/// Stops the looping sounds if there are any
		/// </summary>
		/// <param name="source">Source.</param>
		public virtual void StopLoopingSound(AudioSource source)
		{
			if (source != null)
			{
				Destroy(source.gameObject, source.time - source.clip.length);
			}
		}

        public virtual void ChangeMasterVolume(float value)
        {
            MasterVolume = value;
            PlayerPrefs.SetFloat("masterVolume", MasterVolume);
            UpdateBackgroundMusicVolume();
        }

        public virtual void ChangeSoundVolume(float value)
        {
            SfxVolume = value;
            PlayerPrefs.SetFloat("soundVolume", SfxVolume);
        }

        public virtual void ChangeMusicVolume(float value)
        {
            MusicVolume = value;
            PlayerPrefs.SetFloat("musicVolume", MusicVolume);
            UpdateBackgroundMusicVolume();
        }

        void UpdateBackgroundMusicVolume()
        {
            _backgroundMusic.volume = MusicVolume * MasterVolume;
        }
        bool fading = false;
        public IEnumerator FadeOut(float FadeTime)
        {
            float startVolume = _backgroundMusic.volume;

            while (fading == true)
            {
                yield return null;
            }

            while (_backgroundMusic.volume > 0)
            {
                fading = true;
                _backgroundMusic.volume -= startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            fading = false;
            _backgroundMusic.Stop();
            _backgroundMusic.volume = startVolume;
        }

        public IEnumerator FadeIn(float FadeTime)
        {
            float startVolume = 0.2f;

            _backgroundMusic.volume = 0;
            _backgroundMusic.Play();
            while(fading == true)
            {
                yield return null;
            }

            while (_backgroundMusic.volume < MusicVolume)
            {
                fading = true;

                _backgroundMusic.volume += startVolume * Time.deltaTime / FadeTime;

                yield return null;
            }

            fading = false;
            _backgroundMusic.volume = MusicVolume;
        }

        public virtual void ChangeBackgroundMusic(AudioClip bgm)
        {
            if (_backgroundMusic == null)
                return;
            if (bgm != null)
            {
                _backgroundMusic.clip = bgm;
                StartCoroutine(FadeIn(1f));
            }
            else
            {
                StartCoroutine(FadeOut(1f));
            }
        }
        
	}