using Common.Classes.DB;

namespace Common.Classes.Session
{
    /// <summary>
    /// Represents session data for transferring information between windows.
    /// </summary>
    public class SessionData
    {
        #region Properties

        /// <summary>
        /// Gets the current user.
        /// </summary>
        public Users? CurrentUser
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the shared data for window operations.
        /// </summary>
        public object? Data
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the mode of the window (e.g., Create/Read/Update/Delete).
        /// </summary>
        public WindowMode Mode
        {
            get; set;
        }

        #endregion
    }

    /// <summary>
    /// Specifies the mode of a window for CRUD operations.
    /// </summary>
    public enum WindowMode
    {
        /// <summary>
        /// Create mode.
        /// </summary>
        Create,

        /// <summary>
        /// Read mode.
        /// </summary>
        Read,

        /// <summary>
        /// Update mode.
        /// </summary>
        Update,

        /// <summary>
        /// Delete mode.
        /// </summary>
        Delete
    }
}
