using UnityEngine;

public class Bonus : MonoBehaviour
{
    public int price;
    [SerializeField]
    private AudioClip audioClip;

    private void OnDestroy()
    {
        AudioSource.PlayClipAtPoint(audioClip, transform.position, 0.4f);
    }
}
