using System.Net.Mime;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Resources;
public class Imgs{
	public static void Main(){
		using (ResXResourceWriter resx = new ResXResourceWriter(@".\imgs.resx")){
			resx.AddResource("logo.ico", new Icon(@".\imgs\logo.ico"));
			// resx.AddResource("CelestialEmporerCrown.png", Image.FromFile(@".\FDUEUtils\logo\CelestialEmporerCrown.png"));
            DirectoryInfo imgsDir = new DirectoryInfo(@".\Imgs");
            foreach (FileInfo nextFile in imgsDir.GetFiles())
			{
				if(nextFile.Extension == "png"){
                    resx.AddResource(nextFile.Name, Image.FromFile(imgsDir + "\\" + nextFile.Name));
                }
			}
		}
	}
}