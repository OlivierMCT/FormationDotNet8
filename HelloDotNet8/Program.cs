Console.WriteLine("Hello, World!");

Personne p1 = new Personne();
var p2 = new Personne();
Personne p3 = new() { Id = 3 };

string? s = p3.Nom?.ToUpper();

if(p3.Nom != null) { 
    string s1 = p3.Nom.ToUpper();
}

string s2 = p3.Nom?.ToUpper() ?? "";

// ---------------------------------
Console.WriteLine("-- Record -----------------------------");
VoitureRecord vr1 = new ("Renault", "Clio", 12345);
VoitureRecord vr2 = new ("Renault", "Clio", 12345);
Console.WriteLine("vr1 " + vr1);
Console.WriteLine("vr1 == vr2 ? " + (vr1 == vr2));
Console.WriteLine("vr1 Equals vr2 ? " + (vr1.Equals(vr2)));
Console.WriteLine("ReferenceEquals vr1, vr2 ? " + ReferenceEquals(vr1, vr2));
VoitureRecord vr3 = vr1 with { Modele = "Twingo" };


Console.WriteLine("\n\n-- Class -----------------------------");
VoitureClass vc1 = new("Renault", "Clio", 12345);
VoitureClass vc2 = new("Renault", "Clio", 12345);
Console.WriteLine("vc1 " + vc1);
Console.WriteLine("vc1 == vc2 ? " + (vc1 == vc2));
Console.WriteLine("vc1 Equals vc2 ? " + (vc1.Equals(vc2)));
Console.WriteLine("ReferenceEquals vc1, vc2 ? " + ReferenceEquals(vc1, vc2));
//VoitureClass vc3 = vc1.Clone();

int[] data = { 11, 22, 33 };
List<int> data2 = new(){ 11, 22, 33 };

XmlAttribute attr;


Feu f1 = (Feu)99;
string info = f1 switch { 
    Feu.Vert => "Tu peux passer",
    Feu.Rouge or Feu.Orange => "STOP",
    _ => "Fais attention"
};
Console.WriteLine(info);

int saisie = 55;
const int max = 123;
if(saisie is < 30 or > max) {

}

if (saisie < 30 || saisie > info.Length) {

}

List<VoitureRecord> loto = new (){ 
    new VoitureRecord("a", "aa", 111),
    new VoitureRecord("b", "bb", 666),
    new VoitureRecord("c", "cc", 444),
    new VoitureRecord("d", "bb", 999),
    new VoitureRecord("e", "ee", 222),
    new VoitureRecord("f", "bb", 333),
};
loto.DistinctBy(v => v.Modele).ToList().ForEach( Console.WriteLine );
Console.WriteLine("voiture la moins chere = " + loto.MinBy(i => i.Prix));
Console.WriteLine("voiture la plus chere = " + loto.MaxBy(i => i.Prix));