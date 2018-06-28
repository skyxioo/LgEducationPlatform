using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.IO;

namespace Lg.EducationPlatform.WebHelper
{
    public class UnicodeFontFactory : FontFactoryImp
    {
        private static readonly string arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
            "arialuni.ttf");//arial unicode MS是完整的unicode字型。
        private static readonly string songFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts),
          "simsun.ttc,0");//標楷體


        public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
            bool cached)
        {
            //可用Arial或標楷體，自己選一個
            BaseFont baseFont = BaseFont.CreateFont(songFontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            return new Font(baseFont, size, style, color);
        }
    }
}
