#region MIT License

/*
 * Copyright (c) 2009 Nick Gravelyn (nick@gravelyn.com), Markus Ewald (cygon@nuclex.org)
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a 
 * copy of this software and associated documentation files (the "Software"), 
 * to deal in the Software without restriction, including without limitation 
 * the rights to use, copy, modify, merge, publish, distribute, sublicense, 
 * and/or sell copies of the Software, and to permit persons to whom the Software 
 * is furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all 
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION 
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE 
 * SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
 * 
 */

#endregion

using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using SpriteSheetPacker;
namespace SpriteSheetPacker.XnaExporters
{
	// writes out an XML file ready to be put into a XNA Content project and get compiled as content.
	// this file can be loaded using Content.Load<Dictionary<string, Rectangle>> from inside the game.
	public class BannerlordExporer : sspack.IMapExporter
	{
		public string MapExtension
		{
			get { return "xml"; }
		}

		public void Save(string filename, Dictionary<string, Rectangle> map)
		{
			string imageFilename = filename.Replace(".xml", ".png");
			string categoryName = Path.GetFileNameWithoutExtension(filename);

			System.Drawing.Image img = System.Drawing.Image.FromFile(imageFilename);
			Trace.WriteLine("Width: " + img.Width + ", Height: " + img.Height);

			using (StreamWriter writer = new StreamWriter(filename))
			{
				writer.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
				writer.WriteLine("<SpriteData>");

				writer.WriteLine("\t<SpriteCategories>");
				writer.WriteLine("\t\t<SpriteCategory>");

				writer.WriteLine($"\t\t\t<Name>{categoryName}</Name>");
				writer.WriteLine($"\t\t\t<SpriteSheetCount>1</SpriteSheetCount>");
				writer.WriteLine($"\t\t\t<SpriteSheetSize ID='1' Width='{img.Width}' Height='{img.Height}'/>");

				writer.WriteLine("\t\t</SpriteCategory>");
				writer.WriteLine("\t</SpriteCategories>");

				writer.WriteLine("\t<SpriteParts>");
				foreach (var entry in map)
				{
					Rectangle r = entry.Value;
					writer.WriteLine("\t\t<SpritePart>");

					writer.WriteLine("\t\t\t<SheetID>1</SheetID>");
					writer.WriteLine($"\t\t\t<Name>{Path.GetFileNameWithoutExtension(entry.Key)}</Name>");
					writer.WriteLine($"\t\t\t<Width>{r.Width}</Width>");
					writer.WriteLine($"\t\t\t<Height>{r.Height}</Height>");
					writer.WriteLine($"\t\t\t<SheetX>{r.X}</SheetX>");
					writer.WriteLine($"\t\t\t<SheetY>{r.Y}</SheetY>");
					writer.WriteLine($"\t\t\t<CategoryName>{categoryName}</CategoryName>");

					writer.WriteLine("\t\t</SpritePart>");


				}
				writer.WriteLine("\t</SpriteParts>");

				writer.WriteLine("\t<Sprites>");
				foreach (var entry in map)
				{
					Rectangle r = entry.Value;
					writer.WriteLine("\t\t<GenericSprite>");

					writer.WriteLine($"\t\t\t<Name>{Path.GetFileNameWithoutExtension(entry.Key)}</Name>");
					writer.WriteLine($"\t\t\t<SpritePartName>{Path.GetFileNameWithoutExtension(entry.Key)}</SpritePartName>");

					writer.WriteLine("\t\t</GenericSprite>");
				}
				writer.WriteLine("\t</Sprites>");

				writer.WriteLine("</SpriteData>");

			}
		}
	}
}
