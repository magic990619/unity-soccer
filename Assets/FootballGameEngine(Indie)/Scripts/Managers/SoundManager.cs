using System;
using System.Collections.Generic;
using Assets.FootballGameEngine_Indie_.Scripts.Data.Dtos.Components;
using Assets.FootballGameEngine_Indie_.Scripts.Utilities.Enums;
using Patterns.Singleton;
using UnityEngine;

namespace Assets.FootballGameEngine_Indie.Scripts.Managers
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField]
        private bool _isSoundEnabled;

        [SerializeField]
        [Range(0f, 1f)]
        private float _musicVolume;

        [SerializeField]
        [Range(0f, 1f)]
        private float _soundVolume;

        [SerializeField]
        List<AudioSourceComponentDto> audioSourceComponents;

        Dictionary<int, AudioSource> audioSources = new Dictionary<int, AudioSource>();

        public bool IsSoundEnabled { get => _isSoundEnabled; set => _isSoundEnabled = value; }
        public float MusicVolume { get => _musicVolume; set => _musicVolume = value; }
        public float SoundVolume { get => _soundVolume; set => _soundVolume = value; }

        public override void Awake()
        {
            base.Awake();

            // create audio source components for each defined data
            this.audioSourceComponents.ForEach(asc=>
            {
                AudioSource audioSource = this.CreateAudioSource(asc);
                audioSource.transform.SetParent(transform);
                audioSource.transform.localPosition = Vector3.zero;
                try
                {
                    this.audioSources.Add(asc.Id, audioSource);
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            });
        }

        public AudioSource CreateAudioSource(AudioSourceComponentDto audioSourceComponent)
        {
            // creeate audio source component
            AudioSource newAudioSource = base.gameObject.AddComponent<AudioSource>();

            //initialize audio source component
            newAudioSource.clip = audioSourceComponent.AudioClip;
            newAudioSource.volume = audioSourceComponent.Volume * ((audioSourceComponent.Soundype == SoundTypeEnum.Music) ? this._musicVolume : this._soundVolume);
            newAudioSource.loop = audioSourceComponent.CanLoop;
            newAudioSource.playOnAwake = audioSourceComponent.CanPlayOnAwake;

            // return result
            return newAudioSource;
        }

        public Tuple<bool, AudioSource> GetAudioSource(int id)
        {
            // create rtemp value
            AudioSource audioSource = null;

            // try getting the value
            bool isAudioClipAvailable = this.audioSources.TryGetValue(id, out audioSource);

            // return result
            return new Tuple<bool, AudioSource>(isAudioClipAvailable, audioSource);
        }

   
        public void Init(bool isSoundEnabled, float musicVolume, float sfxVolume)
        {
            _isSoundEnabled = isSoundEnabled;
            _musicVolume = musicVolume;
            _soundVolume = sfxVolume;

            // update each audio source
            audioSourceComponents.ForEach(asc =>
            {
                AudioSource audioSource = GetAudioSource(asc.Id).Item2;

                if (asc.Soundype == SoundTypeEnum.Music)
                    audioSource.volume = _musicVolume;
                else if (asc.Soundype == SoundTypeEnum.Sfx)
                    audioSource.volume = _soundVolume;
            });
        }


        public void PlayAudioClip(int id)
        {
            // get audio clip
            Tuple<bool, AudioSource> result = this.GetAudioSource(id);

            // process result
            bool isAudioClipAvailable = result.Item1;
            AudioSource audioSource = result.Item2;
            bool flag = isAudioClipAvailable;

            // ply audio if available
            if (flag)
            {
                // play audio
                audioSource.Play();

                // return
                return;
            }

            // thro error
            throw new NullReferenceException("Audio source with id " + id + " couldn't be found");
        }

        public void StopAudioClip(int id)
        {
            // get audio clip
            Tuple<bool, AudioSource> result = this.GetAudioSource(id);

            // process result
            bool isAudioClipAvailable = result.Item1;
            AudioSource audioSource = result.Item2;
            bool flag = isAudioClipAvailable;

            // stop audio if available
            if (flag)
            {
                audioSource.Stop();
                return;
            }
            throw new NullReferenceException("Audio source with id " + id + " couldn't be found");
        }
    }
}
