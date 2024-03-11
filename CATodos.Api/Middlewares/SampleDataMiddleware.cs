using CATodos.Entities;
using CATodos.Persistance;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace CATodos.Api.Middlewares {
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class SampleDataMiddleware {
        private readonly RequestDelegate _next;

        public SampleDataMiddleware(RequestDelegate next) {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext, CATodoDbContext dbContext, ILogger<SampleDataMiddleware> logger) {
            if (dbContext.Database.EnsureCreated()) {
                Random rd = new();
                var categories = new List<string>() { "perso", "formation", "popi", "apéro", "famille", "cats", "réunion" }
                    .Select(s => new CategoryEntity() {
                        Label = s,
                        Color = rd.Next(int.MinValue, int.MaxValue)
                    }).ToList();
                dbContext.AddRange(categories);
                dbContext.SaveChanges();

                var todosLabel = new string[] { "Descendre une cascade en rappel", "Prendre une vague en surf", "Nager avec les dauphins", "Faire du parapente", "Explorer une grotte", "Faire du quad", "Sauter d’une falaise dans l’eau", "Se faire une balade en montgolfière", "Faire une descente en snowboard", "Tester le zorbing", "Fendre les flots en jet-ski", "Faire une balade en traîneau à chiens", "Se faire une partie de paintball", "Tester le fat-bike", "Faire une descente en bobsleigh", "Faire une sortie en mer en voilier et se mettre à la barre", "Nager avec les raies manta", "Faire un tour d’airboat dans un marécage", "Sauter à l’élastique", "Planer en parachute ascensionnel", "Se laisser glisser le long d’une tyrolienne", "Faire un tour en hydravion", "Sauter en parachute", "Plonger au milieu des requins", "Cracher du feu", "Faire un saut périlleux sur un trampoline", "Tenir debout en flyboard", "Défier la pesanteur dans une soufflerie", "Faire du kitesurf", "Planer en ULM", "Traverser un pont de singe (pont de corde)", "Marcher sur des charbons ardents", "Prendre un serpent autour de son cou", "Faire de la plongée sous-marine", "Faire un tour en scooter sous-marin", "Tester le ski-joering", "Descendre une rivière en radeau en bambou", "Faire du rafting", "Dormir dans un gîte de montagne et faire une rando au soleil levant", "Tenir quelques minutes en ski nautique", "Dormir dans une cabane dans les bois", "S’adonner à la spéléo", "Monter à bord d’un sous-marin/paquebot", "Aller dans l’espace", "Construire un radeau", "Faire du hors-piste", "Baigner un éléphant", "Tondre un mouton", "Monter au sommet d’un volcan", "Assister à une éclipse totale de soleil", "Faire un bonhomme de neige", "Traire une vache", "Fouler le raisin pour en faire du vin", "Assister à un rodéo", "Baptiser une étoile", "Marcher sur une plage de sable noir", "Voir des séquoias géants", "Nager dans des sources chaudes naturelles", "Passer la nuit dans une yourte", "Regarder le soleil se lever et se coucher le même jour", "Traverser un tunnel de lave", "Dormir dans le foin dans une grange", "Participer à une chasse aux orages/tornades", "Faire une pêche au gros", "Monter à cheval sur la plage", "Faire copain-copain avec un singe", "Traverser une tempête de sable", "Monter à dos de chameau et/ou dromadaire", "Se baigner dans tous les 5 grands océans du monde", "Tenir une araignée géante (mygale) dans la main", "Monter un cheval à cru", "Admirer un récif de corail", "Relâcher des bébés tortues dans l’océan", "Voir des élans", "Nourrir un koala", "Passer derrière une cascade", "Apercevoir les baleines", "Nager au milieu des poissons", "Prendre un cours de fauconnerie", "Nourrir un crocodile", "Nager dans une piscine naturelle d’eau salée", "S’adonner à la pêche sous la glace", "Faire la cueillette des champignons", "Parcourir un des chemins de grande randonnée (GR)", "Assister à la montée du saumon", "Nager avec les tortues de mer", "Voir un geyser", "Voir une pluie de météorites et faire un vœu", "Faire un feu de camp sur la plage", "Participer à la cueillette du riz", "Jouer les touristes dans sa ville natale", "Célébrer une fête locale à l’étranger", "Partir à l’étranger", "Emmener quelqu’un de sa famille en voyage", "Monter tout en haut d’un phare", "Explorer une forêt équatorienne", "Dormir dans le train de nuit", "Loger dans un Gîte/Bed & Breakfast", "Dormir dans un motel", "Prendre l’avion en première/business", "Dormir dans une auberge de jeunesse", "Faire une croisière", "Partir en vacances seul.e", "Faire un safari", "Visiter un château/cité médiéval.e", "Partir en road trip sur un coup de tête", "Visiter un aquarium", "Voir une fontaine spectaculaire", "Participer à une cérémonie du thé japonaise", "Chercher de l’or", "Voir un trou souffleur littoral", "Dormir dans une maison hantée", "Dormir sur une péniche", "Dormir dans un tipi", "Dormir dans une cabane dans les arbres", "Dormir dans une bulle", "Dormir dans un hôtel de glace", "Dormir dans un hôtel sous-marin", "Poser le pied sur les 6 continents (ou 7 si on dissocie les deux Amériques)", "Emmener maman (ou papa) en voyage", "Lancer une fléchette sur une carte et partir là où elle atterrit", "Toucher une pyramide", "Visiter une usine", "Visiter des ruines précolombiennes", "Visiter une ferme", "Visiter un vignoble", "Vivre à l’étranger", "Visiter 100 sites inscrits à l’UNESCO", "Visiter une ville fantôme", "Entrer dans un temple", "Visiter une des attractions touristiques de ma ville", "Marcher sur un glacier", "Poser le pied sur l’équateur", "Se baigner dans des thermes naturels", "Marcher jusqu’au pôle nord", "Voir une aurore boréale", "Voir le soleil de minuit", "Se tenir sur le méridien de Greenwich", "Visiter toutes les capitales d’Europe", "Visiter une plantation de café", "Cueillir du thé", "Aller à l’aéroport et choisir une destination au hasard sur le tableau de départ", "Dormir dans un igloo", "Faire un long voyage en slow travel", "Dormir chez l’habitant", "Plonger dans un cénote", "Faire du woofing", "Participer à un grand carnaval (Venise, Rio, Nice…)", "Dormir sur une île déserte/privée", "Voyager en sac à dos", "Faire un voyage en vélo", "Prendre un billet aller sans retour", "Visiter la ville, la région, le pays natal de ses parents ou grands-parents", "Visiter toutes les grandes villes d’un pays", "Voir le coucher de soleil dans le désert", "Visiter des catacombes", "Visiter une cité engloutie", "Visiter tous les pays d’Europe", "Aménager un bus ou un van", "Faire une rando de plusieurs jours en bivouac", "Dormir à la belle étoile", "Nager dans une eau absolument transparente", "Assister à l’avant-première VIP d’un film", "Assister à un grand festival international du film", "Monter les marches du palais du festival de Cannes", "Être figurant dans un film", "Voir un film dans un cinéma en plein air", "Visiter un studio hollywoodien", "Arpenter le Hollywood Walk of Fame", "Visiter Cinecitta et/ou les studios Pinewood", "Être invité.e sur un tournage", "Rencontrer sa star de ciné préférée", "Avoir sa collection de films", "Visiter un parc à thème comme Harry Potter/Universal", "Visiter les lieux de tournage d’un de ses films préférés", "Visiter les plus beaux cinémas du monde", "Aller seul.e au cinéma", "Voir un film en 3D et/ou en Imax", "Avoir son home cinema chez soi", "Aller à une convention/évènement spécial", "Transmettre sa cinéphilie à quelqu’un", "Assister à une représentation interactive du Rocky Horror Picture Show", "Voir les 100 meilleurs films de l’histoire", "Faire un marathon Bollywood", "Réaliser un film", "Avoir son nom au générique d’un film", "Faire une rencontre via une appli", "Etre invité.e à un mariage homo", "Etre témoin à un mariage", "Etre parrain ou marraine", "Avoir des enfants", "Fêter son anniversaire en organisant une grande soirée avec tous ses proches", "Retrouver ses amis du lycée/collège/primaire", "Recevoir un courrier de fan", "Trouver l’amour", "Se fiancer", "Se marier", "Faire une blind date (rendez-vous à l’aveugle)", "Etre proche de quelqu’un d’une autre génération", "Prendre quelqu’un en autostop", "Embrasser un.e inconnu.e", "S’embrasser en haut d’une grande roue", "Rencontrer quelqu’un qu’on admire", "Chanter un karaoké en duo", "Rencontrer quelqu’un de célèbre", "Rencontrer le président de la République", "Jouer les entremetteur.ses", "Couvrir le lit de pétales de rose", "Retrouver un vieil ami", "Partager un taxi avec un.e inconnu.e", "Faire l’amour dans un endroit insolite", "Faire l’amour dans un avion", "Assister à une naissance", "Couper les cheveux à quelqu’un", "Rédiger une lettre d’amour", "Créer son arbre généalogique", "Faire l’amour à plusieurs", "Organiser une grande réunion de famille", "Passer la nuit à refaire le monde", "Accrocher un love lock quelque part avec son amoureux.se", "Ecrire un petit mot au rouge à lèvre sur le miroir de la salle de bains" };
                var todos = todosLabel.Select(s => new TodoEntity() {
                    DueDate = DateOnly.FromDateTime( DateTime.Today.AddDays(rd.Next(-30, 60)) ),
                    IsDone = rd.Next() > 0.65,
                    Title = s,
                    Categories = categories.Take(rd.Next(0, categories.Count() - 1)).ToList()
                });
                todos.Where(t => rd.Next() > 0.3).ToList().ForEach(t => {
                    t.Latitude = rd.Next(-90, 90);
                    t.Longitude = rd.Next(-180, 180);
                });
                dbContext.AddRange(todos);
                dbContext.SaveChanges();

                logger.LogInformation("Création de la base avec {} catégories et {} tâches", categories.Count(), todos.Count());
            }
            return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class SampleDataMiddlewareExtensions {
        public static IApplicationBuilder UseSampleDataMiddleware(this IApplicationBuilder builder) {
            return builder.UseMiddleware<SampleDataMiddleware>();
        }
    }
}
