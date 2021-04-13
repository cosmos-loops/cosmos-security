﻿using System;
using System.Threading;
using Cosmos.Security.Verification.Core;
// ReSharper disable once CheckNamespace
namespace Cosmos.Security.Verification
{
    public partial class MurmurHash2Function
    {
        protected IHashValue ComputeHash32(ArraySegment<byte> data, CancellationToken cancellationToken)
        {
            var dataArray = data.Array;
            var dataOffset = data.Offset;
            var dataCount = data.Count;

            var endOffset = dataOffset + dataCount;
            var remainderCount = dataCount % 4;

            UInt32 hashValue = (UInt32) _config.Seed ^ (UInt32) dataCount;

            // Process 4-byte groups
            {
                var groupEndOffset = endOffset - remainderCount;

                for (var currentOffset = dataOffset; currentOffset < groupEndOffset; currentOffset += 4)
                {
                    UInt32 k = BitConverter.ToUInt32(dataArray, currentOffset);

                    k *= _mixConstant32;
                    k ^= k >> 24;
                    k *= _mixConstant32;

                    hashValue *= _mixConstant32;
                    hashValue ^= k;
                }
            }

            // Process remainder
            if (remainderCount > 0)
            {
                var remainderOffset = endOffset - remainderCount;

                switch (remainderCount)
                {
                    case 3:
                        hashValue ^= (UInt32) dataArray[remainderOffset + 2] << 16;
                        goto case 2;
                    case 2:
                        hashValue ^= (UInt32) dataArray[remainderOffset + 1] << 8;
                        goto case 1;
                    case 1:
                        hashValue ^= (UInt32) dataArray[remainderOffset];
                        break;
                }

                ;

                hashValue *= _mixConstant32;
            }


            hashValue ^= hashValue >> 13;
            hashValue *= _mixConstant32;
            hashValue ^= hashValue >> 15;

            return new HashValue(
                BitConverter.GetBytes(hashValue),
                32);
        }
    }
}