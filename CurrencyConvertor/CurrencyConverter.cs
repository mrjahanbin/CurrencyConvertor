using Microsoft.Extensions.Caching.Memory;

namespace CurrencyConvertor
{
    public class CurrencyConverter : ICurrencyConverter
    {

        IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
        public static int Counter { get; set; } = 0;
        public string MemoryCache { get; } = "Key";

        public CurrencyConverter(IMemoryCache cache)
        {
            this.cache = cache;
        }

        public void ClearConfiguration()
        {
            cache.Remove(MemoryCache);
        }

        public double Convert(string fromCurrency, string toCurrency, double amount)
        {
            var CurrenciesList = cache.Get<List<Tuple<string, string, double>>>(MemoryCache);

            #region Find Best Way
            List<Tuple<string, string, double>> BestWay;
            int BestWayNumber;
            FindBestWay(fromCurrency, toCurrency, CurrenciesList, out BestWay, out BestWayNumber);
            #endregion

            var Result = Convertor(BestWay[BestWayNumber], fromCurrency, toCurrency, amount, CurrenciesList);
            return Result;
        }

        private static void FindBestWay(string fromCurrency, string toCurrency, List<Tuple<string, string, double>> CurrenciesList, out List<Tuple<string, string, double>> BestWayNumber, out int BestWay)
        {
            List<Tuple<double, int>> Result = new();
            BestWayNumber = CurrenciesList.Where(x => x.Item1 == fromCurrency || x.Item2 == fromCurrency).ToList();
            var Currencies = BestWayNumber.Select(x => x.Item1).ToList();
            Currencies.AddRange(BestWayNumber.Select(x => x.Item2).ToList());
            Currencies = Currencies.Where(x => x != fromCurrency).Distinct().ToList();

            for (int i = 0; i < Currencies.Count; i++)
            {
                Result.Add(new Tuple<double, int>(BestWayMethod(Currencies[i], toCurrency, CurrenciesList), i));
                Counter = 0;
            }
            BestWay = Result.Min(x => x.Item2);
        }

        private static double BestWayMethod(string fromCurrency, string toCurrency, List<Tuple<string, string, double>> list)
        {
            var Select = list.FirstOrDefault(x => x.Item1 == fromCurrency || x.Item2 == fromCurrency);
            if (Select != null)
            {
                Counter++;
            }

            if (Select.Item1 != toCurrency && Select.Item2 != toCurrency)
            {
                return BestWayMethod(Select.Item1, toCurrency, list);
            }

            return Counter;
        }

        private static double Convertor(Tuple<string, string, double>? bestway, string fromCurrency, string toCurrency, double amount, List<Tuple<string, string, double>> list)
        {
            Tuple<string, string, double>? Select;
            if (bestway != null)
            {
                Select = bestway;
            }
            else
            {
                Select = list.FirstOrDefault(x => x.Item1 == fromCurrency || x.Item2 == fromCurrency);
            }

            if (Select != null)
            {
                if (Select.Item1 == fromCurrency)
                {
                    amount = amount * Select.Item3;
                }
                else
                {
                    amount = amount / Select.Item3;
                }
            }


            if (Select.Item1 != toCurrency && Select.Item2 != toCurrency)
            {
                return Convertor(null, Select.Item1, toCurrency, amount, list);
            }

            return amount;
        }

        public void Print()
        {
            var result = cache.Get<IEnumerable<Tuple<string, string, double>>>(MemoryCache);
            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
        }

        public void UpdateConfiguration(IEnumerable<Tuple<string, string, double>> conversionRates)
        {

            cache.Set(MemoryCache, conversionRates);
        }

        public List<Tuple<string, string, double>> FillCurrencyList()
        {
            List<Tuple<string, string, double>> ListCurrency = new();
            ListCurrency.Add(new(CurrencyEnum.USD.ToString(), CurrencyEnum.CAD.ToString(), 1.34));
            ListCurrency.Add(new(CurrencyEnum.CAD.ToString(), CurrencyEnum.GBP.ToString(), 0.58));
            ListCurrency.Add(new(CurrencyEnum.USD.ToString(), CurrencyEnum.EUR.ToString(), 0.86));
            ListCurrency.Add(new(CurrencyEnum.EUR.ToString(), CurrencyEnum.ABC.ToString(), 2));
            ListCurrency.Add(new(CurrencyEnum.ABC.ToString(), CurrencyEnum.GBP.ToString(), 2.5));

            return ListCurrency;
        }
        public IEnumerable<Tuple<string, string, double>> GetAllCurrency(List<Tuple<string, string, double>> listCurrency)
        {
            return listCurrency;
        }
    }
}
