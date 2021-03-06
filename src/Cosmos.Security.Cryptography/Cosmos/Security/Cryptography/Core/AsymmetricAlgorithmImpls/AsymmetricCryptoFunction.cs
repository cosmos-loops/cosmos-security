﻿using System;
using System.Text;
using System.Threading;
using Cosmos.Optionals;

namespace Cosmos.Security.Cryptography.Core.AsymmetricAlgorithmImpls
{
    internal abstract class AsymmetricCryptoFunction<TKey> : AsymmetricSignFunction<TKey>, IAsymmetricCryptoFunction
        where TKey : IAsymmetricCryptoKey
    {
        #region Encrypt

        public virtual ICryptoValue Encrypt(byte[] originalBytes)
        {
            return Encrypt(originalBytes, CancellationToken.None);
        }

        public virtual ICryptoValue Encrypt(byte[] originalBytes, CancellationToken cancellationToken)
        {
            if (originalBytes is null)
                throw new ArgumentNullException(nameof(originalBytes));
            return Encrypt(originalBytes, 0, originalBytes.Length, cancellationToken);
        }

        public virtual ICryptoValue Encrypt(byte[] originalBytes, int offset, int count)
        {
            return Encrypt(originalBytes, offset, count, CancellationToken.None);
        }

        public virtual ICryptoValue Encrypt(byte[] originalBytes, int offset, int count, CancellationToken cancellationToken)
        {
            if (originalBytes is null)
                throw new ArgumentNullException(nameof(originalBytes));
            if (offset < 0 || offset > originalBytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be a value greater than or equal to zero and less than or equal to the length of the array.");
            if (count < 0 || count > originalBytes.Length - offset)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be a value greater than or equal to zero and less than the the remaining length of the array after the offset value.");
            return Encrypt(new ArraySegment<byte>(originalBytes, offset, count), cancellationToken);
        }

        public virtual ICryptoValue Encrypt(string text, Encoding encoding = null)
        {
            return Encrypt(text, encoding, CancellationToken.None);
        }

        public virtual ICryptoValue Encrypt(string text, CancellationToken cancellationToken)
        {
            return Encrypt(text, Encoding.UTF8, cancellationToken);
        }

        public virtual ICryptoValue Encrypt(string text, Encoding encoding, CancellationToken cancellationToken)
        {
            return Encrypt(encoding.SafeEncodingValue().GetBytes(text), cancellationToken);
        }

        public virtual ICryptoValue Encrypt(ArraySegment<byte> originalBytes)
        {
            return Encrypt(originalBytes, CancellationToken.None);
        }

        public virtual ICryptoValue Encrypt(ArraySegment<byte> originalBytes, CancellationToken cancellationToken)
        {
            return EncryptInternal(originalBytes, cancellationToken);
        }


        protected abstract ICryptoValue EncryptInternal(ArraySegment<byte> originalBytes, CancellationToken cancellationToken);

        #endregion

        #region Decrypt

        public virtual ICryptoValue Decrypt(byte[] cipherBytes)
        {
            return Decrypt(cipherBytes, CancellationToken.None);
        }

        public virtual ICryptoValue Decrypt(byte[] cipherBytes, CancellationToken cancellationToken)
        {
            if (cipherBytes is null)
                throw new ArgumentNullException(nameof(cipherBytes));
            return Decrypt(cipherBytes, 0, cipherBytes.Length, cancellationToken);
        }

        public virtual ICryptoValue Decrypt(byte[] cipherBytes, int offset, int count)
        {
            return Decrypt(cipherBytes, offset, count, CancellationToken.None);
        }

        public virtual ICryptoValue Decrypt(byte[] cipherBytes, int offset, int count, CancellationToken cancellationToken)
        {
            if (cipherBytes is null)
                throw new ArgumentNullException(nameof(cipherBytes));
            if (offset < 0 || offset > cipherBytes.Length)
                throw new ArgumentOutOfRangeException(nameof(offset), "Offset must be a value greater than or equal to zero and less than or equal to the length of the array.");
            if (count < 0 || count > cipherBytes.Length - offset)
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be a value greater than or equal to zero and less than the the remaining length of the array after the offset value.");
            return Decrypt(new ArraySegment<byte>(cipherBytes, offset, count), cancellationToken);
        }

        public virtual ICryptoValue Decrypt(string cipherText, Encoding encoding = null)
        {
            return Decrypt(cipherText, encoding, CancellationToken.None);
        }

        public virtual ICryptoValue Decrypt(string cipherText, CancellationToken cancellationToken)
        {
            return Decrypt(cipherText, Encoding.UTF8, cancellationToken);
        }

        public virtual ICryptoValue Decrypt(string cipherText, Encoding encoding, CancellationToken cancellationToken)
        {
            return Decrypt(encoding.SafeEncodingValue().GetBytes(cipherText), cancellationToken);
        }

        public ICryptoValue Decrypt(string cipherText, CipherTextTypes cipherTextType, Encoding encoding = null, Func<string, byte[]> customCipherTextConverter = null)
        {
            return Decrypt(cipherText, cipherTextType, encoding, CancellationToken.None, customCipherTextConverter);
        }

        public ICryptoValue Decrypt(string cipherText, CipherTextTypes cipherTextType, CancellationToken cancellationToken, Func<string, byte[]> customCipherTextConverter = null)
        {
            return Decrypt(cipherText, cipherTextType, Encoding.UTF8, cancellationToken, customCipherTextConverter);
        }

        public ICryptoValue Decrypt(string cipherText, CipherTextTypes cipherTextType, Encoding encoding, CancellationToken cancellationToken, Func<string, byte[]> customCipherTextConverter = null)
        {
            encoding = encoding.SafeEncodingValue();

            var finalCipherText = cipherTextType.GetBytes(cipherText, encoding, customCipherTextConverter);

            return Decrypt(finalCipherText, cancellationToken);
        }

        public virtual ICryptoValue Decrypt(ArraySegment<byte> cipherBytes)
        {
            return Decrypt(cipherBytes, CancellationToken.None);
        }

        public virtual ICryptoValue Decrypt(ArraySegment<byte> cipherBytes, CancellationToken cancellationToken)
        {
            return DecryptInternal(cipherBytes, cancellationToken);
        }


        protected abstract ICryptoValue DecryptInternal(ArraySegment<byte> cipherBytes, CancellationToken cancellationToken);

        #endregion

        protected static ICryptoValue CreateCryptoValue(string original, string cipher, CryptoMode direction, Action<TrimOptions> optionsAct = null)
        {
            return CryptoValueBuilder
                   .Create()
                   .OriginalTextIs(original)
                   .CipherTextIs(cipher)
                   .ProcessingDirection(direction)
                   .Configure(optionsAct)
                   .Build();
        }

        protected static ICryptoValue CreateCryptoValue(byte[] originalBytes, byte[] cipherBytes, CryptoMode direction, Action<TrimOptions> optionsAct = null)
        {
            return CryptoValueBuilder
                   .Create()
                   .OriginalTextIs(originalBytes)
                   .CipherTextIs(cipherBytes)
                   .ProcessingDirection(direction)
                   .Configure(optionsAct)
                   .Build();
        }
    }
}