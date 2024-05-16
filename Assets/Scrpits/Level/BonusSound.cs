using UnityEngine;

public class BonusSound : MonoBehaviour
{
    public Player player;
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] audioClips;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Debug.Log($"audiosource: {audioSource}");
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        Debug.Log($"player: {player}");
        player.OnBonus += PlayCoinSound;
    }

    private void PlayCoinSound()
    {
        if (audioSource.isPlaying)
        {
            audioSource.clip = audioClips[1];
            audioSource.Play();
        }
        else
        {
            audioSource.clip = audioClips[0];
            audioSource.Play();
        }
    }
}