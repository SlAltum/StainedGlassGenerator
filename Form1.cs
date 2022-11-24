using System.Collections.Specialized;
using System;
using System.IO;
using System.Text;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Resources;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Drawing2D;

namespace StainedGlassGenerator{

public class Vertex{
    public Point self;
    public List<Point> neighbours = new List<Point>();
    public List<double> arcs = new List<double>();
    public Vertex(int x, int y){
        this.self = new Point(x,y);
    }
    public void SortAntiClockwise(){
        for(int i=0;i<neighbours.Count;i++){
            Point neighbour = neighbours[i];
            double detX = neighbour.X - self.X;
            double detY = neighbour.Y - self.Y;
            double cos = detX/(detX*detX + detY*detY);
            if(detY >= 0){
                arcs.Add(Math.Acos(cos));
            }else{
                arcs.Add(2*Math.PI-Math.Acos(cos));
            }
        }
        double tempArc;
        Point tempPoint;
        for(int i=0;i<neighbours.Count-1;i++){
            for(int j=i+1;j<neighbours.Count;j++){
                if(arcs[i]>arcs[j]){
                    tempArc = arcs[i];
                    arcs[i] = arcs[j];
                    arcs[j] = tempArc;
                    tempPoint = neighbours[i];
                    neighbours[i] = neighbours[j];
                    neighbours[j] = tempPoint;
                }
            }
        }
    }
    // 删除可能交叉的线段(需要先排序)
    public void DeleteLongVec(){
        if(neighbours.Count<3)
            return;
        for(int i=0;i<neighbours.Count;i++){
            Point midEnd = neighbours[i];
            Point leftEnd = i == 0 ? neighbours[neighbours.Count-1]:neighbours[i-1];
            Point rightEnd = i == neighbours.Count-1 ? neighbours[0]:neighbours[i+1];
            if(!(
                NumMax(leftEnd.X,rightEnd.X) < NumMin(midEnd.X,self.X) ||
                NumMin(leftEnd.X,rightEnd.X) > NumMax(midEnd.X,self.X) ||
                NumMax(leftEnd.Y,rightEnd.Y) < NumMin(midEnd.Y,self.Y) ||
                NumMin(leftEnd.Y,rightEnd.Y) > NumMax(midEnd.Y,self.Y)
                ) &&
                ((leftEnd.X-self.X)*(midEnd.Y-self.Y)-(leftEnd.Y-self.Y)*(midEnd.X-self.X)) *
                ((midEnd.X-self.X)*(rightEnd.Y-self.Y)-(midEnd.Y-self.Y)*(rightEnd.X-self.X)) >=0 &&
                ((self.X-leftEnd.X)*(rightEnd.Y-leftEnd.Y)-(self.Y-leftEnd.Y)*(rightEnd.X-leftEnd.X)) *
                ((rightEnd.X-leftEnd.X)*(midEnd.Y-leftEnd.Y)-(rightEnd.Y-leftEnd.Y)*(midEnd.X-leftEnd.X)) >=0 &&
                ((midEnd.X-self.X)^2+(midEnd.Y-self.Y)^2) >= ((leftEnd.X-rightEnd.X)^2+(leftEnd.Y-rightEnd.Y)^2)
                ){
                // Console.WriteLine("[Vertex] delete");
                neighbours.RemoveAt(i);
                arcs.RemoveAt(i);
                i --;
            }
            if(neighbours.Count<3)
                break;
        }
    }
    private int NumMax(int a, int b){
        return a>b?a:b;
    }
    private int NumMin(int a, int b){
        return a<b?a:b;
    }
}

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
    }
    private void DrawOrigin(object sender, EventArgs e){
        if (!(Directory.Exists(@".\Input")))
            Directory.CreateDirectory(@".\Input");
        if (!(Directory.Exists(@".\Output")))
            Directory.CreateDirectory(@".\Output");
        DirectoryInfo inputDir = new DirectoryInfo(@".\Input");
        Console.WriteLine("[DyeGlasses] inputDir = {0}",inputDir.ToString());
        foreach (FileInfo nextFile in inputDir.GetFiles())
        {
                // Console.WriteLine("[DyeGlasses] Dying: {0}",nextFile.Name);
            if(nextFile.Extension == ".png"){
                Console.WriteLine("[DyeGlasses] Dying: {0}",nextFile.Name);
                // Executing dye work
                Image originImg = Image.FromFile(nextFile.FullName);
                Bitmap originBmp = new Bitmap(originImg);
                try{
                    int iWidth = originBmp.Width;
                    int iHeight = originBmp.Height;

                    // 注意这个地方图像的两维方向与数组两维的方向是转置的关系

                    int[,] channelR = new int[iWidth, iHeight];
                    int[,] channelG = new int[iWidth, iHeight];
                    int[,] channelB = new int[iWidth, iHeight];
                    int[,] channelA = new int[iWidth, iHeight];
                    Bitmap outputBmp = new Bitmap(originImg.Width, originImg.Height);
                    Graphics g = Graphics.FromImage(outputBmp);
                    g.Clear(Color.FromArgb(0,0,0,0));
                    // 设置画布的描绘质量   
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    for (int i = 0; i < iWidth; i++)
                    {
                        for (int j = 0; j < iHeight; j++)
                        {
                            Color colCurr = originBmp.GetPixel(i, j);
                            channelR[i, j] = colCurr.R;
                            channelG[i, j] = colCurr.G;
                            channelB[i, j] = colCurr.B;
                            channelA[i, j] = colCurr.A;
                            Pen pen = new Pen(Color.FromArgb(colCurr.A,colCurr.R,colCurr.G,colCurr.B));
                            g.DrawLine(pen,i,j,i+1,j+1);
                        }
                    }
                    outputBmp.Save(@".\Output\"+nextFile.Name);
                    g.Dispose();
                    outputBmp.Dispose();
                }catch (Exception){
                    int[,] channelR = null;
                    int[,] channelG = null;
                    int[,] channelB = null;
                    int[,] channelA = null;
                }
                originBmp.Dispose();
                originImg.Dispose();
            }
        }
    }
    private void DyeGlasses(object sender, EventArgs e){
        int seed = 777;
        int spacing = 32;
        if (!(Directory.Exists(@".\Input")))
            Directory.CreateDirectory(@".\Input");
        if (!(Directory.Exists(@".\Output")))
            Directory.CreateDirectory(@".\Output");
        DirectoryInfo inputDir = new DirectoryInfo(@".\Input");
        Console.WriteLine("[DyeGlasses] inputDir = {0}",inputDir.ToString());
        foreach (FileInfo nextFile in inputDir.GetFiles())
        {
                // Console.WriteLine("[DyeGlasses] Dying: {0}",nextFile.Name);
            if(nextFile.Extension == ".png"){
                Console.WriteLine("[DyeGlasses] Dying: {0}",nextFile.Name);
                // Executing dye work
                Image originImg = Image.FromFile(nextFile.FullName);
                Bitmap originBmp = new Bitmap(originImg);
                // Bitmap originBmp = new Bitmap(originImg.Width, originImg.Height);
                try{
                    int iWidth = originBmp.Width;
                    int iHeight = originBmp.Height;
    
                    // 注意这个地方图像的两维方向与数组两维的方向是转置的关系
    
                    int[,] channelR = new int[iWidth, iHeight];
                    int[,] channelG = new int[iWidth, iHeight];
                    int[,] channelB = new int[iWidth, iHeight];
                    int[,] channelA = new int[iWidth, iHeight];
    
                    for (int i = 0; i < iWidth; i++)
                    {
                        for (int j = 0; j < iWidth; j++)
                        {
                            Color colCurr = originBmp.GetPixel(i, j);
                            channelR[i, j] = colCurr.R;
                            channelG[i, j] = colCurr.G;
                            channelB[i, j] = colCurr.B;
                            channelA[i, j] = colCurr.A;
                        }
                    }
                    Random vertexSeed = new Random(seed);
                    List<Vertex> vertexes = new List<Vertex>();
                    // 轮廓提取
                    for (int i = 2; i < iWidth-2; i++){
                        for (int j = 2; j < iHeight-2; j++){
                            int filter = Convert.ToInt16(channelA[i-2, j-2]>224) + Convert.ToInt16(channelA[i-2, j-1]>224) + Convert.ToInt16(channelA[i-2, j]>224) + Convert.ToInt16(channelA[i-2, j+1]>224) + Convert.ToInt16(channelA[i-2, j+2]>224) +
                            Convert.ToInt16(channelA[i-1, j-2]>224) + Convert.ToInt16(channelA[i-1, j-1]>224) + Convert.ToInt16(channelA[i-1, j]>224) + Convert.ToInt16(channelA[i-1, j+1]>224) + Convert.ToInt16(channelA[i-1, j+2]>224) +
                            Convert.ToInt16(channelA[i, j-2]>224) + Convert.ToInt16(channelA[i, j-1]>224) + Convert.ToInt16(channelA[i, j+1]>224) + Convert.ToInt16(channelA[i, j+2]>224) +
                            Convert.ToInt16(channelA[i+1, j-2]>224) + Convert.ToInt16(channelA[i+1, j-1]>224) + Convert.ToInt16(channelA[i+1, j]>224) + Convert.ToInt16(channelA[i+1, j+1]>224) + Convert.ToInt16(channelA[i+1, j+2]>224) +
                            Convert.ToInt16(channelA[i+2, j-2]>224) + Convert.ToInt16(channelA[i+2, j-1]>224) + Convert.ToInt16(channelA[i+2, j]>224) + Convert.ToInt16(channelA[i+2, j+1]>224) + Convert.ToInt16(channelA[i+2, j+2]>224);
                            if(channelA[i, j]>224 &&
                                !(channelA[i-1, j-1]>224 && channelA[i-1, j]>224 && channelA[i-1, j+1]>224 &&
                                channelA[i, j-1]>224 && channelA[i, j]>224 && channelA[i, j+1]>224 &&
                                channelA[i+1, j-1]>224 && channelA[i+1, j]>224 && channelA[i+1, j+1]>224) &&
                                (vertexSeed.Next(0,spacing)<1 || filter<7 || filter>18)
                            ){
                                // Console.WriteLine("[DyeGlasses] filter={0}",filter);
                                vertexes.Add(new Vertex(i,j));
                            }
                        }
                    }
                    // 随机散点
                    for (int i = 0; i < iWidth; i+=spacing/2)
                    {
                        for (int j = 0; j < iHeight; j+=spacing/2)
                        {
                            // Console.WriteLine("[DyeGlasses] channelA[{0}, {1}]={2}",i,j,channelA[i, j]);
                            int iN = i + vertexSeed.Next(0,spacing/2);
                            int jN = j + vertexSeed.Next(0,spacing/2);
                            if(iN < iWidth && jN < iHeight &&
                                channelA[i, j]>224){
                                foreach(Vertex vertex in vertexes){
                                    if((vertex.self.X-iN)*(vertex.self.X-iN)+(vertex.self.Y-jN)*(vertex.self.Y-jN)<(spacing/4)*(spacing/4)){
                                        // Console.WriteLine("[DyeGlasses] vertex too close");
                                        continue;
                                    }
                                }
                                // Console.WriteLine("[DyeGlasses] generate vertex [{0},{1}]",iN,jN);
                                vertexes.Add(new Vertex(iN,jN));
                            }
                        }
                    }
                    Bitmap outputBmp = new Bitmap(originImg.Width, originImg.Height);
                    Graphics g = Graphics.FromImage(outputBmp);
                    g.Clear(Color.FromArgb(0,0,0,0));
                    // 设置画布的描绘质量   
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    Pen blackPen = new Pen(Color.FromArgb(255,0,0,0));
                    // Console.WriteLine("[DyeGlasses] add neibours");
                    foreach(Vertex vertexA in vertexes){
                        foreach(Vertex vertexB in vertexes){
                            if(vertexA.self != vertexB.self &&
                                !vertexA.neighbours.Exists(point => point == vertexB.self) &&
                                System.Math.Sqrt((vertexB.self.X-vertexA.self.X)*(vertexB.self.X-vertexA.self.X) + 
                                (vertexB.self.Y-vertexA.self.Y)*(vertexB.self.Y-vertexA.self.Y)) < spacing &&
                                channelA[(vertexB.self.X+vertexA.self.X)/2, (vertexB.self.Y+vertexA.self.Y)/2]>224 &&
                                channelA[vertexB.self.X*2/3+vertexA.self.X/3, vertexB.self.Y*2/3+vertexA.self.Y/3]>224 &&
                                channelA[vertexB.self.X/3+vertexA.self.X*2/3, vertexB.self.Y/3+vertexA.self.Y*2/3]>224
                                ){
                                vertexA.neighbours.Add(vertexB.self);
                                // vertexB.neighbours.Add(vertexA.self);
                            }
                        }
                        vertexA.SortAntiClockwise();
                        vertexA.DeleteLongVec();
                    }
                    Brush brush;
                    Console.WriteLine("[DyeGlasses] dye the glass");
                    foreach(Vertex vertex in vertexes){
                        // Console.WriteLine("[DyeGlasses] vertex.neighbours.Count={0}",vertex.neighbours.Count);
                        if(vertex.neighbours.Count == 2){
                            Point endPoint = vertex.neighbours[0];
                            Point nextPoint = vertex.neighbours[1];
                            int centerI = vertex.self.X + (endPoint.X+nextPoint.X-2*vertex.self.X)/3;
                            int centerJ = vertex.self.Y + (endPoint.Y+nextPoint.Y-2*vertex.self.Y)/3;
                            brush = new SolidBrush(Color.FromArgb(255,channelR[centerI,centerJ],channelG[centerI,centerJ],channelB[centerI,centerJ]));
                            g.FillPolygon(brush,new Point[]{vertex.self,endPoint,nextPoint});
                        }else if(vertex.neighbours.Count > 2){
                            for(int i=0;i<vertex.neighbours.Count;i++){
                                Point endPoint = vertex.neighbours[i];
                                Point nextPoint;
                                double arc = 0;
                                // Console.WriteLine("[DyeGlasses] i={0}",i);
                                // Console.WriteLine("[DyeGlasses] vertex.neighbours.Count={0}",vertex.neighbours.Count);
                                // Console.WriteLine("[DyeGlasses] vertex.arcs.Count={0}",vertex.arcs.Count);
                                if(i<vertex.neighbours.Count-1){
                                    nextPoint = vertex.neighbours[i+1];
                                    // Console.WriteLine("[DyeGlasses] Math.PI={0}",Math.PI);
                                    arc = vertex.arcs[i+1]-vertex.arcs[i];
                                    // Console.WriteLine("[DyeGlasses] arc={0}",arc);
                                }else{
                                    nextPoint = vertex.neighbours[0];
                                    // Console.WriteLine("[DyeGlasses] Math.PI={0}",Math.PI);
                                    arc = vertex.arcs[0] + 2*Math.PI - vertex.arcs[i];
                                    // Console.WriteLine("[DyeGlasses] arc={0}",arc);
                                }
                                if(arc<Math.PI){
                                    // Console.WriteLine("[DyeGlasses] arc<Math.PI");
                                    int centerI = vertex.self.X + (endPoint.X+nextPoint.X-2*vertex.self.X)/3;
                                    int centerJ = vertex.self.Y + (endPoint.Y+nextPoint.Y-2*vertex.self.Y)/3;
                                    // Console.WriteLine("[DyeGlasses] centerI={0}",centerI);
                                    // Console.WriteLine("[DyeGlasses] centerJ={0}",centerJ);
                                    brush = new SolidBrush(Color.FromArgb(255,channelR[centerI,centerJ],channelG[centerI,centerJ],channelB[centerI,centerJ]));
                                    g.FillPolygon(brush,new Point[]{vertex.self,endPoint,nextPoint});
                                }
                            }
                        }
                    }
                    foreach(Vertex vertex in vertexes){
                        for(int i=0;i<vertex.neighbours.Count;i++){
                            // Console.WriteLine("[DyeGlasses] drawline");
                            g.DrawLine(blackPen,vertex.self,vertex.neighbours[i]);
                        }
                    }
                    // Image outputPng = (Image)outputBmp;
                    Console.WriteLine("[DyeGlasses] {0} has been dyed.",nextFile.Name);
                    outputBmp.Save(@".\Output\"+nextFile.Name);
                    g.Dispose();
                    outputBmp.Dispose();
                }catch (Exception){
                    int[,] channelR = null;
                    int[,] channelG = null;
                    int[,] channelB = null;
                    int[,] channelA = null;
                }
                
                originBmp.Dispose();
                originImg.Dispose();
            }
        }
    }
}

}
