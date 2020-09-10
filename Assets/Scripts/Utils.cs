using System.Collections.Generic;
using UnityEngine;

namespace Tactics
{
  public static class Utils
  {
    public static Vector3 GetMouseWorldPosition(Camera worldCamera)
    {
      Vector3 worldPosition = worldCamera.ScreenToWorldPoint(Input.mousePosition);
      return worldPosition;
    }

    //Shuffle Array
    public static void Shuffle<T>(this T[] array)
    {
      int n = array.Length;
      while (n > 1)
      {
        int k = Random.Range(0, n--);
        T temp = array[n];
        array[n] = array[k];
        array[k] = temp;
      }
    }

    //Shuffle List
    public static void Shuffle<T>(this IList<T> list)
    {
      int n = list.Count;
      while (n > 1)
      {
        int k = Random.Range(0, n--);
        T temp = list[n];
        list[n] = list[k];
        list[k] = temp;
      }
    }
  }
}