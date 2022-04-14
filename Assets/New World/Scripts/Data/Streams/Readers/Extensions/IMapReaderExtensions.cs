﻿using System;
using NewWorld.Data.Streams.Readers.Abstractions;

namespace NewWorld.Data.Streams.Readers.Extensions
{
    public static class IMapReaderExtensions
    {
        public static IMapReader<T> UseCustomReader<T>(this IMapReader<T> reader,Func<string, T> method)
        {
            if (method != null)
            {
                reader.ReadMethod = method;
            }
            else
            {
                reader.ReadMethod = reader.Read;
            }

            return reader;
        }

        public static IMapReader<T> UseDefaultReader<T>(this IMapReader<T> reader)
        {
            reader.ReadMethod = reader.Read;

            return reader;
        }
    }
}
