using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Audio
{
    public class EntitySounds : MonoBehaviour
    {
        [Header("Sounds")]
        public AudioClip[] WalkSounds;
        public AudioClip DamagedSound;
        public AudioClip DeathSound;
        public float WalkSoundSpeed = 0.34f;

        [Header("Volumes")]
        [Range(0,1)] public float WalkVolume = 1.0f;
        [Range(0,1)] public float DamagedVolume = 1.0f;
        [Range(0,1)] public float DeathVolume = 1.0f;

        private AudioSource _oneShotSource;
        private AudioSource _movingSource;
        private float currentWalkSpeed = 0f;
        
        public bool IsWalking = false;

        private void Awake()
        {
            var sources = GetComponents<AudioSource>();
            _oneShotSource = sources[0];
            _movingSource = sources[1];
            
            _oneShotSource.loop = false;
            
            _movingSource.loop = false;
        }

        public void PlayDeathSound()
        {
            _oneShotSource.Stop();
            _oneShotSource.volume = DeathVolume;
            _oneShotSource.clip = DeathSound;
            _oneShotSource.Play();
        }
        
        public void PlayDamagedSound()
        {
            _oneShotSource.Stop();
            _oneShotSource.volume = DamagedVolume;
            _oneShotSource.clip = DamagedSound;
            _oneShotSource.Play();
        }
        

        private void Update()
        {

            //IsWalking = Input.GetKey(KeyCode.I);

            
            //if (Input.GetKey(KeyCode.O))
            //{
            //    PlayDamagedSound();
            //}
            
            //if (Input.GetKey(KeyCode.P))
            //{
            //    PlayDeathSound();
            //}
            
            PlayNextRandomFootstep();
        }

        private void PlayNextRandomFootstep()
        {
            currentWalkSpeed += Time.deltaTime;
            
            if (IsWalking && currentWalkSpeed >= WalkSoundSpeed)
            {
                _movingSource.volume = WalkVolume;
                currentWalkSpeed = 0f;

                _movingSource.Stop();
                _movingSource.clip = WalkSounds[Random.Range(0, WalkSounds.Length)];
                _movingSource.Play();
            }
        }
    }
}