using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MouthMovement : MonoBehaviour {

    public AudioSource audioSource;
    public float updateStep = 0.1f;
    public int sampleDataLength = 1024;
    [SerializeField] bool playing;
    [SerializeField] AudioMixer mixer;

    private float currentUpdateTime = 0f;

    [SerializeField] private float clipLoudness;
    private float[] clipSampleData;

    [SerializeField] Sprite[] mouthSprites;
    [SerializeField] Sprite mouthClosed;
    [SerializeField] SpriteRenderer mouthSpriteRenderer;
    [SerializeField] int spriteToSet;
    [SerializeField] float loudnessMult;

    // Use this for initialization
    void Awake()
    {

        if (!audioSource)
        {
            Debug.LogError(GetType() + ".Awake: there was no audioSource set.");
        }
        clipSampleData = new float[sampleDataLength];

    }

    // Update is called once per frame
    void Update()
    {
        if (audioSource.isPlaying)
        {
            FindLoudness();
            SetSprite();
            playing = true;
        }
        else
        {
            SetSprite(mouthClosed);
            playing = false;
        }
    }

    void FindLoudness()
    {
        currentUpdateTime += Time.deltaTime;
        if (currentUpdateTime >= updateStep)
        {
            currentUpdateTime = 0f;
           // mixer.
            audioSource.clip.GetData(clipSampleData, audioSource.timeSamples); //I read 1024 samples, which is about 80 ms on a 44khz stereo clip, beginning at the current sample position of the clip.
            clipLoudness = 0f;
            foreach (var sample in clipSampleData)
            {
                clipLoudness += Mathf.Abs(sample);
            }
            clipLoudness /= sampleDataLength; //clipLoudness is what you are looking for
            clipLoudness *= loudnessMult;
            
        }
    }

    public void SetSprite()
    {

         spriteToSet = Mathf.RoundToInt(clipLoudness);
        if (mouthSprites.Length >= spriteToSet)
            mouthSpriteRenderer.sprite = mouthSprites[spriteToSet];
    }

    public void SetSprite(Sprite sprite)
    {
        mouthSpriteRenderer.sprite = sprite;
    }
}
