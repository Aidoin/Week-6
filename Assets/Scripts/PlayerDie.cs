using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerDie : MonoBehaviour
{
    [SerializeField] private Image deadScreen;
    [SerializeField] private CheckpointController checkpointController;
    [SerializeField] private VitalSigns vitalSigns;

    [SerializeField] private MonoBehaviour[] disableOnDeath;

    public UnityEvent OnDie;

   public void DiePlayer()
   {
        StartCoroutine(Die());
   }


    private void SwitchingActivityComponents(bool active)
    {
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = active;
        }
    }


    private IEnumerator Die()
    {
        OnDie.Invoke();
        vitalSigns.SetInInvulnerability(3);
        SwitchingActivityComponents(false);

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            Color color = Color.HSVToRGB(0, 1, 1 - i);
            color.a = i; 
            deadScreen.color = color;
            yield return null;
        }

        checkpointController.BackToCheckpoint();
        vitalSigns.HealthRestore(2);

        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            Color color = Color.HSVToRGB(0, 0, i);
            color.a = 1 - i;
            deadScreen.color = color;
            yield return null;
        }

        SwitchingActivityComponents(true);
    }
}