using System;
using System.Text;

namespace Gaein.Text
{
    public class HaoEncodeing : Encoding
    {
        private const ushort _numHao = '昊'; //0
        private const ushort _numShen = '神'; //1

        private const char _charHao = '昊';
        private const char _charShen = '神';

        private const int _bitHao = 0;
        private const int _bitShen = 1;

        public override int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars)
        {
            var index = 0;
            var count = 0;
            while (index < bytes.Length)
            {
                var charsOnBytes = UTF8.GetChars(bytes.Slice(count, count + 24).ToArray());
                int intOfChar = 0;

                for (var i = 0; i < 8; i++)
                {
                    var bit = charsOnBytes[i] == _charHao ? _bitHao : _bitShen;
                    intOfChar += (bit << i);
                }

                var byteOfChar = (byte)intOfChar;

                chars[count] = UTF8.GetChars(new byte[] { byteOfChar })[0];

                index++;
                count += 3;
            }

            return index;
        }

        public override int GetByteCount(char[] chars, int index, int count)
        {
            if (chars is null) throw new ArgumentNullException(nameof(chars));
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException(index < 0 ? nameof(index) : nameof(count));
            if (chars.Length - index < count)
                throw new ArgumentOutOfRangeException(nameof(chars));

            if (chars.Length == 0) return 0;

            return UTF8.GetByteCount(chars, index, count) * 24;
        }


        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            throw new NotImplementedException();
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            if (bytes is null) throw new ArgumentNullException(nameof(chars));
            if (index < 0 || count < 0)
                throw new ArgumentOutOfRangeException(index < 0 ? nameof(index) : nameof(count));
            if (bytes.Length - index < count)
                throw new ArgumentOutOfRangeException(nameof(chars));

            if (bytes.Length == 0) return 0;
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            throw new NotImplementedException();
        }

        public override int GetMaxByteCount(int charCount)
        {
            throw new NotImplementedException();
        }

        public override int GetMaxCharCount(int byteCount)
        {
            throw new NotImplementedException();
        }
    }
}
