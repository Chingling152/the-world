﻿using System;

namespace NewWorld.Data.Streams.Writers.Abstractions
{
    public interface IMapWriter<T>
    {
        [Obsolete]
        Func<string,T, T> WriteMethod { get; set; }

        T Write(string path, T data);
    }
}
