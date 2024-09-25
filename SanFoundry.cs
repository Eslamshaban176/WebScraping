namespace WebScraping;

using CsvHelper;
using HtmlAgilityPack;
using System.Collections.Generic;
using System.Globalization;

public class SanFoundry
{
    public string Url { get; set; } = string.Empty;
    public readonly List<Question> Questions;
    public SanFoundry(string url)
    {
        Url = url;
        Questions = new List<Question>();
        GetQuestions();
    }

    private void GetQuestions()
    {
        var document = new HtmlWeb().Load(Url);

        var questionHtmlElements = document.DocumentNode
                    .QuerySelectorAll(ConstantTag.QuestionSection);

        foreach (var questionHtmlElement in questionHtmlElements)
        {
            List<string> questionList = new List<string>();

            questionHtmlElement.InnerHtml.Split(new[] { ConstantTag.Br }, StringSplitOptions.None)
            .ToList().ForEach(questionOrAnswer =>
            {
                if (!string.IsNullOrEmpty(questionOrAnswer))
                    questionList.Add(questionOrAnswer);
            });

            var answerNode = questionHtmlElement.QuerySelectorAll(ConstantTag.Span)
                .Where(a => a.GetAttributeValue(ConstantTag.Title, ConstantTag.Default) == ConstantTag.ViewAnswer)
                .FirstOrDefault();

            if (answerNode != null)
            {
                string answerId = answerNode.GetAttributeValue(ConstantTag.Id, ConstantTag.Default);

                string answerDivId = ConstantTag.Target + answerId;

                var answerDiv = document.DocumentNode.QuerySelectorAll(ConstantTag.Div)
                    .Where(a => a.GetAttributeValue(ConstantTag.Id, ConstantTag.Default) == answerDivId)
                    .FirstOrDefault();

                string answer = answerDiv!.InnerText.Trim() is null ? ConstantTag.Default : answerDiv.InnerText.Trim();

                if (questionList.Count == 6)
                {
                    Questions.Add(new Question
                    {
                        Quest = questionList[0],
                        OptionA = questionList[1],
                        OptionB = questionList[2],
                        OptionC = questionList[3],
                        OptionD = questionList[4],
                        Answer = answer

                    });
                }

            }
        }
    }

    public void WriteQuestionsToCsv(string fileName)
    {
        using (var writer = new StreamWriter(fileName + ".csv"))
        using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            csv.WriteRecords(Questions);
    }

    public void WriteQuestionsToPDF(string fileName)
    {

    }
}
