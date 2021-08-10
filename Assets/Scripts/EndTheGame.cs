using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTheGame : MonoBehaviour
{
    [SerializeField] private CameraMoove cameraMoove;
    [SerializeField] private HeavenlyCall heavenlyCall;
    [SerializeField] private Text endScreen;
    [SerializeField] private Text endTextText;
    [SerializeField] private Image whiteScreen;
    [SerializeField] private Light lightWorld;
    [SerializeField] private AudioSource audioEnd;
    [SerializeField] private AudioSource backgroundAudio;
    [SerializeField] private Transform PositionStart;
    [SerializeField] private Transform PositionUp;
    [SerializeField] private Transform PositionDown;
    [SerializeField] private RectTransform endTextRect;
    [SerializeField] private GameObject ground;
    [SerializeField] private GameObject effectGlow;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject[] objectsDisabling;
    [SerializeField] private MonoBehaviour[] componentsDisabling;

    private Hub hub;
    private bool isActived = false;


    private void Awake()
    {
        hub = FindObjectOfType<Hub>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (isActived) // Отключение многократного срабатывания
            return;

        if (other.attachedRigidbody && other.attachedRigidbody.gameObject == hub.Player) // Если вошедший игрок это плеер
        {
            isActived = true;

            // Отключение компонентов и объектов для Воспроизведения заставки
            for (int i = 0; i < componentsDisabling.Length; i++)
            {
                componentsDisabling[i].enabled = false;
            }
            for (int i = 0; i < objectsDisabling.Length; i++)
            {
                objectsDisabling[i].SetActive(false);
            }
            hub.Player.GetComponent<Rigidbody>().isKinematic = true;

            // Запуск эффекта перемещения
            heavenlyCall.transform.position = hub.Player.transform.position;
            heavenlyCall.gameObject.SetActive(true);
            heavenlyCall.StartParticle();

            audioEnd.Play(); // Запуск звука заставки
            StartCoroutine(WhiteSscreenBrightens()); // Следующий шаг
        }
    }
        
    
    private void StartMoove()
    {
        // Сброс положения игрока
        playerBody.transform.rotation = Quaternion.identity;
        playerBody.transform.localScale = Vector3.one;

        // Перенос игрока на локацию заставки
        hub.Player.transform.position = PositionStart.position;

        StartCoroutine(TextPlay()); // Запускк движения теккста
        StartCoroutine(MooveToUp()); // Запуск движения игрока
        cameraMoove.CameraMovementSpeed = 14; // Уменьшение инертности камеры для больших скоростей

    }


    // Заставка перехода игока на локацию концовки
    private IEnumerator WhiteSscreenBrightens()
    {
        float startAudioVolume = backgroundAudio.volume;
        Color newColor = new Color(1, 1, 1, 0);

        
        for (float i = 0; i < 1; i += Time.deltaTime / 5)
        {
            backgroundAudio.volume = startAudioVolume - i; // Затухание звука музыки

            // Побеление экранс
            newColor.a = i;
            whiteScreen.color = newColor;
            yield return null;
        }
        newColor.a = 1;
        whiteScreen.color = newColor;

        backgroundAudio.Stop(); // Отключение музыки
        effectGlow.SetActive(true); // Включение эффекта подъёма

        heavenlyCall.gameObject.SetActive(false); // Отключение эффекта перемещения в локацию концовки
        StartMoove(); // Запуск движения
        ground.SetActive(true);

        yield return new WaitForSeconds(2.6f);

        // Опрозрачнивание экрана
        for (float i = 1; i > 0; i -= Time.deltaTime / 2)
        {
            newColor.a = i;
            whiteScreen.color = newColor;
            yield return null;
        }

        newColor.a = 0;
        whiteScreen.color = newColor;
    }


    // Запуск текста
    private IEnumerator TextPlay()
    {
        endTextRect.gameObject.SetActive(true);

        // Движение текста в верх
        while (endTextRect.position.y < 4300)
        {
            endTextRect.anchoredPosition = new Vector2(endTextRect.anchoredPosition.x, Mathf.MoveTowards(endTextRect.anchoredPosition.y, 4300, 106 * Time.deltaTime));
            yield return null;
        }
        endTextRect.gameObject.SetActive(false);
    }


    // Движение вверх
    private IEnumerator MooveToUp()
    {
        float step = 1;

        // Движение плеера в верх
        while (true)
        {
            hub.Player.transform.position = Vector3.MoveTowards(hub.Player.transform.position, PositionUp.position, Time.deltaTime * step);

            // Когда плеер достигнет определённой высоты, скорость замедляется
            if (hub.Player.transform.position.y > 37)
            {
                step -= Time.deltaTime;
                if (step < 0)
                    break;
            }

            yield return null;
        }

        // Следующие шаги
        StartCoroutine(MooveToDown());
        StartCoroutine(ChangingTexColor());
        StartCoroutine(DisablingGlowEffect());
    }


    // Движение вниз
    private IEnumerator MooveToDown()
    {
        float step = 0;
        bool darkenScreen = false;
        bool changeColorOfLighting = false;

        while (true)
        {
            // Движение плеера вниз
            hub.Player.transform.position = Vector3.MoveTowards(hub.Player.transform.position, PositionDown.position, -Time.deltaTime * step);

            // Увелечение скорости
            if (step > -4)
            {
                step -= Time.deltaTime;
            }
            else
            {
                step -= Time.deltaTime * 4;
            }

            // Покраснение окружающего света на определённой глубине
            if (changeColorOfLighting == false && hub.Player.transform.position.y < -150)
            {
                changeColorOfLighting = true;
                StartCoroutine(RednessOfLight());
            }

            // Затемнение экрана в конце пути
            if (darkenScreen == false && hub.Player.transform.position.y < -1050)
            {
                darkenScreen = true;
                StartCoroutine(BlackSscreenBrightens());
            }

            // Остановка при достижении цели
            if (hub.Player.transform.position.y == PositionDown.position.y)
                break;

            yield return null;
        }
    }

    // Изменение цвета текста
    private IEnumerator ChangingTexColor()
    {
        Color newColor = new Color(1, 1, 1, 1);

        for (float i = 1; i > 0; i -= Time.deltaTime/15)
        {
            newColor.g = newColor.b = i;
            endTextText.color = newColor;
            yield return null;
        }
        newColor.g = newColor.b = 0;
        endTextText.color = newColor;
    }


    // Отключение эффекта подъёма
    private IEnumerator DisablingGlowEffect()
    {
        for (float i = 1; i > 0; i-= Time.deltaTime/10)
        {
            effectGlow.transform.localScale = Vector3.one * i;

            yield return null;
        }
        effectGlow.SetActive(false);
    }


    // Покраснение света
    private IEnumerator RednessOfLight()
    {
        Color newColor = lightWorld.color;

        for (float i = 0; i < 1; i += Time.deltaTime/30)
        {
            if (newColor.g - i >= 0)
            {
                newColor.g = newColor.g - i;
            }
            else 
            {
                newColor.g = 0;
            }

            if (newColor.b - i >= 0)
            {
                newColor.b = newColor.b - i;
            }
            else
            {
                newColor.b = 0;
            }

            lightWorld.color = newColor;
            yield return null;
        }
        newColor.g = 0;
        newColor.b = 0;
        lightWorld.color = newColor;
    }


    // Затемнение экрана
    private IEnumerator BlackSscreenBrightens()
    {
        Color newColor = new Color(0, 0, 0, 0);

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            newColor.a = i;
            whiteScreen.color = newColor;
            yield return null;
        }
        newColor.a = 1;
        whiteScreen.color = newColor;

        // Следующий шаг
        StartCoroutine(End());
    }


    // Проявление текста
    private IEnumerator End()
    {
        Color newColor = new Color(1, 1, 1, 0);

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            newColor.a = i;
            endScreen.color = newColor;
            yield return null;
        }
        newColor.a = 1;
        endScreen.color = newColor;
        ground.SetActive(false);
    }
}