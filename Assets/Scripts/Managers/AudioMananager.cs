using UnityEngine;

namespace Sweet_And_Salty_Studios
{
    public class AudioMananager : Singelton<AudioMananager>
    {
        private AudioSource musicSource;
        private AudioSource sfxSource;

        public AudioClip[] SoundEffects;
        public AudioClip[] MusicTracks;

        private void Awake()
        {
            CreateAudioSources();

            if(MusicTracks.Length == 0)
            {
                MusicTracks = new AudioClip[1];
            }
        }

        private void Start()
        {
            // PlayMusicTrack(GetRandomAudioClip(MusicTracks));
        }

        private void CreateAudioSources()
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;

            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.playOnAwake = false;
        }

        private AudioClip GetRandomAudioClip(AudioClip[] audioClips)
        {
            return audioClips[Random.Range(0, audioClips.Length)];
        }

        private void PlayMusicTrack(AudioClip musicTrack, bool isLooping = true, float volume = 1f, float pitch = 1f)
        {
            musicSource.volume = volume;
            musicSource.pitch = pitch;
            musicSource.loop = isLooping;

            musicSource.clip = musicTrack;

            musicSource.Play();

        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                PlaySfx(GetRandomAudioClip(SoundEffects));
            }
        }

        public void PlaySfx(AudioClip sfxClip, bool randomize = true, float volume = 1f, float pitch = 1f)
        {
            sfxSource.volume = randomize ? Random.Range(volume - 0.1f, volume + 0.1f) : volume;
            sfxSource.pitch = randomize ? Random.Range(pitch - 0.1f, pitch + 0.1f) : pitch;

            sfxSource.PlayOneShot(sfxClip);
        }
    }
}