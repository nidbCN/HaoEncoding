using System;
using System.Text;
using HaoEncodingLib.Extensions;

namespace HaoEncodingLib.Text
{
    public class HaoEncoding : Encoding
    {
        public static Encoding HaoCode => new HaoEncoding();

        private const char _charHao = '伞';
        private const char _charShen = '菌';

        private const int _bitHao = 0;
        private const int _bitShen = 1;

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

        public byte[] GetBytes(ReadOnlySpan<char> chars)
        {
            if (chars.Length == 0)
                return new byte[0];

            var byteArrayInUtf8 = UTF8.GetBytes(chars.ToArray());
            var charArrayInHaoCode = new char[byteArrayInUtf8.Length * 8];

            var count = 0;
            foreach (var byteInUtf8 in byteArrayInUtf8)
            {
                for (int i = 0; i < 8; i++)
                {
                    var bit = byteInUtf8 >> i & 1;
                    charArrayInHaoCode[count++] = bit == _bitHao ? _charHao : _charShen;
                }
            }

            return UTF8.GetBytes(charArrayInHaoCode);
        }

        public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
        {
            // Validate parameters.
            if (chars is null)
                throw new ArgumentNullException(nameof(chars));
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            if (charIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(charIndex));
            if (charCount < 0)
                throw new ArgumentOutOfRangeException(nameof(charCount));
            if (chars.Length - charIndex < charCount)
                throw new ArgumentOutOfRangeException(nameof(bytes));
            if (byteIndex < 0 || byteIndex > bytes.Length)
                throw new ArgumentOutOfRangeException(nameof(byteIndex));

            // If no input, return 0.
            if (bytes.Length == 0)
                return 0;

            var charSpan = new ReadOnlySpan<char>(chars, charIndex, charCount);
            var byteArray = GetBytes(charSpan);

            return byteArray.CopyToCount(bytes, byteIndex);
        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            var byteSpan = new ReadOnlySpan<byte>(bytes, index, count);
            return GetChars(byteSpan).Length;
        }

        public char[] GetChars(ReadOnlySpan<byte> bytes)
        {
            if (bytes.Length == 0)
                return new char[0];

            var byteArrayInUtf8 = new byte[(bytes.Length / 24)];
            var count = 0;

            // (UTF8)(HaoCode) byte[24 x ?] => (HaoCode(Bit)) char[8 x ?]: '昊' || '神' => (UTF8) byte[1 x ?]
            for (var i = 0; i < bytes.Length; i += 24)
            {
                // (UTF8) byte[24] => (HaoCode(Bit)) char[8]: '昊' || '神'
                var intInHaoCodeBit = UTF8.GetChars(bytes.Slice(i, 24).ToArray());

                // (HaoCode(Bit)) char[8] => (UTF8) byte[1]
                int byteInUtf8 = 0;
                for (var j = 0; j < 8; j++)
                {
                    var bit = intInHaoCodeBit[j] == _charHao ? _bitHao : _bitShen;
                    byteInUtf8 += bit << j;
                }
                byteArrayInUtf8[count++] = (byte)byteInUtf8;
            }

            return UTF8.GetChars(byteArrayInUtf8);
        }

        public override int GetChars(ReadOnlySpan<byte> bytes, Span<char> chars)
        {
            if (bytes.Length == 0)
                return 0;

            return GetChars(bytes).CopyToCount(chars);
        }

        public override char[] GetChars(byte[] bytes)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            if (bytes.Length == 0)
                return new char[0];

            return GetChars(new ReadOnlySpan<byte>(bytes));
        }

        public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
        {
            // Validate parameters.
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));
            if (chars is null)
                throw new ArgumentNullException(nameof(chars));
            if (byteIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(byteIndex));
            if (byteCount < 0)
                throw new ArgumentOutOfRangeException(nameof(byteCount));
            if (bytes.Length - byteIndex < byteCount)
                throw new ArgumentOutOfRangeException(nameof(bytes));
            if (charIndex < 0 || charIndex > chars.Length)
                throw new ArgumentOutOfRangeException(nameof(charIndex));

            // If no input, return 0.
            if (bytes.Length == 0)
                return 0;

            var byteSpan = new ReadOnlySpan<byte>(bytes, byteIndex, byteCount);
            var charArray = GetChars(byteSpan);

            return charArray.CopyToCount(chars, charIndex);
        }

        public override int GetMaxByteCount(int charCount)
            => UTF8.GetMaxByteCount(charCount) * 24;

        public override int GetMaxCharCount(int byteCount)
            => UTF8.GetMaxCharCount(byteCount / 24);
    }
}
