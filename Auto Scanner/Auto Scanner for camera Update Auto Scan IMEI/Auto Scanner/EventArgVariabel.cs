using System;

namespace Auto_Scanner
{
    /// <summary>
    /// Defines the signature for the method that the ValueUpdated handler need
    /// to support.
    /// </summary>
    public delegate void ValueUpdatedEventHandler(object sender, ValueUpdatedEventArgs e);


    /// <summary>
    /// Holds the event arguments for the ValueUpdated event.
    /// </summary>
    public class ValueUpdatedEventArgs : System.EventArgs
    {
        /// <summary>
        /// Used to store the new value
        /// </summary>d
        private string[] newValue;

        /// <summary>
        /// Create a new instance of the ValueUpdatedEventArgs class.
        /// </summary>
        /// <param name="newValue">represents the change to the value</param>
        public ValueUpdatedEventArgs(string[] newValue)
        {
            this.newValue = newValue;
        }

        /// <summary>
        /// Gets the updated value
        /// </summary>
        public string[] NewValue
        {
            get
            {
                return this.newValue;
            }
        }
    }
}
