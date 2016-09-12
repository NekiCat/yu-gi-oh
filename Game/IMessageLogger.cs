using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TigeR.YuGiOh.Core
{
    public interface IMessageLogger
    {
        /// <summary>
        /// Determine whether to print debug messages.
        /// </summary>
        bool Debugging { get; }

        /// <summary>
        /// Called for system messages. Cannot be called from lua code.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void System(string message, params object[] args);

        /// <summary>
        /// Called for system debug messages. Cannot be called from lua code. Only prints messages
        /// if the debug mode is enabled.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void SystemDebug(string message, params object[] args);

        /// <summary>
        /// Assert a condition. If the condition is false, print message as an error, else print nothing.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void Assert(bool condition, string message, params object[] args);

        /// <summary>
        /// Called for lua debug messages. Can be called from lua code. Only prints messages
        /// if the debug mode is enabled.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void Debug(string message, params object[] args);

        /// <summary>
        /// Prints a message. May be called from lua code.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void Print(string message, params object[] args);

        /// <summary>
        /// Prints an error message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        void PrintError(string message, params object[] args);

        /// <summary>
        /// Prints an exception. Format of the exception may vary. Usually redirects to <see cref="PrintError(string, object[])"/>.
        /// </summary>
        /// <param name="ex"></param>
        void PrintError(Exception ex);
    }
}
