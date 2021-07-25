using System;
using System.Text;

namespace Gaein.Text
{
    public class HaoEncodeing : Encoding
    {
        private const ushort _numHao = '昊';
        private const ushort _numShen = '神';

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

        }

        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            throw new NotImplementedException();
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
