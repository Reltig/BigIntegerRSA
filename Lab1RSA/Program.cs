// See https://aka.ms/new-console-template for more information

using Lab1RSA;

var p = new BigInt("13752723258252338103239309175083");
var q = new BigInt("29844690060152886429199167686411");

var n = p * q;
var phi = (p - 1.ToBigInt()) * (q - 1.ToBigInt());
var e = "429496729021".ToBigInt();
var d = BigInt.ReverseByMod(e, phi);
// var m = 111.ToBigInt();
// var c = BigInt.PowByMod(m, e, n);
// m = BigInt.PowByMod(c, d, n);

Console.WriteLine(d.ToString());
//Console.WriteLine(BigInt.ReverseByMod(199.ToBigInt(), 37.ToBigInt()).ToString());
