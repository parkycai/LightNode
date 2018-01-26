using LightNode2.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreSampleApplication.Api
{
    /// <summary>
    /// Array Test Apis
    /// </summary>
    public class ArrayApi : LightNodeContract
    {
        /// <summary>
        /// Sample Array
        /// </summary>
        /// <returns></returns>
        public string[] Sample()
        {
            return new[] { "a", "b", "c" };
        }

        /// <summary>
        /// Sample String Array
        /// </summary>
        /// <param name="xs"></param>
        /// <returns></returns>
        public string[] ArraySendTestGetPost(string[] xs)
        {
            return xs;
        }

        /// <summary>
        /// Sample int Array with Get
        /// </summary>
        /// <param name="xs"></param>
        /// <returns></returns>
        [Get]
        public int[] ArraySendTestGet(int[] xs)
        {
            return xs;
        }

        /// <summary>
        /// Sample int Array with Post
        /// </summary>
        /// <param name="xs"></param>
        /// <returns></returns>
        [Post]
        public int[] ArraySendTestPost(int[] xs)
        {
            return xs;
        }
    }
}
