using System.Numerics;
using Lab1RSA;

namespace BigIntTest;

public class Tests
{
    [TestCase("11", "24")]
    [TestCase("35", "-21")]
    [TestCase("-56", "42")]
    [TestCase("-10", "-11")]
    [TestCase("1", "99")]
    public void AdditionTest(string a, string b)
    {
        var bigA = new BigInt(a);
        var bigB = new BigInt(b);
        Assert.IsTrue(bigA + bigB == a.ToBigInteger() + b.ToBigInteger());
    }
    
    [TestCase("11", "24")]
    [TestCase("35", "-21")]
    [TestCase("-56", "42")]
    [TestCase("-10", "-11")]
    [TestCase("100", "2")]
    public void SubstractionTest(string a, string b)
    {
        var bigA = new BigInt(a);
        var bigB = new BigInt(b);
        Assert.IsTrue(bigA - bigB == a.ToBigInteger() - b.ToBigInteger());
        Assert.IsTrue(bigB - bigA == -(a.ToBigInteger() - b.ToBigInteger()));
    }
    
    [TestCase("11", "24")]
    [TestCase("35", "-21")]
    [TestCase("-56", "42")]
    [TestCase("-10", "-11")]
    public void MultiplicationTest(string a, string b)
    {
        var bigA = new BigInt(a);
        var bigB = new BigInt(b);
        Assert.IsTrue(bigA * bigB == a.ToBigInteger() * b.ToBigInteger());
    }
    
    [TestCase("111111111111", "111111111111")]
    [TestCase("111111111111", "100000000000")]
    [TestCase("9363795622172","7857081434962")]
    public void MultiplicationTestLong(string a, string b)
    {
        var bigA = new BigInt(a);
        var bigB = new BigInt(b);
        var rr = bigA * bigB;
        var r = a.ToBigInteger() * b.ToBigInteger();
        var t = r.ToString();
        Assert.IsTrue(rr == r);
    }
    
    [TestCase("12", "3")]
    [TestCase("123123", "123")]
    [TestCase("111111111111", "111111")]
    public void DivisionTest(string a, string b)
    {
        var bigA = new BigInt(a);
        var bigB = new BigInt(b);
        Assert.IsTrue(bigA / bigB == a.ToBigInteger() / b.ToBigInteger());
    }
    
    [TestCase("12", "7")]
    [TestCase("16", "8")]
    [TestCase("34567", "2143")]
    public void ModTest(string a, string b)
    {
        var bigA = new BigInt(a);
        var bigB = new BigInt(b);
        Assert.IsTrue(bigA % bigB == a.ToBigInteger() % b.ToBigInteger());
    }

    [Test]
    public void RandomTests()
    {
        for (int i = 0; i < 100; i++)
        {
            RandomTest(i);
        }
    }
    
    public void RandomTest(int i)
    {
        var rnd = new Random(DateTime.Now.Second + i);
        var a = RandomString("0123456789", rnd.Next(11, 15), rnd);
        var b = RandomString("0123456789", rnd.Next(11, 15), rnd);
        AdditionTest(a, b);
        SubstractionTest(a, b);
        DivisionTest(a, b);
        MultiplicationTestLong(a, b);
        ModTest(a, b);
    }
    
    private static string RandomString(string alphabet, int lenght, Random ran)
    {
        char[] arr = new char[lenght];
        for (int i = 0; i < arr.Length; ++i)
        {
            arr[i] = alphabet[ran.Next(alphabet.Length)];
        }
        return new String(arr);
    }
    
}