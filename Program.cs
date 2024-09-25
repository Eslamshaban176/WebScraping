using CsvHelper;
using HtmlAgilityPack;
using WebScraping;
using System.Globalization;
using System;
public class Program
{
    const string url = "https://www.sanfoundry.com/data-structure-questions-answers-stack-operations/";
    private static void Main(string[] args)
    {
        SanFoundry sanFoundry = new SanFoundry(url);
        sanFoundry.WriteQuestionsToCsv("MCQ-Data-Structure-Questions");
    }
}