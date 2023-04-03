﻿using System.Collections;
using System.Linq;
using System.Numerics;

namespace Lab1RSA;

public class BigInt
{
    /*содержит целое число со знаком в виде массива однобайтовых элементов. Реализовать конструкторы, деструктор, перегрузить 
        операции: аддитивные (+, –), мультипликативные (*,  /,  %), сравнения (==, !=, <, >), взятие обратного по заданному модулю.*/
    public byte[] Digits { get; private set; } // 1 0 3 === {3, 0, 1}
    public bool Sign { get; private set; } //true когда <0
    public int Lenght { get=>Digits.Length; }

    public byte this[int index]
    {
        get
        {
            if (index >= Lenght)
                return 0;
            return Digits[index];
        }
    }

    public BigInt(byte[] digits, bool sign)
    {
        digits = digits.Reverse().SkipWhile(d => d == 0).Reverse().ToArray();
        if (digits.Length == 0)
            digits = new byte[] { 0 };
        Digits = digits;
        Sign = sign;
    }
    public BigInt(string number)
    {
        Sign = number[0] == '-';
        Digits = number.Skip(Sign ? 1 : 0).Select(c => (byte)(c - '0')).Reverse().ToArray();
    }
    public BigInt(long number) : 
        this(number
            .ToString()
            .Skip(number < 0 ? 1 : 0)
            .Select(c => (byte)(c - '0'))
            .Reverse()
            .ToArray(), 
            number < 0){}

    public static BigInt operator +(BigInt first, BigInt second)
    {
        //  (-a) + (-b) === -(a + b)
        //  a + (-b) === a - b
        //  (-a) + b === b - a
        if (first.Sign & second.Sign)
            return -(-first + -second);
        if (first.Sign ^ second.Sign)
            return second.Sign ? first - -second : second - -first;
        if (first.Sign || second.Sign)
            throw new Exception("NegNum");//TODO: удалить после тестирования
        var result = new List<byte>();
        var len = Math.Max(first.Lenght, second.Lenght);
        int temp, carry = 0;
        for (int i = 0; i < len || carry != 0; i++)
        {
            temp = first[i] + second[i] + carry;
            carry = temp >= 10 ? 1 : 0;
            if (carry != 0) temp -= 10;
            result.Add((byte)temp);
        }
        return new BigInt(result.ToArray(), false);
    }
    
    public static BigInt operator -(BigInt first, BigInt second)
    {
        // first >= second
        //  a - (-b) === a + b
        //  (-a) - b === -(a + b)
        // (-a) - (-b) === b - a
        if (first.Sign & second.Sign)
            return -second - -first;
        if (first.Sign ^ second.Sign)
            return second.Sign ? first + -second : -(-first + second);
        if (first.Sign || second.Sign)
            throw new Exception("NegNum");//TODO: удалить после тестирования
        var sign = first < second;
        if(sign)
            Swap(ref first, ref second);
        var result = new List<byte>();
        var len = first.Lenght;
        int temp;
        bool carry = false;
        for (int i=0; i<len || carry; ++i) {
            temp = first[i] - ((carry ? 1 : 0) + second[i]);
            carry = temp < 0;
            if (carry)  temp += 10;
            result.Add((byte)temp);
        }
        return new BigInt(
            result
            .AsEnumerable()
            .Reverse()
            .SkipWhile(b => b == 0) //TODO:проверить
            .Reverse()
            .ToArray(), sign);
    }

    public static BigInt operator -(BigInt first) => new BigInt(first.Digits, !first.Sign);

    public static BigInt operator *(BigInt first, BigInt second)
    {
        var len = Math.Max(first.Lenght, second.Lenght);
        if (len < 10)//TODO: long
        {
            int firstInt = first.ToInt(), secondInt = second.ToInt();//TODO: TryParseToInt
            long result = (long)firstInt * secondInt;
            return new BigInt(result);
        }
        var halfLen = len / 2;
        var a = new BigInt(first.Digits.Skip(halfLen).Take(len - halfLen).ToArray(), false) ;
        var b = new BigInt(first.Digits.Take(halfLen).ToArray(), false);
        var c = new BigInt(second.Digits.Skip(halfLen).Take(len - halfLen).ToArray(), false);
        var d = new BigInt(second.Digits.Take(halfLen).ToArray(), false);

        var p1 = a * c;
        var p2 = b * d;
        var p3 = (a + b) * (c + d) - p1 - p2;
        //p1 * 10^n + p3 * 10^(n/2) + p2
        var res = (p1 << len) + (p3 << halfLen) + p2;
        if (first.Sign ^ second.Sign)
            res = -res;
        return res; //TODO: степени
    }

    public static BigInt operator *(BigInt first, int second) => first * second.ToBigInt();

    public static bool operator >(BigInt first, BigInt second)
    {
        if (first.Sign ^ second.Sign)
            return second.Sign;
        if (first.Lenght > second.Lenght)
            return true;
        if (first.Lenght < second.Lenght || first == second)
            return false;
        for (int i = first.Lenght-1; i >= 0; i--)
        {
            if (first[i] > second[i])
                return true;
            if (first[i] < second[i])
                return false;
        }

        return false;
    }
    
