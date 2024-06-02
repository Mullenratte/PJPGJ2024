using UnityEngine;

namespace Audio
{
    public class OneShotSound : MonoBehaviour
    {
        [Range(0,1)]
        public float Volume = 1f;
        public AudioClip AC;
        
        private AudioSource AS;
        bool hasStartedPlaying = false;
    
        private void Awake()
        {
            AS = GetComponent<AudioSource>();
            AS.loop = false;
        }
    
        private void Update()
        {
            if (!hasStartedPlaying)
            {
                hasStartedPlaying = true;
                AS.clip = AC;
                AS.volume = Volume;
                AS.Play();
            }

            if (hasStartedPlaying && !AS.isPlaying)
            {
                Destroy(this);
            }
        }
    }
}
