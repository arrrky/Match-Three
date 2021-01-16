using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework.Constraints;
using UnityEngine;

public class MatchesChecker
{
   private readonly Field playingField = PlayingFieldController.Instance.PlayingField;
   
   private Dictionary<Vector2Int, TypeOfTile> tilesToDelete = new Dictionary<Vector2Int, TypeOfTile>();

   private const int MatchValue = 3;

   public Dictionary<Vector2Int, TypeOfTile> GetTilesToDelete()
   {
      ColumnsCheck();
      RowsCheck();
      return tilesToDelete;
   }
   
   private void ColumnsCheck()
   {
      //Field playingField = TransposePlayingFieldMatrix();
      Dictionary<Vector2Int, TypeOfTile> temp = new Dictionary<Vector2Int, TypeOfTile>();
      bool isStacked = false;

      for (int x = 0; x < playingField.Width ; x++)
      {
         for (int y = 0; y < playingField.Height; y++)
         {
            temp.Add(new Vector2Int(x,y), playingField.MatrixOfTiles[x,y].TypeOfTile);

            if (temp.Count >= 2)
            {
               if (temp.ElementAt(temp.Count - 2).Value != temp.ElementAt(temp.Count - 1).Value)
               {
                  if (isStacked)
                  {
                     AddTempDicToMainDic(temp);
                     temp.Clear(); //очищаем после переноса временного словаря в основной
                     isStacked = false;
                     continue;
                  }
                  temp.Clear();
                  temp.Add(new Vector2Int(x, y), playingField.MatrixOfTiles[x, y].TypeOfTile); //добавляем элемент заново, т.к. словарь очищен
               }
            }

            if (temp.Count >= MatchValue)
            {
               isStacked = true;
            }
         }
         temp.Clear(); // очищаем каждый новый ряд / столбец
      }
   }
   
   private void RowsCheck()
   {
      //Field playingField = TransposePlayingFieldMatrix();
      Dictionary<Vector2Int, TypeOfTile> temp = new Dictionary<Vector2Int, TypeOfTile>();
      bool isStacked = false;

      for (int y = 0; y < playingField.Height ; y++)
      {
         for (int x = 0; x < playingField.Width; x++)
         {
            temp.Add(new Vector2Int(x,y), playingField.MatrixOfTiles[x,y].TypeOfTile);

            if (temp.Count >= 2)
            {
               if (temp.ElementAt(temp.Count - 2).Value != temp.ElementAt(temp.Count - 1).Value)
               {
                  if (isStacked)
                  {
                     AddTempDicToMainDic(temp);
                     temp.Clear(); //очищаем после переноса временного словаря в основной
                     isStacked = false;
                     continue;
                  }
                  temp.Clear();
                  temp.Add(new Vector2Int(x, y), playingField.MatrixOfTiles[x, y].TypeOfTile); //добавляем элемент заново, т.к. словарь очищен
               }
            }

            if (temp.Count >= MatchValue)
            {
               isStacked = true;
            }
         }
         temp.Clear(); // очищаем каждый новый ряд / столбец
      }
   }

   private void AddTempDicToMainDic(Dictionary<Vector2Int, TypeOfTile> tempDictionary)
   {
      foreach (var item in tempDictionary)
      {
         if (!tilesToDelete.ContainsKey(item.Key))
         {
            tilesToDelete.Add(item.Key, item.Value);
         }
      }
      //Удаляем последний элемент в словаре, который был добавлен в Check цикле, т.к. он уже несовпадающий
      tilesToDelete.Remove(tilesToDelete.ElementAt(tilesToDelete.Count - 1).Key);
   }

   private Field TransposePlayingFieldMatrix()
   {
      Field temp = playingField;
      for (int x = 0; x < playingField.Width; x++)
      {
         for (int y = 0; y < playingField.Height; y++)
         {
            playingField.MatrixOfTiles[x, y].TilePrefab = playingField.MatrixOfTiles[y, x].TilePrefab;
            playingField.MatrixOfTiles[x, y].TypeOfTile = playingField.MatrixOfTiles[y, x].TypeOfTile;
         }
      }
      return temp;
   }

}
