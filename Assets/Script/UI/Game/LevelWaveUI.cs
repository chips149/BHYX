using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelWaveUI : MonoBehaviour
{
   public static LevelWaveUI instance;
   public Text levelText;
   public Slider waveSlider;

   private void Awake()
   {
      instance = this;
   }

   public void RefreshUI(int currentLevel,int currentWave, int totalWaves)
   {
      levelText.text = $"第{currentLevel}关";

      waveSlider.maxValue = totalWaves;
      waveSlider.value = currentWave;
      Debug.Log($"第{currentLevel}关" );
   }
}
