using ExtensionsMethods;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectController : MonoBehaviour {

    public enum TriggerType {
        ON_CREATE,
        LOOP,
        ON_DESTROY,
        ON_CREATE_AND_DESTROY,
        ON_COLISION_ENTER,
        ON_TRIGGER_ENTER, //TODO
        MANUAL }
    
    [SerializeField] private Effect effectPrefab;
    [SerializeField] private Transform parent;
    [SerializeField] private float scale = 1;
    [SerializeField] private TriggerType playType = TriggerType.ON_CREATE;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float loopLength; // recommended to set this to longer than the length of the effect's audio and it's particlesystem's set length

    private Effect effect;
    private float startTime;

    public void Start()
    {
        startTime = Time.time;

        if (playType == TriggerType.ON_CREATE || playType == TriggerType.ON_CREATE_AND_DESTROY)
        {
            Trigger();
        }
        else if (playType == TriggerType.LOOP)
        {
            Trigger();
            PlayLoopingEffect(0, this);
        }
    }

    public void OnDestroy()
    {
        //Dont play on destroy effects if the object was destroyed almost immediately. 
        if (Time.time - startTime > .1f)
        {
            if (playType == TriggerType.ON_DESTROY || playType == TriggerType.ON_CREATE_AND_DESTROY)
            {
                //Game game = Game.Instance;
                //Level level = Level.Instance;

                //if (game != null && !game.isQuitting && level != null && level.ActiveLevelState == Level.LevelState.PLAYING)
                //{
                //    Trigger();
                //}
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        //Rigidbody rigidbodyHit = collision.rigidbody;
        Collider colliderHit = collision.collider;
        
        if (colliderHit != null &&
            playType == TriggerType.ON_COLISION_ENTER && 
            layerMask.Contains(colliderHit.gameObject.layer))
        {
            Trigger();
        }
    }

    IEnumerator PlayLoopingEffect(float delay, EffectController effectController)
    {
        if (effectController != null)
        {
            yield return new WaitForSeconds(delay);

            effect.Play();
            StartCoroutine(PlayLoopingEffect(loopLength, effectController));
        }
    }

    public void Trigger()
    {
        if (effect == null)
        {
            if (effectPrefab != null)
            {
                if (parent != null)
                {
                    effect = Instantiate(effectPrefab, parent, false);
                    effect.transform.position = parent.position;
                }
                else
                {
                    effect = Instantiate(effectPrefab);
                    effect.transform.position = transform.position;
                }

                effect.Scale = scale;
            }
        }

        if (effect != null)
        {
            if (parent == null)
            {
                effect.transform.position = transform.position;
            }

            effect.Play();
        }
    }

    public void Stop()
    {
        effect.Stop();
    }
}