    public static BigInt operator <<(BigInt first, int bias)
    {
        return Enumerable.Repeat((byte)0, bias).Concat(first.Digits).ToArray().ToBigInt();
    }
    
    public static BigInt operator ^(BigInt first, BigInt power)
    {
        if (power == 1)
            return first;
        if (power % 2.ToBigInt() == 0)
        {
            var halfSquare = first ^ (power / 2.ToBigInt());
            return halfSquare * halfSquare;
        }
        return first * (first ^ (power - 1.ToBigInt()));
        
    }
    
    public static BigInt PowByMod(BigInt num, BigInt power, BigInt mod)
    {
        if (power == 1)
            return num % mod;
        if (power % 2.ToBigInt() == 0.ToBigInt())
        {
            var halfSquare = PowByMod(num, power/2.ToBigInt(), mod);
            return (halfSquare * halfSquare) % mod;
        }
        return ((num % mod) * PowByMod(num, power - 1.ToBigInt(), mod)) % mod;
        
    }
    
    public static BigInt operator %(BigInt first, BigInt second)
    {
        if (first < second)
            return first;
        BigInt curValue = 0.ToBigInt();
        for (int i = first.Lenght-1; i>=0; i--)
        {
            curValue = curValue << 1; // * osn
            curValue = curValue + first[i].ToString().ToBigInt();
            int x = 0;
            int l = 0, r = 10;
            while (l <= r)
            {
                int m = (l + r) >> 1;
                BigInt cur = second * m;
                if (cur <= curValue)
                {
                    x = m;
                    l = m+1;
                }
                else
                    r = m-1;
            }
            curValue = curValue - second * x;
        }
        return curValue;
    }
    
    public static BigInt operator /(BigInt first, BigInt second)
    {
        List<byte> res = new();
        var curValue = 0.ToBigInt();
        for (int i = first.Lenght-1; i>=0; i--)
        {
            curValue = curValue << 1;
            curValue = curValue + first[i].ToString().ToBigInt();//TODO: добавить методы https://metanit.com/sharp/tutorial/3.37.php
            int x = 0;
            int l = 0, r = 10;
            while (l <= r)
            {
                int m = (l + r) / 2;
                BigInt cur = second * m;
                if (cur <= curValue)
                {
                    x = m;
                    l = m+1;
                }
                else
                    r = m-1;
            }
            res.Add((byte)x);
            curValue = curValue - second * x;
        }
        return new BigInt(res.AsEnumerable().Reverse().ToArray(), first.Sign ^ second.Sign);
    }

    public static bool operator <(BigInt first, BigInt second)
    {
        return !(first > second) && first != second;//TODO: переработать?
    }

    public static bool operator <=(BigInt first, BigInt second) => first == second || first < second;

    public static bool operator >=(BigInt first, BigInt second) => first == second || first > second;

    public static bool operator ==(BigInt first, BigInt second)
    {
        if (first.Sign != second.Sign)
            return false;
        if (first.Lenght != second.Lenght)
            return false;
        for (int i = 0; i < first.Lenght; i++)
        {
            if (first[i] != second[i])
                return false;
        }
        return true;
    }

    public static bool operator ==(int first, BigInt second) => new BigInt(first) == second;
    
    public static bool operator ==(BigInt first, BigInteger second) => first.ToString() == second.ToString();

    public static bool operator !=(BigInt first, BigInteger second) => !(first == second);

    public static bool operator ==(BigInt first, int second) => new BigInt(second) == first;

    public static bool operator !=(BigInt first, BigInt second) => !(first == second);
    
    public static bool operator !=(int first, BigInt second) => !(first == second);
    
    public static bool operator !=(BigInt first, int second) => !(first == second);

    public static void Swap(ref BigInt first, ref BigInt second) //TODO: надо ли?
    {
        BigInt temp = first;
        first = second;
        second = temp;
    }

    public string ToString() => string.Join("", Digits.Reverse().Select(b => b.ToString()).Prepend(Sign?"-":""));

    public static BigInt ReverseByMod(BigInt num, BigInt mod)
    {
        BigInt x, y;
        BigInt g = gcd(num, mod, out x, out y);
        if (g != 1)
        {
            Console.Write("no solution");
            return 0.ToBigInt();
        }

        var t = num * x + mod * y;
        Console.WriteLine(t.ToString());
        x = (x % mod + mod) % mod;
        return x;
    }
    
    public static BigInt gcd(BigInt a, BigInt b, out BigInt x, out BigInt y)
    {
        if (b == 0)
        {
            x = 1.ToBigInt();
            y = 0.ToBigInt();
            return a;
        }
        BigInt g = gcd(b, a % b, out y, out x); // x и y - переставляются
        y = y - (a / b) * x;
        return g;
    }
}