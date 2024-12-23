using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using DG.Tweening;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements.Experimental;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem hitParticleSystem;
    public ObjectPool<ParticleSystem> hitParticlepool;
    [SerializeField] ParticleSystem HealParticleSystem;
    public ObjectPool<ParticleSystem> HealParticlepool;
    [SerializeField] ParticleSystem BuffParticleSystem;
    public ObjectPool<ParticleSystem> BuffParticlepool;

    [SerializeField] TMP_Text textParticlePrefab;
    ObjectPool<TMP_Text> tmpParticleOP;
    void Awake()
    {
        tmpParticleOP = new ObjectPool<TMP_Text>(
            () =>
            {
                TMP_Text value = GameObject.Instantiate(textParticlePrefab, transform);
                value.GetComponent<Renderer>().sortingLayerName = "Particle";
                return value;
            },
            (value) =>
            {
                value.gameObject.SetActive(true);
                Color c = value.color;
                c.a = 1.0f;
                value.color = c;
                value.DOFade(0, 1.0f);
                value.transform.DOMoveY(1, 1.0f).SetRelative().OnComplete(() => { tmpParticleOP.Release(value); });
            },
            (value) =>
            {
                value.gameObject.SetActive(false);
            },
            (value) =>
            {
                Destroy(gameObject);
            }
        );
        hitParticlepool = GetParticlePool(1f, hitParticleSystem, hitParticlepool, particleSize: 3);
        HealParticlepool = GetParticlePool(1f, HealParticleSystem, HealParticlepool, particleSize: 3);
        BuffParticlepool = GetParticlePool(1f, BuffParticleSystem, BuffParticlepool, particleSize: 3);
    }
    public void PrintTMPParticle(float x, float y, string value, Color c)
    {
        TMP_Text text = tmpParticleOP.Get();
        text.color = c;
        text.text = value;
        text.transform.position = new UnityEngine.Vector3(x, y, 0);
    }
    private IEnumerator InvokeWithDelay(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
    // Update is called once per frame


    ObjectPool<ParticleSystem> GetParticlePool(float duration, ParticleSystem prefab, ObjectPool<ParticleSystem> pool, float particleRot = 0, float particleSize = 1)
    {
        pool = new ObjectPool<ParticleSystem>(() =>
                        {
                            ParticleSystem ps = GameObject.Instantiate(prefab);
                            ps.Stop();
                            ps.Clear();

                            var psMain = ps.main;
                            ps.transform.localScale = new UnityEngine.Vector3(particleSize, particleSize, particleSize);
                            psMain.startRotation = particleRot;
                            psMain.playOnAwake = true;
                            psMain.loop = false;
                            psMain.duration = duration;
                            ps.GetComponent<Renderer>().sortingLayerName = "Particle";

                            return ps;
                        }
                        , (value) =>
                        {

                            value.gameObject.SetActive(true);
                            value.Emit(1);
                            StartCoroutine(InvokeWithDelay(duration, () => pool.Release(value)));
                        }
                        , (value) =>
                        {
                            value.gameObject.SetActive(false);
                        }
                        , (value) =>
                        {
                            GameObject.Destroy(value);
                        }
                        , true, 10);
        return pool;
    }
}
