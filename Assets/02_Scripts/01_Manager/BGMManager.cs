using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private float defaultVolume = 0.3f;   // ⭐ 기본 볼륨

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (bgmSource == null)
            bgmSource = GetComponent<AudioSource>();

        // ⭐ 시작할 때 기본 볼륨 적용
        SetVolume(defaultVolume);
    }

    public void SetVolume(float volume01)
    {
        volume01 = Mathf.Clamp01(volume01);
        bgmSource.volume = volume01;
    }

    public float GetVolume()
    {
        return bgmSource.volume;
    }
}