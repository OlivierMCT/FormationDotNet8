namespace HelloWebApp {
    public interface IMagicService {
        int MagicNumber { get; init; }
    }

    public class MagicService : IMagicService {
        public int MagicNumber { get; init; }
        public MagicService() {
            MagicNumber = new Random().Next(1, 100);
        }
    }

    public class MagicRangeService : IMagicService {
        public int MagicNumber { get; init; }
        public MagicRangeService(IConfiguration conf) {
            MagicNumber = new Random().Next(
                conf.GetRequiredSection("Magic").GetValue<int>("Min"), 
                conf.GetRequiredSection("Magic").GetValue<int>("Max")
            );
        }
    }

    public class MagicCustomService : IMagicService {
        public int MagicNumber { get; init; }
        public MagicCustomService(int min, int? max = null) {
            MagicNumber = new Random().Next(min, max ?? min + 99);
        }
    }
}
