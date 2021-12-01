using System;
using System.Collections.Generic;
using System.Text;

namespace Amazoom
{
    public static class Dictionary_Storage
    {
        private enum Column : int //Used in initialize warehouse and potentially useful for pathing going forward, as it converts are column to/from a number. 
        {
            A, B, C, D, E, F, G, H, I, J, K, L, M, N, O, P, Q, R, S, T, U, V, W, X, Y, Z, AA, AB, AC, AD, AE, AF, AG, AH, AI, AJ, AK, AL, AM, AN, AO, AP, AQ, AR, AS, AT, AU, AV, AW, AX, AY, AZ
        }
        public static Dictionary<int,string> Column_Dictionary = (Dictionary<int, string>)EnumHelper.ConvertToDictionary<Column>();
        public static Dictionary<string, int> Column_Dictionary_Reverse = EnumHelper.Reverse(Column_Dictionary);
    }

    //Creates a <int, string> Dictionary of an Enum.
    static class EnumHelper
    {
        public static IDictionary<int, string> ConvertToDictionary<T>() where T : struct
        {
            var dictionary = new Dictionary<int, string>();

            var values = Enum.GetValues(typeof(T));

            foreach (var value in values)
            {
                int key = (int)value;

                dictionary.Add(key, value.ToString());
            }

            return dictionary;
        }

        //Useful for reversing a Dictionary
        public static Dictionary<TValue, TKey> Reverse<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            var dictionary = new Dictionary<TValue, TKey>();
            foreach (var entry in source)
            {
                if (!dictionary.ContainsKey(entry.Value))
                    dictionary.Add(entry.Value, entry.Key);
            }
            return dictionary;
        }
    }


}
