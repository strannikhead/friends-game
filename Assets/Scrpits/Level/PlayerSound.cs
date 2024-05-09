using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    public Player player;
    public AudioClip[] clips;
    public AudioSource audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        player = GetComponent<Player>();
        player.OnJump += PlayJumpSound;
        player.OnDash += PlayDashSound;
        player.OnHook += PlayHookSound;
        player.OnMoving += PlayMovingSound;
        player.OnNotMoving += StopMovingSound;
    }

    private void PlayJumpSound()
    {
        audioSource.clip = clips[0];
        audioSource.volume = 0.4f;
        audioSource.Play();
    }

    private void PlayDashSound()
    {
        audioSource.clip = clips[1];
        audioSource.volume = 0.3f;
        audioSource.Play();
    }
    
    private void PlayHookSound()
    {
        audioSource.clip = clips[2];
        audioSource.Play();
    }

    private void PlayMovingSound()
    {
        if (audioSource.clip != clips[3] || !audioSource.isPlaying)
        {
            audioSource.clip = clips[3];
            audioSource.Play();
        }
    }
    
    private void StopMovingSound()
    {
        if (audioSource.clip == clips[3] && audioSource.isPlaying)
            audioSource.Stop();
    }
    
}