namespace HelloDotNet8 {
    internal class Personne {
        public int Id { get; init; }
        public string? Nom { get; set; }
        public string Prenom { get; set; } = null!;
    }

    // class, struct, enum, interface, delegate, record

    public record VoitureRecord(string Marque, string Modele, double Prix) {
        public double Prix { get; set; } = Prix;
        public int Id { get; set; }
    }

    public class VoitureClass {
        public VoitureClass(string marque, string modele, double prix) {
            Marque = marque;
            Modele = modele;
            Prix = prix;
            XmlAttribute attr;
        }

        public string Marque { get; init; }
        public string Modele { get; init; }
        public double Prix { get; init; }

        // ToString
        // operator==
        // operator!=
        // Equals
        // GetHashCode
    }

    public class VoitureClassBis(string marque, string modele, double prix) {
        public string Marque { get; set; } = marque;
        public string Modele { get; set; } = modele;
        public double Prix { get; set; } = prix;
    }

    enum Feu {
        Rouge, Orange, Vert, Clignotant, Eteint
    }
}
