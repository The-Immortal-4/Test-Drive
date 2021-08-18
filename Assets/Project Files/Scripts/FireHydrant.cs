using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireHydrant : MonoBehaviour
{
    [SerializeField] private GameObject ParticleEffect;
    bool activated = false;
    bool fading = false;
    private Renderer model;
    public float fadeSpeed = 3f;

    private void Awake()
    {
        model = GetComponent<Renderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SpawnParticleEffect();
        }
    }

    private void SpawnParticleEffect()
    {
        if (!activated)
        {
            activated = true;
            GameObject par = Instantiate(ParticleEffect, transform.position, ParticleEffect.transform.rotation);
            Destroy(par, 13f);
            fading = true;
            StartCoroutine(FadeModel());
        }
    }

    IEnumerator FadeModel()
    {
        yield return new WaitForSeconds(5f);
        while (fading)
        {
            yield return new WaitForEndOfFrame();
            if (model.material.color.a >= 0)
            {
                Color col = model.material.color;
                col.a -= fadeSpeed * Time.deltaTime;
                model.material.color = col;
            }
            else
            {
                fading = false;
            }
        }
    }
}
