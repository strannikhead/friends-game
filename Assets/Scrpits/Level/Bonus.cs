using UnityEngine;

public class Bonus : MonoBehaviour
{
    public int price; // review(26.05.2024): зачем тут это поле?
    [SerializeField]
    private AudioClip audioClip;

    private void OnDestroy()
    {
        AudioSource.PlayClipAtPoint(audioClip, transform.position, 0.4f);
    }
}
