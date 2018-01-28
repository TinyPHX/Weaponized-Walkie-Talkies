using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionsMethods;

/// <summary>
/// Handles special effects on Items and Characters.
/// Includes both sound effect and visual/particle effects.
/// </summary>

public class Effect : MonoBehaviour
{
    enum EffectState { NONE, PLAYING, STOPPED };

    [SerializeField] private float scale = 1;
    [SerializeField] private bool loop = false;
    [SerializeField] private int loopCount = 0;
    [SerializeField] private float loopTime = 0;
    public bool destroyAfterComplete = false;
    [SerializeField] private bool useColor = false;
    [SerializeField] private Color color = Color.black;

    [Header(" --- AUDIO EFFECTS --- ")]
    public AudioSource audioSource;
    public List<AudioClip> audioClips;
    public bool randomizeClip = false;
    public float pitchMin = 1;
    public float pitchMax = 1;
    public bool pitchChangeOverTime = false;
    public bool randomizePitch = false;
    public bool forceOneShot = false; //Play audio once, even when looping. 
    private float volumeScale = 1;
    
    [Header(" --- LIGHT EFFECTS --- ")]
    public new LightGlow lightGlow;
    
    [Header(" --- PARTICLE EFFECTS --- ")]
    public List<ParticleSystem> particleSystems;
    private float particleScale = 1;
    
    [Header(" --- PHYSICS EFFECTS --- ")]
    Rigidbody target;
    Collider targetArea;
    AnimationCurve dampCurve;
    
    [Header(" --- CAMERA EFFECTS --- ")]
    public CameraShake cameraShake;

    private float lifetime = 0;
    private EffectState effectState = EffectState.NONE;

    private List<ParticleSystemRenderer> particleRenderers = new List<ParticleSystemRenderer>();

    private void Awake()
    {
        Scale = scale;
        Loop = loop;
    }
    
    public Color EffectColor
    {
        get
        {
            return color;
        }

        set
        {
            color = value;

            foreach (ParticleSystemRenderer particleRenderer in particleRenderers)
            {
                particleRenderer.material.SetColor("_TintColor", color);
            }

            if (lightGlow != null)
            {
                lightGlow.Light.color = color;
            }
        }
    }

    public float Scale
    {
        get { return scale; }

        set
        {
            scale = value;
            
            float particleScaleChange = value / particleScale;
            particleScale = value;

            if (particleScaleChange != 1)
            {
                foreach (ParticleSystem particleSystem in particleSystems)
                {
                    ParticleSystem.MainModule particleMain = particleSystem.main;
                    particleMain.startSize = new ParticleSystem.MinMaxCurve(
                        particleMain.startSize.constantMin * particleScaleChange,
                        particleMain.startSize.constantMax * particleScaleChange
                        );
                }
            }

            float volumeScaleChange = value / volumeScale;
            volumeScale = value;

            if (volumeScaleChange != 1)
            {
                audioSource.volume *= volumeScaleChange;
            }
        }
    }

    public bool Loop
    {
        get { return loop; }

        set
        {
            loop = value;

            if (audioSource != null && !forceOneShot)
            {
                audioSource.loop = loop;
            }

            foreach (ParticleSystem particleSystem in particleSystems)
            {
                if (particleSystem != null)
                {
                    ParticleSystem.MainModule particleMain;
                    particleMain = particleSystem.main;
                    particleMain.loop = loop;
                }
            }
        }
    }

    public void Play()
    {
        effectState = EffectState.PLAYING;

        if (audioSource != null)
        {
            if (randomizePitch)
            {
                audioSource.pitch = Random.Range(pitchMin, pitchMax);
            }
            else if (pitchChangeOverTime)
            {
                float pitchSpread = (pitchMax - pitchMin);
                float smoothSpread = pitchSpread * 2 - pitchSpread;
                float pitchAdjust = 0;
                if (smoothSpread != 0)
                {
                    pitchAdjust = Time.time % smoothSpread;
                }
                audioSource.pitch = pitchMin + pitchAdjust;
            }

            if (randomizeClip)
            {
                audioSource.clip = audioClips.PickRandom();
            }
            else if (audioSource.clip == null && audioClips.Count > 0)
            {
                audioSource.clip = audioClips[0];
            }

            if (audioSource != null && audioSource.clip != null)
            {
                lifetime = audioSource.clip.length;
                audioSource.Play();
            }
        }

        if (lightGlow != null)
        {
            lifetime = Mathf.Max(lifetime, lightGlow.Duration);
            lightGlow.Play();
        }

        foreach (ParticleSystem particleSystem in particleSystems)
        {
            if (particleSystem != null)
            {
                ParticleSystemRenderer particleRenderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                if (particleRenderer != null)
                {
                    particleRenderers.Add(particleRenderer);
                }

                float particleSystemDuration = particleSystem.main.duration;
                lifetime = Mathf.Max(lifetime, particleSystemDuration);

                particleSystem.Play();
            }
        }

        if (cameraShake != null)
        {
            cameraShake.Shake();
        }

        if (color != Color.black)
        {
            if (useColor)
            {
                EffectColor = color;
            }
        }

        if (!loop)
        {
            if (destroyAfterComplete)
            {
                gameObject.BlowUp(lifetime);
            }
            else
            {
                StartCoroutine(PlayComplete(lifetime));
            }
        }
        else
        {
            if (loopCount > 0)
            {
                lifetime *= loopCount;
                StartCoroutine(PlayComplete(lifetime));
            }

            if (loopTime != 0)
            {
                lifetime = loopTime;
                StartCoroutine(PlayComplete(lifetime));
            }
        }
    }

    IEnumerator PlayComplete(float delay)
    {
        yield return new WaitForSeconds(delay);

        Stop();
    }

    public void Stop()
    {
        if (effectState == EffectState.PLAYING)
        {
            effectState = EffectState.STOPPED;

            if (audioSource != null)
            {
                audioSource.Stop();
            }

            if (lightGlow != null && !loop)
            {
                lightGlow.Stop();
            }

            foreach (ParticleSystem particleSystem in particleSystems)
            {
                if (particleSystem != null)
                {
                    particleSystem.Stop();
                }
            }

            //cameraShake.Stop(); //TODO: implement Stop for camera shake
        }
    }
}