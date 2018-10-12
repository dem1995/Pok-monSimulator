using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace PokémonAPI
{
    /// <summary>
    /// Miscellaneous methods to make classes more readable
    /// </summary>
    public static class HelperMethods
    {
        /// <summary>
        /// Generates a <see cref="List{T}"/> of objects from the provided entries.
        /// </summary>
        /// <typeparam name="T">The type of the generated list's entries.</typeparam>
        /// <param name="entries">The entries for the list.</param>
        /// <returns>A <see cref="List{T}"/> created from the provided entries.</returns>
        public static List<T> Listify<T>(params T[] entries)
        {
            return entries.ToList();
        }

        /// <summary>
        /// Gets an attribute on an enum field value
        /// </summary>
        /// <typeparam name="T">The type of the attribute you want to retrieve</typeparam>
        /// <param name="enumVal">The enum value</param>
        /// <returns>The attribute of type T that exists on the enum value</returns>
        internal static T GetAttributeOfType<T>(this Enum enumVal) where T : System.Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(T), false);
            return (attributes.Length > 0) ? (T)attributes[0] : null;
        }
    }


}
