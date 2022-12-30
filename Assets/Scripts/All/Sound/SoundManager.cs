using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField] GameObject soundPrefab;
    [SerializeField] Transform soundStorage;
    [SerializeField] AudioClip[] effects;
    [SerializeField] AudioClip[] bgms;
    [SerializeField] int poolCount = 10;

    [HideInInspector] public AudioSource audioSource;
    float wholeSoundVolume;
    float bgmSoundVolume;
    float esSoundVolume;

    bool muteBgm;
    bool muteSfx;

    private IObjectPool<EffectSound> objectPool;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
        objectPool = new ObjectPool<EffectSound>(CreatePool, OnGetPool, OnReleasePool, OnDestroyPool, maxSize: poolCount);
    } 
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        wholeSoundVolume = 1;
        bgmSoundVolume = 1;
        esSoundVolume = 1;
        muteBgm = false;
        muteSfx = false;
    }

    private void Update()
    {
        audioSource.volume = wholeSoundVolume * bgmSoundVolume;
        audioSource.mute = muteBgm;
    }

    public void PlayBGM(int num)
    {
        audioSource.clip = bgms[num];
        audioSource.Play();
    }

    public void WholeVolume(float num)
    {
        wholeSoundVolume = num;
    }

    public void WholeVolumeOnOff(bool isMute)
    {
        muteBgm = isMute;
        muteSfx = isMute;
    }

    public void BgmVolume(float num)
    {
        bgmSoundVolume = num;
    }

    public void BgmOnOff(bool isMute)
    {
        muteBgm = isMute;
    }

    public void EsVolume(float num)
    {
        esSoundVolume = num ;
    }

    public void SfxOnOff(bool isMute)
    {
        muteSfx = isMute;
    }

    public void StopBGM()
    {
        audioSource.Stop();
    }

    public void PauseBGM()
    {
        audioSource.Pause();
    }

    public void PlayES(string name)
    {
        if (!muteSfx)
        {
            for (int i = 0; i < effects.Length; i++)
            {
                if (effects[i].name == name)
                {
                    EffectSound effect = objectPool.Get();
                    effect.PlayES(effects[i]);
                    effect.source.volume = esSoundVolume * wholeSoundVolume;
                    break;
                }
            }
        }
    }

    public void PlayES(AudioClip audioClip)
    {
        if (!muteSfx)
        {
            EffectSound effect = objectPool.Get();
            effect.PlayES(audioClip);
            effect.source.volume = esSoundVolume * wholeSoundVolume;
        }
    }

    private EffectSound CreatePool()
    {
        EffectSound pool = Instantiate(soundPrefab).GetComponent<EffectSound>();
        pool.SetManagedPool(objectPool);
        pool.transform.SetParent(soundStorage);

        return pool;
    }

    private void OnGetPool(EffectSound pool)
    {
        pool.gameObject.SetActive(true);
    }

    private void OnReleasePool(EffectSound pool)
    {
        pool.gameObject.SetActive(false);
    }

    private void OnDestroyPool(EffectSound pool)
    {
        Destroy(pool.gameObject);
    }
}
