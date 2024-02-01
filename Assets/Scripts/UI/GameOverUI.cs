using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
   [SerializeField] TextMeshProUGUI scoreText;
   [SerializeField] TextMeshProUGUI bestTimingText;
   
   private void OnEnable()
   {
      if (GameManager.Instance.Won)
      {
         if (GameManager.Instance.TimeLasted > PlayerPrefs.GetFloat("BestTiming", 0f))
         {
            PlayerPrefs.SetFloat("BestTiming", GameManager.Instance.TimeLasted);
         }

         bestTimingText.text = PlayerPrefs.GetFloat("BestTiming", 0f).ToString("0.00");
         scoreText.text =
            $"You won with timing of {GameManager.Instance.TimeLasted:0.00} seconds\n Hit enter to play again";
      }
      else
      {
         scoreText.text = $"You lasted {GameManager.Instance.TimeLasted:0.00} seconds\n Hit enter to play again";
      }
   }

   private void Update()
   {
      if (Input.GetAxis("Submit") > 0f)
      {
         GameManager.Instance.BeginGame();
      }
   }
}
