using UnityEngine;
using System;

public class TimeController : MonoBehaviour
{
    public static TimeController Instance;

    public int Minutes { get; private set; }
    public int Seconds { get; private set; }

    public event Action OnTimeChanged;

    private float timer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f)
        {
            timer -= 1f;
            AddSecond();
        }
    }

    private void AddSecond()
    {
        Seconds++;

        if (Seconds >= 60)
        {
            Seconds = 0;
            Minutes++;
        }

        OnTimeChanged?.Invoke();
    }

    public void ResetTime()
    {
        Minutes = 0;
        Seconds = 0;
        OnTimeChanged?.Invoke();
    }

    public string GetTimeString()
    {
        return $"{Minutes:00}:{Seconds:00}";
    }
}
