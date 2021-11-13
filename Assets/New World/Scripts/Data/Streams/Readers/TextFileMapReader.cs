﻿using System;
using System.IO;
using UnityEngine;
using NewWorld.Data.Standard;
using System.Collections.Generic;
using NewWorld.Data.Readers.Abstractions;

namespace NewWorld.Data.Streams.Readers
{
    [Serializable]
    public class TextFileMapReader : IMapReader<ChunkData>
    {
        [SerializeField]
        protected string basePath;

        [SerializeField]
        protected List<TileData> Tiles;

        public event Action<Vector2, Vector2> OnChunkLoad;
        public event Action<Exception> OnChunkError;

        public TextFileMapReader(string basePath)
        {
            this.basePath = basePath;
        }

        public Func<string, ChunkData> ReadMethod { get ; set; }

        public ChunkData Read(string path)
        {
            var fullPath = Path.Combine(basePath,path);
            ChunkData chunk;

            if (this.ReadMethod != null)
            {
                chunk = this.ReadMethod(fullPath);
            }
            else
            {
                var fileLines = File.ReadAllLines(fullPath);

                chunk = new ChunkData
                {
                    Tiles = new TileData[fileLines[0].Length, fileLines.Length]
                };

                for (int y = 0; y < fileLines.Length; y++)
                {
                    for (int x = 0; x < fileLines[y].Length; x++)
                    {
                        if(int.TryParse(fileLines[y], out var value))
                        {
                            if(value > 0 && Tiles.Count > value)
                            {
                                chunk.Tiles[x, y] = Tiles[value];
                            }
                            else
                            {
                                throw new KeyNotFoundException($"There is no Tile saved in the index {fileLines[y]}");
                            }
                        }
                        else
                        {
                            throw new KeyNotFoundException($"There is no Tile saved in the index {fileLines[y]}");
                        }
                    }
                }

                this.OnChunkLoad?.Invoke(chunk.Position,chunk.Size);
            }

            return chunk;
        }

        public IEnumerable<ChunkData> Read(params string[] paths)
        {
            var length = paths.Length;
            var chunks = new ChunkData[length];

            for (int i = 0; i < length; i++)
            {
                try
                {
                    chunks[i] = this.Read(paths[i]);
                }
                catch (Exception e)
                {
                    this.OnChunkError?.Invoke(e);
                }
            }

            return chunks;
        }

        public IEnumerator<ChunkData> ReadGenerator(params string[] paths)
        {
            foreach (var path in paths)
            {
                ChunkData chunk;
                try
                {
                    chunk = this.Read(path);
                }
                catch (Exception e)
                {
                    this.OnChunkError?.Invoke(e);
                    chunk = null;
                }

                yield return chunk;
            }

            yield return null;
        }
    }
}
