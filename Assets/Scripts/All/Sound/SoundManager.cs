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
    public AudioSource audioSource;

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
    }

    public void PlayBGM(int num)
    {
        audioSource.clip = bgms[num];
        audioSource.Play();
    }

    public void Volume(float num)
    {
        audioSource.volume = num;
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
        for (int i = 0; i < effects.Length; i++)
        {
            if (effects[i].name == name)
            {                            
                EffectSound effect = objectPool.Get();                  
                effect.PlayES(effects[i]);                                         
                break;
            }
        }
    }

    public void PlayES(AudioClip audioClip)
    {
        EffectSound effect = objectPool.Get();
        effect.PlayES(audioClip);
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
