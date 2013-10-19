using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanklike
{
    /// <summary>
    /// Handles input actions in the game.
    /// </summary>
    public static class InputManager
    {
        #region Actions

        /// <summary>
        /// Possible actions within the game
        /// </summary>
        public enum Action
        {
            MainMenu,
            Ok,
            Back,
            ExitGame,
            NextEntry,
            PreviousEntry,
            //This is not a real action. It's a trick to get the total number of actions from the enum
            TotalActionCount
        }

        /// <summary>
        /// Readable names for actions enum
        /// </summary>
        private static readonly string[] actionNames =
        {
            "MainMenu",
            "Ok",
            "Back",
            "ExitGame",
            "NextEntry",
            "PreviousEntry",
            "TotalActionCount"
        };

        /// <summary>
        /// Get name of the specified action
        /// </summary>
        /// <param name="action">Action to get the name for.</param>
        public static string GetActionName(Action action)
        {
            int index = (int)action;

            if ((index < 0) || (index > actionNames.Length))
            {
                throw new ArgumentException("Action not found");
            }

            return actionNames[index];
        }

        #endregion

        #region Keyboard Data

        private static KeyboardState m_ksCurrentKeyboardState;
        private static KeyboardState m_ksPreviousKeyboardState;

        /// <summary>
        /// The state of the keyboard as of the last update.
        /// </summary>
        public static KeyboardState CurrentKeyboardState
        {
            get { return m_ksCurrentKeyboardState; }
        }

        /// <summary>
        /// Check if a key is pressed.
        /// </summary>
        public static bool IsKeyPressed(Keys key)
        {
            return m_ksCurrentKeyboardState.IsKeyDown(key);
        }

        /// <summary>
        /// Check if a key was just pressed in the most recent update.
        /// </summary>
        public static bool IsKeyTriggered(Keys key)
        {
            return (m_ksCurrentKeyboardState.IsKeyDown(key)) &&
                (!m_ksPreviousKeyboardState.IsKeyDown(key));
        }

        #endregion

        #region Action Mapping

        /// <summary>
        /// A combination of gamepad and keyboard keys mapped to a particular action.
        /// Several keys can be mapped to a single action.
        /// TODO: Touch and Gamepad controls
        /// </summary>
        public class ActionMap
        {
            /// <summary>
            /// List of Keyboard controls to be mapped to a given action.
            /// </summary>
            public List<Keys> keyboardKeys = new List<Keys>();
        }

        /// <summary>
        /// The action mappings for the game.
        /// </summary>
        private static ActionMap[] m_aActionMaps;

        public static ActionMap[] ActionMaps
        {
            get { return m_aActionMaps; }
        }

        /// <summary>
        /// Reset the action maps to their default values.
        /// </summary>
        private static void ResetActionMaps()
        {
            m_aActionMaps = new ActionMap[(int)Action.TotalActionCount];

            m_aActionMaps[(int)Action.MainMenu] = new ActionMap();
            m_aActionMaps[(int)Action.MainMenu].keyboardKeys.Add(Keys.Tab);

            m_aActionMaps[(int)Action.Ok] = new ActionMap();
            m_aActionMaps[(int)Action.Ok].keyboardKeys.Add(Keys.Enter);

            m_aActionMaps[(int)Action.Back] = new ActionMap();
            m_aActionMaps[(int)Action.Back].keyboardKeys.Add(Keys.Escape);

            m_aActionMaps[(int)Action.ExitGame] = new ActionMap();
            m_aActionMaps[(int)Action.ExitGame].keyboardKeys.Add(Keys.Escape);

            m_aActionMaps[(int)Action.NextEntry] = new ActionMap();
            m_aActionMaps[(int)Action.NextEntry].keyboardKeys.Add(Keys.Down);
            m_aActionMaps[(int)Action.NextEntry].keyboardKeys.Add(Keys.Right);

            m_aActionMaps[(int)Action.PreviousEntry] = new ActionMap();
            m_aActionMaps[(int)Action.PreviousEntry].keyboardKeys.Add(Keys.Up);
            m_aActionMaps[(int)Action.PreviousEntry].keyboardKeys.Add(Keys.Left);

            //TODO: Gamepad and touch mapping
        }

        /// <summary>
        /// Check if an action has been pressed.
        /// </summary>
        public static bool IsActionPressed(Action action)
        {
            return IsActionMapPressed(m_aActionMaps[(int)action]);
        }

        /// <summary>
        /// Check if an action was just performed in the most recent update.
        /// </summary>
        public static bool IsActionTriggered(Action action)
        {
            return IsActionMapTriggered(m_aActionMaps[(int)action]);
        }

        /// <summary>
        /// Check if an action map has been pressed.
        /// </summary>
        private static bool IsActionMapPressed(ActionMap actionMap)
        {
            for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
            {
                if (IsKeyPressed(actionMap.keyboardKeys[i]))
                {
                    return true;
                }
            }
            //TODO: Add gamepad and touch support

            return false;
        }

        /// <summary>
        /// Check if an action map has been triggered this frame.
        /// </summary>
        private static bool IsActionMapTriggered(ActionMap actionMap)
        {
            for (int i = 0; i < actionMap.keyboardKeys.Count; i++)
            {
                if (IsKeyTriggered(actionMap.keyboardKeys[i]))
                {
                    return true;
                }
            }
            //TODO: Add gamepad and touch support

            return false;
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Initializes the default control keys for all actions.
        /// </summary>
        public static void Initialize()
        {
            ResetActionMaps();
        }

        #endregion

        #region Updating

        /// <summary>
        /// Updates the keyboard and gamepad control states.
        /// </summary>
        public static void Update()
        {
            // update the keyboard state
            m_ksPreviousKeyboardState = m_ksCurrentKeyboardState;
            m_ksCurrentKeyboardState = Keyboard.GetState();

            // update the gamepad state
            // TODO: Gamepad and touch
        }

        #endregion
    }
}
