using System.Collections.Generic;
using System;
using System.Linq;

namespace MFDSettingsManager.Extensions
{

    /// <summary>
    /// Processes arguments
    /// </summary>
    public static class ArgsProcess
    {

        /// <summary>
        /// Gets arguments for a List of strings
        /// </summary>
        /// <param name="args"></param>
        /// <param name="argumentKey"></param>
        /// <param name="defaultValue"></param>
        /// <param name="isSwitch"></param>
        /// <returns></returns>
        public static T GetSafeArgumentFrom<T>(this IList<string> args, string argumentKey, T defaultValue = default(T), bool isSwitch = false) 
            where T : IConvertible
        {
            if((args?.Count ?? 0) > 0)
            {
                var argsArray = args.ToArray();
                var arg = argsArray.GetSafeArgumentFrom(argumentKey, defaultValue, isSwitch);
                return (T)Convert.ChangeType(arg, typeof(T));
            }
            return defaultValue;
        }

        /// <summary>
        /// Extension method for getting the string arguments from command lines
        /// </summary>
        /// <param name="args"></param>
        /// <param name="argumentKey"></param>
        /// <param name="defaultValue"></param>
        /// <param name="isSwitch"></param>
        /// <returns></returns>
        public static string GetSafeArgumentFrom(this string[] args, string argumentKey, string defaultValue = null, bool isSwitch = false)
        {
            if (args?.Any(arg => arg.Equals(argumentKey, StringComparison.InvariantCultureIgnoreCase)) ?? false)
            {
                if (!isSwitch)
                {
                    var argumentValueIndex = args.Select((s, i) => new { i, s })
                        .Where(t => t.s.Equals(argumentKey, StringComparison.InvariantCultureIgnoreCase))
                        .Select(t => t.i)
                        .ToList().FirstOrDefault() + 1;
                    if (args.Length > argumentValueIndex)
                    {
                        return args[argumentValueIndex];
                    }
                }
                else
                {
                    var switchIndex = args.Select((s, i) => new { i, s })
                        .Where(t => t.s.Equals(argumentKey, StringComparison.InvariantCultureIgnoreCase))
                        ?.Select(t => t.i)
                        ?.ToList().FirstOrDefault();

                    return switchIndex.HasValue ? "true" : string.IsNullOrEmpty(defaultValue) ? "false" : "true";
                }
            }
            return isSwitch ? "false" : defaultValue;
        }
    }
}
