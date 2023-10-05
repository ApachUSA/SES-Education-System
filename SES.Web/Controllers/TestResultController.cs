using Microsoft.AspNetCore.Mvc;
using SES.Domain.Entity.TestE;
using SES.Domain.Enum;
using SES.Service.Implementations;
using SES.Service.Interfaces;
using SES.Web.Models;
using System.Diagnostics;
using iText;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.IO.Font;
using iText.Kernel.Font;
using iText.Layout.Borders;
using SES.Domain.ViewModel;

namespace SES.Web.Controllers
{
	public class TestResultController : Controller
	{
		private readonly ITestHistoryService _historyService;
		private readonly ITestResultService _resultService;

		public TestResultController(ITestResultService resultService, ITestHistoryService historyService)
		{
			_resultService = resultService;
			_historyService = historyService;
		}

		[HttpGet]
		public async Task<IActionResult> TestHistory()
		{
			var response = await _historyService.Get(int.Parse(HttpContext.User.FindFirst("UserID").Value));
			if (response.StatusCode == ResponseStatus.Success)
			{
				return PartialView("_HistoryPartialView", response.Data.OrderByDescending(x => x.Date).ToList());
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public async Task<IActionResult> TestResult()
		{
			var response = await _resultService.Get(1);
			if (response.StatusCode == ResponseStatus.Success)
			{
				return PartialView("_ResultPartialView", response.Data.OrderByDescending(x => x.Date).ToList());
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public async Task<IActionResult> TestResultFiltered(string positions, string date)
		{
			var response = await _resultService.Get(1);
			if (response.StatusCode == ResponseStatus.Success)
			{
				if (positions != null) response.Data = ApplyPositionFilter(response.Data, positions.Split(','));

				if (date != null) response.Data = ApplyDateFilter(response.Data, date);

				if (!response.Data.Any()) return BadRequest("Результатів не знайдено");

				return PartialView("_ResultPartialView", response.Data.OrderByDescending(x => x.Date).ToList());
			}
			return View("ErrorView", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[HttpGet]
		public IActionResult StudentsResultPdf([FromQuery] string result)
		{
			var results = System.Text.Json.JsonSerializer.Deserialize<List<PDFResultVM>>(result);
			using MemoryStream memoryStream = new();

			var pdfDoc = new Document(new PdfDocument(new PdfWriter(memoryStream)));

			string FONT_FILENAME = @"wwwroot/times.ttf";
			PdfFont font = PdfFontFactory.CreateFont(FONT_FILENAME, PdfEncodings.IDENTITY_H);
			pdfDoc.SetFont(font);

			var title = new Paragraph("Державна служба України з надзвичайних ситуацій");
			title.SetTextAlignment(TextAlignment.CENTER);
			title.SetFontSize(14);
			title.SetBold();
			pdfDoc.Add(title);

			var subtitle = new Paragraph("Результати проведення Державної кваліфікаційної атестації");
			subtitle.SetTextAlignment(TextAlignment.CENTER);
			subtitle.SetFontSize(12);
			subtitle.SetBold();
			pdfDoc.Add(subtitle);


			var table = new Table(UnitValue.CreatePercentArray(new float[] { 1, 3, 3, 1, 2 })).UseAllAvailableWidth();
			table.SetMarginTop(100);
			table.AddHeaderCell(new Paragraph("№").SetTextAlignment(TextAlignment.CENTER).SetBold());
			table.AddHeaderCell(new Paragraph("ПІБ").SetTextAlignment(TextAlignment.CENTER).SetBold());
			table.AddHeaderCell(new Paragraph("Посада").SetTextAlignment(TextAlignment.CENTER).SetBold());
			table.AddHeaderCell(new Paragraph("Бал").SetTextAlignment(TextAlignment.CENTER).SetBold());
			table.AddHeaderCell(new Paragraph("Статус").SetTextAlignment(TextAlignment.CENTER).SetBold());

			int i = 1;
			foreach (var row in results)
			{
				table.AddCell(new Paragraph(i++.ToString()).SetTextAlignment(TextAlignment.CENTER));
				table.AddCell(new Paragraph(row.SNP).SetTextAlignment(TextAlignment.CENTER));
				table.AddCell(new Paragraph(row.Position).SetTextAlignment(TextAlignment.CENTER));
				table.AddCell(new Paragraph(row.Mark).SetTextAlignment(TextAlignment.CENTER));
				table.AddCell(new Paragraph(row.Status).SetTextAlignment(TextAlignment.CENTER));
			}

			pdfDoc.Add(table);

			var footer = new Table(UnitValue.CreatePercentArray(new float[] { 2, 3, 2 })).UseAllAvailableWidth();
			footer.SetMarginTop(100);
			footer.SetBorder(Border.NO_BORDER);

			var paragraphDate = new Paragraph();
			var paragraphSnp = new Paragraph();
			var paragraphSign = new Paragraph();

			paragraphDate.Add(new Text("Дата: ")).Add(new Text(DateTime.Now.ToShortDateString()).SetUnderline()).Add(new Text(" р."));
			paragraphSnp.Add(new Text(HttpContext.User.Identity.Name).SetUnderline()).Add("\n").Add(new Text("(ПІБ)").SetFontSize(8));
			paragraphSign.Add(new Text(new string('_', 30))).Add("\n").Add(new Text("(підпис та печатка)").SetFontSize(8));



			footer.AddCell(paragraphDate).SetBorder(Border.NO_BORDER);
			footer.AddCell(paragraphSnp.SetTextAlignment(TextAlignment.CENTER));
			footer.AddCell(paragraphSign.SetTextAlignment(TextAlignment.CENTER));

			foreach (var cell in footer.GetChildren())
			{
				if (cell is Cell tableCell)
				{
					tableCell.SetBorder(Border.NO_BORDER);
				}
			}

			pdfDoc.Add(footer);

			pdfDoc.Close();
			
			return File(memoryStream.ToArray(), "application/pdf");

		}

		private static List<Test_Result> ApplyPositionFilter(List<Test_Result> results, string[]? selectedPosition) => results.Where(x => selectedPosition.Contains(x.User?.Position?.Name)).ToList();

		private static List<Test_Result> ApplyDateFilter(List<Test_Result> results, string date) => results.Where(x => x.Date == date).ToList();
	}
}
