namespace Xapier14.AdventOfCode
{
    /// <summary>
    /// A set of utility methods.
    /// </summary>
    public class Utility
    {
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
