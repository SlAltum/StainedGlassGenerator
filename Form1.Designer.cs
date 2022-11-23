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

namespace StainedGlassGenerator{

partial class Form1
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        string imgsResourceFile = @".\imgs.resx";
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Text = "彩色玻璃生成器";
        using (ResXResourceSet resxSet = new ResXResourceSet(imgsResourceFile)){
            this.Icon = (Icon)resxSet.GetObject("logo.ico");
        }
        this.drawOriginBtn = new System.Windows.Forms.Button();
        this.doneBtn = new System.Windows.Forms.Button();

        // 
        // drawOriginBtn
        // 
        this.drawOriginBtn.Location = new System.Drawing.Point(0, 410);
        this.drawOriginBtn.Name = "drawOriginBtn";
        this.drawOriginBtn.Size = new System.Drawing.Size(80, 20);
        this.drawOriginBtn.Text = "绘制原图";
        this.drawOriginBtn.UseVisualStyleBackColor = true;
        this.drawOriginBtn.Click += new System.EventHandler(this.DrawOrigin);

        // 
        // doneBtn
        // 
        this.doneBtn.Location = new System.Drawing.Point(0, 430);
        this.doneBtn.Name = "doneBtn";
        this.doneBtn.Size = new System.Drawing.Size(80, 20);
        this.doneBtn.Text = "生成玻璃";
        this.doneBtn.UseVisualStyleBackColor = true;
        this.doneBtn.Click += new System.EventHandler(this.DyeGlasses);

        this.Controls.Add(drawOriginBtn);
        this.Controls.Add(doneBtn);
        this.drawOriginBtn.TabStop = false;
        this.doneBtn.TabStop = false;
    }

    #endregion
    private System.Windows.Forms.Button drawOriginBtn;
    private System.Windows.Forms.Button doneBtn;
}

}
