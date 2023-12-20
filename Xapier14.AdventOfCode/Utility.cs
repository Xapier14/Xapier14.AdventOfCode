using System.Text;

namespace Xapier14.AdventOfCode
{
    /// <summary>
    /// A set of utility methods.
    /// </summary>
    public static class Utility
    {
        private static long GcfTwo(long a, long b)
        {
            while (b != 0)
            {
                long temp = b;
                b = a % b;
                a = temp;
            }
            return a;
        }
        
        /// <summary>
        /// Calculates the GCF of an array of long integers.
        /// </summary>
        /// <param name="values">The array of long integers.</param>
        /// <returns>The GCF result. Returns 0 if the array is empty.</returns>
        public static long Gcf(params long[] values)
        {
            if (values.Length <= 0)
                return 0;
            var gcf = values[0];
            for (var i = 1; i < values.Length; i++)
                gcf = GcfTwo(gcf, values[i]);
            return gcf;
        }

        /// <summary>
        /// Calculates the LCM of an array of long integers.
        /// </summary>
        /// <param name="values">The array of long integers.</param>
        /// <returns>The LCM result. Returns 0 if the array is empty.</returns>
        public static long Lcm(params long[] values)
        {
            if (values.Length <= 0)
                return 0;
            var lcm = values[0];
            for (var i = 1; i < values.Length; i++)
                lcm = lcm / GcfTwo(lcm, values[i]) * values[i];
            return lcm;
        }

        /// <summary>
        /// Hashes a pair of values.
        /// </summary>
        /// <param name="x">The first value.</param>
        /// <param name="y">The second value.</param>
        /// <returns>The hash of the pair of values.</returns>
        public static long Hash(long x, long y)
            => (x + y) * (x + y + 1) / 2 + y;

        /// <summary>
        /// Hashes a pair of values.
        /// </summary>
        /// <param name="pair">The pair of values.</param>
        /// <returns>The hash of the pair of values.</returns>
        public static long Hash((long X, long Y) pair)
            => Hash(pair.X, pair.Y);

        /// <summary>
        /// Hashes a char array.
        /// </summary>
        /// <param name="array">The array to hash.</param>
        /// <returns>The hash of the char array.</returns>
        public static long Hash(char[] array)
            => new string(array).GetHashCode();

        /// <summary>
        /// Hashes a jagged char array.
        /// </summary>
        /// <param name="array">The jagged array to hash.</param>
        /// <returns>The hash of the jagged char array.</returns>
        public static long Hash(char[][] array)
        {
            // good enough i guess
            var sb = new StringBuilder();
            foreach (var t in array)
                sb.Append(t);
            return sb.ToString().GetHashCode();
        }

        /// <summary>
        /// Hashes a string.
        /// </summary>
        /// <param name="str">The string to hash.</param>
        /// <returns>The hash of the string.</returns>
        public static long Hash(string str)
            => str.GetHashCode();

        /// <summary>
        /// Hashes a string array.
        /// </summary>
        /// <param name="array">The string array to hash.</param>
        /// <returns>The hash of the string array.</returns>
        public static long Hash(string[] array)
        {
            // good enough i guess
            var sb = new StringBuilder();
            foreach (var t in array)
                sb.Append(t);
            return sb.ToString().GetHashCode();
        }

        /// <summary>
        /// Hashes an object.
        /// </summary>
        /// <typeparam name="T">The type of the object to hash.</typeparam>
        /// <param name="obj">The object to hash.</param>
        /// <returns>The hash of the object.</returns>
        public static long Hash<T>(T obj)
        {
            if (typeof(T) == typeof(string))
                return Hash((string)Convert.ChangeType(obj, typeof(string))!);
            if (typeof(T) == typeof(string[]))
                return Hash((string[])Convert.ChangeType(obj, typeof(string[]))!);
            if (typeof(T) == typeof(char[]))
                return Hash((char[])Convert.ChangeType(obj, typeof(char[]))!);
            if (typeof(T) == typeof(char[][]))
                return Hash((char[][])Convert.ChangeType(obj, typeof(char[][]))!);
            if (typeof(T) == typeof((long, long)))
                return Hash(((long, long))Convert.ChangeType(obj, typeof((long, long)))!);
            if (typeof(T) == typeof(T[]))
                return Hash((T[])Convert.ChangeType(obj, typeof(T[]))!);
            return obj!.GetHashCode();
        }

        /// <summary>
        /// Hashes an object array.
        /// </summary>
        /// <typeparam name="T">The type of the object(s) to hash.</typeparam>
        /// <param name="array">The object array to hash.</param>
        /// <returns>The hash of the object array.</returns>
        public static long Hash<T>(T[] array)
        {
            // good enough i guess
            var sb = new StringBuilder();
            foreach (var t in array)
                sb.Append($"{Hash(t)}");
            return sb.ToString().GetHashCode();
        }

        /// <summary>
        /// Commutatively hashes a pair of values.
        /// </summary>
        /// <param name="a">The first value.</param>
        /// <param name="b">The second value.</param>
        /// <returns>The commutative hash of the pair of values.</returns>
        public static long CommutativeHash(long a, long b)
            => Hash(Math.Min(a, b), Math.Max(a, b));

        /// <summary>
        /// Commutatively hashes two pairs of values.
        /// </summary>
        /// <param name="pair1">The first pair of values.</param>
        /// <param name="pair2">The second pair of values.</param>
        /// <returns>The commutative hash of the pair of values.</returns>
        public static long CommutativeHash((long X, long Y) pair1, (long X, long Y) pair2)
        {
            var hash1 = Hash(pair1);
            var hash2 = Hash(pair2);
            return CommutativeHash(hash1, hash2);
        }

        /// <summary>
        /// <para>Asserts that the result of a function is equal to the expected value.</para>
        /// <para>Calls <c>Environment.Exit(-1)</c> if the function fails.</para>
        /// </summary>
        /// <typeparam name="T1">The function's input parameter type.</typeparam>
        /// <typeparam name="T2">The function's return type.</typeparam>
        /// <param name="function">The function to be called.</param>
        /// <param name="input">The input to be supplied with the function call.</param>
        /// <param name="expected">The expected result of the function call.</param>
        public static void Assert<T1, T2>(Func<T1[], T2> function, T1[] input, T2 expected)
        {
            var sample = function(input);
            if (!EqualityComparer<T2>.Default.Equals(sample, expected))
            {
                Console.WriteLine("[{0}] Test failed: {1} actual, {2} expected.", function.Method.Name, sample, expected);
                Environment.Exit(-1);
            }

            Console.WriteLine("[{0}] Test passed.", function.Method.Name);
        }
        
        /// <summary>
        /// <para>Asserts that the result of a function is equal to the expected value.</para>
        /// <para>Calls <c>Environment.Exit(-1)</c> if the function fails.</para>
        /// </summary>
        /// <typeparam name="T1">The function's input parameter type.</typeparam>
        /// <typeparam name="T2">The function's return type.</typeparam>
        /// <param name="function">The function to be called.</param>
        /// <param name="input">The input to be supplied with the function call.</param>
        /// <param name="expected">The expected result of the function call.</param>
        public static void Assert<T1, T2>(Func<T1, T2> function, T1 input, T2 expected)
        {
            var sample = function(input);
            if (!EqualityComparer<T2>.Default.Equals(sample, expected))
            {
                Console.WriteLine("[{0}] Test failed: {1} actual, {2} expected.", function.Method.Name, sample, expected);
                Environment.Exit(-1);
            }

            Console.WriteLine("[{0}] Test passed.", function.Method.Name);
        }
    }
}
