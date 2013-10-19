using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tanklike.ScreenManagement
{
    /// <summary>
    /// Base class for screens containing menus of options
    /// </summary>
    abstract class MenuScreen : GameScreen
    {
        #region Fields

        List<MenuEntry> m_lMenuEntries = new List<MenuEntry>();
        protected int m_nSelectedEntryIndex = 0;

        #endregion

        #region Properties

        /// <summary>
        /// List of menu entries, so derived classes can add or remove them
        /// </summary>
        protected IList<MenuEntry> MenuEntries
        {
            get { return m_lMenuEntries; }
        }

        /// <summary>
        /// Currently selected menu entry
        /// </summary>
        protected MenuEntry SelectedMenuEntry
        {
            get
            {
                if ((m_nSelectedEntryIndex < 0) || (m_nSelectedEntryIndex >= m_lMenuEntries.Count))
                {
                    return null;
                }
                return m_lMenuEntries[m_nSelectedEntryIndex];
            }
        }

        #endregion

        #region Initialization

        /// <summary>
        /// Constructor.
        /// </summary>
        public MenuScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        #endregion

        #region HandleInput

        /// <summary>
        /// Responds to user input, changing the selected entry and accepting
        /// or cancelling the menu.
        /// </summary>
        public override void HandleInput()
        {
            int oldSelectedEntryIndex = m_nSelectedEntryIndex;

            // Move to the previous menu entry?
            if (InputManager.IsActionTriggered(InputManager.Action.PreviousEntry))
            {
                m_nSelectedEntryIndex--;
                if (m_nSelectedEntryIndex < 0)
                    m_nSelectedEntryIndex = m_lMenuEntries.Count - 1;
            }

            // Move to the next menu entry?
            if (InputManager.IsActionTriggered(InputManager.Action.NextEntry))
            {
                m_nSelectedEntryIndex++;
                if (m_nSelectedEntryIndex >= m_lMenuEntries.Count)
                    m_nSelectedEntryIndex = 0;
            }

            // Accept or cancel the menu?
            if (InputManager.IsActionTriggered(InputManager.Action.Ok))
            {
                AudioManager.PlayCue("Continue");
                OnSelectEntry(m_nSelectedEntryIndex);
            }
            else if (InputManager.IsActionTriggered(InputManager.Action.Back) ||
                InputManager.IsActionTriggered(InputManager.Action.ExitGame))
            {
                OnCancel();
            }
            else if (m_nSelectedEntryIndex != oldSelectedEntryIndex)
            {
                AudioManager.PlayCue("MenuMove");
            }
        }

        /// <summary>
        /// Handler for when the user has chosen a menu entry.
        /// </summary>
        protected virtual void OnSelectEntry(int entryIndex)
        {
            m_lMenuEntries[m_nSelectedEntryIndex].OnSelectEntry();
        }

        /// <summary>
        /// Handler for when the user has cancelled the menu.
        /// </summary>
        protected virtual void OnCancel()
        {
            ExitScreen();
        }


        /// <summary>
        /// Helper overload makes it easy to use OnCancel as a MenuEntry event handler.
        /// </summary>
        protected void OnCancel(object sender, EventArgs e)
        {
            OnCancel();
        }

        #endregion

        #region Update and Draw

        /// <summary>
        /// Updates the menu.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus, bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);

            // Update each nested MenuEntry object.
            for (int i = 0; i < m_lMenuEntries.Count; i++)
            {
                bool isSelected = IsActive && (i == m_nSelectedEntryIndex);

                m_lMenuEntries[i].Update(this, isSelected, gameTime);
            }
        }

        /// <summary>
        /// Draws the menu.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            spriteBatch.Begin();

            // Draw each menu entry in turn.
            for (int i = 0; i < m_lMenuEntries.Count; i++)
            {
                MenuEntry menuEntry = m_lMenuEntries[i];

                bool isSelected = IsActive && (i == m_nSelectedEntryIndex);

                menuEntry.Draw(this, isSelected, gameTime);
            }

            spriteBatch.End();
        }

        #endregion
    }
}
