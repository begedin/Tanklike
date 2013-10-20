using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tanklike
{
    /// <summary>
    /// Component that manages audio playback via cues
    /// </summary>
    public class AudioManager : GameComponent
    {
        #region Singleton

        /// <summary>
        /// The singleton for this type.
        /// </summary>
        private static AudioManager singleton = null;

        #endregion

        #region Audio Data

        private Dictionary<string, SoundEffect> m_Sounds;
        private Dictionary<string, Song> m_Songs;
        private Stack<string> m_SongNameStack;
        private Song m_CurrentSong;

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Constructs the manager for audio playback of all cues.
        /// </summary>
        /// <param name="game">The game that this component will be attached to.</param>
        private AudioManager(Game game) : base(game)
        {
            try
            {
                m_Songs = new Dictionary<string,Song>();
                m_Sounds = new Dictionary<string,SoundEffect>();
                m_SongNameStack = new Stack<string>();


            }
            catch (NoAudioHardwareException)
            {
                // silently fall back to silence
                m_Songs = null;
                m_Sounds = null;
                m_SongNameStack = null;
            }

        }

        /// <summary>
        /// Initialize the static AudioManager functionality.
        /// </summary>
        /// <param name="game">The game that this component will be attached to.</param>
        public static void Initialize(Game game)
        {
            singleton = new AudioManager(game);
            if (game != null)
            {
                game.Components.Add(singleton);
            }
        }

        public static void LoadContent(ContentManager content)
        {
            singleton.m_Songs.Add("MainTheme", content.Load<Song>("Music/song1"));

            singleton.m_Sounds.Add("MenuMove", content.Load<SoundEffect>("Sounds/boom1"));
            singleton.m_Sounds.Add("Continue", content.Load<SoundEffect>("Sounds/boom2"));
        }

        #endregion

        #region Sound effects

        /// <summary>
        /// Plays a cue by name.
        /// </summary>
        /// <param name="cueName">The name of the cue to play.</param>
        public static void PlayCue(string cueName)
        {
            if ((singleton != null) && (singleton.m_Sounds != null) && (singleton.m_Sounds.Count > 0))
            {
                if (singleton.m_Sounds.ContainsKey(cueName))
                {
                    singleton.m_Sounds[cueName].Play();
                }
            }
        }

        #endregion

        #region Music

        /// <summary>
        /// Plays the desired music, clearing the stack of music cues.
        /// </summary>
        /// <param name="cueName">The name of the music cue to play.</param>
        public static void PlayMusic(string cueName)
        {
            singleton.m_SongNameStack.Clear();
            PushMusic(cueName);
        }

        /// <summary>
        /// Plays the music for this game, adding it to the music stack.
        /// </summary>
        /// <param name="cueName">The name of the music cue to play.</param>
        public static void PushMusic(string cueName)
        {
            // start the new music cue
            if ((singleton != null) && (singleton.m_Songs != null) && (singleton.m_Songs.Count > 0))
            {

                singleton.m_SongNameStack.Push(cueName);
                if ((singleton.m_CurrentSong == null) ||
                    (singleton.m_CurrentSong.Name != cueName))
                {
                    if (singleton.m_CurrentSong != null)
                    {
                        MediaPlayer.Stop();
                        singleton.m_CurrentSong = null;
                    }

                    if (singleton.m_Songs.ContainsKey(cueName))
                    {
                        singleton.m_CurrentSong = singleton.m_Songs[cueName];
                        MediaPlayer.Play(singleton.m_CurrentSong);
                        MediaPlayer.IsRepeating = true;
                    }
                }
            }
        }


        /// <summary>
        /// Stops the current music and plays the previous music on the stack.
        /// </summary>
        public static void PopMusic()
        {
            // start the new music cue
            if ((singleton != null) && (singleton.m_Songs != null))
            {
                string cueName = null;
                if (singleton.m_SongNameStack.Count > 0)
                {
                    singleton.m_SongNameStack.Pop();
                    if (singleton.m_SongNameStack.Count > 0)
                    {
                        cueName = singleton.m_SongNameStack.Peek();
                    }
                }
                if ((singleton.m_SongNameStack == null) ||
                    (singleton.m_CurrentSong.Name != cueName))
                {
                    if (singleton.m_SongNameStack != null)
                    {
                        MediaPlayer.Stop();
                        singleton.m_SongNameStack = null;
                    }
                    if (!String.IsNullOrEmpty(cueName))
                    {
                        if (singleton.m_Songs.ContainsKey(cueName))
                        {
                            singleton.m_CurrentSong = singleton.m_Songs[cueName];
                            MediaPlayer.Play(singleton.m_CurrentSong);
                            MediaPlayer.IsRepeating = true;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Stop music playback, clearing the cue.
        /// </summary>
        public static void StopMusic()
        {
            if (singleton != null)
            {
                if (singleton.m_CurrentSong != null)
                {
                    MediaPlayer.Stop();
                    singleton.m_CurrentSong = null;
                }
            }
        }

        #endregion

        #region Updating Methods

        /// <summary>
        /// Update the audio manager, particularly the engine.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        #endregion
        
        #region Instance Disposal Methods
        
        /// <summary>
        /// Clean up the component when it is disposing.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    StopMusic();
                    if (m_Sounds != null)
                    {
                        //soundBank.Dispose();
                        m_Sounds = null;
                    }
                    if (m_Songs != null)
                    {
                        //waveBank.Dispose();
                        m_Songs = null;
                    }
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        #endregion
    }
}
