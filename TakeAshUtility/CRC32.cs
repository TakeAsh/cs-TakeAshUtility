using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TakeAshUtility {

    /// <summary>
    /// Compute CRC32
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>[Cyclic redundancy check - Wikipedia](https://en.wikipedia.org/wiki/Cyclic_redundancy_check)</item>
    /// <item>[巡回冗長検査 - Wikipedia](https://ja.wikipedia.org/wiki/%E5%B7%A1%E5%9B%9E%E5%86%97%E9%95%B7%E6%A4%9C%E6%9F%BB)</item>
    /// <item>[HashAlgorithmの実装（CRC32，CRC16） - kadzusの日記](http://d.hatena.ne.jp/kadzus/20100523/1274625686)</item>
    /// </list>
    /// </remarks>
    public class CRC32 :
        HashAlgorithm {

        private const uint CRC32_MASK = 0xFFFFFFFF;
        private const uint ReversedRepresentations = 0xEDB88320; // Hamming code
        private const int TableSize = 256;
        private static readonly uint[] CRC32Table = new uint[TableSize];

        private uint _crcValue;

        static CRC32() {
            for (uint i = 0; i < TableSize; ++i) {
                uint c = i;
                for (var j = 0; j < 8; j++) {
                    c = ((c & 1) == 1) ?
                        (ReversedRepresentations ^ (c >> 1)) :
                        (c >> 1);
                }
                CRC32Table[i] = c;
            }
        }

        public CRC32() {
            base.HashSizeValue = 32;
            _crcValue = CRC32_MASK;
        }

        protected override void HashCore(byte[] array, int ibStart, int cbSize) {
            unchecked {
                while (--cbSize >= 0) {
                    _crcValue = CRC32Table[(_crcValue ^ array[ibStart++]) & 0xFF] ^ (_crcValue >> 8);
                }
            }
        }

        protected override byte[] HashFinal() {
            _crcValue ^= CRC32_MASK;
            return new byte[] {
                (byte)((_crcValue >> 24) & 0xFF), 
                (byte)((_crcValue >> 16) & 0xFF), 
                (byte)((_crcValue >>  8) & 0xFF), 
                (byte)( _crcValue        & 0xFF)
            };
        }

        public override void Initialize() {
            _crcValue = CRC32_MASK;
        }
    }
}
