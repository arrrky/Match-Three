using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
   [SerializeField] private AudioSource audioSource;
   [SerializeField] private List<AudioClip> listOfSounds = new List<AudioClip>();

   private Dictionary<string, AudioClip> dictionaryOfSounds = new Dictionary<string, AudioClip>();

   private void OnEnable()
   {
      TileController.TileClicked += PlayClickSound;
      TileController.WrongTileClicked += PlayWrongMoveSound;
      FieldManager.TilesSwapped += PlaySwapSound;
      GameController.GameOver += PlayGameOverSound;
   }

   private void Start()
   {
      DictionaryOfSoundsInit();
   }

   private void DictionaryOfSoundsInit()
   {
      foreach (AudioClip sound in listOfSounds)
      {
         dictionaryOfSounds.Add(sound.name, sound);
      }
   }

   private void PlayClickSound()
   {
      audioSource.clip = dictionaryOfSounds["click"];
      audioSource.Play();
   }

   private void PlaySwapSound()
   {
      audioSource.clip = dictionaryOfSounds["swap"];
      audioSource.Play();
   }

   private void PlayWrongMoveSound()
   {
      audioSource.clip = dictionaryOfSounds["wrong_move"];
      audioSource.Play();
   }

   private void PlayGameOverSound()
   {
      audioSource.clip = GameController.IsGameOver ? dictionaryOfSounds["game_over"] : dictionaryOfSounds["you_win"];
      audioSource.Play();
   }

   private void OnDisable()
   {
      TileController.TileClicked -= PlayClickSound;
      TileController.WrongTileClicked -= PlayWrongMoveSound;
      FieldManager.TilesSwapped -= PlaySwapSound;
      GameController.GameOver -= PlayGameOverSound;
   }
}
