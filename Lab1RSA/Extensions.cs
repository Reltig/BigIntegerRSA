using System.Collections;
using System;
using System.Numerics;

namespace Lab1RSA;

public static class Extensions
{
    public static int ToInt(this BigInt number)
    {
        var str = new string(number.Digits.Select(b => b.ToString()[0]).Reverse().ToArray());
        if (string.IsNullOrEmpty(str))
            return 0;
        return Int32.Parse(str) * (number.Sign? -1 : 1);
    }
    
    public static BigInt ToBigInt(this string numberString)
    {
        return new BigInt(numberString);
    }
    
    public static BigInt ToBigInt(this int number)
    {
        return new BigInt(number);
    }
    
    public static BigInt ToBigInt(this byte[] numberString)
    {
        return new BigInt(numberString.ToArray(), false);
    }
    
    public static BigInteger ToBigInteger(this string s)
    {
        return BigInteger.Parse(s);
    }
    
    public static BigInt ToBigInt(this BigInteger number)
    {
        return new BigInt(number.ToString());
    }
    
    public static BigInt FromNumeralSystemByBase(this string num, int b)
    {
        BigInt p = BigInt.One;
        BigInt res = BigInt.Zero;
        foreach (var c in num.Reverse())
        {
            res = res + p * new BigInt(c.ToString());
            p = p * b;
        }

        return res;
    }
    
    public static byte[] BynaryStringToByteArray(this string str, int chunkSize = 8)
    {
        int numOfBytes = str.Length / 8;
        byte[] bytes = new byte[numOfBytes];
        for(int i = 0; i < numOfBytes; ++i)
        {
            bytes[i] = Convert.ToByte(str.Substring(8 * i, 8), 2);
        }

        return bytes;
    }
}