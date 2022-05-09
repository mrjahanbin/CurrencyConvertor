namespace CurrencyConvertor
{
    internal class Olds
    {
        public static int Counter { get; set; } = 0;

        private static decimal loopo(string fromCurrency, string toCurrency, List<Tuple<string, string, double>> list)
        {

            var start = list.Where(x => x.Item1 == fromCurrency || x.Item2 == fromCurrency).ToList();
            var SS = start.Where(x => x.Item2.Contains(toCurrency) || x.Item1.Contains(toCurrency)).ToList();
            if (!SS.Any())
            {
                foreach (var item in start.OrderBy(x => x.Item1))
                {
                    Counter++;
                    return loopo(item.Item1, toCurrency, list);
                }
            }
            else
            {
                return Counter;
            }
            return Counter;
        }
        private static Tuple<string, string, double> Loop(string fromCurrency, List<Tuple<string, string, double>> list)
        {
            var CommonList = ReturnFunction(fromCurrency, list);
            if (CommonList.Count > 1)
            {
                var first = CommonList[0];
                var second = CommonList[1];
                CommonList = ReturnFunction(first, list);
                var CommonList2 = ReturnFunction(second, list);
                var Intersect = CommonList.Intersect(CommonList2).ToList();

                if (Intersect.Count > 0)
                {
                    var Get = list.FirstOrDefault(x => x.Item1 == first && x.Item2 == Intersect.FirstOrDefault());
                    return Get;
                }
                else
                {
                    return Loop(first, list);
                }
            }
            else
            {
                return Loop(CommonList.First(), list);
            }

        }

        private static List<string> ReturnFunction(string Currency, List<Tuple<string, string, double>> list)
        {
            //پیدا کردن لیستی از تبدیل ها که رشته اول با 
            //from
            //یکی باشه
            var start = list.Where(x => x.Item1 == Currency && x.Item2 != Currency).ToList();
            var result = start.Select(x => x.Item2).ToList();
            return result;
        }


        private static Tuple<string, string, double> ReturnFunction(string fromCurrency, string toCurrency, List<Tuple<string, string, double>> list)
        {
            //پیدا کردن لیستی از تبدیل ها که رشته اول با 
            //from
            //یکی باشه
            var start = list.Where(x => x.Item1 == fromCurrency && x.Item2 != fromCurrency).ToList();

            //پیدا کردن لیستی از تبدیل ها که رشته اول با 
            //to
            //یکی باشه
            var end = list.Where(x => x.Item1 == toCurrency && x.Item2 != toCurrency).ToList();


            var start2 = start.Select(x => x.Item2).ToList();
            var end2 = end.Select(x => x.Item2).ToList();

            var Intersect = start2.Intersect(end2).ToList();
            if (Intersect.Count == 0)
            {
                var N2 = fromCurrency;
                var N1 = start2.First();


                ReturnFunction(N1, N2, list);
            }

            var Get = list.FirstOrDefault(x => x.Item1 == fromCurrency && x.Item2 == Intersect.FirstOrDefault());
            return Get;
        }
    }
}
