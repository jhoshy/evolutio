using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EaseFunctions : MonoBehaviour
{

    public string tipo;
    public float incrementoX;
    public float incrementoY;
    public float incrementoZ;

    public float tempo = 1;
    public float delay = 0;

    public bool distancia = false;
    public bool escala = false;
    public bool rotacao = false;
    public bool alpha = false;

    // Use this for initialization
    void OnEnable()
    {
        Invoke("iniciaEaseFunction", delay);
    }

    public void iniciaEaseFunction()
    {
        if (distancia)
        {
            StartCoroutine(movimenta(transform, incrementoX, incrementoY, incrementoZ, tempo, tipo, false));
        }
        else if (escala)
        {
            StartCoroutine(modificaEscala(transform, incrementoX, incrementoY, incrementoZ, tempo, tipo));
        }
        else if (rotacao)
        {
            StartCoroutine(modificaRotacao(transform, incrementoX, incrementoY, incrementoZ, tempo, tipo));
        }
        else if (alpha) {
            StartCoroutine(modificaAlpha(GetComponent<Image>(), incrementoX, tempo, tipo));            
        }
    }
    
    public static float getTipo(string tipo, float porcentagem)
    {
        switch (tipo)
        {
            case "easeQuadIn":
                return EaseFunctions.easeQuadIn(porcentagem);
            case "easeQuintIn":
                return EaseFunctions.easeQuintIn(porcentagem);
            case "easeBounceOut":
                return EaseFunctions.easeBounceOut(porcentagem);
            case "linear":
                return EaseFunctions.linear(porcentagem);
            case "easeBackOut":
                return EaseFunctions.easeBackOut(porcentagem);
            case "easeBackIn":
                return EaseFunctions.easeBackIn(porcentagem);
        }

        return 0f;
    }

    private static Vector3 lerpPersonalizada(Vector3 posI, Vector3 posF, float porcentagem)
    {
        return posI + ((posF - posI) * porcentagem);
    }

    private static Quaternion lerpPersonalizadaQ(Vector3 posI, Vector3 posF, float porcentagem)
    {
        Vector3 resultado;

        resultado = posI + ((posF - posI) * porcentagem);

        return Quaternion.Euler(resultado);
    }

    public static IEnumerator movimenta(Transform transform, float distanciaX, float distanciaY, float distanciaZ, float tempo, string tipo, bool desativa)
    {
        float tempoInicial, tempoAtual, porcentagem = 0;

        Vector3 posInicial = transform.localPosition;
        Vector3 posFinal = transform.localPosition;
        posFinal.x = posFinal.x + distanciaX;
        posFinal.y = posFinal.y + distanciaY;
        posFinal.z = posFinal.z + distanciaZ;

        tempoInicial = Time.timeSinceLevelLoad;

        while ((Time.timeSinceLevelLoad - tempoInicial) <= tempo)
        {
            tempoAtual = Time.timeSinceLevelLoad;

            porcentagem = (tempoAtual - tempoInicial) / tempo;

            porcentagem = getTipo(tipo, porcentagem);

            transform.localPosition = lerpPersonalizada(posInicial, posFinal, porcentagem);

            yield return null;
        }

        //finaliza na proporçao correta
        transform.localPosition = posFinal;

        if (desativa)
        {
            transform.gameObject.SetActive(false);
        }
    }

    public static IEnumerator modificaEscala(Transform transform, float distanciaX, float distanciaY, float distanciaZ, float tempo, string tipo)
    {
        float tempoInicial, tempoAtual, porcentagem = 0;

        Vector3 posInicial = transform.localScale;
        Vector3 posFinal = transform.localScale;

        posFinal.x = posFinal.x + distanciaX;
        posFinal.y = posFinal.y + distanciaY;
        posFinal.z = posFinal.z + distanciaZ;

        tempoInicial = Time.timeSinceLevelLoad;

        while ((Time.timeSinceLevelLoad - tempoInicial) <= tempo)
        {
            tempoAtual = Time.timeSinceLevelLoad;

            porcentagem = (tempoAtual - tempoInicial) / tempo;
            porcentagem = getTipo(tipo, porcentagem);

            transform.localScale = lerpPersonalizada(posInicial, posFinal, porcentagem);

            yield return null;
        }

        //finaliza na proporçao correta
        transform.localScale = posFinal;
    }

    public static IEnumerator modificaAlpha(Image image, float valor, float tempo, string tipo) {
        float tempoInicial, tempoAtual, porcentagem = 0;

        float posInicial = 0;
        float posFinal = 1;

        //posFinal.a = posFinal.a + valor;

        tempoInicial = Time.timeSinceLevelLoad;
        //Aumenta
        while ((Time.timeSinceLevelLoad - tempoInicial) <= tempo) {
            tempoAtual = Time.timeSinceLevelLoad;

            porcentagem = (tempoAtual - tempoInicial) / tempo;
            porcentagem = getTipo(tipo, porcentagem);

            Color newColor = image.color;
            newColor.a = Mathf.Lerp(posInicial, posFinal, porcentagem);  //lerpPersonalizada(posInicial, posFinal, porcentagem);
            image.color = newColor;

            yield return null;
        }

        tempoInicial = Time.timeSinceLevelLoad;
        tempo *= 2;
        //Diminui
        while ((Time.timeSinceLevelLoad - tempoInicial) <= tempo) {
            tempoAtual = Time.timeSinceLevelLoad;

            porcentagem = (tempoAtual - tempoInicial) / tempo;
            porcentagem = getTipo(tipo, porcentagem);

            Color newColor = image.color;
            newColor.a = Mathf.Lerp(posFinal, posInicial, porcentagem);  //lerpPersonalizada(posInicial, posFinal, porcentagem);
            image.color = newColor;

            yield return null;
        }

        //finaliza na proporçao correta
        image.color = new Color(image.color.r, image.color.g, image.color.b,0);
        image.gameObject.SetActive(false);
    }

    public static IEnumerator modificaRotacao(Transform transform, float distanciaX, float distanciaY, float distanciaZ, float tempo, string tipo)
    {
        float tempoInicial, tempoAtual, porcentagem = 0;

        Vector3 posInicial = transform.localRotation.eulerAngles;
        Vector3 posFinal = transform.localRotation.eulerAngles;

        Quaternion final;

        posFinal.x = posFinal.x + distanciaX;
        posFinal.y = posFinal.y + distanciaY;
        posFinal.z = posFinal.z + distanciaZ;

        tempoInicial = Time.timeSinceLevelLoad;

        while ((Time.timeSinceLevelLoad - tempoInicial) <= tempo)
        {
            tempoAtual = Time.timeSinceLevelLoad;

            porcentagem = (tempoAtual - tempoInicial) / tempo;
            porcentagem = getTipo(tipo, porcentagem);

            transform.localRotation = lerpPersonalizadaQ(posInicial, posFinal, porcentagem);

            yield return null;
        }

        final = Quaternion.Euler(posFinal);

        //finaliza na proporçao correta
        transform.localRotation = final;
    }

    public static float linear(float porcentagem)
    {
        return porcentagem;
    }

    public static float easeBounceOut(float porcentagem)
    {
        if (porcentagem < 1f / 2.75f)
        {
            return 7.5625f * porcentagem * porcentagem;
        }
        else if (porcentagem < 2f / 2.75f)
        {
            float t = porcentagem - (1.5f / 2.75f);
            return 7.5625f * t * t + 0.75f;
        }
        else if (porcentagem < 2.5f / 2.75f)
        {
            float t = porcentagem - (2.25f / 2.75f);
            return 7.5625f * t * t + 0.9375f;
        }
        else
        {
            float t = porcentagem - (2.625f / 2.75f);
            return 7.5625f * t * t + 0.984375f;
        }
    }

    public static float easeBackOut(float porcentagem)
    {
        float t = porcentagem - 1f;
        return 1f + t * t * ((1.70158f + 1f) * t + 1.70158f);
    }

    public static float easeBackIn(float porcentagem)
    {
        return porcentagem * porcentagem * ((1.70158f + 1f) * porcentagem - 1.70158f);
    }

    public static float easeQuadIn(float porcentagem)
    {
        return porcentagem * porcentagem;
    }

    public static float easeQuartIn(float porcentagem)
    {
        return porcentagem * porcentagem * porcentagem * porcentagem;
    }

    public static float easeQuintIn(float porcentagem)
    {
        return porcentagem * porcentagem * porcentagem * porcentagem * porcentagem;
    }

    public static float easeExponetialIn(float porcentagem)
    {
        return (float)((porcentagem == 0) ? 0 : Mathf.Pow(2f, 10 * (porcentagem - 1)) - 0.001f);
    }
}

