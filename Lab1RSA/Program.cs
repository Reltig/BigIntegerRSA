using Lab1RSA;

var p = new BigInt("983246265474006258191377");
var q = new BigInt("175222743997186280065999");

var n = p * q;
var phi = (p - 1) * (q - 1);
var e = "65537".ToBigInt();
var d = BigInt.ReverseByMod(e, phi);

var message = File.ReadAllBytes("text.txt");
var m = Cipher(message);

var c = BigInt.PowByMod(m, e, n);
File.WriteAllText("cipheredText.txt",c.ToString());
m = BigInt.PowByMod(c, d, n);

Console.WriteLine(Uncipher(m));

#region Cipher

BigInt Cipher(byte[] message)
{
    var binaryString = string.Join("", message.Select(b => Convert.ToString(b, 2)).Select(s => new string('0', 8 - s.Length) + s).ToArray());
    return binaryString.FromNumeralSystemByBase(2);
}

string Uncipher(BigInt m)
{
    var binaryString = m.ToNumeralSystemByBase(2);
    binaryString = new string('0', (binaryString.Length / 8 + 1) * 8 - binaryString.Length) + binaryString;
    return new string(binaryString.BynaryStringToByteArray().Select(b => (char)b).ToArray());

}

#endregion
