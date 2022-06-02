// See https://aka.ms/new-console-template for more information
using O2S.Components.PDFRender4NET;
using System.Drawing;
using System.Drawing.Imaging;

var fileName = "kiosk.pdf";
var outPutPath =  "output/";

Console.WriteLine("Hello, World!");
FileStream fileStream = new FileStream(fileName, FileMode.Open);    
ConvertAllPDF2Images(fileStream, outPutPath, "kiosk", ImageFormat.Png, Definition.Ten);

/// <summary>
/// PDF文档所有页全部转换为图片
/// </summary>
/// <param name="pdfInputPath">PDF文件流</param>
/// <param name="imageOutputPath">图片输出路径</param>
/// <param name="imageName">生成图片的名字</param>
/// <param name="imageFormat">设置所需图片格式</param>
/// <param name="definition">设置图片的清晰度，数字越大越清晰</param>
static void ConvertAllPDF2Images(Stream pdfStream, string imageOutputPath, string imageName, ImageFormat imageFormat, Definition definition)
{
    PDFFile pdfFile = PDFFile.Open(pdfStream);
    int startPageNum = 1;
    int endPageNum = pdfFile.PageCount;
    //  var bitMap = new Bitmap[endPageNum];
    for (int i = startPageNum; i <= endPageNum; i++)
    {
        try
        {
            Bitmap pageImage = pdfFile.GetPageImage(i - 1, 56 * (int)definition);

            int canKao = pageImage.Width > pageImage.Height ? pageImage.Height : pageImage.Width;
            int newHeight = canKao > 1080 ? pageImage.Height / 2 : pageImage.Height;
            int newWidth = canKao > 1080 ? pageImage.Width / 2 : pageImage.Width;
            Bitmap newPageImage = new Bitmap(newWidth, newHeight);

            Graphics g = Graphics.FromImage(newPageImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            //重新画图的时候Y轴减去40，高度也减去40  这样水印就看不到了
            g.DrawImage(pageImage, new Rectangle(0, -45, newWidth, newHeight),
            new Rectangle(0, 40, pageImage.Width, pageImage.Height - 40), GraphicsUnit.Pixel);
            newPageImage.Save(imageOutputPath + imageName +"."+ imageFormat.ToString());//+ i.ToString() imageFormat
            g.Dispose();
            newPageImage.Dispose();
            pageImage.Dispose();

        }
        catch (Exception ex)
        {

        }
    }
    pdfFile.Dispose();
}
/// <summary>
/// 转换的图片清晰度，1最不清醒，10最清晰
/// </summary>
public enum Definition
{
    One = 1, Two = 2, Three = 3, Four = 4, Five = 5, Six = 6, Seven = 7, Eight = 8, Nine = 9, Ten = 10
}