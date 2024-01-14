using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayStats : MonoBehaviour
{
   [SerializeField] private CharacterStats characterStats;
   [SerializeField] private TMP_Text moveSpeed, throwForce, accuracy, head, drinkSpeed;

   private void Awake()
   {
      moveSpeed.text = "szybkosc: " + characterStats.moveSpeed;
      throwForce.text = "moc: " + characterStats.throwForce;
      accuracy.text = "celnosc: " + characterStats.accuracy;
      head.text = "glowa: " + characterStats.head;
      drinkSpeed.text = "zerowanie: " + characterStats.drinkSpeed;
   }
}
