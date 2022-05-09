// See https://aka.ms/new-console-template for more information
using CurrencyConvertor;
using Microsoft.Extensions.Caching.Memory;

IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
ICurrencyConverter currencyConverter = new CurrencyConverter(cache);


////Set Currency Convert
currencyConverter.ClearConfiguration();

////Fill
double Amount = 1000;
var ListCurrency = currencyConverter.FillCurrencyList();

//Set
currencyConverter.UpdateConfiguration(currencyConverter.GetAllCurrency(ListCurrency));

//Convert
//var Result = currencyConverter.Convert(CurrencyEnum.USD.ToString(), CurrencyEnum.CAD.ToString(), 1000);
var Result = currencyConverter.Convert(CurrencyEnum.ABC.ToString(), CurrencyEnum.USD.ToString(), Amount);


//Print
currencyConverter.Print();
//Console.WriteLine(Result);
Console.WriteLine();
Console.WriteLine($"{Amount} {CurrencyEnum.ABC} ==> {CurrencyEnum.USD} = {Result} ");
Console.ReadLine();
