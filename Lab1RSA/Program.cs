using Lab1RSA;

var p = new BigInt("13");//752723258252338103239309175083");
var q = new BigInt("29");//"844690060152886429199167686411");

var n = p * q;
var phi = (p - 1) * (q - 1);
var e = "5".ToBigInt();
var d = BigInt.ReverseByMod(e, phi);

var message1 = File.ReadAllBytes("text.txt");
var binaryString = string.Join("", message1.Select(b => Convert.ToString(b, 2)).ToArray());
var m = binaryString.FromNumeralSystemByBase(2);

var c = BigInt.PowByMod(m, e, n);
m = BigInt.PowByMod(c, d, n);
var message2 = new string(m.ToNumeralSystemByBase(2).BynaryStringToByteArray().Select(b => (char)b).ToArray());

Console.WriteLine(message2);
